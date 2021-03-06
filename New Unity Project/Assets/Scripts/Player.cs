using System.Collections;
using System.Collections.Generic;
using UnityEngine;
enum AttackState
{
    Idle,
    Windip,
    Swing
}
public class Player : MonoBehaviour, IDamageInfo
{
    [SerializeField] Camera cam;
    float moveAccl = 40;
    Rigidbody rb;
    [SerializeField] float walkingSpeed = 40;
    [SerializeField]float runningSpeed = 80;
    bool running;
    PlayerUI ui;
    [SerializeField]GameObject flashlight;
    Vector3 moveInput = new Vector3(0, 0, 0);
    Vector3 forward;
    [SerializeField]HandAnim weaponAnim;
    Collider objectLook;
    Rigidbody heldObject;
    InteractableWith heldInteractable;
    bool locked = false;
    bool alive = true;
    [SerializeField] Transform referenceHoldTransform;
    Transform heldTransform;
    [SerializeField] float healthPointsMax = 100;
    float currentHealth;
    FootstepSouds soundSteps;
    KeyCode keyDrop = KeyCode.G;
    KeyCode keyInteract = KeyCode.E;
    ConfigurableJoint handJoint;
    
    // Start is called before the first frame update
    void Start()
    {
        soundSteps = GetComponent<FootstepSouds>();
        currentHealth = healthPointsMax;
        ui = GetComponent<PlayerUI>();
        rb = GetComponent<Rigidbody>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb.sleepThreshold = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            InputDoer();
            MouseLook();
            RayCastFromPlayer();
            soundSteps.UpdateFootstep(rb.velocity.magnitude, running);
        }

    }
    private void DyingCauseOfDeath()
    {
        alive = false;
        rb.freezeRotation = false;
        rb.AddTorque(cam.transform.right * 1000);
        rb.drag = 5f;
        rb.angularDrag = 0.1f;
        rb.sleepThreshold = 0.1f;

        weaponAnim.WeaponDrop();
        DropHeld();
        
    }
    void InputDoer()
    {
        running = false;
        moveInput = Vector3.zero;
        InputWeapon();
        
        
        if (Input.GetKey(KeyCode.W))
        {
            moveInput += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveInput += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveInput += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveInput += Vector3.right;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            running = true;
        }
        moveInput.Normalize();
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(!flashlight.activeSelf);
        }
    }
    private void FixedUpdate()
    {
        
        if (alive)
        {
            ObjectHeld();
            if (!locked)
            {
                MovementMethod();
            }
            Drag();
        }
        
    }
    private void MovementMethod()
    {        
        moveAccl = running ? runningSpeed : walkingSpeed;
        rb.AddForce(forward * moveInput.z * moveAccl, ForceMode.Acceleration);
        rb.AddForce(cam.transform.right * moveInput.x * moveAccl, ForceMode.Acceleration);
        
    }
    private void DropHeld()
    {
        if(heldInteractable != null) heldInteractable.Trigger();
        heldInteractable = null;
        heldObject = null;
        locked = false;
        if (heldTransform != null) heldTransform.SetParent(null);
    }
    void RayCastFromPlayer()
    {
        bool btnInteract = Input.GetKeyDown(keyInteract);
        if (heldInteractable != null || heldObject != null)
        {
           
            if (btnInteract)
            {
                DropHeld();
            }
            return;
        }
        Ray r = new Ray(cam.transform.position, cam.transform.forward);
        Physics.Raycast(r, out RaycastHit hit, 3);
        if(hit.collider)
        {
            InteractableWith interact = hit.collider.GetComponent<InteractableWith>();
            if (interact != null)
            {
                if (btnInteract)
                {
                    objectLook = hit.collider;
                    interact.Trigger();
                    PickupCase p = interact.Pickup();

                    if (p == PickupCase.Look)
                    {
                        heldInteractable = interact;
                        hit.collider.transform.position = referenceHoldTransform.position;
                        hit.collider.transform.rotation = referenceHoldTransform.rotation;
                        locked = true;
                    }
                    if (p == PickupCase.Hold)
                    {
                        heldTransform = hit.collider.transform;
                        heldInteractable = interact;
                        heldObject = hit.collider.attachedRigidbody;
                        
                        //hit.collider.transform.position = Vector3.Lerp(hit.collider.transform.position, referenceHoldTransform.position, 2);
                    }
                    if (p == PickupCase.Weapon)
                    {
                        weaponAnim.WeaponSet(hit.collider);
                        
                    }

                }
                InteractiveInfo text = interact.PromptInfo();
                ui.SetTextStuff(text);

            }
        }
        
    }
    void Drag()
    {
        Vector3 vel = rb.velocity;
        vel -= vel * 7 * Time.fixedDeltaTime;
        rb.velocity = new Vector3(vel.x, rb.velocity.y, vel.z);
    }
    float LimitMouseXRotation(float angle)
    {
        if (angle > 89 && angle < 180) return 89;
        if (angle < 271 && angle > 180) return 271;
        return angle;
    }
    void MouseLook()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 rotation = cam.transform.rotation.eulerAngles;
        cam.transform.rotation = Quaternion.Euler(LimitMouseXRotation(rotation.x - mouseInput.y), rotation.y + mouseInput.x, rotation.z);

        forward = cam.transform.forward;
        forward = new Vector3(forward.x, 0, forward.z);
        forward.Normalize();
    }
    void InputWeapon()
    {
        if (Input.GetMouseButtonDown(0)) weaponAnim.WantToWindup();
        if (Input.GetMouseButtonUp(0)) weaponAnim.WantToAttack();
        bool btnDrop = Input.GetKeyDown(keyDrop);
        if (btnDrop)
        {
            weaponAnim.WeaponDrop();
        }
    }
    
    void ObjectHeld()
    {
        if (heldObject != null)
        {
            Vector3 offset = referenceHoldTransform.position - heldObject.position;
            float dist = offset.magnitude;
            offset /= dist;
            //modified curved
            if (dist > .5) dist = 1;
            else
            {
                //2x
                dist *= 2;
                //1-x
                dist = 1 - dist;
                //x = x^3
                dist = Mathf.Pow(dist, 3);
                //1 - x^3
                dist = 1 - dist;
            }
            heldObject.AddForce(offset * 10000 * dist * Time.deltaTime);
            if (dist >= 1) dist = 0.95f;

            heldObject.velocity *= dist;
            heldObject.angularVelocity *= dist;
        }
    }

    public void DamageTaken(float damage, Vector3 position, Vector3 force)
    {
        
        if (alive)
        {
            rb.AddForce(force, ForceMode.Impulse);
            currentHealth -= damage;
            Debug.Log(damage);
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                DyingCauseOfDeath();
            }
            else
            {
                ui.GotHurt(.5f);
            }
            
        }
        
        
    }
    
}
