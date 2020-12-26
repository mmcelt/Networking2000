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

	#endregion

	#region MonoBehaviour Methods

	void Awake() 
	{
		CC = this;
	}
	
	void Update() 
	{
		
	}
	#endregion

	#region Public Methods

	public void ChangeTopTexture(int i)
	{
		if (myCharacter.name.Contains("Jane"))
			myCharacter.transform.Find("Tops").GetComponent<Renderer>().material.mainTexture = JaneTops[i];
		else if(myCharacter.name.Contains("Brutius"))
			myCharacter.transform.Find("Tops").GetComponent<Renderer>().material.mainTexture = BrutiusTops[i];

		myCharacter.GetComponent<SetupLocalHumanPlayer>().CmdChangeTop(i);
	}

	public static Texture GetTop(int i,string name)
	{
		if (name.Contains("Janie"))
			return (CC.JaneTops[i]);
		else if (name.Contains("Brutius"))
			return (CC.BrutiusTops[i]);

		return null;
	}
	#endregion

	#region Private Methods


	#endregion
}
