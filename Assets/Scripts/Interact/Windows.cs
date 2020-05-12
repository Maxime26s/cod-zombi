using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windows : Interactable
{
    public GameObject destroyerCollider;
    private Coroutine routine = null;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F) && routine != null)
        {
            StopCoroutine(routine);
            routine = null;
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
                yield return new WaitForSeconds(1f);
            }
        }
        routine = StartCoroutine(RepairWindow());
    }
}
