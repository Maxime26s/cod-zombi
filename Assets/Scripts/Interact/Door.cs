using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Interactable
{
    private bool porteOuverte = false;
    public override void Interacting(Player player)
    {
        if (!porteOuverte && player.GetComponent<Player>().money >= price)
        {
            foreach (Transform pivot in transform)
            {
                if (pivot.gameObject.CompareTag("Pivot"))
                    pivot.Rotate(0, 90, 0);
            }
            porteOuverte = true;
            player.GetComponent<Player>().money -= price;
        }
    }
}
