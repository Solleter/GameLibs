using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class CommonSingleton<T> where T : CommonSingleton<T>  {
    private static T instance = null;

    private static void Init()
    {
        if (instance == null)
        {
            instance = default(T);
            // get all non_public constructor
            ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            // get constructor without parameters
            ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
            if (ctor == null)
            {
                Debug.LogWarningFormat("Non-public ctor() not found! in: {0}", typeof(T));
                ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.Public);
                ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
            }
            instance = ctor.Invoke(null) as T;
            instance.OnInit();
        }
    }
    

    public static T Inst {
        get {
            Init();
            return instance;
        }
    }

    protected virtual void OnInit()
    {

    }
    
}
