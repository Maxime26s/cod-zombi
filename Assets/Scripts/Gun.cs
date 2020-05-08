using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeGun {Auto, Semi, Rafale }
public enum ModelesGun {Pistolet, AK, Sniper, PistoletRafale}

public class Gun : MonoBehaviour
{
    public ModelesGun modele;
    public float damage;
    public float fireRate;
    public int ammo;
    public int maxAmmo;
    public int bulletAmount;
    public float arc;
    public float[] angles;
    public bool isOwned = false;
    public bool inUse = false;
    public bool piercing;
    public bool spray;
    public TypeGun type;
    public float damageMultiplier = 1f;
    public float frMultiplier = 1f;

    private void OnEnable()
    {
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
