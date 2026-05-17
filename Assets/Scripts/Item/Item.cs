using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Outline outline;
    [SerializeField]private string text;

    public void Interact()
    {
        BaseInteract();
        Destroy(gameObject);
    }
    public Outline GetOutline() {  return outline; }  
    public string GetText() { return text; }
    public abstract void BaseInteract();
}
