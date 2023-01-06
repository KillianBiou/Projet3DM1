using System;

public enum DebuffType{
    MOVEMENT_SPEED
}

[Serializable]
public struct Debuff
{
    public DebuffType debuff_type;
    public int percentage;
}