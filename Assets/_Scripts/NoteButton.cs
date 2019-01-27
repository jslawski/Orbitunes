using UnityEngine;
using UnityEngine.UI;

public class NoteButton : MonoBehaviour {
	public string asteroidName = string.Empty;

	private Image buttonImage;

	private void Awake()
	{
		this.buttonImage = this.gameObject.GetComponent<Image>();
		AsteroidSelector.OnAsteroidSelected += ChangeButtonColor;
	}

	public void SelectButton()
	{
		AsteroidSelector.SelectAsteroid(asteroidName);
	}

	private void ChangeButtonColor(string selectedAsteroidName)
	{
		if (selectedAsteroidName == this.asteroidName)
		{
            this.buttonImage.material.SetFloat("_Selected", 1.0f);
		}
		else
		{
            this.buttonImage.material.SetFloat("_Selected", 0.0f);
		}
	}
}
