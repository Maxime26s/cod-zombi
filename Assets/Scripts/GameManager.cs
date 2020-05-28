using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    List<GameObject> players = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    List<GameObject> windows = new List<GameObject>();
    List<GameObject> guns = new List<GameObject>();
    List<GameObject> boxes = new List<GameObject>();
    public List<GameObject> powerUps = new List<GameObject>();
    public TextMeshProUGUI entityText, roundText;
    public bool oneShotEnabled, infiniteAmmoEnabled, pause;
    public float pointMultiplier;
    public int round, zombieMax, zombieSpawned;

    private void Start()
    {
        CheckRound();
        players = GameObject.FindGameObjectsWithTag("Player").ToList();
    }

    void Update()
    {
        entityText.text = enemies.Count.ToString();
    }

    public void EnemySpawned(GameObject enemy)
    {
        enemies.Add(enemy);
        zombieSpawned++;
        CheckRound();
    }

    void CheckRound()
    {
        if(zombieSpawned >= zombieMax)
        {
            IEnumerator ChangeRound()
            {
                pause = true;
                round++;
                zombieSpawned = 0;
                zombieMax = 24 + 2 * round * players.Count;
                roundText.text = round.ToString();
                yield return new WaitForSeconds(5f);
                pause = false;
            }
            StartCoroutine(ChangeRound());
        }
    }

    public void Drop(Vector3 position)
    {
        Instantiate(powerUps[Random.Range(0, powerUps.Count)], position, Quaternion.identity);
    }

    public void MaxAmmo()
    {
        for(int i = 0; i < players.Count; i++)
            players[i].GetComponent<Player>().gun.GetComponent<Gun>().ammo = players[i].GetComponent<Player>().gun.GetComponent<Gun>().maxAmmo;
    }

    public void Carpenter()
    {
        for(int i = 0; i < windows.Count; i++)
        {
            Window window = windows[i].GetComponent<Window>();
            window.hp = 5;
            for(int j = 0; j < window.planks.Length; j++)
                window.planks[j].SetActive(true);
        }
        for (int i = 0; i < players.Count; i++)
            players[i].GetComponent<Player>().AddMoney(200);
    }

    public void MOAB()
    {
        for (int i = enemies.Count-1; i >= 0; i--)
        {
            Destroy(enemies[i]);
            enemies.RemoveAt(i);
        }
        for (int i = 0; i < players.Count; i++)
            players[i].GetComponent<Player>().AddMoney(400);
    }

    public void DoublePoint()
    {
        IEnumerator DoublePointManager()
        {
            pointMultiplier = 2;
            yield return new WaitForSeconds(10f);
            pointMultiplier = 1;
        }
        StartCoroutine(DoublePointManager());
    }
    public void OneShot()
    {
        IEnumerator OneShotManager()
        {
            oneShotEnabled = true;
            yield return new WaitForSeconds(10f);
            oneShotEnabled = false;
        }
        StartCoroutine(OneShotManager());
    }
    public void FireSale()
    {
        IEnumerator FireSaleManager()
        {
            for (int i = 0; i < boxes.Count; i++)
                boxes[i].GetComponent<Interactable>().price = 10;
            yield return new WaitForSeconds(10f);
            for (int i = 0; i < boxes.Count; i++)
                boxes[i].GetComponent<Interactable>().price = 900;
        }
        StartCoroutine(FireSaleManager());
    }

    public void ReviveAll()
    {
        foreach(GameObject player in players)
        {
            if (player.GetComponent<Player>().isDown)
                player.GetComponent<Player>().Revive();
        }
    }

    public void InfiniteAmmo()
    {
        IEnumerator InfiniteAmmoManager()
        {
            infiniteAmmoEnabled = true;
            yield return new WaitForSeconds(10f);
            infiniteAmmoEnabled = false;
        }
        StartCoroutine(InfiniteAmmoManager());
    }
}
