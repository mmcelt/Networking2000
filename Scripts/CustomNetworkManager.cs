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

	public int selGridInt;
	public string[] selStrings = new string[] { "Princess", "Brutius", "Jane", "Funky" };

	#endregion

	#region Unity Callbacks

	void OnGUI()
	{
		if (!isNetworkActive)
		{
			selGridInt = GUI.SelectionGrid(new Rect(Screen.width - 200, 10, 200, 50), selGridInt, selStrings, 2);
			playerPrefabIndex = (short)(selGridInt);
		}
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

	public void SwitchPlayer(SetupLocalHumanPlayer player, int cID)
	{
		GameObject newPlayer = Instantiate(spawnPrefabs[cID], player.gameObject.transform.position, player.gameObject.transform.rotation);
		playerPrefab = spawnPrefabs[cID];
		Destroy(player.gameObject);
		NetworkServer.ReplacePlayerForConnection(player.connectionToClient, newPlayer, 0);
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
