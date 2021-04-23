using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface InteractableWith
{
    abstract void Trigger();

    abstract TextAttributes PromptInfo();
}
public struct TextAttributes
{
    public string text;
    public Color color;
}
public class InteractWithSphere : MonoBehaviour, InteractableWith
{
    public TextAttributes PromptInfo()
    {
        return new TextAttributes { color = Color.white, text = "Throw me!" };
    }

    public void Trigger()
    {
        transform.position += Vector3.up;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
