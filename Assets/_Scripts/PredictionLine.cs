using UnityEngine;

//v = Ft + u
public class PredictionLine : MonoBehaviour {

    private LineRenderer lineObject;

    private float maxGravitationalForce = 3000;

    private void Awake()
    {
        this.lineObject = this.gameObject.GetComponent<LineRenderer>();

        Color lineColor = AsteroidSelector.selectedAsteroid.asteroidColor;
        this.lineObject.startColor = lineColor;
        this.lineObject.endColor = lineColor * new Color(1.0f, 1.0f, 1.0f, 0f);
    }

    private float GetDistance(Vector2 point1, Vector2 point2)
    {
        return (Mathf.Sqrt(Mathf.Pow((point2.x - point1.x), 2) + Mathf.Pow((point2.y - point1.y), 2)));
    }

    private Vector3 GetTotalGravitationalForce(Vector3 currentTheoreticalPosition)
    {
        Vector3 totalAppliedForce = Vector3.zero;

        for (int i = 0; i < GameManager.instance.stars.Count; i++)
        {
            float radius = this.GetDistance(currentTheoreticalPosition, GameManager.instance.stars[i].starTransform.position);
            Vector3 direction = (GameManager.instance.stars[i].starTransform.position - currentTheoreticalPosition).normalized;
            float starGravitationalForce = GameManager.instance.stars[i].starMass / Mathf.Pow(radius, 2.0f);

            if (Mathf.Abs(starGravitationalForce) > this.maxGravitationalForce)
            {
                starGravitationalForce = 3000;
            }

            totalAppliedForce += (direction * starGravitationalForce);
        }

        return totalAppliedForce;
    }

    public void UpdatePredictionLine(LaunchValues currentLaunchValues)
    {
        Vector3 currentTheoreticalPosition = this.transform.position;
        Vector3 currentTheoreticalVelocity = currentLaunchValues.curDirection * currentLaunchValues.rawMagnitude;

        this.lineObject.SetPosition(0, currentTheoreticalPosition);

        for (int i = 1; i < this.lineObject.positionCount; i++)
        {
            /*float radius = Vector3.Magnitude(currentTheoreticalPosition);
            float gravitationalForce = -(CenterStar.mass) / (Mathf.Pow(radius, 2.0f));

            if (Mathf.Abs(gravitationalForce) > 3000)
            {
                gravitationalForce = -3000;
            }
            */
            Vector3 appliedForce = this.GetTotalGravitationalForce(currentTheoreticalPosition);//currentTheoreticalPosition.normalized * gravitationalForce;

            currentTheoreticalVelocity += appliedForce * Time.fixedDeltaTime;

            if (currentTheoreticalVelocity.magnitude > 120f)
            {
                currentTheoreticalVelocity = currentTheoreticalVelocity.normalized * 120f;
            }

            currentTheoreticalPosition += currentTheoreticalVelocity * Time.fixedDeltaTime;

            this.lineObject.SetPosition(i, currentTheoreticalPosition);
        }
    }
}
