using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public int hp = 5;
    public GameObject[] planks = new GameObject[5];
    bool cd = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            //other.gameObject.GetComponent<Enemy>().ai.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
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
