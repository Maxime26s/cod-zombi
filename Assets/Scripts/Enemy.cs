﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum State { Spawned, Chasing }
public class Enemy : MonoBehaviour
{
    public NavMeshAgent ai;
    GameObject player;
    public GameObject window, hpBar;
    bool cd;
    public bool electrified;
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

    public void TakeDamage(float damage, Player player)
    {
        health -= damage;
        hpBar.SetActive(true);
        hpBar.GetComponent<Slider>().value = health / maxHealth;
        if (health <= 0)
        {
            OnKill(player);
        }
        else
            player.AddMoney(10);
    }

    public void OnKill(Player player)
    {
        if (Random.Range(0, 100) <= 1)
            GameObject.Find("GameManager").GetComponent<GameManager>().Drop(transform.position);
        Destroy(gameObject);
        player.AddMoney(100);
    }

    public void Electrify()
    {
        IEnumerator Electrify()
        {
            electrified = true;
            yield return new WaitForSeconds(0.33f);
            electrified = false;
        }
        StartCoroutine(Electrify());
    }
    IEnumerator Attack(Player player)
    {
        cd = true;
        player.TakeDamage(5);
        yield return new WaitForSeconds(0.1f);
        cd = false;
    }
}
