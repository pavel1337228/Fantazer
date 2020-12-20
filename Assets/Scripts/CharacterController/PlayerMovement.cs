using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private MoveConfig _moveConfig;

    private float _vertical, _horizontal;
    private bool idle => _horizontal == 0f && _vertical == 0f;
    private const float DISTANCE_OFFSET_CAMERA = 5f;


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
    }

    private void Update()
    {
        CharacterMove();
        CharacterRotation();
        PlayAnimation();
    }

    public void CharacterMove()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        if (_controller.isGrounded)
        {
            _direction = transform.TransformDirection(_horizontal, 0.0f, _vertical).normalized;
        }

        _direction.y -= _moveConfig.Gravity * Time.deltaTime;
        Vector3 dir = _direction * _moveConfig.MovementSpeed * Time.deltaTime;
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
        _anim.SetFloat("Vertical", _vertical);
        _anim.SetFloat("Horizontal", _horizontal);
    }
}
