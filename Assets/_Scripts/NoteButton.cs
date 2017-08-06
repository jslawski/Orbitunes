using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteButton : MonoBehaviour {

	public delegate void NoteButtonClicked(string asteroidName);
	public static event NoteButtonClicked OnNoteSelected;

	public string asteroidName = string.Empty;
	public SpriteRenderer sprite;
	public Color selectedColor;

	void OnMouseDown()
	{
		this.sprite.color = this.selectedColor;
		NoteButton.OnNoteSelected(asteroidName);
	}
}
