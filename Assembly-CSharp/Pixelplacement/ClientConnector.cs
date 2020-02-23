using System.Collections.Generic;
using UnityEngine;

namespace Pixelplacement
{
	public class ClientConnector : MonoBehaviour
	{
		private class AvailableServer
		{
			public string ip;

			public int port;

			public string deviceID;

			public AvailableServer(string ip, int port, string deviceID)
			{
				this.ip = ip;
				this.port = port;
				this.deviceID = deviceID;
			}
		}

		[SerializeField]
		private bool _connectToFirstAvailable;

		[SerializeField]
		private string _requiredDeviceId;

		private List<AvailableServer> _availableServers = new List<AvailableServer>();

		private bool _cleanUp;

		private float _lastCleanUpTime;

		private float _cleanUpTimeout = 4f;

		private float _cleanUpTimeoutBackup = 0.5f;

		private void Awake()
		{
			Client.OnConnected += HandleConnected;
			Client.OnDisconnected += HandleDisconnected;
			Client.OnServerAvailable += HandleServerAvailable;
			HandleDisconnected();
		}

		private void CleanUp()
		{
			if (Time.realtimeSinceStartup - _lastCleanUpTime > _cleanUpTimeout + _cleanUpTimeoutBackup)
			{
				_lastCleanUpTime = Time.realtimeSinceStartup;
				_availableServers.Clear();
			}
			else
			{
				_cleanUp = true;
			}
		}

		private void HandleConnected()
		{
			CancelInvoke("CleanUp");
		}

		private void HandleDisconnected()
		{
			InvokeRepeating("CleanUp", 0f, _cleanUpTimeout);
		}

		private void HandleServerAvailable(ServerAvailableMessage message)
		{
			if (string.IsNullOrEmpty(_requiredDeviceId) || !(message.deviceId != _requiredDeviceId))
			{
				if (_connectToFirstAvailable)
				{
					Client.Connect(message.ip, message.port);
				}
				if (_cleanUp)
				{
					_cleanUp = false;
					_lastCleanUpTime = Time.realtimeSinceStartup;
					_availableServers.Clear();
				}
				foreach (AvailableServer availableServer in _availableServers)
				{
					if (availableServer.ip == message.ip)
					{
						return;
					}
				}
				_availableServers.Add(new AvailableServer(message.ip, message.port, message.deviceId));
			}
		}

		private void OnGUI()
		{
			if (!Client.Connected)
			{
				if (_availableServers.Count == 0)
				{
					GUILayout.Label("Waiting for servers...");
				}
				else if (!_connectToFirstAvailable)
				{
					GUILayout.Label("Select a server:");
					foreach (AvailableServer availableServer in _availableServers)
					{
						if (GUILayout.Button(availableServer.deviceID, GUILayout.ExpandWidth(expand: false)))
						{
							Client.Connect(availableServer.ip, availableServer.port);
						}
					}
				}
			}
		}
	}
}
