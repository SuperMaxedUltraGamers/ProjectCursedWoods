namespace CursedWoods
{
    /// <summary>
    /// List of registered player inputs, used with FSM which has states based on player input.
    /// </summary>
    public enum PlayerInputType
    {
        None = 0,
        Move,
        Dash,
        Attack,
        Spellcast,
        Interact
    }

    /// <summary>
    /// Every spell this game has.
    /// </summary>
    public enum Spells
    {
        None = 0,
        Fireball,
        IceRay,
        MagicBeam,
        Shockwave
    }

    /// <summary>
    /// The type of spell, can be used to decrease or increase damage amount if 
    /// enemy is weak against certain type for example.
    /// </summary>
    public enum DamageType
    {
        Fire = 0,
        Ice,
        Magic,
        Shock,
        Melee
    }

    /// <summary>
    /// Determinates how player is alloved to move while doing some actions like attacking or spellcasting.
    /// </summary>
    public enum PlayerMoveType
    {
        Hold = 0,
        HalfSpeed,
        Free
    }

    /// <summary>
    /// All the type of pools we want to have.
    /// </summary>
    public enum ObjectPoolType
    {
        Fireball = 0,
        IceRay,
        MagicBeam,
        Shockwave,
        SkeletonMelee,
    }

    public enum EnemyType
    {
        SkeletonMelee = 0,
    }

    /// <summary>
    /// List of all behaviours that enemy can have.
    /// </summary>
    public enum EnemyBehaviours
    {
        Idle = 0,
        Patrol,
        ChasePlayer,
        AttackPlayer,
        IncreaseDistanceToPlayer,
        FleeFromPlayer,
        Knockback,
        Dead
    }
}