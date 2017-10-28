using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{	
	[SerializeField] private Rigidbody _rigidbody;
	[SerializeField] private float _speed = 25f;
	[SerializeField] private float _lifeTime = 5f;

	public void StartMovement()
	{
		_rigidbody.AddForce(transform.forward * _speed, ForceMode.Impulse);
		Destroy(gameObject, _lifeTime);
	}

	private void OnCollisionEnter(Collision col)
	{
		if (col.transform.CompareTag("Floor")) 
		{
			Destroy(gameObject);
		}
		else if (col.transform.CompareTag("Player"))
		{
			var controller = col.transform.GetComponent<TankController>();
			controller.Damage();
		}
	}
}
