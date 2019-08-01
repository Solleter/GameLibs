using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultGun : MonoBehaviour
{
    public GameObject sourceBullet;

    private Vector3 normDir;
    private bool onShooting = false;

    private float ticker = 0;
    private float interval = 0.0f;

    private Transform myTrans;

    private void Start()
    {
        myTrans = transform;
    }

    public void Shooting(Vector3 normDir)
    {
        this.normDir = normDir;
        onShooting = true;
        interval = 1.0f / (float)DataAgent.Inst.shootingData.GunShootingFrequency;
    }

    public void EndShooting()
    {
        onShooting = false;
    }

    private void Update()
    {
        if (!onShooting)
        {
            return;
        }

        ticker -= Time.deltaTime;
        if(ticker <= 0)
        {
            ticker = interval;
            ShootABullet();
        }


    }

    private void ShootABullet()
    {
        GameObject newbullet = Instantiate(sourceBullet);
        Bullet sb = newbullet.GetComponent<Bullet>();
        sb.Init(myTrans.position, normDir);
    }
}
