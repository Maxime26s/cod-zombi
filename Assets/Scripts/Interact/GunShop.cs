using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShop : Interactable
{
    public GameObject gunInShop;

    public override void Interacting(Player player)
    {
        bool alreadyOwned = false;
        GunManager gm = player.GetComponentInChildren<GunManager>();
        foreach (Transform weapon in gm.transform)
        {
            Gun g = weapon.gameObject.GetComponent<Gun>();
            if ((g.modele == gunInShop.GetComponent<Gun>().modele) && g.isOwned)
                alreadyOwned = true;
        }
        if (player.money >= price && !alreadyOwned)
        {
            player.money -= price;
            if (gm.nbGunsOwned == 1 || (gm.nbGunsOwned == 2 && gm.muleKick))
            {
                AjouterNewGun(player, gm.nbGunsOwned + 1);
            }
            else if ((gm.nbGunsOwned == 2 && !gm.muleKick) || gm.nbGunsOwned == 3)
            {
                foreach (Transform weapon in gm.transform)
                {
                    Gun g = weapon.gameObject.GetComponent<Gun>();
                    if (g.modele == gunInShop.GetComponent<Gun>().modele)
                        g.isOwned = true;
                    if (g.inUse)
                    {
                        g.isOwned = false;
                        g.inUse = false;
                        weapon.gameObject.SetActive(false);
                    }
                }
                gm.gunsOwned.RemoveAt(gm.inUse - 1);
                gm.gunsOwned.Insert(gm.inUse - 1, gunInShop.GetComponent<Gun>().modele);
                gm.ChangerDarme(gm.inUse);
            }
        }
    }
    private void AjouterNewGun(Player player, int index)
    {
        GunManager gm = player.GetComponentInChildren<GunManager>();
        foreach (Transform weapon in gm.transform)
        {
            Gun g = weapon.gameObject.GetComponent<Gun>();
            if (g.modele == gunInShop.GetComponent<Gun>().modele)
                g.isOwned = true;
        }
        gm.nbGunsOwned = index;
        gm.gunsOwned.Add(gunInShop.GetComponent<Gun>().modele);
        gm.ChangerDarme(index);
    }
}
