using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private MoveConfig _moveConfig;

    private float _vertical, _horizontal, _run;
    private bool idle => _horizontal == 0f && _vertical == 0f;
    private const float DISTANCE_OFFSET_CAMERA = 5f;
    public CameraConfig sc;

    private CharacterController _controller;
    private Animator _anim;
    
    private Vector3 _direction;

    private Quaternion _look;

    private Vector3 _targetRotate => _camera.forward * DISTANCE_OFFSET_CAMERA;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
        Cursor.visible = _moveConfig.VisibleCursor;

        flag = false;
    }

    private void Update()
    {
        CharacterMove();
        CharacterRotation();
        PlayAnimation();
        CameraChenging();
    }
    //-1.5
    public bool flag; public bool flag1;

    private void CameraChenging()
    {
        //ChengePivotX
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (flag == false)
            {
                flag = true;
            }
            else
            {
                flag = false;
            }

        }
        
        if (flag == true)
        {
            sc.NormalX = -0.3f;
        }
        else
        {
            sc.NormalX = 0.3f;
        }
        //ChengePivotZ

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (flag1 == false)
            {
                flag1 = true;
            }
            else
            {
                flag1 = false;
            }

        }

        if (flag1 == true)
        {
            sc.NormalZ = -3f;
        }
        else
        {
            sc.NormalZ = -1.5f;
        }
        //ChengePivotZ
    }


    public void CharacterMove()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _run = Input.GetAxis("Run");

        if (_controller.isGrounded)
        {
            _anim.ResetTrigger("Jump");
            _direction = transform.TransformDirection(_horizontal, 0.0f, _vertical).normalized;

            //if (Input.GetButtonDown("Jump"))
            //{
            //    _direction.y += _moveConfig.JumpForce;
            //    _anim.SetTrigger("Jump");
            //}
        }

        float speed = _run * _moveConfig.RunSpeed + _moveConfig.MovementSpeed;

        _direction.y -= _moveConfig.Gravity * Time.deltaTime;
        Vector3 dir = _direction * speed * Time.deltaTime;
        dir.y = _direction.y;

        _controller.Move(dir);
    }

    public void CharacterRotation()
    {
        if (idle == false)
        {
            Vector3 target = _targetRotate;
            target.y = 0;
            float Speed = _moveConfig.AngularSpeed * Time.deltaTime;

            _look = Quaternion.LookRotation(target);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, _look, Speed);
        }
    }

    private void PlayAnimation()
    {
        float horizontal = _run * _horizontal + _horizontal;
        float vertical = _run * _vertical + _vertical;

        _anim.SetFloat("Vertical", vertical);
        _anim.SetFloat("Horizontal", horizontal);
    }
}
