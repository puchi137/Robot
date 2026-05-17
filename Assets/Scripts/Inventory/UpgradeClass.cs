using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Other Class", menuName = "Item/Upgrade")]
public class UpgradeClass : ItemClass
{
    public UpgradeType upgradeType;
    public Tier tier;
    public enum UpgradeType
    {
        bulletDamage,
        weaponSpeed,
        bulletCapacity,
        playerSpeed,
        jetPackCapacity,
        playerHealth,
    }
    public enum Tier
    {
        C,
        B,
        A,
        S,
        X,
    }
    public float upgradeAmount;

    public override ItemClass GetItem() { return this; }
    public override OtherClass GetOther() { return null; }
    public override UpgradeClass GetUpgrade() { return this; }
    public override ConsumableClass GetConsumable() { return null; }
}
