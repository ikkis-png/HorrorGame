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
    TextAttributes InteractableWith.PromptInfo()
    {
        return new TextAttributes { text = IsDoorOpen ? "Close the door!!!" : "Open the door", color = Color.white };
    }

    // Start is called before the first frame update
    void Start()
    {
        closedAngle = transform.rotation.y;
    }

    void InteractableWith.Trigger()
    {
        if (!moving)
        {
            IsDoorOpen = !IsDoorOpen;
            moving = true;
        }
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
}
