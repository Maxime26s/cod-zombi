using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyType { Slow, Normal, Jog, Fast, Sprint }
public enum State { Spawned, Chasing }
public enum DamageType { Hit, AOE, DOT }
public enum NumberType { Whole, Percent }
public class Enemy : MonoBehaviour
{
    public NavMeshAgent ai;
    GameObject player;
    public GameObject window, hpBar, deathParticle;
    bool cd;
    public bool electrified, frozen, poisoned;
    public State state = State.Spawned;
    public float health = 100f, maxHealth, speed, damageMultiplier, startTime, animSpeed;

    public EnemyType enemyType = EnemyType.Normal;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        ai = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        ai.speed = speed;
        ai.angularSpeed = 210;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeType(enemyType);
        switch (state)
        {
            case State.Spawned:
                if (window != null)
                    ai.SetDestination(window.transform.position);
                break;
            case State.Chasing:
                if (ai.enabled && !player.GetComponent<Player>().isDown)
                    ai.SetDestination(player.transform.position);
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !cd)
        {
            IEnumerator Attack(Player player)
            {
                cd = true;
                player.TakeDamage(5);
                yield return new WaitForSeconds(0.1f);
                cd = false;
            }
            StartCoroutine(Attack(collision.gameObject.GetComponent<Player>()));
        }
    }

    public void TakeDamage(float damage, Player player, DamageType damageType, NumberType numberType)
    {
        if (damage > 0)
        {
            if (!GameManager.Instance.oneShotEnabled)
            {
                switch (numberType)
                {
                    case NumberType.Whole:
                        health -= damage * damageMultiplier;
                        break;
                    case NumberType.Percent:
                        health -= damage * maxHealth * damageMultiplier;
                        break;
                }
                hpBar.SetActive(true);
                hpBar.GetComponent<Slider>().value = health / maxHealth;
                if (health <= 0)
                {
                    OnKill(player);
                }
                else
                {
                    switch (damageType)
                    {
                        case DamageType.Hit:
                            player.AddMoney(10);
                            break;
                        case DamageType.AOE:
                            player.AddMoney(5);
                            break;
                    }
                }
            }
            else
                OnKill(player);
        }
    }

    public void OnKill(Player player)
    {
        if (Random.Range(0, 100) <= 1)
            GameManager.Instance.Drop(transform.position);
        GameManager.Instance.enemies.Remove(gameObject);
        player.AddMoney(100);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(deathParticle, transform.position, Quaternion.identity);
    }

    public void ChangeType(EnemyType t)
    {
        switch (t)
        {
            case EnemyType.Slow:
                ai.speed = 2;
                break;
            case EnemyType.Normal:
                ai.speed = 3;
                break;
            case EnemyType.Jog:
                ai.speed = 4.5f;
                break;
            case EnemyType.Fast:
                ai.speed = 6;
                break;
            case EnemyType.Sprint:
                ai.speed = 7;
                break;
        }
    }
}
