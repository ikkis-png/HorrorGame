using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour ,InteractableWith
{
    // Start is called before the first frame update
    [SerializeField] char buttonChar = '0';
    [SerializeField] Pad keyPadCode;

    public PickupCase Pickup()
    {
        return PickupCase.None;
    }

    public InteractiveInfo PromptInfo()
    {
        return new InteractiveInfo { text = "Press " + buttonChar, color = Color.white };
    }

    public void Trigger()
    {
        keyPadCode.InputCode(buttonChar.ToString());
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
