using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StepButton : MonoBehaviour  {

	private ushort stepNumber = 1;

	private Button button;

	[SerializeField]
	private Image[] buttonImages;

	private bool _selected;
	private bool selected
	{
		get { return this._selected; }
		set {
			this._selected = value;
			foreach (Image buttonImage in this.buttonImages)
			{
				if (value == true)
				{
					buttonImage.color = AsteroidCreator.instance.asteroidColor;
				}
				else
				{
					buttonImage.color = Color.gray;
				}
			}
		}
	}

	public void SetupStepButton(ushort stepNumber)
	{
		this.stepNumber = stepNumber;

		buttonImages = this.gameObject.GetComponentsInChildren<Image>();

		this.button = this.gameObject.GetComponent<Button>();

		button.onClick.AddListener(delegate{AsteroidCreator.instance.UpdatePhraseNumber(stepNumber);});
		button.onClick.AddListener(ToggleButton);

		if ((this.stepNumber & AsteroidCreator.instance.phraseNumber) > 0)
		{
			this.selected = true;
		}
	}

	private void OnDestroy()
	{
		this.button.onClick.RemoveAllListeners();
	}

	private void OnDisable()
	{
		Destroy(this.gameObject);
	}

	public void ToggleButton()
	{
		this.selected = !this.selected;
	}
}
