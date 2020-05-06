using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    int lastChanged = 1;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && lastChanged != 1)
        {
            changerDarme();
            lastChanged = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && lastChanged != 2)
        {
            changerDarme();
            lastChanged = 1;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            changerDarme();
            if(lastChanged == 2)
            {
                lastChanged = 1;
            }
            else
            {
                lastChanged = 2;
            }
        }
        foreach (Transform weapon in transform)
        {
            if (weapon.GetComponent<Gun>().inUse)
                this.GetComponentInParent<Player>().gun = weapon.gameObject;
        }
    }

    void changerDarme()
    {
        foreach(Transform weapon in transform)
        {
            if (weapon.GetComponent<Gun>().isOwned && weapon.GetComponent<Gun>().inUse)
            {
                weapon.GetComponent<Gun>().inUse = false;
                weapon.gameObject.SetActive(false);
            }
            else if (weapon.GetComponent<Gun>().isOwned)
            {
                weapon.GetComponent<Gun>().inUse = true;
                weapon.gameObject.SetActive(true);
            }
        }
    }
}
