using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridMap2 : MonoBehaviour
{
    public Transform refTrans;
    public GameObject lineUnit;
    public int range = 10;
    public float gridSize = 0.5f;
    public float lineWidth = 0.3f;
    private int refX;
    private int refY;
    private int lastRefX;
    private int lastRefY;

    private bool isDirty;

    private Dictionary<int, GameObject> hRenderedLineDict = new Dictionary<int, GameObject>();
    private Dictionary<int, GameObject> vRenderedLineDict = new Dictionary<int, GameObject>();

    private Dictionary<int, int> hRenderedLineIdDict = new Dictionary<int, int>();
    private Dictionary<int, int> vRenderedLineIdDict = new Dictionary<int, int>();

    private Dictionary<int, int> hNeedRenderLineIdDict = new Dictionary<int, int>();
    private Dictionary<int, int> vNeedREnderLineIdDict = new Dictionary<int, int>();

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Start()
    {
        UpdateGrid();
    }

    private void Update()
    {
        CheckDirtyStatus();
        if (isDirty)
        {
            UpdateGrid();
        }
    }


    private void CheckDirtyStatus()
    {
        refX = (int)(refTrans.position.x / gridSize);
        refY = (int)(refTrans.position.y / gridSize);

        if(refX != lastRefX || refY != lastRefY)
        {
            isDirty = true;
        }
    }

    private void UpdateGrid()
    {
        lastRefX = refX;
        lastRefY = refY;
        isDirty = false;
        ConstructNeedRenderId();
        CacheUnNeedRenderLine();
        RenderLines();
    }

    private void ConstructNeedRenderId()
    {
        hNeedRenderLineIdDict.Clear();
        vNeedREnderLineIdDict.Clear();
        hNeedRenderLineIdDict.Add(refY, refX);
        vNeedREnderLineIdDict.Add(refX, refY);

        for(int i = 1; i < range; ++i)
        {
            int id = 0;
            id = refY + i;
            hNeedRenderLineIdDict.Add(id, refX);
            id = refY - i;
            hNeedRenderLineIdDict.Add(id, refX);

            id = refX + i;
            vNeedREnderLineIdDict.Add(id, refY);
            id = refX - i;
            vNeedREnderLineIdDict.Add(id, refY);
        }
    }

    private void CacheUnNeedRenderLine()
    {
        var hEnumer = hRenderedLineIdDict.GetEnumerator();
        while (hEnumer.MoveNext())
        {
            int id = hEnumer.Current.Key;
            if (!hNeedRenderLineIdDict.ContainsKey(id))
            {
                GameObject obj = hRenderedLineDict[id];
                hRenderedLineDict.Remove(id);
                CacheObj(obj);
            }
        }

        var vEnumer = vRenderedLineIdDict.GetEnumerator();
        while (vEnumer.MoveNext())
        {
            int id = vEnumer.Current.Key;
            if (!vNeedREnderLineIdDict.ContainsKey(id))
            {
                GameObject obj = vRenderedLineDict[id];
                vRenderedLineDict.Remove(id);
                CacheObj(obj);
            }
        }
    }

    private void RenderLines()
    {
        hRenderedLineIdDict.Clear();
        vRenderedLineIdDict.Clear();

        var hEnumer = hNeedRenderLineIdDict.GetEnumerator();
        while (hEnumer.MoveNext())
        {
            int id = hEnumer.Current.Key;
            hRenderedLineIdDict.Add(id, 0);
            if (!hRenderedLineDict.ContainsKey(id))
            {
                RenderHLine(id, hEnumer.Current.Value);
            }
        }

        var vEnumer = vNeedREnderLineIdDict.GetEnumerator();
        while (vEnumer.MoveNext())
        {
            int id = vEnumer.Current.Key;
            vRenderedLineIdDict.Add(id, 0);
            if (!vRenderedLineDict.ContainsKey(id))
            {
                RenderVLine(id, vEnumer.Current.Value);
            }
        }
    }

    private void RenderHLine(int y, int x)
    {
        GameObject newLine = CreateNewLineObj();
        Vector3 pos = new Vector3(x * gridSize, y * gridSize, 0);
        newLine.transform.position = pos;
        newLine.GetComponent<SpriteRenderer>().size = new Vector2(65536, lineWidth);
        hRenderedLineDict.Add(y, newLine);
    }

    private void RenderVLine(int x, int y)
    {
        GameObject newLine = CreateNewLineObj();
        Vector3 pos = new Vector3(x * gridSize, y * gridSize, 0);
        newLine.transform.position = pos;
        newLine.GetComponent<SpriteRenderer>().size = new Vector2(lineWidth, 65536);
        vRenderedLineDict.Add(x, newLine);
    }


    private void CacheObj(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    private GameObject CreateNewLineObj()
    {
        GameObject obj = null;
        if(pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.SetActive(true);
        }
        else
        {
            obj = Instantiate(lineUnit);
        }

        return obj;

    }


    private void OnGUI()
    {
        GUILayout.Label(string.Format("RefX: {0}  RefY: {1}", refX, refY));
    }


}
