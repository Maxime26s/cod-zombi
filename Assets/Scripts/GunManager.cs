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
        if (canUse && !player.isDown && nbGunsOwned > 1)
        {
            canUse = false;
            DropGun();
        }
        else if (canUse)
            canUse = false;
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

    public void DropGun()
    {
        GameObject dropped = null;
        foreach (Transform weapon in transform)
            if (weapon.GetComponent<Gun>().modele == gunsOwned[inUse - 1])
            {
                dropped = Instantiate(weapon.gameObject.GetComponent<Gun>().prefab, transform.position, this.transform.rotation);
                weapon.gameObject.GetComponent<Gun>().isOwned = false;
                weapon.gameObject.GetComponent<Gun>().inUse = false;
                weapon.gameObject.SetActive(false);
                gunsOwned.Remove(weapon.gameObject.GetComponent<Gun>().modele);
                nbGunsOwned--;
                break;
            }
        ChangerDarme(1);
        if (dropped != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane plane = new Plane(Vector3.up, new Vector3(0, transform.position.y, 0));
            if (plane.Raycast(ray, out float length))
            {
                Vector3 pos = (new Vector3(ray.GetPoint(length).x, transform.position.y, ray.GetPoint(length).z));
                dropped.transform.LookAt(pos);
            }
            dropped.GetComponent<Rigidbody>().AddForce(dropped.transform.forward * 500);
            dropped.GetComponent<Rigidbody>().useGravity = true;
            foreach (Transform child in dropped.transform)
            {
                if (child.gameObject.CompareTag("Dropped"))
                {
                    child.gameObject.SetActive(true);
                }
            }
            Destroy(dropped, 30f);
        }
    }
}