using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text textDataInC;
    [SerializeField]float fadeMultiplier = 2;
    float fadeTimerOnText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeTimerOnText > 0 )
        {
            
            fadeTimerOnText -= Time.deltaTime;
            float FadeAlpha = fadeTimerOnText / fadeMultiplier;
            textDataInC.color = new Color(textDataInC.color.r, textDataInC.color.g, textDataInC.color.b, FadeAlpha);
            if (fadeTimerOnText <= 0)
            {
                fadeTimerOnText = 0;
            }
        }
    }
    public void SetTextStuff(InteractiveInfo text)
    {
        fadeTimerOnText = fadeMultiplier;
        textDataInC.text = text.text;
        textDataInC.color = text.color;
    }
}
