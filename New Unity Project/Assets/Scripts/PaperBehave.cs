using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperBehave : MonoBehaviour ,InteractableWith
{
    bool pickedUp = false;
    public PickupCase Pickup()
    {
        return PickupCase.Look;
    }

    public InteractiveInfo PromptInfo()
    {
        return new InteractiveInfo { text = "Hold me", color = Color.white };
    }

    public void Trigger()
    {
        pickedUp = !pickedUp;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
