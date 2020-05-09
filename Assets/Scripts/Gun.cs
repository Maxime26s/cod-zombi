using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeGun {Auto, Semi, Rafale }
public enum ModelesGun {M1911, AK, Sniper, B23R, Kap40, FAL, M8A1}

public class Gun : MonoBehaviour
{
    public ModelesGun modele;
    public float damage;
    public float fireRate;
    public int ammo;
    public int maxAmmo;
    public bool isOwned = false;
    public bool inUse = false;
    public bool piercing;
    public TypeGun type;
    public float damageMultiplier = 1f;
    public float frMultiplier = 1f;
}
