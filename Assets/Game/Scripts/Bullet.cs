using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 dir;
    private Vector3 startPos;
    
    private Transform myTrans;

    public void Init(Vector3 startPos, Vector3 normDir)
    {
        this.dir = normDir;
        this.startPos = startPos;
        transform.position = startPos;
        transform.rotation = Quaternion.LookRotation(normDir);
    }

    private void Start()
    {
        myTrans = transform;
    }

    private void Update()
    {
        myTrans.position += dir * DataAgent.Inst.shootingData.BulletSpeed * Time.deltaTime;
        float currDis = (myTrans.position - startPos).magnitude;
        if(currDis >= DataAgent.Inst.shootingData.BulletMaxDistance)
        {
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        //this.enabled = false;
        Destroy(gameObject);
    }
}
