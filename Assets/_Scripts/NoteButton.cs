using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteButton : MonoBehaviour {
	public string asteroidName = string.Empty;

	private Image buttonImage;

	private void Start()
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
			Color buttonColor = AsteroidSelector.selectedAsteroid.GetComponentInChildren<ParticlePulse>().particleColor;
			this.buttonImage.color = buttonColor;
		}
		else
		{
			this.buttonImage.color = Color.white;
		}
	}
}
