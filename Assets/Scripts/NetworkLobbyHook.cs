using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
	#region Fields & Properties


	#endregion

	public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
	{
		//base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);
		LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
		SetupLocalPlayer car = gamePlayer.GetComponent<SetupLocalPlayer>();

		//these need to be SyncVar's with hooks...
		car._pName = lobby.playerName;
		car._pColor = ColorUtility.ToHtmlStringRGBA(lobby.playerColor);
	}
}
