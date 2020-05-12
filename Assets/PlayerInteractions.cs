using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    Interactable inter;
    public float coolDown = 1f;

    private bool canUse = true;

    private void Update()
    {
        IEnumerator Buy()
        {
            canUse = false;
            yield return new WaitForSeconds(coolDown);
            inter.Interacting(transform.parent.gameObject.GetComponent<Player>());
            Debug.Log(1);
            inter = null;
            canUse = true;
        }
        if (Input.GetKeyDown(KeyCode.F) && canUse && inter != null )
            if(!inter.blocked)
                StartCoroutine(Buy());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
           inter = other.GetComponent<Interactable>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null && (inter == null 
            || (other.gameObject != inter 
            && Vector3.Distance(other.transform.position, transform.position) < Vector3.Distance(inter.transform.position, transform.position))))
        {
            inter = other.gameObject.GetComponent<Interactable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() && inter == other.gameObject)
        {
            inter = null;
        }
    }
}
