using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditButton : MonoBehaviour {

	[SerializeField]
	private string asteroidToEditName;

	private Button button;

	// Use this for initialization
	void Start() 
	{
		this.button = this.gameObject.GetComponent<Button>();
		this.button.onClick.AddListener(LaunchEdit);
	}

	public void LaunchEdit()
	{
		GameManager.instance.OpenAsteroidCreator(asteroidToEditName);
		AsteroidSelector.SelectAsteroid(asteroidToEditName);
	}

}
