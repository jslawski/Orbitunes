using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

	[SerializeField]
	private Image title;
	[SerializeField]
	private Text myName;
	[SerializeField]
	private Text instructions;

	private float fadeOutThreshold = 0.1f;
	private float fadeInThreshold = 0.9f;

	private float fadeDuration = 2f;
	private float displayDuration = 3f;

	private float instructionsFadeInThreshold = 0.5f;
	private float instructionsFadeDuration = 3f;
	private float instructionsDisplayDuration = 5f;

	// Use this for initialization
	void Start () {
		StartCoroutine(this.DisplayTitle());
	}
	

	private IEnumerator DisplayTitle()
	{
		PitchManager.instance.bassSound.volume = 0;

		StartCoroutine(PitchManager.instance.StartMetronome());
		StartCoroutine(PitchManager.instance.StartBass());

		for (float i = 0; i < this.fadeInThreshold; i += Time.fixedDeltaTime / this.fadeDuration) 
		{
			if (Input.GetMouseButtonDown(0)) 
			{
				break;
			}

			title.color = new Color(title.color.r, title.color.g, title.color.b, i);
			myName.color = new Color(myName.color.r, myName.color.g, myName.color.b, i);

			yield return new WaitForFixedUpdate();
		}

		PitchManager.instance.bassSound.volume = 1f;;

		title.color = new Color(title.color.r, title.color.g, title.color.b, 1);
		myName.color = new Color(myName.color.r, myName.color.g, myName.color.b, 1);

		yield return new WaitForFixedUpdate();

		for (float i = 0; i < this.displayDuration; i += Time.fixedDeltaTime / this.displayDuration) 
		{
			if (Input.GetMouseButtonDown(0)) 
			{
				break;
			}

			yield return new WaitForFixedUpdate();
		}

		yield return new WaitForFixedUpdate();

		for (float i = 1; i > this.fadeOutThreshold; i -= Time.fixedDeltaTime / this.fadeDuration) 
		{
			title.color = new Color(title.color.r, title.color.g, title.color.b, i);
			myName.color = new Color(myName.color.r, myName.color.g, myName.color.b, i);

			yield return new WaitForFixedUpdate();
		}

		title.color = new Color(title.color.r, title.color.g, title.color.b, 0);
		myName.color = new Color(myName.color.r, myName.color.g, myName.color.b, 0);

		StartCoroutine(this.DisplayInstructions());
	}

	private IEnumerator DisplayInstructions()
	{
		for (float i = 0; i < this.instructionsFadeInThreshold; i += Time.fixedDeltaTime / this.instructionsFadeDuration) 
		{
			instructions.color = new Color(instructions.color.r, instructions.color.g, instructions.color.b, i);

			yield return new WaitForFixedUpdate();
		}

		yield return new WaitForSeconds(this.instructionsDisplayDuration);

		for (float i = this.instructionsFadeInThreshold; i > this.fadeOutThreshold; i -= Time.fixedDeltaTime / this.instructionsFadeDuration) 
		{
			instructions.color = new Color(instructions.color.r, instructions.color.g, instructions.color.b, i);

			yield return new WaitForFixedUpdate();
		}

		instructions.color = new Color(instructions.color.r, instructions.color.g, instructions.color.b, 0);
	}
}
