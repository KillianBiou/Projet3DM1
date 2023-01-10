using System;

public enum ModifierType{
    MOVEMENT_SPEED
}

[Serializable]
public struct Modifier
{
    public ModifierType modifier;
    public int value;
}