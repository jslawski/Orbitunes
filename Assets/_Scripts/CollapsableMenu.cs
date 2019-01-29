using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class CollapsableMenu : MonoBehaviour {
	private RectTransform[] menuRectTransforms;

	private float menuExpandSize;
	private bool menuExpanded = false;

	[SerializeField]
	private RectTransform panelRect;
	[SerializeField]
	private RectTransform buttonRect;

    private bool firstTimeMenuOpened = false;

    [SerializeField]
    private bool beginOpen = false;

	private void Start()
	{
        //AimManager.OnLaunchAreaClicked += AutoCloseMenu;

        this.menuExpandSize = this.panelRect.rect.height;

        if (this.beginOpen == true)
        {
            this.menuExpanded = true;
        }
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

            AnalyticsEvent.Custom("Asteroid_Menu_Closed", new Dictionary<string, object> { { "Time_Elapsed", Time.timeSinceLevelLoad } });
        }
		else
		{
			ChangeMenuPosition(Vector2.up * this.menuExpandSize);

            if (this.firstTimeMenuOpened == false)
            {
                AnalyticsEvent.Custom("Asteroid_Menu_Opened", new Dictionary<string, object> { { "Time_Elapsed", Time.timeSinceLevelLoad } });
                this.firstTimeMenuOpened = true;
            }
        }

        this.buttonRect.Rotate(new Vector3(0, 0, 180));

		this.menuExpanded = !this.menuExpanded;
	}

	private void AutoCloseMenu()
	{
		if (this.menuExpanded == true)
		{
			this.ChangeMenuPosition(Vector2.down * this.menuExpandSize);
			this.menuExpanded = false;

            AnalyticsEvent.Custom("Asteroid_Menu_AutoClosed", new Dictionary<string, object> { { "Time_Elapsed", Time.timeSinceLevelLoad } });
        }
	}
}
