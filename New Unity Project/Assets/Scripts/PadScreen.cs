using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadScreen : MonoBehaviour
{
    [SerializeField]
    MeshRenderer m;
    Color colorStart;
    float lightTimer = 0;
    bool lightChanged = false;
    // Start is called before the first frame update
    void Start()
    {
        m = GetComponent<MeshRenderer>();
        colorStart = new Color(m.material.color.r, m.material.color.g, m.material.color.b);
    }
    public void SuccesOnSreen(float time)
    {
        lightTimer = time;
        m.material.color = Color.green;
        lightChanged = true;
    }
    public void FailOnScreen(float time)
    {
        lightTimer = time;
        m.material.color = Color.red;
        lightChanged = true;


    }
    public void IdleOnScreen()
    {
        m.material.color = colorStart;
    }

    // Update is called once per frame
    void Update()
    {
        if (lightChanged)
        {
            if (lightTimer <= 0)
            {
                lightTimer = 0;
                IdleOnScreen();
                lightChanged = false;
            }
            lightTimer -= Time.deltaTime;

        }

        
    }
}
