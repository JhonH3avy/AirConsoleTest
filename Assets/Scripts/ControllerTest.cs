using UnityEngine;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using Cinemachine;

public class ControllerTest : MonoBehaviour
{
    [SerializeField] private int _maxPlayers = 8;
    [SerializeField] private int _minPlayers = 4;

    [SerializeField] private GameObject _tankPrefab;

    [SerializeField] private CinemachineTargetGroup _targetGroup;
    [SerializeField] private CinemachineVirtualCamera _mainCamera;

    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private Material[] _tankMaterials;

    private List<TankController> _tankControllers = new List<TankController>();
    private List<CinemachineTargetGroup.Target> _tankTargets = new List<CinemachineTargetGroup.Target>();

	private Dictionary<int, TankController> _controllersDictionary = new Dictionary<int, TankController>();

    private int _connectedDevices = 0;

	private void OnEnable()
    {
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
	}

	private void OnDisable()
    {
		AirConsole.instance.onMessage -= OnMessage;
		AirConsole.instance.onConnect -= OnConnect;
		AirConsole.instance.onDisconnect -= OnDisconnect;

        _mainCamera.m_Priority = 10;
    }

	private void OnMessage(int device_id, JToken data)
    {
		Debug.Log("From device: " + AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id) + " message: " + data);
        int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);
		if ((string)data["action"] != null && active_player != -1) 
		{
			HandleAction(active_player, (string)data["action"]);
		}
	}

	private void HandleAction(int playerId, string action)
	{
		var controller = _controllersDictionary[playerId];
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
        if(action == "tank-left")
        {
            controller.Turn(-1);
        }
        if (action == "tank-right")
        {
            controller.Turn(1);
        }
        if (action == "cannon-left")
        {
            controller.TurnCannon(-1);
        }
        if (action == "cannon-right")
        {
            controller.TurnCannon(1);
        }
    }

    private void OnConnect(int device_id)
    {
        _connectedDevices++;

        if (AirConsole.instance.GetActivePlayerDeviceIds.Count == 0)
        {
			Debug.Log("Connected device:" + device_id + " and player number:" + AirConsole.instance.ConvertDeviceIdToPlayerNumber(device_id));
			if (AirConsole.instance.GetControllerDeviceIds().Count >= _minPlayers && _connectedDevices % 2 == 0)
            {
				StartGame ();
			} else {
				//uiText.text = "NEED MORE PLAYERS";
			}
		}
	}

	private void OnDisconnect(int device_id)
    {
        _connectedDevices--;

        var playerCount = AirConsole.instance.GetControllerDeviceIds().Count;
        int active_player = AirConsole.instance.ConvertDeviceIdToPlayerNumber (device_id);

        Debug.Log("Device " + device_id + " has disconected. Players left: " + playerCount);

        _controllersDictionary.Remove(device_id);

		if (active_player != -1)
        {
			if (playerCount >= _minPlayers)
            {
                Debug.Log("There are enough players to continue");
				StartGame ();
			} else
            {
                // No one is a player
                Debug.Log("There are not enough players to continue. Setting players to none");
                // Cleaning dictionary cache
                _controllersDictionary.Clear();
                // Destroy current tanks
                foreach (var tank in _tankControllers)
                {
                    Destroy(tank.gameObject);
                }
                // Set active players to cero (no players)
                AirConsole.instance.SetActivePlayers (0);
			}
		}
	}

	private void StartGame () {
		Debug.Log("Start game");

		AirConsole.instance.SetActivePlayers (_maxPlayers);

        CreateTanks();

		var controllerIds = AirConsole.instance.GetActivePlayerDeviceIds;		

        Debug.Log("Starting game with " + controllerIds.Count);

        for (int i = 0; i < controllerIds.Count; i++)
		{
            var controller = _tankControllers[i / 2];
            var playerId = AirConsole.instance.ConvertDeviceIdToPlayerNumber(controllerIds[i]);
			_controllersDictionary.Add(playerId, controller);
			if (playerId % 2 == 0)
			{
				AirConsole.instance.Message(controllerIds[i], new {view = "player-a"});
			}
			else
			{
				AirConsole.instance.Message(controllerIds[i], new {view = "player-b"});
			}
			Debug.Log("Connected device:" + controllerIds[i] + " and player number:" + AirConsole.instance.ConvertDeviceIdToPlayerNumber(controllerIds[i]));
		}
	}

    private void CreateTanks()
    {
        var controllerIds = AirConsole.instance.GetActivePlayerDeviceIds;
        for (int i = 0; i < controllerIds.Count / 2; i++)
        {
            var tank = Instantiate(_tankPrefab);

            var renderers = tank.GetComponentsInChildren<MeshRenderer>();
            if (renderers.Length == 0)
            {
                Debug.LogWarning("There are no MeshRenderers in the gameObject");
            }
            foreach (var renderer in renderers)
            {
                var materials = renderer.materials;
                materials[0] = _tankMaterials[i];
                renderer.materials = materials;
            }

            var tankController = tank.GetComponent<TankController>();

            var position = _spawnPoints[i].position;
            var rotation = _spawnPoints[i].rotation;

            tank.transform.position = position;
            tank.transform.rotation = rotation;

            _tankControllers.Add(tankController);

            var target = new CinemachineTargetGroup.Target();
            target.target = tank.transform;
            target.weight = 1;
            target.radius = 0;

            _tankTargets.Add(target);

            _targetGroup.m_Targets = _tankTargets.ToArray();

            _mainCamera.m_Priority = 20;
        }
    }
}

