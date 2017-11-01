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

	private int _lives = 3;
	private bool _isTurningBody = false;
	private float _turnDirection;
	private float _currentSpeed = 0;

	private bool _isAccelerating = false;

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
		var rotation = _cannonBody.localRotation;
		var desiredRotation = rotation.eulerAngles + new Vector3(0, speed * _turnCannonSpeed, 0);
		_cannonBody.localEulerAngles = desiredRotation;
	}

	public void Shoot()
	{
		var bullet = Instantiate(_bulletPrefab);
		bullet.transform.position = _muzzle.position;
		bullet.transform.rotation = _muzzle.rotation;
		bullet.GetComponent<Bullet>().StartMovement();
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
