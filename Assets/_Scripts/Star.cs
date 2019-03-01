using UnityEngine;

public class Star : MonoBehaviour {

    [HideInInspector]
    public Transform starTransform;
    [HideInInspector]
    public SphereCollider starCollider;
    public float starMass;
    public string audioEffect;

    private Vector3 mouseWorldPosition;

    private void Awake()
    {
        this.starTransform = this.GetComponent<Transform>();
        this.starCollider = this.GetComponent<SphereCollider>();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePositionScreenToWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float mouseToObjectZOffset = this.starTransform.position.z;
        Vector3 adjustedMouseWorldPosition = new Vector3(mousePositionScreenToWorld.x, mousePositionScreenToWorld.y, mouseToObjectZOffset);


        return adjustedMouseWorldPosition;
    }

    private void OnMouseDrag()
    {
        this.starTransform.position = Vector3.Lerp(this.starTransform.position, this.GetMouseWorldPosition(), 0.05f);
    }
}
