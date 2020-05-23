using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float bulletSelfDestruct;
    public int hitAmount;
    public GameObject explosionPrefab, deathParticles, firePrefab, icePrefab, poisonPrefab, electricPrefab;
    public Player player;
    public PiercingModifier piercing;
    public ExplosiveModifier explosive;
    public FireModifier fire;
    public ElectricModifier electric;
    public IceModifier ice;
    public PoisonModifier poison;
    public Coroutine destroyCoroutine;

    private void OnEnable()
    {
        if(destroyCoroutine!=null)
            StopCoroutine(destroyCoroutine);
        IEnumerator Destroy()
        {
            yield return new WaitForSeconds(bulletSelfDestruct);
            gameObject.SetActive(false);
        }
        destroyCoroutine = StartCoroutine(Destroy());
        GetComponent<TrailRenderer>().Clear();
        hitAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            if (other.CompareTag("Enemy"))
            {
                if (explosive.enabled)
                    Explosive(other);
                else
                    Hit(other, other.GetComponent<Enemy>());
                Piercing();
            }
            else
                gameObject.SetActive(false);
        }
    }

    void Hit(Collider other, Enemy enemy)
    {
        Electricity(other, enemy);
        Fire(enemy);
        Ice(other, enemy);
        Poison(enemy);
        if (explosive.enabled)
            enemy.TakeDamage(player.gun.GetComponent<Gun>().damage * player.gun.GetComponent<Gun>().damageMultiplier, player, DamageType.AOE, NumberType.Whole);
        else
            enemy.TakeDamage(player.gun.GetComponent<Gun>().damage * player.gun.GetComponent<Gun>().damageMultiplier, player, DamageType.Hit, NumberType.Whole);
    }
    
    void Piercing()
    {
        if (piercing.enabled)
        {
            if (hitAmount < piercing.amount)
                hitAmount++;
            else
                gameObject.SetActive(false);
        }
        else
            gameObject.SetActive(false);
    }
    
    void Explosive(Collider other)
    {
        GameObject go = ObjectPooler.Instance.GetPooledObject(player.playerName + "BulletExplosion");
        go.transform.position = transform.position;
        go.SetActive(true);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosive.radius, 1 << 9);
        foreach (Collider collider in colliders)
        {
            Hit(collider, collider.GetComponent<Enemy>());
        }
    }

    void Electricity(Collider other, Enemy enemy)
    {
        if (electric.enabled)
        {
            ElectricityManager em = Instantiate(electricPrefab).GetComponent<ElectricityManager>();
            em.electric = electric;
            em.player = player;
            em.enemies.Add(other);
            em.ready = true;
            enemy.TakeDamage(electric.damage, player, DamageType.AOE, NumberType.Whole);
        }
    }

    void Fire(Enemy enemy)
    {
        if (fire.enabled && enemy.GetComponentInChildren<OnFire>() == null)
        {
            OnFire onFire = Instantiate(firePrefab, enemy.transform).GetComponent<OnFire>();
            onFire.player = player;
            onFire.enemy = enemy;
            onFire.fireDamage = fire.damage;
            onFire.onFire = true;
        }
    }

    void Ice(Collider other, Enemy enemy)
    {
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
    }

    void Poison(Enemy enemy)
    {
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
    }

    private void OnDisable()
    {
        GameObject go = ObjectPooler.Instance.GetPooledObject(player.playerName + "BulletDeath");
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        go.SetActive(true);
    }
}
