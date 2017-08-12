using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO: Account for hitting buttons.  Don't instantiate an aim line then.

public class AimManager : MonoBehaviour {

	public static AimManager instance;

	private GameObject aimLinePrefab;
	private GameObject asteroidGeneratorPrefab;
	private AimLine aimLine;

	[HideInInspector]
	public bool buttonPressed = false;  //TODO: Change this when we implement a new menu system

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	private void Start()
	{
		this.aimLinePrefab =  Resources.Load("AimLine") as GameObject;
		this.asteroidGeneratorPrefab = Resources.Load("AsteroidGenerator") as GameObject;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.InstantiateAimLine();
		}
	}

	private void InstantiateAimLine()
	{
		if (this.buttonPressed == true)
		{
			this.buttonPressed = false;
			return;
		}

		Vector2 instantiationPoint = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		GameObject currentAimLine = Instantiate(this.aimLinePrefab, instantiationPoint, new Quaternion()) as GameObject;
		this.aimLine = currentAimLine.GetComponent<AimLine>();
		this.aimLine.OnAimLineReleased += CreateAsteroidGenerator;
		this.aimLine.SetupAimLine();
	}

	private void CreateAsteroidGenerator(LaunchValues launchValues)
	{
		GameObject newAsteroidGenerator = Instantiate(this.asteroidGeneratorPrefab, launchValues.startPoint, new Quaternion()) as GameObject;
		AsteroidGenerator generator = newAsteroidGenerator.GetComponent<AsteroidGenerator>();
		generator.SetupAsteroidGenerator(launchValues);
	}
}
