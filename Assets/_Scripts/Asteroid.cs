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

	private float gravitationalForce = 0;
	private Vector3 appliedForce = Vector3.zero;

    private float maxGravitationalForce = 3000f;
    private float maxVelocity = 120f;

    public bool isStationary = true;

    public void SetupAsteroid(LaunchValues launchValues)
	{
        Vector2 initialVelocity = launchValues.curDirection * launchValues.rawMagnitude;
		this.rigidBody.velocity = initialVelocity;

		GameManager.OnRestartButtonClicked += this.DestroyAsteroid;
	}

    private float GetDistance(Vector2 point1, Vector2 point2)
    {
        return (Mathf.Sqrt(Mathf.Pow((point2.x - point1.x), 2) + Mathf.Pow((point2.y - point1.y), 2)));
    }

    //Iterate through each star in the scene to get the cumulative gravitational force applied to the asteroid
    private Vector3 GetTotalGravitationalForce()
    {
        Vector3 totalAppliedForce = Vector3.zero;

        for (int i = 0; i < GameManager.instance.stars.Count; i++)
        {
            float radius = this.GetDistance(this.transform.position, GameManager.instance.stars[i].starTransform.position);
            Vector3 direction = (GameManager.instance.stars[i].starTransform.position - this.transform.position).normalized;
            float starGravitationalForce = GameManager.instance.stars[i].starMass / Mathf.Pow(radius, 2.0f);

            if (Mathf.Abs(starGravitationalForce) > this.maxGravitationalForce)
            {
                starGravitationalForce = 3000;
            }

            totalAppliedForce += (direction * starGravitationalForce);
        }

        return totalAppliedForce;
    }

	private void FixedUpdate () {
		if (this.isStationary == true)
		{
			return;
		}

        /*this.radius = Vector3.Magnitude(this.transform.position);
		this.gravitationalForce = -(CenterStar.mass) / (Mathf.Pow(this.radius, 2.0f));

        if (Mathf.Abs(this.gravitationalForce) > this.maxGravitationalForce)
        {
            this.gravitationalForce = -3000;
        }
        */
        this.appliedForce = this.GetTotalGravitationalForce();//(this.transform.position).normalized * this.gravitationalForce;
		this.rigidBody.AddForce(this.appliedForce);

        if (this.rigidBody.velocity.magnitude > this.maxVelocity)
        {
            this.rigidBody.velocity = this.rigidBody.velocity.normalized * this.maxVelocity;
        }

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
