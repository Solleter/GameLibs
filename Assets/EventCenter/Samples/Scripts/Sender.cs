using UnityEngine;
using System.Collections;

public class Sender : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        OnLoginEventData data = new OnLoginEventData("尼奥", 1);
        Debug.LogFormat("Sender 发送 OnLogin 事件: {0}", Time.frameCount);
        EventCenter.Instance.SendEvent(EventEnum.OnLogin, data);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
