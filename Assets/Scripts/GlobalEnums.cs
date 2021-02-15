namespace CursedWoods
{
    public enum PlayerControlType
    {
        IgnoreControl = 0,
        InControl
    }

    public enum PlayerVulnerableType
    {
        Invulnerable = 0,
        Vulnerable
    }

    public enum PlayerInputType
    {
        None = 0,
        Move,
        Dash,
        Attack,
        Spellcast,
        Interact
    }

    public enum PlayerGroundType
    {
        None = 0,
        Grounded,
        LeftGround,
        InAir
    }
}