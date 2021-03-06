﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractions : MonoBehaviour
{
    Interactable inter;
    private TextMeshProUGUI text;
    public GameObject ui;
    [HideInInspector]
    public bool quickActions = false;

    private float actionCoolDown = 1f;
    private bool canUse = true;

    private void Start()
    {
        text = ui.GetComponentInChildren<TextMeshProUGUI>();
        ui.SetActive(false);
    }

    private void Update()
    {
        IEnumerator Buy()
        {
            canUse = false;
            yield return new WaitForSeconds(actionCoolDown);
            inter.Interacting(transform.parent.GetComponent<Player>());
            inter = null;
            canUse = true;
        }
        if (inter != null)
        {
            if ((inter.GetComponent<Revive>() != null || inter.GetComponent<Windows>() != null) && quickActions)
                actionCoolDown = 0.1f;
            else
                actionCoolDown = 1f;
            inter.UpdateMessage();
            text.text = inter.message;
            if (inter.blocked)
                ui.SetActive(false);
            if (Input.GetKeyDown(KeyCode.F) && canUse && !transform.parent.gameObject.GetComponent<Player>().isDown)
                if (!inter.blocked)
                    StartCoroutine(Buy());
        }
        if (inter == null)
            ui.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null)
        {
            inter = other.GetComponent<Interactable>();
            text.text = inter.message;
            ui.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null && (inter == null
            || (other.gameObject != inter
            && Vector3.Distance(other.transform.position, transform.position) < Vector3.Distance(inter.transform.position, transform.position))))
        {
            inter = other.gameObject.GetComponent<Interactable>();
            text.text = inter.message;
            ui.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Interactable>() != null && inter == other.GetComponent<Interactable>())
        {
            inter = null;
            ui.SetActive(false);
        }
    }
}
