using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireControl : MonoBehaviour
{
	#region Fields

	public GameObject _bulletPrefab;
	public Transform _firePoint;
	[SerializeField] float _fireStrength = 2000f;

	#endregion

	#region MonoBehaviour Methods

	void Update() 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);
			bullet.GetComponent<Rigidbody>().AddForce(_firePoint.forward * _fireStrength);

			Destroy(bullet, 3.0f);
		}
	}
	#endregion

	#region Public Methods


	#endregion

	#region Private Methods


	#endregion
}
