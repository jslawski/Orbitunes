using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* * *
 * The AimLine class handles the behavior of the line that is seen when the player clicks and drags the mouse
 * across the screen.  As the line updates every frame, it also updates the physics variables used by the AsteroidGenerator class
 * * */
public class AimLine : MonoBehaviour {

	private LineRenderer aimLineRenderer;

	[HideInInspector]
	public Vector2 startPoint = Vector2.zero;
	[HideInInspector]
	public Vector2 endPoint = Vector2.zero;
	[HideInInspector]
	public Vector2 curDirection = Vector2.zero;
	[HideInInspector]
	public Vector2 initialVelocity = Vector2.zero;

	private void SetupAimLineRenderer()
	{
		this.aimLineRenderer = gameObject.GetComponentInChildren<LineRenderer>();
		this.aimLineRenderer.SetPosition(0, this.startPoint);
		this.aimLineRenderer.SetPosition(0, this.startPoint);

		Color lineColor = AsteroidSelector.instance.selectedAsteroid.GetComponentInChildren<Note>().particleColor;
		this.aimLineRenderer.startColor = lineColor;
		this.aimLineRenderer.endColor = lineColor;
	}

	public void SetupAimLine()
	{
		this.startPoint = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
		this.SetupAimLineRenderer();
		this.endPoint = this.startPoint;

		StartCoroutine(this.UpdateDirection());
	}

	private IEnumerator UpdateDirection()
	{	
		while (Input.GetMouseButton(0))
		{
			this.endPoint = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);
			this.curDirection = (this.endPoint - this.startPoint).normalized;

			Vector2 lineRendererEndPoint = this.startPoint + (this.curDirection * AsteroidGenerator.instance.curScaledMagnitude);
			this.aimLineRenderer.SetPosition(1, lineRendererEndPoint);

			AsteroidGenerator.instance.UpdateCurrentMagnitude();

			yield return null;
		}

		AsteroidGenerator.instance.queuedToLaunch = true;
		Destroy(this.gameObject);
	}
}
