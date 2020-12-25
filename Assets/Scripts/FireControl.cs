using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FireControl : NetworkBehaviour
{
	#region Fields

	public GameObject _bulletPrefab;
	public Transform _firePoint;
	[SerializeField] float _fireStrength = 2000f;

	#endregion

	#region MonoBehaviour Methods

	[ClientCallback]
	void Update() 
	{
		if (!isLocalPlayer) return;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdShoot();
		}
	}
	#endregion

	#region Server Methods

	[Command]
	void CmdShoot()
	{
		GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
		//bullet.GetComponent<Rigidbody>().AddForce(_firePoint.forward * _fireStrength);
		bullet.GetComponent<Rigidbody>().velocity = _firePoint.forward *_fireStrength;

		NetworkServer.Spawn(bullet);

		Destroy(bullet, 3.0f);
	}
	#endregion

	#region Private Methods


	#endregion
}
