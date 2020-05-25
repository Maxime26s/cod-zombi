using System.Collections.Generic;
using System.Linq;
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

                KdTree<Collider> hits = new KdTree<Collider>();
                hits.AddAll(colliders.ToList());
                closest = hits.FindClosest(enemies[enemies.Count - 1].transform.position);
                closestDistance = (closest.transform.position - enemies[enemies.Count - 1].transform.position).sqrMagnitude;

                /*
                foreach (Collider collider in colliders)
                {  
                float distance = (collider.transform.position - enemies[enemies.Count - 1].transform.position).sqrMagnitude;
                if (distance < closestDistance && !IsEnemyTouched(collider))
                {
                    closestDistance = distance;
                    closest = collider;
                }
                */

                /*
                for (Collider collider in colliders)
                {
                    if (!IsEnemyTouched(collider))
                    {
                        closestDistance = (collider.transform.position - enemies[enemies.Count - 1].transform.position).sqrMagnitude;
                        closest = collider;
                    }
                }*/

                if (closest != null)
                {

                    GameObject go = ObjectPooler.Instance.GetPooledObject("ElectricityChain");
                    go.transform.position = enemies[enemies.Count - 1].transform.position;
                    go.transform.LookAt(closest.transform);
                    ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime = go.GetComponent<ParticleSystem>().velocityOverLifetime;
                    velocityOverLifetime.z = Mathf.Sqrt(closestDistance)*10;
                    go.SetActive(true);
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
    }

    bool IsEnemyTouched(Collider collider)
    {
        foreach (Collider enemy in enemies)
            if (collider == enemy)
                return true;
        return false;
    }
}
