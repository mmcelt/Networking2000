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
		}
		else
		{
			GetComponent<PlayerController>().enabled = false;
		}
	}
	#endregion

	#region Client Methods

	void OnChangeAnimation(string newState)
	{
		if (isLocalPlayer) return;

		UpdateAnimationState(newState);
	}
	#endregion

	#region Server Methods

	[Command]
	public void CmdChangeAnimState(string newState)
	{
		UpdateAnimationState(newState);
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
