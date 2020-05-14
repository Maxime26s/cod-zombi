using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public int hp = 5;
    public GameObject[] planks = new GameObject[5];
    [HideInInspector]
    public bool cd = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            //other.gameObject.GetComponent<Enemy>().ai.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (hp != 0 && !cd)
            {

                IEnumerator TakeDamage()
                {
                    cd = true;
                    hp--;
                    planks[hp].SetActive(false);
                    yield return new WaitForSeconds(1f);
                    cd = false;
                }
                StartCoroutine(TakeDamage());
            }
            else if(hp == 0)
            {
                other.GetComponent<Enemy>().state = State.Chasing;
            }
        }
    }

    public void Repair()
    {
        if (hp < 5)
        {
            planks[hp].SetActive(true);
            hp++;
        }
    }
}
