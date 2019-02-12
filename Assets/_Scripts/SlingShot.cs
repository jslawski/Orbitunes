using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolarCoordinates;

public class SlingShot : MonoBehaviour
{
    private LineRenderer lineObject;

    public delegate void OnReleased(LaunchValues launchValues);
    public event OnReleased OnSlingShotReleased;

    private float slingShotRadius = 2f;
    private float slingShotLaunchRadius = 3f;
    private float launchMagnitude = 7f;

    private LaunchValues slingShotLaunchValues;

    [SerializeField]
    private PredictionLine predictionLine;

    private void Awake()
    {
        this.lineObject = this.gameObject.GetComponent<LineRenderer>();
        this.OnSlingShotReleased += AnimateSlingShot;
    }

    // Use this for initialization
    void Start ()
    {
        Vector3 middlePosition = this.transform.position;

        this.lineObject.SetPosition(0, new Vector3(middlePosition.x - slingShotRadius, middlePosition.y, middlePosition.z));
        this.lineObject.SetPosition(1, middlePosition);
        this.lineObject.SetPosition(2, new Vector3(middlePosition.x + slingShotRadius, middlePosition.y, middlePosition.z));

        this.slingShotLaunchValues.startPoint = middlePosition;

        StartCoroutine(this.UpdatePosition());

        Color lineColor = AsteroidSelector.selectedAsteroid.GetComponentInChildren<ParticlePulse>().particleColor;
        this.lineObject.startColor = lineColor;
        this.lineObject.endColor = lineColor;
    }

    private float GetDistance(Vector2 point1, Vector2 point2)
    {
        return (Mathf.Sqrt(Mathf.Pow((point2.x - point1.x), 2) + Mathf.Pow((point2.y - point1.y), 2)));
    }

    private void UpdateCurrentMagnitude()
    {
        float normalizedLaunchMagnitude = this.GetDistance(this.slingShotLaunchValues.startPoint, this.slingShotLaunchValues.endPoint) / this.slingShotLaunchRadius;
        this.slingShotLaunchValues.rawMagnitude = normalizedLaunchMagnitude * this.launchMagnitude;
    }

    private void UpdateLineRendererEndpoints(float deltaAngle)
    {
        PolarCoordinate localLeftPoint = PolarCoordinate.CartesianToPolar((Vector2)this.lineObject.GetPosition(0) - this.slingShotLaunchValues.startPoint);
        PolarCoordinate localRightPoint = PolarCoordinate.CartesianToPolar((Vector2)this.lineObject.GetPosition(2) - this.slingShotLaunchValues.startPoint);

        localLeftPoint.angleInDegrees = 180f - deltaAngle;
        localRightPoint.angleInDegrees = -deltaAngle;

        this.lineObject.SetPosition(0, this.slingShotLaunchValues.startPoint + (Vector2)localLeftPoint.PolarToCartesian());
        this.lineObject.SetPosition(2, this.slingShotLaunchValues.startPoint + (Vector2)localRightPoint.PolarToCartesian());
    }

    private IEnumerator UpdatePosition()
    {
        while (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = GameManager.instance.mainCamera.ScreenToWorldPoint(Input.mousePosition);

            Vector2 positionDifference = mousePosition - this.transform.position;

            float radius = this.slingShotLaunchRadius;

            if (positionDifference.magnitude < this.slingShotLaunchRadius)
            {
                radius = positionDifference.magnitude;
            }

            PolarCoordinate polarPositionDifference = new PolarCoordinate(radius, positionDifference);

            this.slingShotLaunchValues.endPoint = this.transform.position + polarPositionDifference.PolarToCartesian();
            this.slingShotLaunchValues.curDirection = this.slingShotLaunchValues.startPoint - this.slingShotLaunchValues.endPoint;

            this.lineObject.SetPosition(1, this.slingShotLaunchValues.endPoint);

            if (polarPositionDifference.angleInDegrees != 0f)
            {
                this.UpdateLineRendererEndpoints(270f - polarPositionDifference.angleInDegrees);
            }

            this.UpdateCurrentMagnitude();

            if (this.slingShotLaunchValues.rawMagnitude >= 1f)
            {
                this.predictionLine.UpdatePredictionLine(this.slingShotLaunchValues);
            }

            yield return null;
        }

        this.OnSlingShotReleased(this.slingShotLaunchValues);
    }

    private void AnimateSlingShot(LaunchValues finalLaunchValues)
    {
        StartCoroutine(AnimateSlingShotCoroutine(finalLaunchValues));
    }

    private IEnumerator AnimateSlingShotCoroutine(LaunchValues finalLaunchValues)
    {
        while ((finalLaunchValues.endPoint - finalLaunchValues.startPoint).magnitude > 0.01f)
        {
            finalLaunchValues.endPoint = Vector2.Lerp(finalLaunchValues.endPoint, finalLaunchValues.startPoint, 0.5f);

            this.lineObject.SetPosition(1, finalLaunchValues.endPoint);

            yield return null;
        }

        Destroy(this.gameObject);
    }
}
