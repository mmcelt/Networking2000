using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalHumanPlayer: NetworkBehaviour
{
	#region Fields

	Animator _anim;

	[SyncVar(hook = nameof(OnChangeAnimation))]
	public string _animState;

	[SyncVar(hook = nameof(OnChangeTop))]
	public int tID;

	#endregion

	#region MonoBehaviour Methods

	void Start() 
	{
		_anim = GetComponent<Animator>();
		_anim.SetBool("Idling", true);

		if (isLocalPlayer)
		{
			GetComponent<PlayerController>().enabled = true;
			CameraFollow360.player = gameObject.transform;
			CharacterCustomizer.myCharacter = gameObject;
			CmdChangeTop(0);
		}
		else
		{
			GetComponent<PlayerController>().enabled = false;
		}
	}

	void Update()
	{
		if (!isLocalPlayer) return;

		int charID = -1;
		if (Input.GetKeyDown(KeyCode.Alpha1))
			charID = 0;
		if (Input.GetKeyDown(KeyCode.Alpha2))
			charID = 1;
		if (Input.GetKeyDown(KeyCode.Alpha3))
			charID = 2;
		if (Input.GetKeyDown(KeyCode.Alpha4))
			charID = 3;

		if (charID == -1) return;

		CmdUpdatePlayerCharacter(charID);
	}

	#endregion

	#region Client Methods

	void OnChangeAnimation(string newState)
	{
		if (isLocalPlayer) return;

		UpdateAnimationState(newState);
	}

	void OnChangeTop(int t)
	{
		if (isLocalPlayer) return;

		tID = t;
		transform.Find("Tops").GetComponent<Renderer>().material.mainTexture = CharacterCustomizer.GetTop(tID, name);
	}
	#endregion

	#region Server Methods

	[Command]
	public void CmdChangeAnimState(string newState)
	{
		UpdateAnimationState(newState);
	}

	[Command]
	public void CmdUpdatePlayerCharacter(int cId)
	{
		NetworkManager.singleton.GetComponent<CustomNetworkManager>().SwitchPlayer(this, cId);
	}

	[Command]
	public void CmdChangeTop(int i)
	{
		tID = i;
		transform.Find("Tops").GetComponent<Renderer>().material.mainTexture = CharacterCustomizer.GetTop(tID, name);
	}
	#endregion

	#region Common Private Methods
	void UpdateAnimationState(string newState)
	{
		if (_animState == newState) return;

		_animState = newState;

		if (_animState == "idle")
			_anim.SetBool("Idling", true);
		else if (_animState == "run")
			_anim.SetBool("Idling", false);
		else if (_animState == "attack")
			_anim.SetTrigger("Attacking");
	}
	#endregion
}
