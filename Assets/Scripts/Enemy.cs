using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum State { Spawned, Chasing }
public class Enemy : MonoBehaviour
{
    public NavMeshAgent ai;
    GameObject player;
    GameObject[] translation = new GameObject[2];
    public GameObject window, hpBar;
    bool cd;
    public State state = State.Spawned;
    public float health = 100f, maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        ai = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Spawned:
                if(window != null)
                    ai.SetDestination(window.transform.position);
                break;
            case State.Chasing:
                if (ai.enabled)
                    ai.SetDestination(player.transform.position);
                break;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && !cd)
        {
            StartCoroutine(Attack(collision.gameObject.GetComponent<Player>()));
        }
    }

    IEnumerator Attack(Player player)
    {
        cd = true;
        player.hp--;
        player.regenTime = player.regenCoolDown + Time.time;
        player.healthBar.GetComponent<Slider>().value = player.hp / (float)player.maxhealth;
        if (player.hp <= 0)
            Destroy(player.gameObject);
        yield return new WaitForSeconds(0.1f);
        cd = false;
    }
}
