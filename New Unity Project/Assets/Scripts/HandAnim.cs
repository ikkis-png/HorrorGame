using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandAnim : MonoBehaviour
{
    Animator animator;
    MeleeDamage weapon;
    [SerializeField] float windUpMin = 1.0f;
    [SerializeField] float windUpMax = 2.0f;
    [SerializeField] float windUpMult = 1.0f;
    Vector3 localWeaponRotation;
    float windUpTimer = 0;
    bool isPoweringUp = false;
    bool isAttacking = false;
    bool wantsToSwing = false;
    bool wantsToAttack = false;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
    }
    public bool CanStartAttack()
    {
        return !isPoweringUp && animator.GetCurrentAnimatorStateInfo(0).IsName("idle");
    }
    public bool StartAttack()
    {
        if (!CanStartAttack()) return false;

            isAttacking = false;
            animator.SetTrigger("WindUp");
            isPoweringUp = true;
            windUpTimer = windUpMin;
        return true;
    }
    public void WantToWindup()
    {
        wantsToAttack = false;
        wantsToSwing = true;
    }
    public void WantToAttack()
    {
        wantsToAttack = true;
        wantsToSwing = false;
    }
    public bool CanAttack()
    {
        return isPoweringUp && animator.GetCurrentAnimatorStateInfo(0).IsName("meleeWindup");
    }
    public bool Attack()
    {
        if (!CanAttack()) return false;
        isPoweringUp = false;
        if(weapon != null)
        {
            weapon.SetDmgMult(Mathf.Clamp(windUpTimer, windUpMin, windUpMin + windUpMax));
        }
        
        Debug.Log("Hej");
        animator.SetTrigger("Atack");
        return true;
    }
    // Update is called once per frame
    void Update()
    {
        if (isPoweringUp) windUpTimer += Time.deltaTime * windUpMult;
        if(weapon != null)transform.localRotation = Quaternion.Euler(localWeaponRotation + new Vector3(Random.value * windUpTimer * .5f, Random.value * windUpTimer * .5f, Random.value * windUpTimer * .5f));
        if (wantsToSwing && StartAttack())
        {
            wantsToSwing = false;
        }
        if (wantsToAttack && Attack())
        {
            wantsToAttack = false;
        }

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

        c.gameObject.layer = 6;
        weapon = tPos.GetComponent<MeleeDamage>();
        weapon.gameObject.layer = 6;
        localWeaponRotation = weapon.transform.localRotation.eulerAngles;

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

            weapon.gameObject.layer = 0; 
            weapon = null;
            
        }
        

    }
}
