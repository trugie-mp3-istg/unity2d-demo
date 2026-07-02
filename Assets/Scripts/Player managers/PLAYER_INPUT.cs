using UnityEngine;

// Player movement (up-down-right-left) is handled natively though Input Manager
public static class PLAYER_INPUT
{
    // General
    public static readonly KeyCode menu = KeyCode.Q;

    // Overworld specifics
    public static readonly KeyCode run = KeyCode.LeftShift;
    public static readonly KeyCode interact = KeyCode.Z;

    // Battle specifics
    public static readonly KeyCode crouchfocus = KeyCode.LeftShift;
    public static readonly KeyCode dash = KeyCode.A;
    public static readonly KeyCode spellcard_switch = KeyCode.Z;
    public static readonly KeyCode spellcard_load = KeyCode.X;
    public static readonly KeyCode spellcard_use = KeyCode.C;
}
