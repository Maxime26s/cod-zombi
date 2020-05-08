using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    GameManager gameManager;
    List<GameObject> players = new List<GameObject>();
    public List<GameObject> spawners = new List<GameObject>();
    public GameObject enemyPrefab;
    bool spawning = true, spawningCd;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawning && !spawningCd && players.Count > 0)
        {
            IEnumerator Spawn()
            {
                spawningCd = true;
                GameObject spawner = spawners[Random.Range(0, spawners.Count)];
                GameObject go = Instantiate(enemyPrefab, spawner.transform.position, Quaternion.identity);
                gameManager.enemies.Add(go);
                gameManager.enemies[gameManager.enemies.Count-1].GetComponent<Enemy>().window = spawner.GetComponent<Spawner>().window;
                yield return new WaitForSeconds(1f);
                spawningCd = false;
            }
            StartCoroutine(Spawn());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        players.Add(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        players.Remove(other.gameObject);
    }
}
