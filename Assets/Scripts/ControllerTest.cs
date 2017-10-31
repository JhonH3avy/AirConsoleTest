using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class ControllerTest : MonoBehaviour
{
	private void OnEnable() {
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
	}

	private void OnMessage(int device_id, JToken data) {
        int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		if (active_player != -1) {
			if (active_player == 0) {
				//this.racketLeft.velocity = Vector3.up * (float)data ["move"];
			}
			if (active_player == 1) {
				//this.racketRight.velocity = Vector3.up * (float)data ["move"];
			}
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
		AirConsole.instance.SetActivePlayers (2);
		var controllerIds = AirConsole.instance.GetControllerDeviceIds();
		foreach(var device_id in controllerIds)
		{
			if (AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id) % 2 == 0)
			{
				AirConsole.instance.Message(device_id, new {view = "view-0"});
			}
			else
			{
				AirConsole.instance.Message(device_id, new {view = "view-1"});
			}
			Debug.Log("Connected device:" + device_id + " and player number:" + AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id));
		}
	}
}

