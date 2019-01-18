using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;

public class EditButton : MonoBehaviour {

	[SerializeField]
	private string asteroidToEditName;

	private Button button;

    private bool firstTimeEditButton = false;

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

        if (this.firstTimeEditButton == false)
        {
            AnalyticsEvent.Custom("Edit_Button_Clicked_First", new Dictionary<string, object> { { "Time_Elapsed", Time.timeSinceLevelLoad }, { "Asteroid_Name", asteroidToEditName } });
        }

        AnalyticsEvent.Custom("Edit_Button_Clicked", new Dictionary<string, object> { { "Asteroid_Name", asteroidToEditName } });
    }

}
