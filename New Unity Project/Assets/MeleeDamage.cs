using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeDamage : MonoBehaviour
{
    Collider[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        colliders = GetComponentsInChildren<Collider>();
    }

    // Update is called once per frame
   public void SetIsTrigger(bool isTrigger)
    {
        foreach(Collider c in colliders)
        {
            c.isTrigger = isTrigger;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamageInfo dmg = other.gameObject.GetComponentInParent<IDamageInfo>();
        if (dmg == null) other.gameObject.GetComponentInChildren<IDamageInfo>();
        if (dmg == null) other.gameObject.GetComponent<IDamageInfo>();
        if (dmg != null) dmg.DamageTaken(10, Vector3.zero, transform.right * 10);
    }
}
