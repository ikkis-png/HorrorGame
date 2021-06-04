using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text textDataInC;
    [SerializeField]float fadeMultiplier = 2;
    
    float fadeTimerOnText;

    [SerializeField] UnityEngine.UI.Image bloodEffect;
    [SerializeField] Color bloodEffectColor;
    [SerializeField] Color bloodEffectFadeoutColor;
    float hurtTimer = 0;
    float hurtTime = 0;
    bool gotHurt = false;
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
        UpdateBloodEffect();
    }
    public void SetTextStuff(InteractiveInfo text)
    {
        fadeTimerOnText = fadeMultiplier;
        textDataInC.text = text.text;
        textDataInC.color = text.color;
    }
    public void GotHurt(float amount)
    {
        hurtTimer = amount;
        hurtTime = amount;
        gotHurt = true;
        bloodEffect.enabled = true;
        bloodEffect.color = bloodEffectColor;
    }
    private void UpdateBloodEffect()
    {
        if (gotHurt)
        {
            hurtTimer -= Time.deltaTime;
            bloodEffect.color = Color.Lerp(bloodEffectFadeoutColor, bloodEffectColor, hurtTimer / hurtTime);
            if (hurtTimer <= 0)
            {
                bloodEffect.enabled = false;
                gotHurt = false;
            }
        }
    }
}
