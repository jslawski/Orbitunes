using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* * *
 * The AsteroidSelector class handles the management of which asteroid is currently selected.
 * AsteroidGenerator uses this class to determine which asteroid will be fired next.
 * * */
public class AsteroidSelector : MonoBehaviour {

	public static AsteroidSelector instance;

	public Dictionary<string, GameObject> asteroidMasterDict;

	[HideInInspector]
	public GameObject selectedAsteroid;
	[HideInInspector]
	public List<string> asteroidNames = new List<string> { "QuarterNote", "EighthNote", "SixteenthNote", "BassSixteenthNote" };

	// Use this for initialization
	void Awake () {
		if (instance == null)
		{
			instance = this;
		}

		this.LoadAsteroidMasterDict();
		NoteButton.OnNoteSelected += this.SelectAsteroid;
		this.selectedAsteroid = this.asteroidMasterDict["QuarterNote"];
	}

	private void LoadAsteroidMasterDict()
	{
		this.asteroidMasterDict = new Dictionary<string, GameObject>();

		foreach (string asteroidName in this.asteroidNames)
		{
			GameObject asteroidPrefab = Resources.Load("Asteroids/" + asteroidName) as GameObject;
			this.asteroidMasterDict.Add(asteroidName, asteroidPrefab);
		}
	}

	private void SelectAsteroid(string selectedAsteroidName)
	{
		AsteroidGenerator.instance.buttonPressed = true;
		this.selectedAsteroid = Resources.Load("Asteroids/" + selectedAsteroidName) as GameObject;
	}
}
