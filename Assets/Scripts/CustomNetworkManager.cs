using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MsgTypes
{
	public const short PlayerPrefabSelect = MsgType.Highest + 1;

	public class PlayerPrefabMsg : MessageBase
	{
		public short controllerID;
		public short prefabIndex;
	}
}

public class CustomNetworkManager : NetworkManager
{
	#region Fields

	public short playerPrefabIndex;

	#endregion

	#region MonoBehaviour Methods

	void Start() 
	{
		
	}
	#endregion

	#region Server Methods

	public override void OnStartServer()
	{
		NetworkServer.RegisterHandler(MsgTypes.PlayerPrefabSelect, OnResponsePrefab);
		base.OnStartServer();
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
		msg.controllerID = playerControllerId;
		NetworkServer.SendToClient(conn.connectionId, MsgTypes.PlayerPrefabSelect, msg);
	}

	void OnResponsePrefab(NetworkMessage netMsg)
	{
		MsgTypes.PlayerPrefabMsg msg = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>();
		playerPrefab = spawnPrefabs[msg.prefabIndex];
		base.OnServerAddPlayer(netMsg.conn, msg.controllerID);
	}
	#endregion

	#region Client Methods

	public override void OnClientConnect(NetworkConnection conn)
	{
		client.RegisterHandler(MsgTypes.PlayerPrefabSelect, OnRequestPrefab);
		base.OnClientConnect(conn);
	}

	void OnRequestPrefab(NetworkMessage netMsg)
	{
		MsgTypes.PlayerPrefabMsg msg = new MsgTypes.PlayerPrefabMsg();
		msg.controllerID = netMsg.ReadMessage<MsgTypes.PlayerPrefabMsg>().controllerID;
		msg.prefabIndex = playerPrefabIndex;
		client.Send(MsgTypes.PlayerPrefabSelect, msg);
	}
	#endregion
}
