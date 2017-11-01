using UnityEngine;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class ControllerTest : MonoBehaviour
{
	[SerializeField] private int _maxPlayers = 8;

	private Dictionary<int, TankController> _controllers = new Dictionary<int, TankController>();

	private void OnEnable() {
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
	}

	private void OnDisable() {
		AirConsole.instance.onMessage -= OnMessage;
		AirConsole.instance.onConnect -= OnConnect;
		AirConsole.instance.onDisconnect -= OnDisconnect;
	}

	private void OnMessage(int device_id, JToken data) {
		Debug.Log("From device: " + AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id) + " message: " + data);
        int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		if ((string)data["action"] != null && active_player != -1) 
		{
			HandleAction(active_player, (string)data["action"]);
		}
	}

	private void HandleAction(int playerId, string action)
	{
		var controller = _controllers[playerId];
		if (controller == null) return;

		if (action == "accel")
		{
			controller.Accelerate(1);
		}
		if (action == "reverse")
		{
			controller.Accelerate(-1);
		}
		if (action == "shoot")
		{
			controller.Shoot();
		}
	}

	private void OnConnect(int device_id) {
		if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0) {
			Debug.Log("Connected device:" + device_id + " and player number:" + AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id));
			if (AirConsole.instance.GetControllerDeviceIds().Count >= 2) {
				StartGame ();
			} else {
				//uiText.text = "NEED MORE PLAYERS";
			}
		}
	}

	private void OnDisconnect(int device_id) {
		int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		if (active_player != -1) {
			if (AirConsole.instance.GetControllerDeviceIds ().Count >= 2) {
				StartGame ();
			} else {
				AirConsole.instance.SetActivePlayers (0);
				//uiText.text = "PLAYER LEFT - NEED MORE PLAYERS";
			}
		}
	}

	void StartGame () {
		Debug.Log("Start game");
		AirConsole.instance.SetActivePlayers (_maxPlayers);
		var controllerIds = AirConsole.instance.GetControllerDeviceIds();
		var controller = (TankController)FindObjectOfType(typeof(TankController));
		foreach(var device_id in controllerIds)
		{
			var playerId = AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id);
			_controllers.Add(playerId, controller);
			if (playerId % 2 == 0)
			{
				AirConsole.instance.Message(device_id, new {view = "player-a"});
			}
			else
			{
				AirConsole.instance.Message(device_id, new {view = "player-b"});
			}
			Debug.Log("Connected device:" + device_id + " and player number:" + AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id));
		}
	}
}

