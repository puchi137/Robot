using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class crate : Item
{
    private InventoryManager inventory;
    public OtherClass scrap;
    public ConsumableClass battery;
    private int random;

    private void Start()
    {
        random = Random.Range(1, 100);
        inventory = GameObject.FindObjectOfType<InventoryManager>();
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }
    public override void BaseInteract()
    {
        inventory.Add(scrap, Random.Range(1, 15));
        inventory.Add(battery, Random.Range(1, 5));
        if(random >= 90)
        {

        }
        if(random == 1)
        {

        }
    }

}
