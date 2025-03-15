using UnityEngine;
using UnityEngine.UI;

public class GridSquareCellRelativeCellSize : MonoBehaviour
{
    [SerializeField]
    private float relativeCellSizeX;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<GridLayoutGroup>().cellSize = new Vector2 (Screen.width * relativeCellSizeX, Screen.width * relativeCellSizeX);
    }
}
