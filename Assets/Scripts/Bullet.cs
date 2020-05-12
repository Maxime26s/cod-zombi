using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float dissolveTime;
    public bool isPiercing, isExplosive, dissolve, destroying;
    public GameObject explosionPrefab, deathParticles, firePrefab, icePrefab, poisonPrefab, electricPrefab;
    public Player player;
    public FireModifier fire;
    public ElectricModifier electric;
    public IceModifier ice;
    public PoisonModifier poison;

    // Update is called once per frame
    void Update()
    {
        if (!destroying || isPiercing)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            if (dissolve)
                transform.localScale -= new Vector3(dissolveTime, dissolveTime, dissolveTime) * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (electric.enabled && !enemy.electrified)
                {
                    ElectricityManager em = Instantiate(electricPrefab).GetComponent<ElectricityManager>();
                    em.electric = electric;
                    em.player = player;
                    em.enemies.Add(other);
                    em.ready = true;
                    enemy.TakeDamage(electric.damage, player, DamageType.AOE, NumberType.Whole);
                }
                if (fire.enabled && enemy.GetComponentInChildren<OnFire>() == null)
                {
                    OnFire onFire = Instantiate(firePrefab, enemy.transform).GetComponent<OnFire>();
                    onFire.player = player;
                    onFire.enemy = enemy;
                    onFire.fireDamage = fire.damage;
                    onFire.onFire = true;
                }
                if (ice.enabled && !enemy.frozen)
                {
                    Instantiate(icePrefab, enemy.transform);
                    IEnumerator Freeze()
                    {
                        enemy.frozen = true;
                        other.gameObject.GetComponent<NavMeshAgent>().speed = enemy.speed * ice.slowMultiplier;
                        yield return new WaitForSeconds(2.5f);
                        other.gameObject.GetComponent<NavMeshAgent>().speed = enemy.speed;
                        enemy.frozen = false;
                    }
                    enemy.StartCoroutine(Freeze());
                }
                if (poison.enabled && !enemy.poisoned)
                {
                    Instantiate(poisonPrefab, enemy.transform);
                    IEnumerator Poison()
                    {
                        enemy.poisoned = true;
                        enemy.damageMultiplier = poison.damageMultiplier;
                        yield return new WaitForSeconds(2.5f);
                        enemy.damageMultiplier = 1;
                        enemy.poisoned = false;
                    }
                    enemy.StartCoroutine(Poison());
                }
                if (isExplosive)
                    enemy.TakeDamage(player.gun.GetComponent<Gun>().damage, player, DamageType.AOE, NumberType.Whole);
                else
                    enemy.TakeDamage(player.gun.GetComponent<Gun>().damage * player.gun.GetComponent<Gun>().damageMultiplier, player, DamageType.Hit, NumberType.Whole);
            }
            CheckDestroy(other.CompareTag("Enemy"));
        }
    }

    private void CheckDestroy(bool isEnemy)
    {
        if (!destroying)
        {
            destroying = true;
            if (isExplosive)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity).SetActive(true);
                gameObject.AddComponent<SphereCollider>().isTrigger = true;
                GetComponent<SphereCollider>().radius = 15;
                if (!isPiercing)
                {
                    GetComponent<MeshRenderer>().enabled = false;
                    GetComponent<TrailRenderer>().enabled = false;
                    GetComponent<Light>().enabled = false;
                    Destroy(gameObject, 0.05f);
                }
            }
            else if (!isPiercing)
                Destroy(gameObject);
        }
        if (!isEnemy)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(deathParticles, transform.position, transform.rotation).SetActive(true);
    }
}
