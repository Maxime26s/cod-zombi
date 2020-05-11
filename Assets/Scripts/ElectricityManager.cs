using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityManager : MonoBehaviour
{
    public bool ready;
    public ElectricModifier electric;
    public Player player;
    public List<Collider> enemies = new List<Collider>();
    public GameObject electricityEffect;

    private void Update()
    {

        if (ready)
        {
            if (enemies.Count == electric.maxTarget)
                Destroy(gameObject);
            try
            {
                float closestDistance = Mathf.Infinity;
                Collider closest = null;
                Collider[] colliders = Physics.OverlapSphere(enemies[enemies.Count - 1].transform.position, electric.radius, 1 << 9);
                foreach (Collider collider in colliders)
                {
                    float distance = (collider.transform.position - enemies[enemies.Count - 1].transform.position).sqrMagnitude;
                    if (distance < closestDistance && !IsEnemyTouched(collider))
                    {
                        closestDistance = distance;
                        closest = collider;
                    }
                }
                if (closest != null)
                {

                    GameObject go = Instantiate(electricityEffect, enemies[enemies.Count - 1].transform.position, Quaternion.identity);
                    go.transform.LookAt(closest.transform);
                    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = go.GetComponent<ParticleSystem>().velocityOverLifetime;
                    velocityOverLifetime.z = Mathf.Sqrt(closestDistance)*10;
                    enemies.Add(closest);
                    closest.GetComponent<Enemy>().TakeDamage(electric.damage, player, DamageType.AOE, NumberType.Whole);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
            catch (System.Exception)
            {
                Destroy(gameObject);
            }
        }

        bool IsEnemyTouched(Collider collider)
        {
            foreach (Collider enemy in enemies)
                if (collider == enemy)
                    return true;
            return false;
        }
    }
}
