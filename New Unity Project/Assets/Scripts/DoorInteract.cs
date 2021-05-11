using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : MonoBehaviour, InteractableWith
{
    [SerializeField] Transform pivotPoint;
    bool IsDoorOpen = false;
    float openAngle = 90;
    float closedAngle;
    float lerp = 0;
    bool moving = false;
    bool locked = true;
    InteractiveInfo InteractableWith.PromptInfo()
    {
        return DoorText();
    }

    // Start is called before the first frame update
    void Start()
    {
        closedAngle = transform.rotation.y;
    }

    public void Trigger()
    {
        if (!IsDoorOpen && locked) return;
        if (!moving)
        {
            IsDoorOpen = !IsDoorOpen;
            moving = true;
        }
    }
    public void DoorLock()
    {
        locked = true;
    }
    public void DoorUnlock()
    {
        locked = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            if (IsDoorOpen) lerp += Time.deltaTime;
            else lerp -= Time.deltaTime;
            if (lerp < 0)
            {
                moving = false;
                lerp = 0;
            }
            else if (lerp >= 1)
            {
                moving = false;
                lerp = 1;
            }
            Vector3 rot = pivotPoint.transform.localRotation.eulerAngles;
            pivotPoint.transform.localRotation = Quaternion.Euler(new Vector3(rot.x, Mathf.SmoothStep(closedAngle, openAngle, lerp), rot.z));
        }
    }
    InteractiveInfo DoorText()
    {
        if (locked)
        {
            return new InteractiveInfo { text = "It's locked!", color = Color.white };
        }
        else
        {
            return new InteractiveInfo { text = IsDoorOpen ? "Close the door!!!" : "Open the door", color = Color.white };
        }
    }

    public PickupCase Pickup()
    {
        return PickupCase.None;
    }
}
