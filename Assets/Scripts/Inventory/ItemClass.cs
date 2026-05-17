using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemClass : ScriptableObject
{
    [Header("Item")]
    public string itemName;
    public Sprite itemSprite;
    public bool isStackable = true;

    public abstract ItemClass GetItem();
    public abstract OtherClass GetOther();
    public abstract UpgradeClass GetUpgrade();
    public abstract ConsumableClass GetConsumable();
}
