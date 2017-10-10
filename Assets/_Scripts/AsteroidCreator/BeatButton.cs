using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeatButton : MonoBehaviour {

	[SerializeField]
	private int beatsPerPhrase = 0;
	private Button button;

	// Use this for initialization
	void Start () {
		this.button = this.gameObject.GetComponent<Button>();

		this.button.onClick.RemoveListener(delegate{AsteroidCreator.instance.UpdateBeatsPerPhrase(beatsPerPhrase);});
		this.button.onClick.AddListener(delegate{AsteroidCreator.instance.UpdateBeatsPerPhrase(beatsPerPhrase);});
	}
}
