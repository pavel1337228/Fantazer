using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ControllerMovement : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private MoveConfig _moveConfig;
    [SerializeField] private CameraConfig _cameraConfig;
    [SerializeField] private bool _isWeaponing;

    private Animator _anim;

    private float _horizontal, _vertical, _run;

    private const float DISTANCE_OFFSET_CAMERA = 5f;
    private Vector3 _targetRotate => _camera.forward * DISTANCE_OFFSET_CAMERA;

    private CharacterController _controller;
    private Vector3 _direction;
    private Quaternion _look;

    private bool idle => _horizontal == 0f && _vertical == 0f;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        CharacterRotation();
        CharacterMovement();
        PlayAnimation();
        CameraChanging();
    }

    public void CharacterMovement()
    {
        if (_controller.isGrounded)
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
            _run = Input.GetAxis("Run");

            _direction = transform.TransformDirection(_horizontal, 0, _vertical).normalized;
        }

        _direction.y -= 2f * Time.deltaTime; //gravity

        if (_isWeaponing)
        {
            float speed = (_moveConfig.MovementSpeed - 0.5f) + _run * _moveConfig.RunSpeed;
            Vector3 dir = _direction * speed * Time.deltaTime;
            _controller.Move(dir);
        }
        else
        {
            float speed = _moveConfig.MovementSpeed + _run * _moveConfig.RunSpeed;
            Vector3 dir = _direction * speed * Time.deltaTime;
            _controller.Move(dir);
        }
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

        if (_isWeaponing == true)
        {
            _anim.SetTrigger("Weapon");
        }
        else
        {
            _anim.ResetTrigger("Weapon");
        }
        _anim.SetFloat("Vertical", vertical);
        _anim.SetFloat("Horizontal", horizontal);
    }

    public bool flag; public bool flag1;

    private void CameraChanging()
    {
        //ChangePivotX
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
            _cameraConfig.NormalX = -0.3f;
        }
        else
        {
            _cameraConfig.NormalX = 0.3f;
        }

        //ChangePivotZ
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
            _cameraConfig.NormalZ = -3f;
        }
        else
        {
            _cameraConfig.NormalZ = -1.5f;
        }


        //takeWeapon
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_isWeaponing)
            {
                _isWeaponing = false;
            }
            else
            {
                _isWeaponing = true;
            }
        }
    }
}
