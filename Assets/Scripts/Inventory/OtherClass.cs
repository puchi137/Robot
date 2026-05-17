using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Other Class", menuName ="Item/Other")]
public class OtherClass : ItemClass
{
    public override ItemClass GetItem() { return this; }
    public override OtherClass GetOther() { return this; }
    public override UpgradeClass GetUpgrade() { return null; }
    public override ConsumableClass GetConsumable() { return null; }
}
