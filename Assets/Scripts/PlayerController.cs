using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	Rigidbody _rb;
	Animator _anim;

	[SerializeField] float _speed = 25.0F;
	[SerializeField] float _rotationSpeed = 50.0F;

	void Start()
	{
		_rb = GetComponent<Rigidbody>();
		_anim = GetComponent<Animator>();
	}

	void FixedUpdate()
	{
		float translation = Input.GetAxis("Vertical") * _speed;
		float rotation = Input.GetAxis("Horizontal") * _rotationSpeed;
		translation *= Time.deltaTime;
		rotation *= Time.deltaTime;
		Quaternion turn = Quaternion.Euler(0f, rotation, 0f);
		_rb.MovePosition(_rb.position + transform.forward * translation);
		_rb.MoveRotation(_rb.rotation * turn);

		//THIS CODE IS FOR THE ANIMATION PART OF THE COURSE ONLY!
		if (translation != 0)
		{
			_anim.SetBool("Idling", false);
			GetComponent<SetupLocalHumanPlayer>().CmdChangeAnimState("run");
		}
		else
		{
			_anim.SetBool("Idling", true);
			GetComponent<SetupLocalHumanPlayer>().CmdChangeAnimState("idle");
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			_anim.SetTrigger("Attacking");
			GetComponent<SetupLocalHumanPlayer>().CmdChangeAnimState("attack");
		}
	}
}
