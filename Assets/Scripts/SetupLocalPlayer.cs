using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLocalPlayer : NetworkBehaviour
{
	#region Fields

	public Text _namePrefab;
	public Text _nameText;
	public Transform _namePos;

	#endregion

	#region MonoBehaviour Methods

	void Start() 
	{
		if (isLocalPlayer)
		{
			GetComponent<PlayerController>().enabled = true;
			CameraFollow360.player = gameObject.transform;
		}
		else
		{
			GetComponent<PlayerController>().enabled = false;

		}

		MakeThePlayerNameLabel();
	}

	void Update()
	{
		Vector3 namePos = Camera.main.WorldToScreenPoint(_namePos.position);
		_nameText.transform.position = namePos;
	}
	#endregion

	#region Public Methods


	#endregion

	#region Private Methods

	void MakeThePlayerNameLabel()
	{
		GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");
		_nameText = Instantiate(_namePrefab,canvas.transform);
	}
	#endregion
}
