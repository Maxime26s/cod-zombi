using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum TypeInteractable { Gun, Door, Cola, Window, Box }
public enum TypeCola { StaminUp, JuggerNog, ElectricCherry, MuteKick, DoubleTap, Quick, DeadshotDai, Random}
public class Interactable : MonoBehaviour
{
    public int price;
    public float interactionRadius = 1;
    public TypeInteractable interactable;
    public GameObject destroyerCollider, gunInShop;
    public List<GameObject> box;
    public float coolDown = 1;
    public TypeCola typeCola;
    public List<TypeCola> listeColas = new List<TypeCola>
    { TypeCola.JuggerNog, TypeCola.MuteKick, TypeCola.DoubleTap, TypeCola.DeadshotDai};

    private bool canUse = false;
    private float pressTime;
    private bool coolDownOver = false;
    private bool porteOuverte = false;
    private bool blocked = false;


    private void Update()
    {

    }

    private void OnTriggerStay(Collider collision)
    {
        GameObject col = collision.gameObject;
        if (col.CompareTag("Player") && !blocked)
        {
            if (Input.GetKey(KeyCode.F) && !coolDownOver)
            {
                pressTime = Time.time + coolDown;
                coolDownOver = true;
            }
            if (Input.GetKeyUp(KeyCode.F))
            {
                coolDownOver = false;
            }
            if (Time.time >= pressTime && coolDownOver)
            {
                canUse = true;
                coolDownOver = false;
            }
            if (canUse)
            {
                Interacting(col);
                canUse = false;
            }
        }
    }

    private void Interacting(GameObject player)
    {
        switch (interactable)
        {
            case TypeInteractable.Gun:
                AcheterGun(player);
                break;

            case TypeInteractable.Door:
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

                break;
            case TypeInteractable.Window:
                if (destroyerCollider.GetComponent<Window>().hp < 5)
                {
                    Debug.Log("touchingPlayer");
                    destroyerCollider.GetComponent<Window>().Repair();
                    player.GetComponent<Player>().AddMoney(price);
                }
                break;
            case TypeInteractable.Cola:
                if (player.GetComponent<Player>().money >= price && player.GetComponent<Player>().nbCola < 4)
                {
                    AcheterCola(player);
                    player.GetComponent<Player>().money -= price;
                    player.GetComponent<Player>().nbCola++;
                }
                break;
            case TypeInteractable.Box:
                if (player.GetComponent<Player>().money >= price)
                {
                    IEnumerator AfficherGun()
                    {
                        blocked = true;
                        foreach (GameObject gunShop in box)
                        {
                            gunShop.GetComponent<Interactable>().price = 0;
                        }
                        GameObject gunTemp = Instantiate(box[Random.Range(0, box.Count)], transform.position + transform.up, transform.rotation);
                        yield return new WaitForSeconds(5f);
                        //coolDownOver = true;
                        if (gunTemp != null)
                            Destroy(gunTemp);
                        blocked = false;
                    }
                    StartCoroutine(AfficherGun());
                    player.GetComponent<Player>().money -= price;
                }
                break;
        }
    }

    private void AcheterCola(GameObject player)
    {
        switch (typeCola)
        {
            case TypeCola.DeadshotDai:
                foreach (Transform enfant in player.transform)
                    if (enfant.gameObject.GetComponent<GunManager>() != null)
                        foreach (Transform gun in enfant)
                            gun.GetComponent<Gun>().damageMultiplier = 1.3f;
                break;
            case TypeCola.DoubleTap:
                foreach (Transform enfant in player.transform)
                    if (enfant.gameObject.GetComponent<GunManager>() != null)
                        foreach (Transform gun in enfant)
                            gun.GetComponent<Gun>().fireRateMultiplier = 1.3f;
                break;
            case TypeCola.ElectricCherry:
                break;
            case TypeCola.JuggerNog:
                player.GetComponent<Player>().maxhealth = 200;
                break;
            case TypeCola.MuteKick:
                player.GetComponentInChildren<GunManager>().muleKick = true;
                break;
            case TypeCola.Quick:
                break;
            case TypeCola.Random:
                typeCola = listeColas[Random.Range(0, listeColas.Count)];
                AcheterCola(player);
                typeCola = TypeCola.Random;
                break;
            case TypeCola.StaminUp:
                break;
        }
    }


    private void AcheterGun(GameObject player)
    {
        bool alreadyOwned = false;
        foreach (Transform weapon in player.GetComponentInChildren<GunManager>().transform)
        {
            if ((weapon.gameObject.GetComponent<Gun>().modele == gunInShop.GetComponent<Gun>().modele)
                        && weapon.gameObject.GetComponent<Gun>().isOwned)
            {
                alreadyOwned = true;
            }
        }
        if (player.GetComponent<Player>().money >= price && !alreadyOwned)
        {
            player.GetComponent<Player>().money -= price;
            if (player.GetComponentInChildren<GunManager>().nbGunsOwned == 1 || 
                (player.GetComponentInChildren<GunManager>().nbGunsOwned == 2 && player.GetComponentInChildren<GunManager>().muleKick))
            {
                AjouterNewGun(player, player.GetComponentInChildren<GunManager>().nbGunsOwned + 1);
            }
            else if ((player.GetComponentInChildren<GunManager>().nbGunsOwned == 2 && !player.GetComponentInChildren<GunManager>().muleKick) || player.GetComponentInChildren<GunManager>().nbGunsOwned == 3)
            {
                foreach (Transform weapon in player.GetComponentInChildren<GunManager>().transform)
                {
                    if (weapon.gameObject.GetComponent<Gun>().modele == gunInShop.GetComponent<Gun>().modele)
                        weapon.gameObject.GetComponent<Gun>().isOwned = true;
                    if (weapon.gameObject.GetComponent<Gun>().inUse)
                    {
                        weapon.gameObject.GetComponent<Gun>().isOwned = false;
                        weapon.gameObject.GetComponent<Gun>().inUse = false;
                        weapon.gameObject.SetActive(false);
                    }
                }
                player.GetComponentInChildren<GunManager>().gunsOwned.RemoveAt(player.GetComponentInChildren<GunManager>().inUse - 1);
                player.GetComponentInChildren<GunManager>().gunsOwned.Insert(player.GetComponentInChildren<GunManager>().inUse-1,
                    gunInShop.GetComponent<Gun>().modele);
                player.GetComponentInChildren<GunManager>().ChangerDarme(player.GetComponentInChildren<GunManager>().inUse);
            }
        }
    }
    private void AjouterNewGun(GameObject player, int index)
    {
        foreach (Transform weapon in player.GetComponentInChildren<GunManager>().transform)
            if (weapon.gameObject.GetComponent<Gun>().modele == gunInShop.GetComponent<Gun>().modele)
                weapon.gameObject.GetComponent<Gun>().isOwned = true;
        player.GetComponentInChildren<GunManager>().nbGunsOwned = index;
        player.GetComponentInChildren<GunManager>().gunsOwned.Add(gunInShop.GetComponent<Gun>().modele);
        player.GetComponentInChildren<GunManager>().ChangerDarme(index);
    }
}
/*
[CustomEditor(typeof(Interactable))]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Interactable interactable = target as Interactable;

        interactable.interactable = (TypeInteractable)EditorGUILayout.EnumFlagsField("Type Interactable", interactable.interactable);
        if (interactable.interactable == TypeInteractable.Box)
        {
            interactable.box = EditorGUILayout.PropertyField("Box", interactable.box);
            EditorGUILayout.
            gun.arc = EditorGUILayout.FloatField("Arc", gun.arc);
        }
    }
}
*/