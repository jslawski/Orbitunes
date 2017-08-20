/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

	public List<NoteButton> noteButtons;

	// Use this for initialization
	void Start () {
		NoteButton.OnNoteSelected += this.UpdateButtons;
	}

	//Change all not-selected buttons to white
	private void UpdateButtons(string selectedAsteroidName)
	{
		foreach (NoteButton button in this.noteButtons) 
		{
			if (button.asteroidName != selectedAsteroidName) 
			{
				button.sprite.color = Color.white;
			}
		}
	}
}
*/