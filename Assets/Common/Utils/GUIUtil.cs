using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIUtil : MonoBehaviour
{
    private static GUIUtil inst;
    public static GUIUtil Inst
    {
        get
        {
            if(inst == null)
            {
                GameObject obj = new GameObject("GUIUtil");
                inst = obj.AddComponent<GUIUtil>();
            }
            return inst;
        }
    }

    public System.Action onGUIEvent;

    public void AddCall(System.Action callback)
    {
        onGUIEvent += callback;
    }

    public void RemoveCall(System.Action callback)
    {
        onGUIEvent -= callback;
    }
    

    private void OnGUI()
    {
        if(onGUIEvent != null)
        {
            onGUIEvent.Invoke();
        }
    }
}
