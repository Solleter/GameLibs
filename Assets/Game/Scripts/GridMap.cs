using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    struct GridLine
    {
        public int id;
        public int x;
        public int y;
        public GameObject hLineObj;
        public GameObject vLineObj;

        private float gridSize;
        private float halfGridSize;
        private Vector3 gridPos;
        private float lineWidth;

        private SpriteRenderer hLineRender;
        private SpriteRenderer vLineRender;

        public GridLine(GameObject hLineObj, GameObject vLineObj, float lineWidth)
        {
            this.id = 0;
            this.x = 0;
            this.y = 0;
            this.hLineObj = hLineObj;
            this.vLineObj = vLineObj;
            this.lineWidth = lineWidth;
            this.gridSize = 0.0f;
            this.halfGridSize = 0.0f;
            this.gridPos = Vector3.zero;
            this.hLineRender = this.hLineObj.GetComponent<SpriteRenderer>();
            this.vLineRender = this.vLineObj.GetComponent<SpriteRenderer>();
        }

        public void InitData(int id, int x, int y, float gridSize)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.gridSize = gridSize;
            this.halfGridSize = this.gridSize / 2.0f;
        }

        public void ResetLine()
        {
            // 计算位置，然后重置Line
            this.gridPos = new Vector3(this.x * this.gridSize, this.y * this.gridSize, 0);
            Vector3 linePos = this.gridPos - new Vector3(this.halfGridSize, this.halfGridSize, 0);
            this.hLineObj.transform.position = linePos;
            this.vLineObj.transform.position = linePos;
            Vector2 hLineScale = new Vector2(10000, this.lineWidth);
            Vector2 vLineScale = new Vector2(this.lineWidth, 10000);
            this.hLineRender.size = hLineScale;
            this.vLineRender.size = vLineScale;
        }


        public void SetActive(bool isActive)
        {
            this.hLineObj.SetActive(isActive);
            this.vLineObj.SetActive(isActive);
        }
    }

   
    public float gridSize = 0.5f;
    public Transform refTrans;
    public int renderCount = 10;
    public Vector3 startPos;
    public GameObject lineSample;
    public float lineWidth = 0.05f;

    private const int MagicID = 1000000;

    private int refX = 0;
    private int refY = 0;
    private int lastRefX = -1;
    private int lastRefY = -1;
    private bool isDirty = false;

    private Dictionary<int, GridLine> renderedDict = new Dictionary<int, GridLine>();

    /// <summary>
    /// 已经显示的ID字典 ID-0
    /// </summary>
    private Dictionary<int, int> renderedIDDict = new Dictionary<int, int>();

    /// <summary>
    /// 需要显示的ID字典 ID-0
    /// </summary>
    private Dictionary<int, int> needRenderIDDict = new Dictionary<int, int>();

    private Queue<GridLine> pool = new Queue<GridLine>();

    private void Start()
    {

    }

    private void LateUpdate()
    {
        UpdateDirtyState();

        if (isDirty)
        {
            isDirty = false;
            UpdateGridMap();
        }
        
    }

    private void UpdateDirtyState()
    {
        CalculateRefGridIndex(out refX, out refY);
        if(refX != lastRefX || refY != lastRefY)
        {
            lastRefX = refX;
            lastRefY = refY;
            isDirty = true;
        }
    }

    private void UpdateGridMap()
    {
        ConstructNeedRenderLine(refX, refY);
        CacheUnRenderLine();
        RenderNeedRenderLine();
    }

    private void CalculateRefGridIndex(out int x, out int y)
    {
        float xDiff = refTrans.position.x - startPos.x;
        float yDiff = refTrans.position.y - startPos.y;

        x = (int)(xDiff / gridSize);
        y = (int)(yDiff / gridSize);

        x = Mathf.Clamp(x, 0, x);
        y = Mathf.Clamp(y, 0, y);
    }

    private void ConstructNeedRenderLine(int startX, int startY)
    {
        needRenderIDDict.Clear();
        int lineId = GetLineID(startX, startY);
        needRenderIDDict.Add(lineId, 0);
        //for (int i = 1; i <= 10; ++i)
        //{
        //    int x = startX + i;
        //    int y = startY + i;
        //    lineId = GetLineID(x, y);
        //    needRenderIDDict.Add(lineId, 0);

        //    x = startX - i;
        //    y = startY - i;
        //    if (x >= 0 && y >= 0) {
        //        lineId = GetLineID(x, y);
        //        needRenderIDDict.Add(x, y);
        //    }
        //}
    }

    private void CacheUnRenderLine()
    {
        var enumer = renderedIDDict.GetEnumerator();
        while (enumer.MoveNext())
        {
            int lineId = enumer.Current.Key;
            if(needRenderIDDict.ContainsKey(lineId) == false)
            {
                GridLine gridLine;
                renderedDict.TryGetValue(lineId, out gridLine);
                renderedDict.Remove(lineId);
                CacheLine(gridLine);
            }
        }
        renderedIDDict.Clear();
    }

    private void RenderNeedRenderLine()
    {
        var enumer = needRenderIDDict.GetEnumerator();
        while (enumer.MoveNext())
        {
            int lineId = enumer.Current.Key;
            renderedIDDict.Add(lineId, 0);
            if(renderedDict.ContainsKey(lineId) == false)
            {
                GridLine lineData = RenderOneLine(lineId);
                renderedDict.Add(lineId, lineData);
            }
        }
    }

    private GridLine RenderOneLine(int lineId)
    {
        GridLine line = GetNewLine();
        int x = 0, y = 0;
        GetGridXY(lineId, out x, out y);
        line.InitData(lineId, x, y, gridSize);
        line.SetActive(true);
        line.ResetLine();

        return line;
    }

    private GridLine GetNewLine()
    {
        if(pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            GameObject line1 = Instantiate(lineSample);
            GameObject line2 = Instantiate(lineSample);
            GridLine line = new GridLine(line1, line2, lineWidth);
            return line;
        }
    }

    private void CacheLine(GridLine lineData)
    {
        lineData.SetActive(false);
        pool.Enqueue(lineData);
    }

    private Vector3 CalculateGridCenterPos(int x, int y)
    {
        return new Vector3(x * gridSize, y * gridSize, 0);
    }


    private int GetLineID(int x, int y)
    {
        return x * 1000000 + y;
    }

    private void GetGridXY(int lineId, out int x, out int y)
    {
        x = (int)(lineId / 1000000);
        y = lineId % 1000000;
    }


    private void OnGUI()
    {
        GUILayout.Label(string.Format("RefX: {0}  RefY: {1}", refX, refY));
    }

}
