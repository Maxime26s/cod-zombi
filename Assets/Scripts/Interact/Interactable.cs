using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public enum TypeCola { StaminUp, JuggerNog, ElectricCherry, MuteKick, DoubleTap, Quick, DeadshotDai, Random}
public abstract class Interactable : MonoBehaviour
{
    public int price;
    [HideInInspector]
    public bool blocked = false;
    [HideInInspector]
    public string message;

    public abstract void Interacting(Player player);
    public abstract void UpdateMessage();
    //public abstract bool CheckConditions();
}