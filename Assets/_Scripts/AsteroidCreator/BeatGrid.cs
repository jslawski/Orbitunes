using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BeatGrid : MonoBehaviour 
{
	public int numSteps = 0;

	private GameObject stepButtonObject;

    [SerializeField]
    private RectTransform gridArea;
    [SerializeField]
    private GridLayoutGroup layoutGroup;

	public void SetupBeatGrid(int beatsPerPhrase)
	{
		this.stepButtonObject = Resources.Load("StepButton") as GameObject;

        float cellSize = this.gridArea.rect.width / this.numSteps;

        if (this.gridArea.rect.height < cellSize)
        {
            cellSize = this.gridArea.rect.height;
        }

        this.layoutGroup.cellSize = new Vector2(cellSize, cellSize);

		this.GenerateStepButtons();
	}

	private void GenerateStepButtons()
	{
		for (int i = 0; i < this.numSteps; i++)
		{
			GameObject currentStepButton = Instantiate(this.stepButtonObject, this.transform) as GameObject;
			ushort stepNumber = (ushort)(Phrase.maxPhraseMask >> i);
			currentStepButton.GetComponent<StepButton>().SetupStepButton(stepNumber);
		}
	}
}
