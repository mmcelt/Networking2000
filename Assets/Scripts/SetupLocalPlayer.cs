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

	string _textboxName = "";

	[SyncVar(hook = nameof(OnChangeName))]
	public string pName = "";

	#endregion

	#region MonoBehaviour Methods

	void OnGUI()
	{
		if (isLocalPlayer)
		{
			_textboxName = GUI.TextField(new Rect(25, 15, 100, 25), _textboxName);
			
			if(GUI.Button(new Rect(130, 15, 35, 25), "Set"))
			{
				CmdChangeName(_textboxName);
			}
		}
	}

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

	#region Server Methods

	[Command]
	void CmdChangeName(string newName)
	{
		pName = newName;
		_nameText.text = pName;
	}
	#endregion

	#region Client Methods

	void MakeThePlayerNameLabel()
	{
		GameObject canvas = GameObject.FindGameObjectWithTag("MainCanvas");
		_nameText = Instantiate(_namePrefab, canvas.transform);
	}

	void OnChangeName(string newName)
	{
		_nameText.text = newName;
	}
	#endregion
}
