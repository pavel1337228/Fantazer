using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRBmove : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _runSpeed = 0.5f;
    [SerializeField] private MoveConfig _moveConfig;

    private Animator _anim;
    private Rigidbody _rb;

    private float _horizontal, _vertical, _run;

    private bool idle => _horizontal == 0f && _vertical == 0f;
    private const float DISTANCE_OFFSET_CAMERA = 5f;
    public CameraConfig sc;

    private Quaternion _look;

    private Vector3 _targetRotate => _camera.forward * DISTANCE_OFFSET_CAMERA;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponent<Animator>();

        flag = false;
    }

    private void Update()
    {
        PlayAnimation();
        CharacterRotation();
        CameraChanging();
    }

    //Moving
    private void FixedUpdate()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _run = Input.GetAxis("Run");

        if (_run != 0)
        {
            _rb.drag = 3.5f;
        }
        else
        {
            _rb.drag = 2.8f;
        }

        float speed = _run * _runSpeed + _speed;

        _rb.AddForce(((transform.right * _horizontal) + (transform.forward * _vertical)) * speed / Time.deltaTime);
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
            sc.NormalX = -0.3f;
        }
        else
        {
            sc.NormalX = 0.3f;
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
            sc.NormalZ = -3f;
        }
        else
        {
            sc.NormalZ = -1.5f;
        }
    }
}
