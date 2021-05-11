using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pad : MonoBehaviour
{
    [SerializeField] string code = "0000";
    [SerializeField] float activeTime = 4;
    [SerializeField] float errorTime = 4;
    [SerializeField] UnityEvent successEvent;
    [SerializeField] UnityEvent idleEvent;
    [SerializeField] UnityEvent keypadScreenIdleEvent;
    [SerializeField] UnityEvent keypadScreenSuccEvent;
    [SerializeField] UnityEvent keypadScreenFailEvent;
    float errorTimer = 0;
    float activeTimer = 0;
    bool successBool = false;
    string inputSave = "";
    public void InputCode(string s)
    {
        inputSave += s;
        if (inputSave.Length == code.Length)
        {
            if (inputSave == code) SuccesfulInput();
            else FailedInput(); 
            inputSave = "";
            Debug.Log(successBool ? "Door is unlocked" : "Door is locked");
        }
    }
    void SuccesfulInput()
    {
        keypadScreenSuccEvent.Invoke();
        activeTimer = activeTime;
        successBool = true;
        successEvent.Invoke();
        
    }
    void FailedInput()
    {      
        keypadScreenFailEvent.Invoke();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (successBool)
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0)
            {
                activeTimer = 0;
                successBool = false;
                idleEvent.Invoke();
            }
            
            
            

        }
    }
}
