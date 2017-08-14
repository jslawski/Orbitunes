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

	private float radius = 0;
	private float gravitationalForce = 0;
	private Vector3 appliedForce = Vector3.zero;

	public void SetupAsteroid(LaunchValues launchValues)
	{
		Vector2 initialVelocity = launchValues.curDirection * launchValues.rawMagnitude;
		this.rigidBody.velocity = initialVelocity;

		Restart.OnRestartButtonClicked += this.DestroyAsteroid;
	}

	private void FixedUpdate () {
		this.radius = Vector3.Magnitude(this.transform.position);
		this.gravitationalForce = -(CenterStar.mass) / (Mathf.Pow(this.radius, 2.0f));
		this.appliedForce = (this.transform.position).normalized * this.gravitationalForce;
		this.rigidBody.AddForce(this.appliedForce);

		if (this.IsWithinViewport() != true) 
		{
			Destroy(this.gameObject);
		}
	}

	private bool IsWithinViewport()
	{
		Vector3 viewportPosition = GameManager.instance.mainCamera.WorldToViewportPoint(this.gameObject.transform.position);
		return ((viewportPosition.x > 0) && (viewportPosition.x < 1) && (viewportPosition.y > 0) && (viewportPosition.y < 1));
	}

	private void OnDestroy()
	{
		Restart.OnRestartButtonClicked -= this.DestroyAsteroid;
	}

	private void DestroyAsteroid()
	{
		Destroy(this.gameObject);
	}
}
