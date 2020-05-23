using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeGun { Auto, Semi, Rafale }
public enum ModelesGun { M1911, AK, Barett, B23R, Kap40, FAL, M8A1, Olympia, Remington, M1216, FlameThrower, RPG }
public enum PoidsGun { Light, Normal, Heavy }
public enum Effects { Spray, Fire, Electric, Ice, Poison, Piercing, Explosive };

public class Gun : MonoBehaviour
{
    public ModelesGun modele;
    public TypeGun type;
    public PoidsGun poids;
    public float damage;
    public float fireRate;
    public int maxAmmo;
    public int ammo;
    public bool isOwned = false;
    public bool inUse = false;
    public bool pap;
    [HideInInspector]
    public float damageMultiplier = 1f;
    [HideInInspector]
    public float fireRateMultiplier = 1f;
    public float bulletSelfDestruct = 1f;
    [SerializeField]
    public PiercingModifier piercing;
    [SerializeField]
    public ExplosiveModifier explosive;
    [SerializeField]
    public SprayModifier spray;
    [SerializeField]
    public FireModifier fire;
    [SerializeField]
    public ElectricModifier electric;
    [SerializeField]
    public IceModifier ice;
    [SerializeField]
    public PoisonModifier poison;
    public GameObject prefab;

    private void OnEnable()
    {
        ammo = maxAmmo;
        if (spray.enabled)
            spray.CalculateAngles();
        if (transform.parent != null)
        {
            if (transform.parent.parent != null)
            {
            transform.parent.parent.GetComponent<Player>().ChangeSpeed(GetSpeedModifier());
            }
        }
    }
    public float GetSpeedModifier()
    {
        switch (poids)
        {
            case PoidsGun.Light:
                return 1f;
            case PoidsGun.Normal:
                return 0.85f;
            case PoidsGun.Heavy:
                return 0.7f;
            default:
                return 1f;
        }
    }
}

[System.Serializable]
public class PiercingModifier
{
    public bool enabled;
    public int amount;
}

[System.Serializable]
public class ExplosiveModifier
{
    public bool enabled;
    public float radius;
}

[System.Serializable]
public class FireModifier
{
    public bool enabled;
    public float damage;
}

[System.Serializable]
public class ElectricModifier
{
    public bool enabled;
    public float damage;
    public float radius;
    public int maxTarget;
}

[System.Serializable]
public class IceModifier
{
    public bool enabled;
    public float slowMultiplier;
}

[System.Serializable]
public class PoisonModifier
{
    public bool enabled;
    public float damageMultiplier;
}

[System.Serializable]
public class SprayModifier
{
    public bool enabled;
    public int bulletAmount;
    public float halfArc;
    [HideInInspector]
    public float[] angles;

    public void CalculateAngles()
    {
        float anglePerBullet = halfArc * 2 / (bulletAmount - 1);
        angles = new float[bulletAmount];
        for (int i = 0; i < bulletAmount; i++)
        {
            angles[i] = halfArc - anglePerBullet * i;
        }
    }
}