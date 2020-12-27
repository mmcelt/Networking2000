using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCustomizer : MonoBehaviour
{
	#region Fields

	static CharacterCustomizer CC;
	public static GameObject myCharacter;
	public Texture[] JaneTops;
	public Texture[] BrutiusTops;
	public int currentTop;

	#endregion

	#region MonoBehaviour Methods

	void Awake() 
	{
		CC = this;
	}
	#endregion

	#region Public Methods

	public void ChangeTopTexture(int i)	//called from the Dropdown
	{
		Debug.Log(i);
		Debug.Log(myCharacter.name);

		if (myCharacter.name.Contains("Jane"))
		{
			myCharacter.transform.Find("Tops").GetComponent<Renderer>().material.mainTexture = JaneTops[i];

			Debug.Log(JaneTops[i].name);
		}
		else if (myCharacter.name.Contains("Brutius"))
		{
			myCharacter.transform.Find("Tops").GetComponent<Renderer>().material.mainTexture = BrutiusTops[i];
		}

		myCharacter.GetComponent<SetupLocalHumanPlayer>().CmdChangeTop(i);
		CC.currentTop = i;
	}

	public static Texture GetTop(int i,string name)
	{
		if (name.Contains("Janie"))
			return (CC.JaneTops[i]);
		else if (name.Contains("Brutius"))
			return (CC.BrutiusTops[i]);

		return null;
	}

	public static int GetTopId()
	{
		return CC.currentTop;
	}
	#endregion

	#region Private Methods


	#endregion
}
