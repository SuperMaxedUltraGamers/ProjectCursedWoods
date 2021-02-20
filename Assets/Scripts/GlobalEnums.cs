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

    public enum Spells
    {
        Fireball = 0,
        IceRay,
        MagicBeam,
        Shockwave
    }

    public enum SpellType
    {
        Fire = 0,
        Ice,
        Magic,
        Shock
    }

    public enum SpellMoveType
    {
        Hold = 0,
        Restricted,
        Free
    }
}