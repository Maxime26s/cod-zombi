using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
    public List<GameObject> spawners = new List<GameObject>();
    public GameObject enemyPrefab;
    bool spawning = true, spawningCd;

    // Update is called once per frame
    void Update()
    {
        if (spawning && !spawningCd && players.Count > 0 && GameManager.Instance.zombieSpawned < GameManager.Instance.zombieMax && !GameManager.Instance.pause)
        {
            IEnumerator Spawn()
            {
                spawningCd = true;
                GameObject spawner = spawners[Random.Range(0, spawners.Count)];
                GameObject go = Instantiate(enemyPrefab, spawner.transform.position, Quaternion.identity);
                GameManager.Instance.EnemySpawned(go);
                go.GetComponent<Enemy>().window = spawner.GetComponent<Spawner>().window;
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

    public void SpawnLot()
    {
        for(int i = 0; i < 50; i++)
        {
            GameObject go = Instantiate(enemyPrefab, spawners[0].transform.position, Quaternion.identity);
            GameManager.Instance.enemies.Add(go);
            GameManager.Instance.enemies[GameManager.Instance.enemies.Count - 1].GetComponent<Enemy>().window = spawners[0].GetComponent<Spawner>().window;
        }
    }
}
