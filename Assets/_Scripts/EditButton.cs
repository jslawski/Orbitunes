using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditButton : MonoBehaviour {

	string asteroidToEditName;

	private Button button;

	// Use this for initialization
	void Start() 
	{
		this.asteroidToEditName = this.gameObject.GetComponentInParent<NoteButton>().asteroidName;

		this.button = this.gameObject.GetComponent<Button>();
		this.button.onClick.AddListener(LaunchEdit);
	}

	public void LaunchEdit()
	{
		GameObject asteroidToEdit = Resources.Load("Asteroids/" + this.asteroidToEditName) as GameObject;
		GameManager.instance.OpenAsteroidCreator(asteroidToEdit);
	}

}
