using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum State { Spawned, TravelWindow, Chasing }
public class Enemy : MonoBehaviour
{
    public NavMeshAgent ai;
    GameObject player;
    GameObject[] translation = new GameObject[2];
    public GameObject window;
    bool cd;
    public State state = State.Spawned;
    // Start is called before the first frame update
    void Start()
    {
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
            case State.TravelWindow:
                break;
            case State.Chasing:
                if (ai.enabled)
                    ai.SetDestination(player.transform.position);
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ai.enabled = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ai.enabled = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(Attack(gameObject.GetComponent<Player>()));
        }
    }

    IEnumerator Attack(Player player)
    {
        cd = true;
        player.hp -= 1;
        yield return new WaitForSeconds(0.5f);
        cd = false;
    }
}
