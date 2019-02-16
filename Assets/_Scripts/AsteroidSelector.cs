using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

/* * *
 * The AsteroidSelector class handles the management of which asteroid is currently selected.
 * AsteroidGenerator uses AsteroidSelector class to determine which asteroid will be fired next.
 * * */
public static class AsteroidSelector {

	public delegate void AsteroidSelected(int selectedAsteroidIndex);
	public static event AsteroidSelected OnAsteroidSelected;

	[HideInInspector]
	public static AsteroidTemplate selectedAsteroid;

	// Use AsteroidSelector for initialization
	public static void SetupAsteroidSelector () {
		AsteroidSelector.SelectAsteroid(0);
	}

	public static void SelectAsteroid(int selectedAsteroidIndex)
	{
		AsteroidSelector.selectedAsteroid = Resources.Load("AsteroidTemplates/Asteroid" + selectedAsteroidIndex) as AsteroidTemplate;
		AsteroidSelector.OnAsteroidSelected(selectedAsteroidIndex);

        AnalyticsEvent.Custom("Asteroid_Selected", new Dictionary<string, object> { { "Asteroid_Name", "Asteroid" + selectedAsteroidIndex } });
    }
}
