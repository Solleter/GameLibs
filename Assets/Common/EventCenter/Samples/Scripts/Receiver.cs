using UnityEngine;
using System.Collections;

public class Receiver : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        Debug.LogFormat("Receiver 注册 OnLogin 事件: {0}", Time.frameCount);
        EventCenter.Instance.AddListener(EventEnum.OnLogin , this.EventHandler);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void EventHandler(EventEnum eventName, BaseEventData eventData)
    {
        Debug.LogFormat("Receiver 收到事件 {0}", eventName);
        OnLoginEventData data = (OnLoginEventData)eventData;
        Debug.LogFormat("昵称: {0}, 性别: {1}", data.nickName, data.gender);
    }
}
