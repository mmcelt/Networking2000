using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SetupLocalPlayer : NetworkBehaviour
{
	public Text _namePrefab;
	public Text _nameLabel;
	public Transform _namePos;

	public Slider _healthbarPrefab;
	public Slider _healthbar;

	public GameObject _explosionPrefab;

	NetworkStartPosition[] _spawnPositions;

	string _textboxName = "";
	string _colorboxName = "";
	
	[SyncVar(hook = nameof(OnChangeName))]
	public string _pName = "player";

	[SyncVar(hook = nameof(OnChangeColor))]
	public string _pColor = "#ffffff";

	[SyncVar(hook = nameof(OnChangeHealth))]
	public int _healthValue = 100;

	#region Client Methods

	public override void OnStartClient()
	{
		base.OnStartClient();
		Invoke(nameof(UpdateStates), 0.5f);
	}

	void UpdateStates()
	{
		OnChangeName(_pName);
		OnChangeColor(_pColor);
		OnChangeHealth(100);
	}

	void OnChangeName(string n)
	{
		_pName = n;
		_nameLabel.text = _pName;
	}

	void OnChangeColor(string n)
	{
		_pColor = n;
		Renderer[] rends = GetComponentsInChildren<Renderer>();

		foreach (Renderer r in rends)
		{
			if (r.gameObject.name == "BODY")
				r.material.SetColor("_Color", ColorFromHex(_pColor));
		}
	}

	void OnChangeHealth(int newHealth)
	{
		if (_healthbar == null) return;

		_healthValue = newHealth;
		_healthbar.value = _healthValue;
	}

	[ClientRpc]
	public void RpcRespawn()
	{
		if (!isLocalPlayer) return;

		if (_spawnPositions != null && _spawnPositions.Length > 0)
		{
			transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Length)].transform.position;
		}
	}
	#endregion

	#region Server Methods

	[Command]
	public void CmdChangeName(string newName)
	{
		_pName = newName;
		_nameLabel.text = _pName;
	}

	[Command]
	public void CmdChangeColor(string newColour)
	{
		_pColor = newColour;
		Renderer[] rends = GetComponentsInChildren<Renderer>();

		foreach (Renderer r in rends)
		{
			if (r.gameObject.name == "BODY")
				r.material.SetColor("_Color", ColorFromHex(_pColor));
		}
	}

	[Command]
	public void CmdChangeHealth(int amount)
	{
		_healthValue += amount;

		_healthbar.value = _healthValue;

		if (_healthValue <= 0)
		{
			GameObject explosion = Instantiate(_explosionPrefab, transform.position + new Vector3(0, 4, 0), Quaternion.identity);
			NetworkServer.Spawn(explosion);
			Destroy(explosion, 2f);

			GetComponent<Rigidbody>().velocity = Vector3.zero;
			RpcRespawn();
			_healthValue = 100;
		}
	}
	#endregion

	#region Unity Methods

	void OnGUI()
	{
		if (isLocalPlayer)
		{
			_textboxName = GUI.TextField(new Rect(25, 15, 100, 25), _textboxName);
			if (GUI.Button(new Rect(130, 15, 35, 25), "Set"))
				CmdChangeName(_textboxName);

			_colorboxName = GUI.TextField(new Rect(170, 15, 100, 25), _colorboxName);
			if (GUI.Button(new Rect(275, 15, 35, 25), "Set"))
				CmdChangeColor(_colorboxName);
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

	void Awake()
	{
		GameObject canvas = GameObject.FindWithTag("MainCanvas");

		_nameLabel = Instantiate(_namePrefab, canvas.transform);

		if (_healthbar != null)
			_healthbar = Instantiate(_healthbarPrefab, canvas.transform);
	}

	void Start()
	{
		if (isLocalPlayer)
		{
			GetComponent<MyPlayerController>().enabled = true;
			CameraFollow360.player = gameObject.transform;
		}
		else
		{
			GetComponent<MyPlayerController>().enabled = false;
		}

		_spawnPositions = FindObjectsOfType<NetworkStartPosition>();
	}

	public void OnDestroy()
	{
		if (_nameLabel != null)
			Destroy(_nameLabel.gameObject);
		if (_healthbar != null)
			Destroy(_healthbar.gameObject);
	}

	void Update()
	{
		//determine if the object is inside the camera's viewing volume
		if (_nameLabel != null)
		{
			Vector3 screenPoint = Camera.main.WorldToViewportPoint(this.transform.position);
			bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 &&
								 screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
			//if it is on screen draw its label attached to is name position
			if (onScreen)
			{
				Vector3 nameLabelPos = Camera.main.WorldToScreenPoint(_namePos.position);
				_nameLabel.transform.position = nameLabelPos;
				//_healthbar.transform.position = nameLabelPos + new Vector3(0, 35, 0);
			}
			else //otherwise draw it WAY off the screen 
			{
				_nameLabel.transform.position = new Vector3(-1000, -1000, 0);
				//_healthbar.transform.position = new Vector3(-1000, -1000, 0);
			}
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if (isLocalPlayer && other.gameObject.CompareTag("Bullet"))
		{
			Destroy(other.gameObject);

			CmdChangeHealth(-10);
		}
	}
	#endregion
}
