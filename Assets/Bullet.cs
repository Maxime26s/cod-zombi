using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

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
                enemy.health -= (player.gun.GetComponent<Gun>().damage * player.gun.GetComponent<Gun>().damageMultiplier);
                player.money += (int)(player.gun.GetComponent<Gun>().damage * player.gun.GetComponent<Gun>().damageMultiplier);
                enemy.hpBar.SetActive(true);
                enemy.hpBar.GetComponent<Slider>().value = enemy.health/enemy.maxHealth;
                if(enemy.health <= 0)
                {
                    Destroy(other.gameObject);
                }
             
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
