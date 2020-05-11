using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum TypeGun { Auto, Semi, Rafale }
public enum ModelesGun { M1911, AK, Barett, B23R, Kap40, FAL, M8A1, Olympia, Remington, M1216, FlameThrower, RPG }
public enum PoidsGun { Light, Normal, Heavy }

public class Gun : MonoBehaviour
{
    public ModelesGun modele;
    public TypeGun type;
    public float damage;
    public float fireRate;
    public int maxAmmo;
    public int ammo;
    public bool isOwned = false;
    public bool inUse = false;
    [HideInInspector]
    public float damageMultiplier = 1f;
    [HideInInspector]
    public float fireRateMultiplier = 1f;
    public bool piercing;
    [HideInInspector]
    public bool spray;
    [HideInInspector]
    public int bulletAmount;
    [HideInInspector]
    public float arc;
    [HideInInspector]
    public float[] angles;
    [HideInInspector]
    public bool explosive;
    [HideInInspector]
    public float explosionDamage;
    [HideInInspector]
    public bool electrify;
    [HideInInspector]
    public float electrifyDamage;
    public float bulletSelfDestruct = 1f;
    public bool fireEnabled = false;
    public float fireDamage;
    public bool ice;
    public bool poison;
    public PoidsGun poids;

    private void OnEnable()
    {
        ammo = maxAmmo;
        if (spray)
        {
            float anglePerBullet = arc * 2 / (bulletAmount - 1);
            angles = new float[bulletAmount];
            for (int i = 0; i < bulletAmount; i++)
            {
                angles[i] = arc - anglePerBullet * i;
            }
            if (this.transform.parent != null)
            {
                switch (poids)
                {
                    case PoidsGun.Light:
                        this.transform.parent.parent.GetComponent<Player>().ChangeSpeed(1f);
                        break;
                    case PoidsGun.Normal:
                        this.transform.parent.parent.GetComponent<Player>().ChangeSpeed(0.9f);
                        break;
                    case PoidsGun.Heavy:
                        this.transform.parent.parent.GetComponent<Player>().ChangeSpeed(1f);
                        break;
                }
            }
        }
    }
}

[CustomEditor(typeof(Gun))]
public class GunEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Gun gun = target as Gun;

        gun.spray = EditorGUILayout.Toggle("Spray", gun.spray);
        if (gun.spray)
        {
            gun.bulletAmount = EditorGUILayout.IntField("Bullet Amount", gun.bulletAmount);
            gun.arc = EditorGUILayout.FloatField("Arc", gun.arc);
        }

        gun.explosive = EditorGUILayout.Toggle("Explosive", gun.explosive);
        if (gun.explosive)
        {
            gun.explosionDamage = EditorGUILayout.FloatField("Explosion Damage", gun.explosionDamage);
        }

        gun.electrify = EditorGUILayout.Toggle("Electrified", gun.electrify);
        if (gun.electrify)
        {
            gun.electrifyDamage = EditorGUILayout.FloatField("Electrify Damage", gun.electrifyDamage);
        }
    }
}