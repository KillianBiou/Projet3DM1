using System;

public enum ModifierType{
    MOVEMENT_SPEED,
    HP,
    SHIELD
}

[Serializable]
public struct Modifier
{
    public ModifierType type;
    public int value;
}