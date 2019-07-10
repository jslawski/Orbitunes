using UnityEngine;
using UnityEngine.UI;

public class ScaleSelectorButton : MonoBehaviour
{
    [SerializeField]
    private Scale scaleToSelect;

    private Image buttonImage;

    private void Awake()
    {
        this.buttonImage = this.gameObject.GetComponent<Image>();
        PitchManager.OnScaleSelected += ChangeButtonColor;
    }

    public void SelectScale()
    {
        PitchManager.instance.SelectScale(this.scaleToSelect);
    }

    private void ChangeButtonColor(Scale selectedScale)
    {
        if (selectedScale == this.scaleToSelect)
        {
            this.buttonImage.material.SetFloat("_Selected", 1.0f);
        }
        else
        {
            this.buttonImage.material.SetFloat("_Selected", 0.0f);
        }
    }
}
