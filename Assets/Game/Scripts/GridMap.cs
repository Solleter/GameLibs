using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    struct LineData
    {
        public int id;
        public int x;
        public int y;
        public Vector3 pos;
    }


    public float gridSize = 0.5f;
    public Transform refTrans;
    public int renderCount = 10;
    public Vector3 startPos;
    public GameObject lineSample;
    public float lineWidth = 0.05f;

    private const int MagicID = 10000;

    /// <summary>
    /// 当前已经显示的线 ID-LineData
    /// </summary>
    private Dictionary<int, LineData> renderedDict = new Dictionary<int, LineData>();

    /// <summary>
    /// 已经显示的ID字典 ID-0
    /// </summary>
    private Dictionary<int, int> renderedIDDict = new Dictionary<int, int>();

    /// <summary>
    /// 需要显示的ID字典 ID-0
    /// </summary>
    private Dictionary<int, int> needRenderIDDict = new Dictionary<int, int>();


    private void Start()
    {
        int x = 0, y = 0;
        CalculateGridCenterPos(x, y);
        Vector3 centerPos = CalculateGridCenterPos(x, y);
        ShowLine(centerPos);

    }

    private void CalcualteRefGridIndex(out int x, out int y)
    {
        float xDiff = refTrans.position.x - startPos.x;
        float yDiff = refTrans.position.y - startPos.y;

        x = (int)(xDiff / gridSize);
        y = (int)(yDiff / gridSize);
    }

    private Vector3 CalculateGridCenterPos(int x, int y)
    {
        return new Vector3(x * gridSize, y * gridSize, 0);
    }

    private void ShowLine(Vector3 startPos)
    {
        // 画横线
        Vector3 size = new Vector3(renderCount * gridSize * 2, lineWidth);
        GameObject hLine = GetNewLine();
        hLine.GetComponent<SpriteRenderer>().size = size;
        hLine.transform.position = startPos;
        for(int i = 1; i <= renderCount; ++i)
        {
            Vector3 upLinePos = startPos + new Vector3(0, i * gridSize);
            Vector3 downLinePos = startPos + new Vector3(0, -i * gridSize);
            GameObject upLine = GetNewLine();
            GameObject downLine = GetNewLine();
            upLine.GetComponent<SpriteRenderer>().size = size;
            downLine.GetComponent<SpriteRenderer>().size = size;
            upLine.transform.position = upLinePos;
            downLine.transform.position = downLinePos;
        }

        // 画竖线
        size = new Vector3(lineWidth, renderCount * gridSize * 2);
        GameObject vLine = GetNewLine();
        vLine.GetComponent<SpriteRenderer>().size = size;
        vLine.transform.position = startPos;
        for(int i = 1; i < renderCount; ++i)
        {
            Vector3 leftLinePos = startPos + new Vector3(-i * gridSize, 0);
            Vector3 rightLinePos = startPos + new Vector3(i * gridSize, 0);
            GameObject leftLine = GetNewLine();
            GameObject rightLine = GetNewLine();
            leftLine.GetComponent<SpriteRenderer>().size = size;
            rightLine.GetComponent<SpriteRenderer>().size = size;
            leftLine.transform.position = leftLinePos;
            rightLine.transform.position = rightLinePos;
        }
    }

    private GameObject GetNewLine()
    {
        GameObject obj = Instantiate(lineSample);
        return obj;
    }

    private int GetLineID(int x, int y)
    {
        return x * MagicID + y;
    }


    /// <summary>
    /// 1. 先遍历已经显示的线ID字典，回收需要回收的线
    /// 2. 再遍历需要显示的线ID字典，显示需要现实的线
    /// 3. Clear 已经显示的线ID字典和需要现实的线ID字典
    /// </summary>
    private void UpdateRender()
    {

    }

}
