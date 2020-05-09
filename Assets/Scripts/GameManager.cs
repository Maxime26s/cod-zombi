using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    List<GameObject> windows = new List<GameObject>();
    List<GameObject> guns = new List<GameObject>();
    List<GameObject> boxes = new List<GameObject>();
    public List<GameObject> powerUps = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
            for (int i = 0; i < players.Count; i++)
                players[i].GetComponent<Player>().pointMultiplier = 2;
            yield return new WaitForSeconds(10f);
            for (int i = 0; i < players.Count; i++)
                players[i].GetComponent<Player>().pointMultiplier = 1;
        }
        StartCoroutine(DoublePointManager());
    }
    public void OneShot()
    {
        IEnumerator OneShotManager()
        {
            for (int i = 0; i < players.Count; i++)
                players[i].GetComponent<Player>().oneShot = true;
            yield return new WaitForSeconds(10f);
            for (int i = 0; i < players.Count; i++)
                players[i].GetComponent<Player>().oneShot = false;
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
}
