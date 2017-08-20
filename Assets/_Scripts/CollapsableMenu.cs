using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapsableMenu : MonoBehaviour {
	private RectTransform[] menuRectTransforms;

	public float menuExpandSize = 100f;
	private bool menuExpanded = false;

	[SerializeField]
	private RectTransform panelRect;
	[SerializeField]
	private RectTransform buttonRect;

	private void Start()
	{
		AimManager.OnLaunchAreaClicked += AutoCloseMenu;
	}

	private void ChangeMenuPosition(Vector2 changeVector)
	{
		panelRect.anchoredPosition = panelRect.anchoredPosition + changeVector;
		buttonRect.anchoredPosition = buttonRect.anchoredPosition + changeVector;
	}

	public void ChangeMenuState()
	{
		if (this.menuExpanded == true)
		{
			this.ChangeMenuPosition(Vector2.down * this.menuExpandSize);
		}
		else
		{
			ChangeMenuPosition(Vector2.up * this.menuExpandSize);
		}

		this.menuExpanded = !this.menuExpanded;
	}

	private void AutoCloseMenu()
	{
		if (this.menuExpanded == true)
		{
			this.ChangeMenuPosition(Vector2.down * this.menuExpandSize);
			this.menuExpanded = false;
		}
	}
}
