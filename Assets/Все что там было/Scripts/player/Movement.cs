using UnityEngine;

public class Movement : MonoBehaviour
{
    public Animator anim;

    private GameObject _player;
    private Rigidbody _rb;

    private float _speed;
    private float _buster;
    private float _maxSpeed;

    private float _jumpForce;

    private bool _isGrounded;

    private void Start()
    {
    	_player = (GameObject)this.gameObject; 
    	_rb = _player.GetComponent<Rigidbody>();

        _maxSpeed = 400f;
        _jumpForce = 200f;
    }

    private void FixedUpdate()
    {
    	PlayerMove();
    	PlayerJump();
    }

    private void PlayerMove()
    {
    	var moveX = Input.GetAxis("Horizontal");
    	var moveY = Input.GetAxis("Vertical");

        _rb.AddForce(transform.forward * moveY * _speed * Time.deltaTime);
        _rb.AddForce(transform.right * moveX * _speed * Time.deltaTime);

        if (moveX != 0 || moveY != 0)
        {
            _rb.drag = 0;

            if (_buster != 0 & _speed == _maxSpeed)
            {
                if ((_maxSpeed / 2f) >= _speed && _speed >= (_maxSpeed / 1.5f))
                {
                    _buster /= 1.2f;
                }

                if ((_maxSpeed / 1.49f) >= _speed && _speed >= (_maxSpeed / 1f))
                {
                    _buster /= 1.5f;
                }
            }

            if (_speed < _maxSpeed)
            {
                _speed += _buster;
            }
        }
        else
        {
            _buster = 20f;

            if (_speed > 0)
            {
                _speed -= _buster;
            }

            _rb.drag = 4;
        }
        var LookY = Input.GetAxis("Mouse X") * 15;

        if (moveX != 0 || moveY != 0)
            _player.transform.rotation = Quaternion.Euler(0, CameraMovement.LookAngle, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }

    private void OnCollisionExit(Collision collision)
    {
        IsGroundedUpate(collision, false);
    }

    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground"))
            _isGrounded = value;
    }

    private void PlayerJump()
    {
    	if (Input.GetAxis("Jump") > 0)
    	{
    		if (_isGrounded)
    			_rb.AddForce(Vector3.up * _jumpForce);
    	}
    }
}
