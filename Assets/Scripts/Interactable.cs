﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeInteractable { Gun, Door, Cola, Window, Box }
public class Interactable : MonoBehaviour
{
    public int price;
    public float interactionRadius = 1;
    public TypeInteractable interactable;
    public GameObject destroyerCollider, gunInShop;

    public float coolDown = 1;

    //private List<Gun> box;
    private bool canUse = false;
    private float pressTime;
    private bool coolDownOver = false;

    private void Start()
    {
        //box.Add();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F) && coolDownOver == false)
        {
            pressTime = Time.time + coolDown;
            coolDownOver = true;
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            coolDownOver = false;
        }
        if (Time.time >= pressTime && coolDownOver == true)
        {
            canUse = true;
            coolDownOver = false;
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        GameObject col = collision.gameObject;
        if (col.CompareTag("Player") && canUse)
        {
            Interacting(col);
            canUse = false;
        }
    }

    private void Interacting(GameObject player)
    {
        switch (interactable)
        {
            case TypeInteractable.Gun:
                Debug.Log("lol");
                AcheterGun(player);
                break;

            case TypeInteractable.Door:
                player.GetComponent<Player>().money -= price;
                break;
            case TypeInteractable.Window:
                Debug.Log("lol");
                if (destroyerCollider.GetComponent<Window>().hp < 5)
                {
                    Debug.Log("touchingPlayer");
                    destroyerCollider.GetComponent<Window>().Repair();
                    player.GetComponent<Player>().money += price;
                }
                break;
            case TypeInteractable.Cola:
                player.GetComponent<Player>().money -= price;
                break;
            case TypeInteractable.Box:
                player.GetComponent<Player>().money -= price;
                break;
        }
    }


    private void AcheterGun(GameObject player)
    {
        if (player.GetComponent<Player>().money >= price)
        {
            player.GetComponent<Player>().money -= price;
            if (player.GetComponentInChildren<GunManager>().gunOwned == 1)
            {
                foreach (Transform weapon in player.GetComponentInChildren<GunManager>().transform)
                {
                    if ((weapon.gameObject.GetComponent<Gun>().modele == gunInShop.GetComponent<Gun>().modele)
                        && !weapon.gameObject.GetComponent<Gun>().isOwned)
                    {
                        foreach (Transform weapon2 in player.GetComponentInChildren<GunManager>().transform)
                        {
                            if (weapon2.gameObject.GetComponent<Gun>().inUse)
                            {
                                weapon2.gameObject.SetActive(false);
                                weapon2.gameObject.GetComponent<Gun>().inUse = false;
                            }
                        }
                        weapon.gameObject.GetComponent<Gun>().isOwned = true;
                        weapon.gameObject.GetComponent<Gun>().inUse = true;
                        weapon.gameObject.SetActive(true);
                        player.GetComponentInChildren<GunManager>().gunOwned = 2;
                        
                    }
                }
            }
            else if (player.GetComponentInChildren<GunManager>().gunOwned == 2)
            {
                foreach (Transform weapon in player.GetComponentInChildren<GunManager>().transform)
                {
                    if ((weapon.gameObject.GetComponent<Gun>().modele == gunInShop.GetComponent<Gun>().modele)
                        && !weapon.gameObject.GetComponent<Gun>().isOwned)
                    {
                        foreach (Transform weapon2 in player.GetComponentInChildren<GunManager>().transform)
                        {
                            if (weapon2.gameObject.GetComponent<Gun>().inUse)
                            {
                                weapon2.gameObject.GetComponent<Gun>().inUse = false;
                                weapon2.gameObject.GetComponent<Gun>().isOwned = false;
                                weapon2.gameObject.SetActive(false);
                            }
                        }
                        weapon.gameObject.GetComponent<Gun>().isOwned = true;
                        weapon.gameObject.GetComponent<Gun>().inUse = true;
                        weapon.gameObject.SetActive(true);
                        
                    }
                }
            }
        }
    }
}


