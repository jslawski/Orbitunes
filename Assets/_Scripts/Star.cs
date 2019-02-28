using UnityEngine;

public class Star : MonoBehaviour {

    [HideInInspector]
    public Transform starTransform;
    public float starMass;
    public string audioEffect;

    private void Awake()
    {
        this.starTransform = this.GetComponent<Transform>();
    }
}
