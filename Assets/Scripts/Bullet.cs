using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float explosionDamage, electricityDamage, fireDamage;
    public float dissolveTime;
    public bool oneShot, piercing, explosive, dissolve, electrifying, fire;
    public GameObject explosionPrefab, deathParticles, electricityFinder, firePrefab;
    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction.normalized * speed * Time.deltaTime;
        if (dissolve)
            transform.localScale -= new Vector3(dissolveTime, dissolveTime, dissolveTime) * Time.deltaTime;
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
                    if(electrifying)
                    {
                        /*
                        IEnumerator Electrify()
                        {
                            enemy.electrified = true;
                            GameObject go = Instantiate(electricityFinder, transform.position, Quaternion.identity);
                            go.GetComponent<Electricity>().electricityDamage = electricityDamage;
                            go.GetComponent<Electricity>().player = player;
                            go.GetComponent<SphereCollider>().enabled = true;
                            yield return new WaitForSeconds(0.2f);
                            if(enemy != null)
                                enemy.electrified = false;
                        }
                        StartCoroutine(Electrify());
                        */
                        GameObject go = Instantiate(electricityFinder, enemy.transform);
                        go.GetComponent<Electricity>().electricityDamage = electricityDamage;
                        go.GetComponent<Electricity>().player = player;
                        go.GetComponent<SphereCollider>().enabled = true;
                    }
                    if (fire && enemy.GetComponentInChildren<OnFire>() == null)
                    {
                        GameObject go = Instantiate(firePrefab, enemy.transform);
                        go.GetComponent<OnFire>().player = player;
                        go.GetComponent<OnFire>().enemy = enemy;
                        go.GetComponent<OnFire>().fireDamage = fireDamage;
                        go.GetComponent<OnFire>().onFire = true;
                    }
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

    private void OnDestroy()
    {
        Instantiate(deathParticles, transform.position, transform.rotation);
    }
}
