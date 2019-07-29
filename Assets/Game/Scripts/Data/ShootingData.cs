using UnityEngine;

[CreateAssetMenu(fileName = "New ShootingData", menuName = "Shooting Data", order = 51)]
public class ShootingData : ScriptableObject
{
    [SerializeField]
    private float bulletMaxDistance;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private int gunShootingFrequency;


    public float BulletMaxDistance {
        get {
            return bulletMaxDistance;
        }
    }

    public float BulletSpeed {
        get {
            return bulletSpeed;
        }
    }

    public int GunShootingFrequency {
        get {
            return gunShootingFrequency;
        }
    }
}
