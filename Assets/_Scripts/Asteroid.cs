using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* * *
 * The Asteroid class handles all physics-based aspects of a launched asteroid.
 * Nothing else should be handled within this class
 * * */
public class Asteroid : MonoBehaviour {
	[SerializeField]
	private Rigidbody rigidBody;

	[HideInInspector]
	public Vector2 initialVelocity = Vector2.zero;
	private float radius = 0;
	private float gravitationalForce = 0;
	private Vector3 appliedForce = Vector3.zero;

	[HideInInspector]
	public float curDistanceFromStar = 0f;
	[HideInInspector]
	public float maxDistanceFromStar = 12f;

	//Cached variables for trailing Asteroids
	[HideInInspector]
	public Vector2 instantiationPoint = Vector2.zero;
	[HideInInspector]
	public Vector2 launchDirection = Vector2.zero;
	[HideInInspector]
	public float launchMagnitude = 0;

	[Header("Modify these numbers for each asteroid type")]
	public int numTrailingAsteroids = 0;
	public float incrementsPerBeat = 0;

	public bool isTrailingAsteroid = false;

	// Use this for initialization
	public virtual void Start () {
		this.rigidBody.velocity = this.initialVelocity;
		this.curDistanceFromStar = this.GetDistance(Vector2.zero, this.transform.position);
	}
	
	void FixedUpdate () {
		this.radius = Vector3.Magnitude(this.transform.position);
		this.gravitationalForce = -(CenterStar.mass) / (Mathf.Pow(this.radius, 2.0f));
		this.appliedForce = (this.transform.position).normalized * this.gravitationalForce;
		this.rigidBody.AddForce(this.appliedForce);

		this.curDistanceFromStar = this.GetDistance(Vector2.zero, this.transform.position);

		if (this.IsWithinViewport() != true) 
		{
			Destroy(this.gameObject);
		}
	}

	public float GetDistance(Vector2 point1, Vector2 point2)
	{
		return (Mathf.Sqrt(Mathf.Pow((point2.x - point1.x), 2) + Mathf.Pow((point2.y - point1.y), 2)));
	}
		
	private bool IsWithinViewport()
	{
		Vector3 viewportPosition = GameManager.instance.mainCamera.WorldToViewportPoint(this.gameObject.transform.position);

		return ((viewportPosition.x > 0) && (viewportPosition.x < 1) && (viewportPosition.y > 0) && (viewportPosition.y < 1));
	}
}
