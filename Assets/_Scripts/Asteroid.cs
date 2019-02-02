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

    private float maxGravitationalForce = 1500f;
    private float maxVelocity = 90f;

    public bool isStationary = true;

    public void SetupAsteroid(LaunchValues launchValues)
	{
		Vector2 initialVelocity = launchValues.curDirection * launchValues.rawMagnitude;
		this.rigidBody.velocity = initialVelocity;

		GameManager.OnRestartButtonClicked += this.DestroyAsteroid;
	}

	private void FixedUpdate () {
		if (this.isStationary == true)
		{
			return;
		}

		this.radius = Vector3.Magnitude(this.transform.position);
		this.gravitationalForce = -(CenterStar.mass) / (Mathf.Pow(this.radius, 2.0f));

        if (Mathf.Abs(this.gravitationalForce) > this.maxGravitationalForce)
        {
            this.gravitationalForce = -2000f;
        }

		this.appliedForce = (this.transform.position).normalized * this.gravitationalForce;
		this.rigidBody.AddForce(this.appliedForce);

        if (this.rigidBody.velocity.magnitude > this.maxVelocity)
        {
            this.rigidBody.AddForce(this.appliedForce);
        }

        //this.rigidbody.velocity = 0.5f * this.rigidbody.velocity;

		if (this.IsWithinViewport() != true) 
		{
            DestroyAsteroid();
		}
	}

	private bool IsWithinViewport()
	{
		Vector3 viewportPosition = GameManager.instance.mainCamera.WorldToViewportPoint(this.gameObject.transform.position);
		return ((viewportPosition.x > 0) && (viewportPosition.x < 1) && (viewportPosition.y > 0) && (viewportPosition.y < 1));
	}

	private void OnDestroy()
	{
		GameManager.OnRestartButtonClicked -= this.DestroyAsteroid;
	}

	private void DestroyAsteroid()
	{
        Destroy(this.gameObject);
	}
}
