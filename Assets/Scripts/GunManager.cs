using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public int inUse = 1;
    public int nbGunsOwned = 1;
    public List<ModelesGun> gunsOwned = new List<ModelesGun> { ModelesGun.Pistolet };
    public bool muleKick = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangerDarme(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && nbGunsOwned > 1)
        {
            ChangerDarme(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && nbGunsOwned > 2)
        {
            ChangerDarme(3);
        }
        if (Input.GetKeyDown(KeyCode.Tab) && nbGunsOwned > 1)
        {
            inUse++;
            if (inUse > nbGunsOwned)
                inUse = 1;
            ChangerDarme(inUse);
        }
    }

    public void ChangerDarme(int index)
    {
        foreach (Transform weapon in transform)
        {
            if (weapon.GetComponent<Gun>().isOwned && weapon.GetComponent<Gun>().modele == gunsOwned[index - 1])
            {
                weapon.GetComponent<Gun>().inUse = true;
                weapon.gameObject.SetActive(true);
                this.GetComponentInParent<Player>().gun = weapon.gameObject;
                inUse = index;
                foreach (Transform weapon2 in transform)
                {
                    if (weapon2.GetComponent<Gun>().isOwned && weapon2.GetComponent<Gun>().inUse && weapon2.GetComponent<Gun>().modele != gunsOwned[inUse - 1])
                    {
                        weapon2.GetComponent<Gun>().inUse = false;
                        weapon2.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}