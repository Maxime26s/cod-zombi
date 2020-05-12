using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Interactable
{
    public List<GameObject> box;

    public override void Interacting(Player player)
    {
        if (player.money >= price)
        {
            IEnumerator AfficherGun()
            {
                blocked = true;
                foreach (GameObject gunShop in box)
                {
                    gunShop.GetComponentInChildren<Interactable>().price = 0;
                }
                GameObject gunTemp = Instantiate(box[Random.Range(0, box.Count)], transform.position + transform.up, transform.rotation * Quaternion.Euler(0, 90, 0));
                yield return new WaitForSeconds(5f);
                if (gunTemp != null)
                    Destroy(gunTemp);
                blocked = false;
            }
            StartCoroutine(AfficherGun());
            player.GetComponent<Player>().money -= price;
        }
    }
}
