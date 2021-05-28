using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDamageInfo
{
    abstract void DamageTaken(float damage, Vector3 position, Vector3 force);
}
public class DamageableBox : MonoBehaviour, IDamageInfo
{
    Rigidbody rb;

    public void DamageTaken(float damage, Vector3 position, Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
