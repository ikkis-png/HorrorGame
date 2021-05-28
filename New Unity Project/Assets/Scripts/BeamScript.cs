using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamScript : MonoBehaviour, InteractableWith
{
    [SerializeField] string itempicktext = "Grab me";
    [SerializeField] string itemdroptext = "Release me";
    [SerializeField] public Transform grabPosition;
    bool pickedUp = false;
    [SerializeField] PickupCase pickUp;
    public PickupCase Pickup()
    {
        return pickUp;
    }

    public InteractiveInfo PromptInfo()
    {
        return new InteractiveInfo { text = pickedUp ? itemdroptext : itempicktext, color = Color.green };
    }

    public void Trigger()
    {
        pickedUp = !pickedUp;
    }

}