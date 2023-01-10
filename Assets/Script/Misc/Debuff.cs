using System;

public enum ModifierType{
    MOVEMENT_SPEED
}

[Serializable]
public struct Debuff
{
    public DebuffType debuff_type;
    public int percentage;
}