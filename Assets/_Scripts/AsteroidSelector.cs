using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* * *
 * The AsteroidSelector class handles the management of which asteroid is currently selected.
 * AsteroidGenerator uses AsteroidSelector class to determine which asteroid will be fired next.
 * * */
public static class AsteroidSelector {

	public static Dictionary<string, GameObject> asteroidMasterDict;

	public delegate void AsteroidSelected(string selectedAsteroidName);
	public static event AsteroidSelected OnAsteroidSelected;

	[HideInInspector]
	public static GameObject selectedAsteroid;
	[HideInInspector]
	public static List<string> asteroidNames = new List<string> { "QuarterNote", "EighthNote", "SixteenthNote", "BassSixteenthNote" };

	// Use AsteroidSelector for initialization
	public static void SetupAsteroidSelector () {
		AsteroidSelector.LoadAsteroidMasterDict();
		AsteroidSelector.SelectAsteroid("QuarterNote");
	}

	private static void LoadAsteroidMasterDict()
	{
		AsteroidSelector.asteroidMasterDict = new Dictionary<string, GameObject>();

		foreach (string asteroidName in AsteroidSelector.asteroidNames)
		{
			GameObject asteroidPrefab = GameObject.Find(asteroidName);
			AsteroidSelector.asteroidMasterDict.Add(asteroidName, asteroidPrefab);
		}
	}

	public static void SelectAsteroid(string selectedAsteroidName)
	{
		AsteroidSelector.selectedAsteroid = AsteroidSelector.asteroidMasterDict[selectedAsteroidName];
		AsteroidSelector.OnAsteroidSelected(selectedAsteroidName);
	}
}
