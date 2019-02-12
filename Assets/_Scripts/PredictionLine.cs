using UnityEngine;

//v = Ft + u
public class PredictionLine : MonoBehaviour {

    private LineRenderer lineObject;

    private void Awake()
    {
        this.lineObject = this.gameObject.GetComponent<LineRenderer>();

        Color lineColor = AsteroidSelector.selectedAsteroid.GetComponentInChildren<ParticlePulse>().particleColor;
        this.lineObject.startColor = lineColor;
        this.lineObject.endColor = lineColor * new Color(1.0f, 1.0f, 1.0f, 0f);
    }

    public void UpdatePredictionLine(LaunchValues currentLaunchValues)
    {
        Vector3 currentTheoreticalPosition = this.transform.position;
        Vector3 currentTheoreticalVelocity = currentLaunchValues.curDirection * currentLaunchValues.rawMagnitude;

        this.lineObject.SetPosition(0, currentTheoreticalPosition);

        for (int i = 1; i < this.lineObject.positionCount; i++)
        {
            float radius = Vector3.Magnitude(currentTheoreticalPosition);
            float gravitationalForce = -(CenterStar.mass) / (Mathf.Pow(radius, 2.0f));

            if (Mathf.Abs(gravitationalForce) > 3000)
            {
                gravitationalForce = -3000;
            }

            Vector3 appliedForce = currentTheoreticalPosition.normalized * gravitationalForce;

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
