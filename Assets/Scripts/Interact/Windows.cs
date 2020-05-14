using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windows : Interactable
{
    public GameObject destroyerCollider;
    private Coroutine routine = null;
    private float damageCD = 1f;
    private float nextDamageTime;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && destroyerCollider.GetComponent<Window>().cd && Time.time > nextDamageTime)
        {
            other.GetComponent<Player>().TakeDamage(10);
            nextDamageTime = Time.time + damageCD;
        }
            
    }

    public override void Interacting(Player player)
    {
        IEnumerator RepairWindow()
        {
            while (destroyerCollider.GetComponent<Window>().hp < 5)
            {
                destroyerCollider.GetComponent<Window>().Repair();
                player.GetComponent<Player>().AddMoney(price);
                if (player.GetComponentInChildren<PlayerInteractions>().quickActions)
                    yield return new WaitForSeconds(0.1f);
                else
                    yield return new WaitForSeconds(1f);
            }
        }
        routine = StartCoroutine(RepairWindow());
    }

    public override void UpdateMessage()
    {
        if (destroyerCollider.GetComponent<Window>().hp < 5)
            message = "Maintenez F pour réparer";
        else
            message = "Fenêtre réparée";
    }
}
