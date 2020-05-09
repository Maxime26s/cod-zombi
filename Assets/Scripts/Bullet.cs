using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;
    public bool oneShot, piercing;
    public float pointMultiplier;

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
                if (oneShot)
                    OnKill(other, other.GetComponent<Enemy>());
                else
                {
                    Enemy enemy = other.GetComponent<Enemy>();
                    enemy.health -= (player.gun.GetComponent<Gun>().damage * player.gun.GetComponent<Gun>().damageMultiplier);
                    enemy.hpBar.SetActive(true);
                    enemy.hpBar.GetComponent<Slider>().value = enemy.health/enemy.maxHealth;
                    if(enemy.health <= 0)
                    {
                        OnKill(other, enemy);
                    }
                    else
                        player.AddMoney(10);
                }
                if(!piercing)
                    Destroy(gameObject);
            }
            else
                Destroy(gameObject);
        }
    }

    private void OnKill(Collider other, Enemy enemy)
    {
        if (Random.Range(0, 100) <= 1)
            GameObject.Find("GameManager").GetComponent<GameManager>().Drop(enemy.transform.position);
        Destroy(other.gameObject);
        player.AddMoney(100);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
