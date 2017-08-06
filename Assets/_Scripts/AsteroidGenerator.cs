using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* * *
 * The AsteroidGenerator class should only handle the behavior of launching an asteroid
 * as well as its accompanying trailing asteroids.  Relies on AsteroidSelector in order to determine
 * which asteroid to launch, as well as it's currently instantiated AimLine, which provides physics values
 * needed for the launch.
 * * */
public class AsteroidGenerator : MonoBehaviour 
{
	public static AsteroidGenerator instance;

	[HideInInspector]
	public bool queuedToLaunch = false;
	[HideInInspector]
	public bool buttonPressed = false;

	[HideInInspector]
	public float curScaledMagnitude = 0;
	private float curRawMagnitude = 0;

	[SerializeField]
	private float launchVectorMaxMagnitude = 5f;

	[SerializeField]
	private float scaleFactor = 2f;

	private GameObject aimLinePrefab;
	private AimLine aimLine;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}

		PitchManager.OnStep += this.Step;
		this.aimLinePrefab =  Resources.Load("AimLine") as GameObject;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.InstantiateAimLine();
		}
	}
		
	public void UpdateCurrentMagnitude()
	{
		float launchVectorRawMagnitude = this.GetDistance(this.aimLine.startPoint, this.aimLine.endPoint);
		this.curScaledMagnitude = (launchVectorRawMagnitude > this.launchVectorMaxMagnitude) ? this.launchVectorMaxMagnitude : launchVectorRawMagnitude; 
		this.curRawMagnitude = this.curScaledMagnitude * this.scaleFactor; 
	}

	private float GetDistance(Vector2 point1, Vector2 point2)
	{
		return (Mathf.Sqrt(Mathf.Pow((point2.x - point1.x), 2) + Mathf.Pow((point2.y - point1.y), 2)));
	}

	private void LaunchAsteroid()
	{
		if (this.buttonPressed == true) 
		{
			this.buttonPressed = false;
			this.queuedToLaunch = false;
			return;
		}

		this.aimLine.initialVelocity = this.aimLine.curDirection * this.curRawMagnitude;

		GameObject asteroidObject = GameObject.Instantiate(AsteroidSelector.instance.selectedAsteroid, this.aimLine.startPoint, new Quaternion()) as GameObject;
		Asteroid newAsteroid = asteroidObject.GetComponent<Asteroid>();
		newAsteroid.initialVelocity = this.aimLine.initialVelocity;
		newAsteroid.instantiationPoint = this.aimLine.startPoint;
		newAsteroid.launchDirection = this.aimLine.curDirection;
		newAsteroid.launchMagnitude = this.curRawMagnitude;

		this.queuedToLaunch = false;

		GameManager.instance.allAsteroids.Add(asteroidObject);

		this.StartCoroutine(this.GenerateTrailingAsteroids(newAsteroid));
	}

	private IEnumerator GenerateTrailingAsteroids(Asteroid initialAsteroid)
	{
		for (int i = 0; i < initialAsteroid.numTrailingAsteroids; i++) {
			yield return new WaitForSeconds(PitchManager.instance.secondsBetweenBeats / initialAsteroid.incrementsPerBeat);
			this.LaunchTrailingAsteroid(initialAsteroid);
		}
	}

	private void LaunchTrailingAsteroid(Asteroid trailingAsteroid)
	{
		Vector2 launchVelocity = trailingAsteroid.launchDirection * trailingAsteroid.launchMagnitude;

		GameObject asteroidObject = GameObject.Instantiate(AsteroidSelector.instance.selectedAsteroid, trailingAsteroid.instantiationPoint, new Quaternion()) as GameObject;
		Asteroid newAsteroid = asteroidObject.GetComponent<Asteroid>();
		newAsteroid.initialVelocity = launchVelocity;
		newAsteroid.isTrailingAsteroid = true;

		GameManager.instance.allAsteroids.Add(asteroidObject);
	}

	private void InstantiateAimLine()
	{
		Vector2 instantiationPoint = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		GameObject currentAimLine = Instantiate(this.aimLinePrefab, instantiationPoint, new Quaternion()) as GameObject;
		this.aimLine = currentAimLine.GetComponent<AimLine>();
		this.aimLine.SetupAimLine();
	}

	private void Step()
	{
		if (this.queuedToLaunch == true) 
		{
			this.LaunchAsteroid();
		}
	}
}
