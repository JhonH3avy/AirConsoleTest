using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
	[SerializeField] private TankController _controller;

	private void Update()
	{
		if (Input.GetButton("Vertical"))
		{			
			var speed = Input.GetAxisRaw("Vertical");
			_controller.Accelerate(speed);
		}
		if (Input.GetButtonUp("Vertical"))
		{			
			return;
			_controller.Accelerate(0);
		}
		if (Input.GetButton("Horizontal"))
		{			
			var speed = Input.GetAxisRaw("Horizontal");
			_controller.Turn(speed);
		}
		if (Input.GetButtonUp("Horizontal"))
		{			
			return;
			_controller.Turn(0);
		}
		if (Input.GetKey(KeyCode.H))
		{			
			var speed = -1;
			_controller.TurnCannon(speed);
		}
		if (Input.GetKey(KeyCode.J))
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
