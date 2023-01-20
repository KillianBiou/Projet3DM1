using System;

public enum ModifierType{
    MOVEMENT_SPEED,
    HP,
    SHIELD,
    CUT_TRAP,
    BLIND
}

[Serializable]
public struct Modifier
{
    public ModifierType type;
    public int value;
}