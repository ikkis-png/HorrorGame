using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnim : MonoBehaviour
{
    Animator animator;
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
    public void InputDropWeapon(GameObject weapon)
    {

        Collider c = weapon.GetComponentInChildren<Collider>();

        c.attachedRigidbody.isKinematic = false;
        c.attachedRigidbody.useGravity = true;
        Transform tPos = c.GetComponent<BeamScript>().grabPosition;
        tPos.SetParent(null);
        c.attachedRigidbody.gameObject.layer = 0;
        weapon = null;
        weapon.gameObject.layer = 0;

    }
}
