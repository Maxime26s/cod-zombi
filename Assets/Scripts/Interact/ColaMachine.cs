using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColaMachine : Interactable
{
    public TypeCola typeCola;
    public List<TypeCola> listeColas = new List<TypeCola>
    { TypeCola.JuggerNog, TypeCola.MuteKick, TypeCola.DoubleTap, TypeCola.DeadshotDai, TypeCola.ElectricCherry, TypeCola.StaminUp};

    public override void Interacting(Player player)
    {
        if (player.money >= price && player.nbCola < 4 && !player.VerifyCola(typeCola))
        {
            AcheterCola(player);
            player.money -= price;
            player.nbCola++;
        }
    }

    private void AcheterCola(Player player)
    {
        switch (typeCola)
        {
            case TypeCola.DeadshotDai:
                foreach (Transform enfant in player.gameObject.transform)
                    if (enfant.gameObject.GetComponent<GunManager>() != null)
                        foreach (Transform gun in enfant)
                            gun.GetComponent<Gun>().damageMultiplier = 1.3f;
                player.colasOwned.Add(TypeCola.DeadshotDai);
                break;
            case TypeCola.DoubleTap:
                foreach (Transform enfant in player.gameObject.transform)
                    if (enfant.gameObject.GetComponent<GunManager>() != null)
                        foreach (Transform gun in enfant)
                            gun.GetComponent<Gun>().fireRateMultiplier = 1.3f;
                player.colasOwned.Add(TypeCola.DoubleTap);
                break;
            case TypeCola.ElectricCherry:
                player.colasOwned.Add(TypeCola.ElectricCherry);
                break;
            case TypeCola.JuggerNog:
                player.maxHealth = 200;
                player.colasOwned.Add(TypeCola.JuggerNog);
                break;
            case TypeCola.MuteKick:
                player.GetComponentInChildren<GunManager>().muleKick = true;
                player.colasOwned.Add(TypeCola.MuteKick);
                break;
            case TypeCola.Quick:
                player.colasOwned.Add(TypeCola.Quick);
                break;
            case TypeCola.Random:
                typeCola = listeColas[Random.Range(0, listeColas.Count)];
                while(player.VerifyCola(typeCola))
                    typeCola = listeColas[Random.Range(0, listeColas.Count)];
                AcheterCola(player);
                typeCola = TypeCola.Random;
                Debug.Log(2);
                break;
            case TypeCola.StaminUp:
                player.dashCooldown = 2.5f;
                player.colasOwned.Add(TypeCola.StaminUp);
                break;
        }
    }

    public override void UpdateMessage()
    {
        message = "Maintenez F pour "+typeCola.ToString()+"\nCoût: " + price;
    }
}
