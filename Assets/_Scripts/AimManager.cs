using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AimManager : MonoBehaviour, IPointerDownHandler 
{

	public static AimManager instance;

	public delegate void LaunchAreaClicked();
	public static event LaunchAreaClicked OnLaunchAreaClicked;

	private GameObject aimLinePrefab;
	private GameObject asteroidGeneratorPrefab;
	private AimLine aimLine;

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

	public void OnPointerDown(PointerEventData data)
	{
        if (AimManager.OnLaunchAreaClicked != null)
        {
            AimManager.OnLaunchAreaClicked();
        }

		this.InstantiateAimLine();
	}

	private void InstantiateAimLine()
	{
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
