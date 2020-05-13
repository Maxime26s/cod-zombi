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

    public override void UpdateMessage()
    {
        if (porteOuverte)
            message = "Cette porte est ouverte";
        else
            message = "Maintez F pour ouvrir la porte\n Coût: " + price;
    }
}
