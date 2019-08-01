using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EventHandleDelegate(EventEnum eventName, BaseEventData eventData = null);

public class EventCenter
{
    private static EventCenter instance;
    public static EventCenter Instance {
        get {
            if(instance == null)
            {
                instance = new EventCenter();
            }
            return instance;
        }
    }

    private Dictionary<EventEnum, EventHandleDelegate> eventDict = new Dictionary<EventEnum, EventHandleDelegate>();
    
    /// <summary>
    /// 注册事件监听
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="handler"></param>
    public void AddListener(EventEnum eventName, EventHandleDelegate handler)
    {
        if (!eventDict.ContainsKey(eventName))
        {
            eventDict.Add(eventName, handler);
        }
        else
        {
            // 先尝试移除一下，防止多次注册
            eventDict[eventName] -= handler;
            eventDict[eventName] += handler;
        }
    }

    /// <summary>
    /// 移除事件监听
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="handler"></param>
    public void RemoveListener(EventEnum eventName, EventHandleDelegate handler)
    {
        if (eventDict.ContainsKey(eventName))
        {
            eventDict[eventName] -= handler;
            if(eventDict[eventName] == null)
            {
                eventDict.Remove(eventName);
            }
        }
    }

    /// <summary>
    /// 发送事件
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventData"></param>
    public void SendEvent(EventEnum eventName, BaseEventData eventData = null)
    {
        if (eventDict.ContainsKey(eventName))
        {
            eventDict[eventName].Invoke(eventName, eventData);
        }
    }


}
