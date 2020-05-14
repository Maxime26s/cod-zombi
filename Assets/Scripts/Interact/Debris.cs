﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : Interactable
{
    public override void Interacting(Player player)
    {
        if (player.GetComponent<Player>().money >= price)
            Destroy(transform.parent.gameObject);
    }

    public override void UpdateMessage()
    {
        message = "Maintenez F pour dégager les obstacles\nCoût: " + price;
    }
}
