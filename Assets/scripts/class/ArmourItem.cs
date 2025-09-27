using UnityEngine;

public abstract class ArmourItem : Item
{
    public readonly float armourDefence;
    public readonly float armourSpeedMult;

    protected ArmourItem() { }

    public abstract void equipItem();
}