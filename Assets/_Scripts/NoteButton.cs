using UnityEngine;
using UnityEngine.UI;

public class NoteButton : MonoBehaviour {
	public int asteroidIndex = 0;

	private Image buttonImage;

	private void Awake()
	{
		this.buttonImage = this.gameObject.GetComponent<Image>();
		AsteroidSelector.OnAsteroidSelected += ChangeButtonColor;
	}

	public void SelectButton()
	{
		AsteroidSelector.SelectAsteroid(this.asteroidIndex);
	}

	private void ChangeButtonColor(int selectedIndex)
	{
		if (selectedIndex == this.asteroidIndex)
		{
            this.buttonImage.material.SetFloat("_Selected", 1.0f);
		}
		else
		{
            this.buttonImage.material.SetFloat("_Selected", 0.0f);
		}
	}
}
