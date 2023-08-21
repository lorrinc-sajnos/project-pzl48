using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditorGenerateGrid : MonoBehaviour
{
    public EditorData editorData;
    public GameObject grid;
    public GameObject gridRectObj;

    public RectTransform gridRect;
    public Vector2 gridSize;

    public Vector2 offset;

    public GameObject gridPiece;

    public Vector3 gridPieceScale;
    public float gridWidth;
    public float baseGridPieceSize = 500f;

    public float gridPieceSize;

    public void VarInit()
    {
        gridSize = gridRect.rect.size;
        gridWidth = gridSize.x;

        gridPieceSize = gridWidth / editorData.gridSize.x;
        gridPieceScale = new Vector3(gridPieceSize / baseGridPieceSize, gridPieceSize / baseGridPieceSize, 1);

        offset.x = gridPieceSize / 2 - (gridWidth / 2);// + center.x;
        offset.y = gridPieceSize / 2 - (gridWidth / 2);// + center.y;

        //Debug.Log("AT START Scale" + gridPieceScale.x + ";" + gridPieceScale.y);
    }
    public void GenerateGrid()
    {
        //gameObjGrid = new GameObject[gridSize.x, gridSize.y];

        for (int x = 0; x < editorData.gridSize.x; x++)
        {
            for (int y = 0; y < editorData.gridSize.y; y++)
            {
                editorData.gameObjGrid[x, y] = Instantiate(gridPiece);
                editorData.gameObjGrid[x, y].transform.SetParent(gridRectObj.transform);
                editorData.gameObjGrid[x, y].name = "Grid Piece " + x + ":" + y;

                var tempPiece = editorData.gameObjGrid[x, y].GetComponent<EditorPieceScript>();
                tempPiece.editorData = editorData;
                tempPiece.VarInit();

                //rectTransform = gameObjGrid[x, y].GetComponent<RectTransform>();

                RectTransform temp = editorData.gameObjGrid[x, y].GetComponent<RectTransform>();

                temp.localScale = gridPieceScale;
                temp.anchoredPosition = GetPositionFromGrid(x, y);
            }
        }
    }
    public Vector3 GetPositionFromGrid(float x, float y)
    {
        return new Vector3(offset.x + x * gridPieceSize, offset.y + y * gridPieceSize, 0);//* unitScale.x
    }
}
