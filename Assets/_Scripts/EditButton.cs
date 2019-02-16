using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class EditButton : MonoBehaviour {

	[SerializeField]
	private int asteroidToEditIndex;

	private Button button;

	// Use this for initialization
	void Start() 
	{
		this.button = this.gameObject.GetComponent<Button>();
		this.button.onClick.AddListener(LaunchEdit);
	}

	public void LaunchEdit()
	{
        AsteroidSelector.SelectAsteroid(asteroidToEditIndex);
        GameManager.instance.OpenAsteroidCreator();
		
        AnalyticsEvent.Custom("Edit_Button_Clicked", new Dictionary<string, object> { { "Asteroid_Name", AsteroidSelector.selectedAsteroid.asteroidName } });
    }

}
