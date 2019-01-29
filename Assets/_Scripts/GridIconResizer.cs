using UnityEngine;
using UnityEngine.UI;

public class GridIconResizer : MonoBehaviour
{

    [SerializeField]
    private int numCells;
    [SerializeField]
    private RectTransform gridArea;
    [SerializeField]
    private GridLayoutGroup layoutGroup;

    private void Start()
    {
        float emptySpace = ((this.numCells - 1) * this.layoutGroup.spacing.x) / this.numCells;
        float cellSize = (this.gridArea.rect.width / this.numCells) - this.layoutGroup.spacing.x;

        if (this.gridArea.rect.height < cellSize)
        {
            cellSize = this.gridArea.rect.height;
        }

        this.layoutGroup.cellSize = new Vector2(cellSize, cellSize);
    }
}
