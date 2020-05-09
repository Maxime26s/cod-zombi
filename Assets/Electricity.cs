using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Electricity : MonoBehaviour
{
    public float electricityDamage;
    public GameObject electricityParticles;
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SphereCollider>().enabled == true)
            Destroy(gameObject, 1f);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (!enemy.electrified)
            {
                enemy.Electrify();
                Instantiate(gameObject, enemy.transform);
                enemy.TakeDamage(electricityDamage, player);

            }
        }
    }
}
