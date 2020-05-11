using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public int inUse = 1;
    public int nbGunsOwned = 1;
    public List<ModelesGun> gunsOwned = new List<ModelesGun> { ModelesGun.M1911 };
    public bool muleKick = false;

    private bool coolDownOver;
    private float pressTime;
    private float coolDown = 1f;
    private bool canUse = false;
    private Player player;

    private void Start()
    {
        player = this.GetComponentInParent<Player>();
    }

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

        /*
        if (Input.GetKey(KeyCode.Q) && !coolDownOver)
        {
            pressTime = Time.time + coolDown;
            coolDownOver = true;
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            coolDownOver = false;
        }
        if (Time.time >= pressTime && coolDownOver)
        {
            canUse = true;
            coolDownOver = false;
        }
        if (canUse && !player.isDown && nbGunsOwned >= 1)
        {
            DropGun();
            canUse = false;
        }
        */
    }

    public void ChangerDarme(int index)
    {
        foreach (Transform weapon in transform)
        {
            if (weapon.GetComponent<Gun>().isOwned && weapon.GetComponent<Gun>().modele == gunsOwned[index - 1])
            {
                weapon.GetComponent<Gun>().inUse = true;
                weapon.gameObject.SetActive(true);
                this.GetComponentInParent<Player>().gun = weapon.GetComponent<Gun>();
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
        this.GetComponentInParent<Player>().UpdateBulletSP();
    }

    /*
    public void DropGun()
    {
        GameObject dropped = null;
        foreach (Transform weapon in transform)
            if (weapon.GetComponent<Gun>().modele == gunsOwned[inUse - 1])
                dropped = Instantiate(weapon.gameObject, this.transform.position, this.transform.rotation);
        if (dropped != null)
        {
            dropped.GetComponent<Rigidbody>().AddTorque(transform.forward*10);
            //dropped.GetComponent<Rigidbody>().useGravity = true;
            Destroy(dropped, 2f);
        }
    }
    */
}