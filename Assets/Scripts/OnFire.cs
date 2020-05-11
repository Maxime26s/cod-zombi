using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFire : MonoBehaviour
{
    public bool onFire;
    public float fireDamage;
    public Enemy enemy;
    public Player player;

    void Update()
    {
        if (onFire)
        {
            enemy.TakeDamage(fireDamage * Time.deltaTime, player, DamageType.DOT);
        }
    }
}
