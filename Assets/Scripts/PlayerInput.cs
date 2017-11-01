using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	[SerializeField] private TankController _controller;

	private void Update()
	{
		if (Input.GetButtonDown("Vertical"))
		{			
			var speed = Input.GetAxisRaw("Vertical");
			_controller.Accelerate(speed);
		}
		if (Input.GetButtonDown("Horizontal"))
		{			
			var speed = Input.GetAxisRaw("Horizontal");
			_controller.Turn(speed);
		}
		if (Input.GetKeyDown(KeyCode.H))
		{			
			var speed = -1;
			_controller.TurnCannon(speed);
		}
		if (Input.GetKeyDown(KeyCode.J))
		{			
			var speed = 1;
			_controller.TurnCannon(speed);
		}
		if (Input.GetButtonDown("Fire1"))
		{
			_controller.Shoot();
		}
	}
}
