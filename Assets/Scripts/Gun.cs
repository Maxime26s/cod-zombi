using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeGun {Auto, Semi, Sniper, Explosif}
public enum ModelesGun {Pistolet, AK, Sniper}

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
}
