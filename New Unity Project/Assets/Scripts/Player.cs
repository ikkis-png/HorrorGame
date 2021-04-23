using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float moveAccl = 40;
    Rigidbody rb;
    PlayerUI ui;
    [SerializeField]GameObject flashlight;
    Vector3 moveInput = new Vector3(0, 0, 0);
    Vector3 forward;
    // Start is called before the first frame update
    void Start()
    {
        ui = GetComponent<PlayerUI>();
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        InputDoer();
        MouseLook();
        RayCastFromPlayer();
    }
    void InputDoer()
    {
        moveInput = Vector3.zero;
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
        moveInput.Normalize();
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashlight.SetActive(!flashlight.activeSelf);
        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(forward * moveInput.z * moveAccl, ForceMode.Acceleration);
        rb.AddForce(cam.transform.right * moveInput.x * moveAccl, ForceMode.Acceleration);
    }
    void RayCastFromPlayer()
    {
        Ray r = new Ray(cam.transform.position, cam.transform.forward);
        Physics.Raycast(r, out RaycastHit hit, 2);
        if(hit.collider)
        {
            InteractableWith interact = hit.collider.GetComponent<InteractableWith>();
            if (interact != null)
            {
                if (Input.GetKeyDown(KeyCode.E)) interact.Trigger();
                TextAttributes text = interact.PromptInfo();
                ui.SetTextStuff(text);

            }
        }
        
    }
    void MouseLook()
    {
        Vector2 mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        Vector3 rotation = cam.transform.rotation.eulerAngles;
        cam.transform.rotation = Quaternion.Euler(rotation.x - mouseInput.y, rotation.y + mouseInput.x, rotation.z);

        forward = cam.transform.forward;
        forward = new Vector3(forward.x, 0, forward.z);
        forward.Normalize();
    }
}
