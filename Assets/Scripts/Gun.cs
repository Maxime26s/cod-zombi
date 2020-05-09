using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum TypeGun {Auto, Semi, Rafale }
public enum ModelesGun {Pistolet, AK, Sniper, PistoletRafale}

public class Gun : MonoBehaviour
{
    public ModelesGun modele;
    public TypeGun type;
    public float damage;
    public float fireRate;
    public int maxAmmo;
    [HideInInspector]
    public int ammo;
    [HideInInspector]
    public bool isOwned = false;
    [HideInInspector]
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

    private void OnEnable()
    {
        ammo = maxAmmo;
        if (spray)
        {
            float anglePerBullet = arc * 2 / (bulletAmount-1);
            angles = new float[bulletAmount];
            for (int i = 0; i < bulletAmount; i++)
            {
                    angles[i] = arc - anglePerBullet * i;
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
    }
}