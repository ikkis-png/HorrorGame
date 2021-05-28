using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class MeleeDamage : MonoBehaviour
{
    [SerializeField] AudioClip audioHit;
    //[SerializeField] AudioClip audioHitFail;
    //abstract delegate void HitStatic();
    //public event HitStatic onHitEvent;
    bool didHit;
    float damageMultiplier = 1;
    List<Collider> hitColliders;
    AudioSource audioSource;
    Collider[] colliders;
    // Start is called before the first frame update
    void Start()
    {
        hitColliders = new List<Collider>();
        colliders = GetComponentsInChildren<Collider>();
        audioSource = GetComponent<AudioSource>();
    }
    public void SetDmgMult(float dmg)
    {
        damageMultiplier = dmg;
    }

    // Update is called once per frame
   public void SetIsTrigger(bool isTrigger)
    {
        hitColliders.Clear();
        foreach(Collider c in colliders)
        {
            c.isTrigger = isTrigger;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hitColliders.Contains(other) || hitColliders.Count > 2)
        {
            return;
        }
        float dmgMult = (3.0f - hitColliders.Count) / 3.0f;
        dmgMult *= damageMultiplier;
        Debug.Log(hitColliders.Count);
        Debug.Log(other.gameObject.name);

        hitColliders.Add(other);

        IDamageInfo dmg = other.gameObject.GetComponentInParent<IDamageInfo>();
        if (dmg == null) other.gameObject.GetComponentInChildren<IDamageInfo>();
        if (dmg == null) other.gameObject.GetComponent<IDamageInfo>();
        if (dmg != null)
        {
            dmg.DamageTaken(10*dmgMult, Vector3.zero, transform.right * 10 * dmgMult);
            audioSource.PlayOneShot(audioHit, dmgMult);
        }
    }
}
