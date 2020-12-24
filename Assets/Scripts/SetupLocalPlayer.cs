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

	string _textboxName;
	string _colorboxName;

	[SyncVar(hook = nameof(OnChangeName))]
	public string _pName = "";

	[SyncVar(hook = nameof(OnChangeColor))]
	public string _pColor = "";

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

			_colorboxName = GUI.TextField(new Rect(170, 15, 100, 25), _colorboxName);

			if (GUI.Button(new Rect(275, 15, 35, 25), "Set"))
				CmdChangeColor(_colorboxName);
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
		_pName = newName;
		_nameText.text = _pName;
	}

	[Command]
	void CmdChangeColor(string newColor)
	{
		_pColor = newColor;
		Renderer[] renderers = GetComponentsInChildren<Renderer>();

		foreach (Renderer renderer in renderers)
		{
			if (renderer.gameObject.name == "BODY")
				renderer.material.SetColor("_Color", ColorFromHex(_pColor));
		}
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

	void OnChangeColor(string newColor)
	{
		_pColor = newColor;
		Renderer[] renderers = GetComponentsInChildren<Renderer>();

		foreach(Renderer renderer in renderers)
		{
			if (renderer.gameObject.name == "BODY")
				renderer.material.SetColor("_Color", ColorFromHex(_pColor));
		}
	}

	//Credit for this method: from http://answers.unity3d.com/questions/812240/convert-hex-int-to-colorcolor32.html
	//hex for testing green: 04BF3404  red: 9F121204  blue: 221E9004
	Color ColorFromHex(string hex)
	{
		hex = hex.Replace("0x", "");
		hex = hex.Replace("#", "");
		byte a = 255;
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		if (hex.Length == 8)
		{
			a = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		}
		return new Color32(r, g, b, a);
	}
	#endregion
}
