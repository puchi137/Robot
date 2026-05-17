using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotClass 
{
    [SerializeField] private ItemClass item;
    [SerializeField] private float quantity;
    public SlotClass()
    {
        item = null;
        quantity = 0;
    }
    public SlotClass(SlotClass slot)
    {
        quantity = slot.GetQuantity();
        item = slot.GetItem();
    }

    public SlotClass(ItemClass _item, float _quantity) 
    {
        item = _item;
        quantity = _quantity;
    }
    
    public void Clear()
    {
        item = null;
        quantity = 0;
    }
    public ItemClass GetItem() { return item; }
    public float GetQuantity() { return quantity; }
    public void AddQuantity(float _quantity) { quantity += _quantity; } 
    public void TakeQuantity(float _quantity) { quantity -= _quantity; } 
    public void AddItem(ItemClass item, float quantity) 
    {
        this.item = item; 
        this.quantity = quantity;
    }
}
