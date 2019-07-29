using UnityEngine;
using System.Collections;

public class DataAgent : MonoBehaviour
{
    private static DataAgent inst;
    public static DataAgent Inst {
        get {
            return inst;
        }
    }

    public ShootingData shootingData;

    private void Awake()
    {
        inst = this;
    }



}

