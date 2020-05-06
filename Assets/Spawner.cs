using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    GameObject window, enemy;
    bool cd;
    // Start is called before the first frame update
    void Start()
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
        Instantiate(enemy, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);
        cd = false;
    }
}
