using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 direction;
    public float speed;

    private GameObject assignedPlayer;

    public Bullet(GameObject player)
    {
        assignedPlayer = player;
    }
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
            if (other.tag == "Ennemy")
            {
                other.GetComponent<Enemy>().health -= assignedPlayer.GetComponent<Player>().gun.GetComponent<Gun>().damage;
                assignedPlayer.GetComponent<Player>().money += (int)assignedPlayer.GetComponent<Player>().gun.GetComponent<Gun>().damage;
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
