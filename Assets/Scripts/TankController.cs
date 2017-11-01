using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
 {
	[SerializeField] private Transform _cannonBody;
	[SerializeField] private Transform _muzzle;
	[SerializeField] private Rigidbody _tankRigidbody;

	[SerializeField] private GameObject _bulletPrefab;

	[SerializeField] private float _movementSpeed = 10f;
	[SerializeField] private float _turnSpeed = 3f;
	[SerializeField] private float _turnCannonSpeed = 2.5f;
    [SerializeField] private float _rateOfFire = 3f;

	private int _lives = 3;

    private float _nextShoot = 0;

	private bool _isTurningBody = false;
    private bool _isTurningCannon = false;
    private bool _isAccelerating = false;

    private float _turnDirection = 0;
    private float _cannonDirection = 0;
	private float _currentSpeed = 0;


	private void FixedUpdate()
	{
		if (_isTurningBody) 
		{
        	_tankRigidbody.AddTorque(transform.up * _turnSpeed * _turnDirection, ForceMode.VelocityChange);
		}
		if (_isAccelerating)
		{
			var desiredPosition = transform.position + transform.forward * _currentSpeed * _movementSpeed;
			_tankRigidbody.MovePosition(desiredPosition);
		}
    }

    private void Update()
    {
        if (_isTurningCannon)
        {
            var rotation = _cannonBody.localRotation;
            var desiredRotation = rotation.eulerAngles + new Vector3(0, _cannonDirection * _turnCannonSpeed * Time.deltaTime, 0);
            _cannonBody.localEulerAngles = desiredRotation;
        }
    }

    private void Die()
	{
		gameObject.SetActive(false);
	}

	public void Accelerate(float speed)
	{
		if (speed == _currentSpeed)
		{
			_isAccelerating = false;
			_currentSpeed = 0;
		}
		else
		{
			_isAccelerating = true;
			_currentSpeed = speed;
		}
	}

	public void Turn(float speed)
	{
		if (speed == _turnDirection)
		{
			_isTurningBody = false;
			_turnDirection = 0;
		}
		else
		{
			_isTurningBody = true;
			_turnDirection = speed;
		}
	}

	public void TurnCannon(float speed)
	{
        if (speed == _cannonDirection)
        {
            _isTurningCannon = false;
            _cannonDirection = 0;
        }
        else
        {
            _isTurningCannon = true;
            _cannonDirection = speed;
        }
	}

	public void Shoot()
	{
        if (Time.time > _nextShoot)
        {
            var bullet = Instantiate(_bulletPrefab);
            bullet.transform.position = _muzzle.position;
            bullet.transform.rotation = _muzzle.rotation;
            bullet.GetComponent<Bullet>().StartMovement();
            _nextShoot = Time.time + _rateOfFire;
        }
	}

	public void Damage()
	{
		_lives--;
		if (_lives <= 0)
		{
			Die();
		}
	}
}
