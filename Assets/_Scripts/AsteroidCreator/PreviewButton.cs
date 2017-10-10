using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewButton : MonoBehaviour {

	[SerializeField]
	private Button button;
	[SerializeField]
	private Image buttonImage;

	private void ToggleToStopButton()
	{
		this.button.onClick.RemoveAllListeners();

		this.button.onClick.AddListener(this.ToggleToPlayButton);
		this.button.onClick.AddListener(AsteroidCreator.instance.EndPreview);

		Sprite buttonSpriteRenderer = Resources.Load<Sprite>("Buttons/stop") as Sprite;
		this.buttonImage.sprite = buttonSpriteRenderer;
	}

	public void ToggleToPlayButton()
	{
		this.button.onClick.RemoveAllListeners();

		this.button.onClick.AddListener(this.ToggleToStopButton);
		this.button.onClick.AddListener(AsteroidCreator.instance.StartPreview);

		Sprite buttonSpriteRenderer = Resources.Load<Sprite>("Buttons/play") as Sprite;
		this.buttonImage.sprite = buttonSpriteRenderer;
	}

}
