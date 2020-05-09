using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float explosionDamage;
    public bool oneShot, piercing, explosive;
    public GameObject explosionPrefab;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player")
        {
            if (other.tag == "Enemy")
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (oneShot)
                    enemy.OnKill(player);
                else
                {
                    if (explosive)
                    {
                        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                        go.GetComponent<Explosion>().damage = explosionDamage;
                        go.GetComponent<Explosion>().player = player;
                        go.GetComponent<SphereCollider>().enabled = true;
                    }
                    enemy.TakeDamage(player.gun.GetComponent<Gun>().damage * player.gun.GetComponent<Gun>().damageMultiplier, player);
                }
                if(!piercing)
                    Destroy(gameObject);
            }
            else
                Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
