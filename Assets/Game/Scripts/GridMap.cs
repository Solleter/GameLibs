﻿using System.Collections;
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
        public GameObject lineObj;

        public LineData(int id, int x, int y, Vector3 pos, GameObject lineObj)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.pos = pos;
            this.lineObj = lineObj;
        }
    }

    enum En_LineType
    {
        Horizontal,
        Verticle,
    }

    public float gridSize = 0.5f;
    public Transform refTrans;
    public int renderCount = 10;
    public Vector3 startPos;
    public GameObject lineSample;
    public float lineWidth = 0.05f;

    private const int MagicID = 10000;
    private const int VMagicOffset = 200000;

    private int refX = 0;
    private int refY = 0;
    private int lastRefX = -1;
    private int lastRefY = -1;
    private bool isDirty = false;

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

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        //int x = 0, y = 0;
        //CalculateGridCenterPos(x, y);
        //Vector3 centerPos = CalculateGridCenterPos(x, y);
        //ShowLine(centerPos);
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
    }

    private void ConstructNeedRenderLine(int startX, int startY)
    {
        needRenderIDDict.Clear();
        // 横线
        int startHLineId = GetLineID(startX, startY, En_LineType.Horizontal);
        needRenderIDDict.Add(startHLineId, 0);
        for(int i = 1; i <= renderCount; ++i)
        {
            int upHLineId = GetLineID(startX, startY + i, En_LineType.Horizontal);
            int downHLineId = GetLineID(startX, startY - i, En_LineType.Horizontal);
            needRenderIDDict.Add(upHLineId, 0);
            needRenderIDDict.Add(downHLineId, 0);
        }

        int startVLineId = GetLineID(startX, startY, En_LineType.Verticle);
        needRenderIDDict.Add(startVLineId, 0);
        for(int i = 1; i < renderCount; ++i)
        {
            int leftVLineId = GetLineID(startX - i, startY, En_LineType.Verticle);
            int rightVLineId = GetLineID(startX + i, startY, En_LineType.Verticle);
            needRenderIDDict.Add(leftVLineId, 0);
            needRenderIDDict.Add(rightVLineId, 0);
        }
    }

    private void CacheUnRenderLine()
    {
        var enumer = renderedIDDict.GetEnumerator();
        while (enumer.MoveNext())
        {
            int lineId = enumer.Current.Key;
            if(needRenderIDDict.ContainsKey(lineId) == false)
            {
                LineData lineData;
                renderedDict.TryGetValue(lineId, out lineData);
                renderedDict.Remove(lineId);
                CacheLine(lineData);
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
                LineData lineData = RenderOneLine(lineId);
                renderedDict.Add(lineId, lineData);
            }
        }
    }

    private LineData RenderOneLine(int lineId)
    {
        int x = 0, y = 0;
        En_LineType lineType;
        GetLineIndex(lineId, out x, out y, out lineType);
        Vector3 linePos = new Vector3(x * gridSize, y * gridSize, 0);
        Vector3 size;
        if(lineType == En_LineType.Horizontal)
        {
            size = new Vector3(renderCount * gridSize * 2, lineWidth);
        }
        else
        {
            size = new Vector3(lineWidth, renderCount * gridSize * 2);
        }

        GameObject lineObj = GetNewLine();
        lineObj.GetComponent<SpriteRenderer>().size = size;
        lineObj.transform.position = linePos;

        LineData lineData = new LineData(lineId, x, y, linePos, lineObj);
        return lineData;
    }

    private GameObject GetNewLine()
    {
        GameObject lineObj = null;
        if(pool.Count > 0)
        {
            lineObj = pool.Dequeue();
        }
        else
        {
            lineObj = Instantiate(lineSample);
        }
        lineObj.SetActive(true);
        return lineObj;
    }

    private void CacheLine(LineData lineData)
    {
        GameObject lineObj = lineData.lineObj;
        lineObj.SetActive(false);
        pool.Enqueue(lineObj);
    }

    private Vector3 CalculateGridCenterPos(int x, int y)
    {
        return new Vector3(x * gridSize, y * gridSize, 0);
    }

    //private void ShowLine(Vector3 startPos)
    //{
    //    // 画横线
    //    Vector3 size = new Vector3(renderCount * gridSize * 2, lineWidth);
    //    GameObject hLine = GetNewLine();
    //    hLine.GetComponent<SpriteRenderer>().size = size;
    //    hLine.transform.position = startPos;
    //    for(int i = 1; i <= renderCount; ++i)
    //    {
    //        Vector3 upLinePos = startPos + new Vector3(0, i * gridSize);
    //        Vector3 downLinePos = startPos + new Vector3(0, -i * gridSize);
    //        GameObject upLine = GetNewLine();
    //        GameObject downLine = GetNewLine();
    //        upLine.GetComponent<SpriteRenderer>().size = size;
    //        downLine.GetComponent<SpriteRenderer>().size = size;
    //        upLine.transform.position = upLinePos;
    //        downLine.transform.position = downLinePos;
    //    }

    //    // 画竖线
    //    size = new Vector3(lineWidth, renderCount * gridSize * 2);
    //    GameObject vLine = GetNewLine();
    //    vLine.GetComponent<SpriteRenderer>().size = size;
    //    vLine.transform.position = startPos;
    //    for(int i = 1; i < renderCount; ++i)
    //    {
    //        Vector3 leftLinePos = startPos + new Vector3(-i * gridSize, 0);
    //        Vector3 rightLinePos = startPos + new Vector3(i * gridSize, 0);
    //        GameObject leftLine = GetNewLine();
    //        GameObject rightLine = GetNewLine();
    //        leftLine.GetComponent<SpriteRenderer>().size = size;
    //        rightLine.GetComponent<SpriteRenderer>().size = size;
    //        leftLine.transform.position = leftLinePos;
    //        rightLine.transform.position = rightLinePos;
    //    }
    //}

    

    private int GetLineID(int x, int y, En_LineType lineType)
    {
        if (lineType == En_LineType.Horizontal)
        {
            return x * MagicID + y;
        }
        else
        {
            return x * MagicID + y + VMagicOffset;
        }
    }

    private void GetLineIndex(int lineId, out int x, out int y, out En_LineType lineType)
    {
        lineType = En_LineType.Horizontal;
        if(lineId >= VMagicOffset)
        {
            lineType = En_LineType.Verticle;
            lineId -= VMagicOffset;
        }

         x = (int)(lineId / MagicID);
         y = lineId % MagicID;
    }


}
