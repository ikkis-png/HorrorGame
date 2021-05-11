using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnim : MonoBehaviour
{
    Animator animator;
    GameObject weapon;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && animator.GetCurrentAnimatorStateInfo(0).IsName("idle")) animator.SetTrigger("Atack");
    }
    public void DeactivateDMG()
    {
        MeleeDamage m = GetComponentInChildren<MeleeDamage>();
        if (m != null) m.SetIsTrigger(false);
    }
    public void ActivateDMG()
    {
        MeleeDamage m = GetComponentInChildren<MeleeDamage>();
        if (m != null) m.SetIsTrigger(true);
    }
    public void WeaponSet(Collider c)
    {
        if (weapon != null)
        {
            WeaponDrop();
        }
        c.attachedRigidbody.isKinematic = true;
        c.attachedRigidbody.useGravity = false;

        BeamScript interPickAct = c.GetComponent<BeamScript>();
        Transform tPos = interPickAct.grabPosition;
        
        tPos.SetParent(transform);
        tPos.rotation = transform.rotation;
        tPos.position = transform.position;

        c.attachedRigidbody.gameObject.layer = 6;
        weapon = tPos.gameObject;
        weapon.layer = 6;
    }
    public void WeaponDrop()
    {
        if (weapon != null)
        {
            Collider c = weapon.GetComponentInChildren<Collider>();

            c.attachedRigidbody.isKinematic = false;
            c.attachedRigidbody.useGravity = true;

            DeactivateDMG();
            Transform tPos = c.GetComponent<BeamScript>().grabPosition;
            tPos.SetParent(null);
            c.attachedRigidbody.gameObject.layer = 0;

            weapon.layer = 0; 
            weapon = null;
            
        }
        

    }
}
