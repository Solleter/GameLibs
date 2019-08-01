using UnityEngine;
using System.Collections;

public class AimController : MonoBehaviour
{
    public DefaultGun gun;
    private Vector3 screenCenter;
    private Vector3 normDir;

    private Transform myTrans;

    private void Start()
    {
        myTrans = transform;
        screenCenter = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0);
    }

    private void Update()
    {
        UpdateDirInput();
        UpdateAim();
        UpdateShooting();
    }

    /// <summary>
    /// 更新瞄准朝向
    /// </summary>
    private void UpdateAim()
    {
        myTrans.rotation = Quaternion.LookRotation(normDir);
    }

    /// <summary>
    /// 更新朝向输入，根据不同操控设备，获得输入
    /// </summary>
    private void UpdateDirInput()
    {
        Vector3 mousePos = Input.mousePosition;
        normDir = (mousePos - screenCenter).normalized;
    }

    private void UpdateShooting()
    {
        if (Input.GetMouseButton(0))
        {
            gun.Shooting(this.normDir);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            gun.EndShooting();
        }
    }

    private void OnGUI()
    {
        GUILayout.Label(Screen.width + "  " + Screen.height + "  " + screenCenter);
        GUILayout.Label("NormDir: " + normDir);
    }
}
