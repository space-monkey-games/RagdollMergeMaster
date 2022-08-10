using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.Dynamics;
using UnityEngine.Events;

public class DamageParams
{
    public int damage;
    public Vector3 weaponPosition;
    public float forceFoRB;
    public float forceForMove;

    public DamageParams (int _damage, Vector3 _forcePos, float _forceForRb, float _forceForMove)
    {
        damage = _damage;
        weaponPosition = _forcePos;
        forceFoRB = _forceForRb;
        forceForMove = _forceForMove;
    }
}


public class DamageBase : MonoBehaviour
{
    public bool useActiveRagdol = true;
    public DamageBase target; 

    public Transform weapon;
    public GameObject projectilePrefab;
    public ParticleSystem damageParticle;
    [Tooltip("Префаб бара здоровья")]
    public GameObject hpBar;
    public float speedProjectile = 10f;

    public Material deffaultMaterial;
    public int hp = 10;
    private int maxHp;
    public int damage = 2;
    public float offcetForUI = 10f;
    public float distanceToAttack = 3f;
    public float forceForRB = 1000;
    public float forceForMove = 10f;
    public float attackRate = 1f;
    public Rigidbody[] rigidbodyForForce = new Rigidbody[0]; 
    public Animator animator;
    public AudioClip damageAudioClip;
    private Controller _controller;
    private FightController _fightController;
    private bool _attackMode;
    private Transform tr;
    private Vector3 _currentForce;
    public static UnityEvent deadEvent = new UnityEvent();
    private AudioSource _audioSource;
    private bool _isDead;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        _controller = GetComponent<Controller>();
        tr = transform;
        _fightController = FindObjectOfType<FightController>();
        _audioSource = FindObjectOfType<AudioSource>();
        maxHp = hp;
    }    

    private void Update()
    {        
        if (target == null)
        {
            _controller.Move(Vector3.zero);
            return;
        }
        _controller.Rotation(target.transform.position);
        if (FightController.Distance(tr, target.transform) <= distanceToAttack && !_attackMode)
        {
            AttackTimer();            
        }
        else
        {
            Vector3 moveDirection = tr.TransformDirection(new Vector3(0, -10, _controller.speedMove));
            _controller.Move(moveDirection);
            //_attackMode = false;
        }
        if (_currentForce != Vector3.zero)
                _currentForce = Vector3.MoveTowards(_currentForce, Vector3.zero, 300 * Time.deltaTime);
    }

    public void AttackTimer()
    {
        _attackMode = true;
        animator.SetTrigger("attack");     
    }

    //animation attack event in add impulse and damage moment
    public void Attack()
    {
        if (!_attackMode)
            return;
        if (hp <= 0 )
            return;
        if (projectilePrefab != null)
            StartCoroutine(ProjectileMove());
        else
            GetDamage();
    }

    private Transform projectile;
    IEnumerator ProjectileMove()
    {
        //if (target == null)
        //    StopCoroutine(ProjectileMove());
        projectile = Instantiate(projectilePrefab, tr.position, Quaternion.identity).transform;
        projectile.position = tr.position;
        projectile.gameObject.SetActive(true);
        //float delay = FightController.Distance(tr, target.transform) / speedProjectile;
        //Invoke("GetDamage", delay);
        while(target != null)
        {
            if (FightController.Distance(projectile, target.transform) > 1)
            {
                projectile.LookAt(target.transform);
                projectile.position = Vector3.MoveTowards(projectile.position, target.transform.position, speedProjectile * Time.deltaTime);
            }
            else
            {
                GetDamage();
                break;
            }
            yield return null;
        }
        if (projectile != null)
            Destroy(projectile.gameObject);
        
    }

    void GetDamage ()
    {
        if (target == null)
            return;
        target.Damage(new DamageParams(damage, weapon.position, forceForRB, forceForMove));
        _attackMode = false;
        if (target == null || target.hp <= 0)
        {
            target = _fightController.FindTarget(this);
            _attackMode = false;
        }
        if (damageAudioClip == null) //&& !_audioSource.isPlaying)
            return;
        float t = Random.Range(0f, .35f);
        PlayAudioDamage();
        //Invoke("PlayAudioDamage", t);
    }

    void PlayAudioDamage()
    {
        _audioSource.PlayOneShot(damageAudioClip);
    }

    public void Damage(DamageParams _damageParams)
    {
        if (_isDead)
            return;
        if(damageParticle != null)
            damageParticle.Play();
        
        hp -= _damageParams.damage;
        if (hp <= 0)
        {
            Dead();
            _isDead = true;
            return;
        }
        if(useActiveRagdol)
            StartCoroutine(Inertia(_damageParams));     
    }


    IEnumerator Inertia(DamageParams _damageParams)
    {
        float t = .3f;
        Vector3 moveDirection = (transform.position -_damageParams.weaponPosition).normalized;        
        moveDirection.y = 0;
        SetImpulse(moveDirection, _damageParams.forceFoRB);
        moveDirection *= _damageParams.forceForMove;
        while (t > 0)
        {
            _controller.Move(moveDirection);
            t -= Time.deltaTime;
            yield return null;
        }
        //yield return new WaitForSeconds(.3f);
        
    }

    void SetImpulse(Vector3 dir, float forceForRb)
    {
        if (useActiveRagdol == false)
            return;
        foreach (Rigidbody rb in rigidbodyForForce)
        {
            rb.AddForce(dir * forceForRb - _currentForce, ForceMode.Impulse);
            _currentForce = dir * forceForRb;
        }
    }

    void Dead ()
    {
        hp = 0;
        if (useActiveRagdol)
            transform.root.GetComponentInChildren<PuppetMaster>().state = PuppetMaster.State.Dead;
        StopAllCoroutines();
        _controller.enabled = false;
        GetComponent<CharacterController>().enabled = false;
        SkinnedMeshRenderer[] renderers = tr.root.GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer r in renderers)
            r.material = deffaultMaterial;
        SetImpulse(-tr.forward, forceForRB);
        _fightController.BotDead(this);
        if (projectile != null)
            Destroy(projectile.gameObject);
        if (deadEvent != null)
            deadEvent.Invoke();

        animator.SetTrigger("death");
        enabled = false;
        //Destroy(this);
    }

    public void Healing (int b)
    {
        int h = maxHp * b / 100;
        hp += h;
    }
}

