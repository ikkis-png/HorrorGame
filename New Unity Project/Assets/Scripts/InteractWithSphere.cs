using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InteractableWith
{
    abstract void Trigger();

    abstract InteractiveInfo PromptInfo();
    abstract PickupCase Pickup();   
}
    public enum PickupCase
{
    None,
    Hold,
    Look,
    Weapon

}
public struct InteractiveInfo
{
    public string text;
    public Color color;
}
public class InteractWithSphere : MonoBehaviour, InteractableWith
{
    public PickupCase Pickup()
    {
        return PickupCase.None;
    }

    public InteractiveInfo PromptInfo()
    {
        return new InteractiveInfo { color = Color.white, text = "Throw me!" };
    }

    public void Trigger()
    {
        transform.position += Vector3.up;
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
