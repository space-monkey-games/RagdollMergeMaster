using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{

    public float speedMove = 5;
    public float speedRotation = 5f;

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
}
