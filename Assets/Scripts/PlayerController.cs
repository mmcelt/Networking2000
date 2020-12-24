//adapted from example script available at
//https://docs.unity3d.com/ScriptReference/Input.GetAxis.html
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;

	float _translation, _rotation;

	Rigidbody _theRB;

	void Start()
	{
		_theRB = GetComponent<Rigidbody>();
	}

	// Update is called once per frame
	void Update()
	{

		_translation = Input.GetAxis("Vertical") * speed;
		_rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		_translation *= Time.deltaTime;
		_rotation *= Time.deltaTime;

		//THIS IS FOR SYNCING THE TRANSFORM...
		//transform.Translate(0, 0, translation);
		//transform.Rotate(0, rotation, 0);

		//THIS IS FOR SYNCING THE RIGIBODY...
		Quaternion turn = Quaternion.Euler(0f, _rotation, 0f);
		_theRB.MovePosition(_theRB.position + transform.forward * _translation);
		_theRB.MoveRotation(_theRB.rotation * turn);
	}
}
