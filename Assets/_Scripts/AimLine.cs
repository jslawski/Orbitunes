using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* * *
 * The AimLine class handles the behavior of the line that is seen when the player clicks and drags the mouse
 * across the screen.  This class contains all of the initial values that the AsteroidGenerator needs to launch asteroids
 * * */
public class AimLine : MonoBehaviour {

	[Header("Change these values to determine how much force will launch the asteroids")]
	[SerializeField]
	private float launchVectorMaxMagnitude = 5f;
	[SerializeField]
	private float scaleFactor = 2f;

	public delegate void AimLineReleased(LaunchValues launchValues);
	public event AimLineReleased OnAimLineReleased;

	private float curScaledMagnitude = 0;

	private LineRenderer aimLineRenderer;

	private LaunchValues aimLineLaunchValues;

	public void SetupAimLine()
	{
		this.aimLineLaunchValues.startPoint = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		this.SetupAimLineRenderer();
		this.aimLineLaunchValues.endPoint = this.aimLineLaunchValues.startPoint;

		StartCoroutine(this.UpdateDirection());
	}

	private void SetupAimLineRenderer()
	{
		this.aimLineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
		this.aimLineRenderer.SetPosition(0, this.aimLineLaunchValues.startPoint);
		this.aimLineRenderer.SetPosition(0, this.aimLineLaunchValues.startPoint);

		Color lineColor = AsteroidSelector.selectedAsteroid.asteroidColor;
		this.aimLineRenderer.startColor = lineColor;
		this.aimLineRenderer.endColor = lineColor;
	}

	private float GetDistance(Vector2 point1, Vector2 point2)
	{
		return (Mathf.Sqrt(Mathf.Pow((point2.x - point1.x), 2) + Mathf.Pow((point2.y - point1.y), 2)));
	}

	private void UpdateCurrentMagnitude()
	{
		float launchVectorRawMagnitude = this.GetDistance(this.aimLineLaunchValues.startPoint, this.aimLineLaunchValues.endPoint);
		this.curScaledMagnitude = (launchVectorRawMagnitude > this.launchVectorMaxMagnitude) ? this.launchVectorMaxMagnitude : launchVectorRawMagnitude; 
		this.aimLineLaunchValues.rawMagnitude = this.curScaledMagnitude * this.scaleFactor; 
	}

	private IEnumerator UpdateDirection()
	{	
		while (Input.GetMouseButton(0))
		{
			this.aimLineLaunchValues.endPoint = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
			this.aimLineLaunchValues.curDirection = (this.aimLineLaunchValues.endPoint - this.aimLineLaunchValues.startPoint).normalized;

			Vector2 lineRendererEndPoint = this.aimLineLaunchValues.startPoint + (this.aimLineLaunchValues.curDirection * this.curScaledMagnitude);
			this.aimLineRenderer.SetPosition(1, lineRendererEndPoint);

			this.UpdateCurrentMagnitude();

			yield return null;
		}

		this.OnAimLineReleased(this.aimLineLaunchValues);
		Destroy(this.gameObject);
	}
}