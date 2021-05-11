using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningBox : MonoBehaviour, InteractableWith, IDamageInfo
{
    bool readyToInteract = true;
    float baseRotation = 0;
    float RotationTo = 0;
    //Linear Interpolation
    float lerp = 0;

    public void DamageTaken(float damage, Vector3 position, Vector3 force)
    {
        Trigger();
    }

    public PickupCase Pickup()
    {
        return PickupCase.None;
    }

    public void Trigger()
    {
        if (readyToInteract)
        {
            lerp = 0;
            readyToInteract = false;
            baseRotation = transform.rotation.eulerAngles.y;
            RotationTo = baseRotation + 90;
        }
    }

    InteractiveInfo InteractableWith.PromptInfo()
    {
        return new InteractiveInfo { text = "Spin me!", color = Color.red };
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if(!readyToInteract)
        {
            lerp += Time.deltaTime;
            Vector3 rotation = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(new Vector3(rotation.x, Mathf.SmoothStep(baseRotation,RotationTo,lerp), rotation.z));
            if(lerp >= 1)
            {
                transform.rotation = Quaternion.Euler(new Vector3(rotation.x, RotationTo, rotation.z));
                readyToInteract = true;
            }
        }
    }
}
