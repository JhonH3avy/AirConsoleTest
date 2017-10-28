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
	private void FixedUpdate() {
		if (_isTurningBody) {
        	_tankRigidbody.AddTorque(transform.up * _turnSpeed * _turnDirection, ForceMode.VelocityChange);
		}
    }

	private void Die()
	{
		gameObject.SetActive(false);
	}

	public void Accelerate(float speed)
	{
		var desiredPosition = transform.position + transform.forward * speed * _movementSpeed;
		_tankRigidbody.MovePosition(desiredPosition);
	}

	public void Turn(float speed)
	{
		_isTurningBody = speed != 0;
		_turnDirection = speed;
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
