using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Other Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass
{
    [Header("Consumable")]
    public float healthAdded;

    public override ItemClass GetItem() { return this; } 
    public override OtherClass GetOther() { return null; }
    public override UpgradeClass GetUpgrade() { return null; }
    public override ConsumableClass GetConsumable() {  return this; }   
}
