using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public float speedMove = 5;
    public float speedRotation = 5f;
    private bool _move = true;

    private CharacterController _characterController;
    private Animator _animator;
    private Transform tr;
    

    private void Start()
    {
        tr = transform;
        _animator = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
    }
    
    public void Move (Vector3 moveDirection)
    {
        if (_move == false)
            return;
        if (speedMove <= 0)
            return;
        _characterController.Move(moveDirection * Time.deltaTime);

        if (_animator == null)
            return;
        _animator.SetFloat("speed", _characterController.velocity.magnitude / speedMove);

        /*
        if (_characterController.velocity.magnitude > .2f)
            _animator.SetBool("move", true);
        else
            _animator.SetBool("move", false);
            */
    }

    public void Rotation (Vector3 target)
    {
        Vector3 direction = target - tr.position;
        if (direction != Vector3.zero)
        {
            Quaternion targetDir = Quaternion.LookRotation(direction, Vector3.up);
            tr.rotation = Quaternion.Lerp(tr.rotation, targetDir, speedRotation * Time.deltaTime);
        }
    }

    public void Freezing (Material freezeMaterial, float freezeTime)
    {
        _animator.speed = 0;
        _move = false;
        SkinnedMeshRenderer mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        Material standartMaterial = mesh.material;
        mesh.material = freezeMaterial;
        StartCoroutine(Unfreezing(standartMaterial, mesh, freezeTime));
    }
    IEnumerator Unfreezing (Material standartMaterial, SkinnedMeshRenderer mesh, float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);
        mesh.material = standartMaterial;
        _animator.speed = 1;
        _move = true;
    }
}
