using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject window, enemy;
    bool cd;
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if(!cd)
            StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        cd = true;
        GameObject go = Instantiate(enemy, transform.position, Quaternion.identity);
        go.GetComponent<Enemy>().window = window;
        yield return new WaitForSeconds(2f);
        cd = false;
    }
}
