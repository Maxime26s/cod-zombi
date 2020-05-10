using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public float explosionDamage, electricityDamage, fireDamage;
    public float dissolveTime;
    public bool oneShot, piercing, explosive, dissolve, electrifying, fire, ice, poison;
    public GameObject explosionPrefab, deathParticles, electricityFinder, effectPrefab;
    public Material fireMat, poisonMat, iceMat;
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
                        GameObject go = Instantiate(effectPrefab, enemy.transform);
                        go.AddComponent<OnFire>();
                        go.GetComponent<OnFire>().player = player;
                        go.GetComponent<OnFire>().enemy = enemy;
                        go.GetComponent<OnFire>().fireDamage = fireDamage;
                        go.GetComponent<OnFire>().onFire = true;
                    }
                    if (ice && !enemy.frozen)
                    {
                        GameObject go = Instantiate(effectPrefab, enemy.transform);
                        go.GetComponent<ParticleSystemRenderer>().material = iceMat;
                        var main = go.GetComponent<ParticleSystem>().main;
                        main.startColor = iceMat.color - new Color32(50, 50, 50, 0);
                        go.GetComponent<Light>().color = iceMat.color;
                        IEnumerator Freeze()
                        {
                            enemy.frozen = true;
                            other.gameObject.GetComponent<NavMeshAgent>().speed = enemy.speed * 0.8f;
                            yield return new WaitForSeconds(2.5f);
                            other.gameObject.GetComponent<NavMeshAgent>().speed = enemy.speed;
                            enemy.frozen = false;
                        }
                        enemy.StartCoroutine(Freeze());
                    }
                    if (poison && !enemy.poisoned)
                    {
                        GameObject go = Instantiate(effectPrefab, enemy.transform);
                        go.GetComponent<ParticleSystemRenderer>().material = poisonMat;
                        var main = go.GetComponent<ParticleSystem>().main;
                        main.startColor = poisonMat.color - new Color32(50, 50, 50, 0);
                        go.GetComponent<Light>().color = poisonMat.color;
                        IEnumerator Poison()
                        {
                            enemy.poisoned = true;
                            yield return new WaitForSeconds(2.5f);
                            enemy.poisoned = false;
                        }
                        enemy.StartCoroutine(Poison());
                    }
                    if (explosive)
                    {
                        GameObject go = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                        go.GetComponent<ParticleSystemRenderer>().sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;
                        go.GetComponent<ParticleSystemRenderer>().trailMaterial = GetComponent<MeshRenderer>().sharedMaterial;
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
        Instantiate(deathParticles, transform.position, transform.rotation).GetComponent<ParticleSystemRenderer>().sharedMaterial = GetComponent<MeshRenderer>().sharedMaterial;
    }
}
