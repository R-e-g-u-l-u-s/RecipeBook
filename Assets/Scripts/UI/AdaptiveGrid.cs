using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class AdaptiveGrid : MonoBehaviour
{
    [SerializeField] private float padding = 20f;
    [SerializeField] private float spacing = 20f;
    [SerializeField] private float cellAspect = 0.6f; // высота = ширина * aspect

    private GridLayoutGroup grid;
    private float lastWidth = -1f;

    void Awake()
    {
        grid = GetComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = 2;
        grid.padding = new RectOffset((int)padding, (int)padding, (int)padding, (int)padding);
        grid.spacing = new Vector2(spacing, spacing);
        grid.childAlignment = TextAnchor.UpperCenter;
    }

    void Update()
    {
        float width = ((RectTransform)transform).rect.width;
        if (Mathf.Approximately(width, lastWidth)) return;
        lastWidth = width;

        UpdateCellSize(width);
    }

    private void UpdateCellSize(float totalWidth)
    {
        float usableWidth = totalWidth - padding * 2 - spacing;
        float cellWidth = usableWidth / 2f;
        float cellHeight = cellWidth * cellAspect;
        grid.cellSize = new Vector2(cellWidth, cellHeight);
    }
}