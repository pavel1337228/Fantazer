using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRBmove : MonoBehaviour
{
    [SerializeField] private Transform _camera;
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _runSpeed;
    [SerializeField] private MoveConfig _moveConfig;

    private Animator _anim;
    private Rigidbody _rb;

    private float _horizontal, _vertical, _run;

    private bool idle => _horizontal == 0f && _vertical == 0f;
    private const float DISTANCE_OFFSET_CAMERA = 5f;
    public CameraConfig sc;

    public GameObject rs;
    public GameObject re;

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

    [SerializeField] private int _shootline = 1;
    [SerializeField] private float _shootdistance;
    [SerializeField] private float _shootstepdistance;

    //Moving
    private void FixedUpdate()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
        _run = Input.GetAxis("Run");

        Ray ray = new Ray(rs.transform.position, Vector3.down);
        Ray raystep = new Ray(re.transform.position, transform.forward);
        RaycastHit hit;
        RaycastHit hitstep;

        if (Physics.Raycast(raystep, out hitstep, _shootline))
        {
            Debug.DrawLine(raystep.origin, hitstep.point, Color.yellow);
        }
        else
        {
            Debug.DrawLine(raystep.origin, raystep.origin + raystep.direction * 1, Color.green);
        }

        if (Physics.Raycast (ray, out hit, _shootline))
        {

            Debug.DrawLine(ray.origin, hit.point, Color.red);

            _shootdistance = hit.distance;
            _shootstepdistance = hitstep.distance;

            float hoverForce = 5.0f;
            float hoverDamp = 0.5f;
            float hoverHeight = 30f;

            if ((_speed != 0))
            {
                if (((_shootdistance > 0.72f) & (_shootdistance < 0.89f)) & ((_shootstepdistance > 0f) & (_shootstepdistance < 0.225f)))
                {
                    float hoverError = hoverHeight - hit.distance;
                    float upwardSpeed = _rb.velocity.y;
                    //float f = _shootline - _shootdistance;
                    float lift = hoverError * hoverForce - upwardSpeed * hoverDamp;
                    _rb.AddForce((lift * Vector3.up));
                }
            }

        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 1, Color.green);
        }

        if (_run != 0)
        {
            _rb.drag = 3.5f;
        }
        else
        {
            _rb.drag = 2.5f;
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

        _anim.SetTrigger("Weapon");
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
