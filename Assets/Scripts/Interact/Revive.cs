using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revive : Interactable
{
    private void Update()
    {
        
    }
    public override void Interacting(Player player)
    {
        IEnumerator Reviving()
        {
            yield return new WaitForSeconds(3f);
            this.GetComponentInParent<Player>().Revive();
        }
        StartCoroutine(Reviving());
    }

    public override void UpdateMessage()
    {
        message = "Maintenez F pour soigner";
    }
}
