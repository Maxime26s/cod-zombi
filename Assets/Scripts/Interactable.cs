using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeInteractable { Gun, Door, Cola, Window, Box }
public class NewBehaviourScript : MonoBehaviour
{
    public SphereCollider collider;
    public int price;
    public float interactionRadius = 1;
    public TypeInteractable interactable;

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
        if (Input.GetKeyDown(KeyCode.F) && coolDownOver == false)
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

    private void OnCollisionStay(Collision collision)
    {
        GameObject col = collision.gameObject;
        if (gameObject.CompareTag("Player") && canUse)
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
                player.GetComponent<Player>().money -= price;
                break;
            case TypeInteractable.Door:
                player.GetComponent<Player>().money -= price;
                break;
            case TypeInteractable.Window:
                player.GetComponent<Player>().money += price;
                break;
            case TypeInteractable.Cola:
                player.GetComponent<Player>().money -= price;
                break;
            case TypeInteractable.Box:
                player.GetComponent<Player>().money -= price;
                break;
        }
    }
}
