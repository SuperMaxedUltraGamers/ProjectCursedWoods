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
        Shockwave,
        IceRay,
        MagicBeam
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
        Physical,
        Poison,
        KillTrigger
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
        PossessedTree,
        MushroomEnemy,
        SkeletonBoss1,
        SkeletonBoss2,
        SkeletonBoss3,
        TreeBoss,
        SkeletonMace,
        FinalBoss,
        TreeProjectile,
        MushroomProjectile,
        FinalBossProjectile,
        HealthPickUp,
        MaxHealthPickUp,
        MaxHealthIncrease,
        MeleeHitParticles,
        DamageNumber
    }

    public enum EnemyType
    {
        SkeletonMelee = 0,
        PossessedTree,
        MushroomEnemy,
        SkeletonBoss1,
        SkeletonBoss2,
        SkeletonBoss3,
        TreeBoss,
        SkeletonMace,
        FinalBoss
    }

    /// <summary>
    /// List of all behaviours that normal enemy can have.
    /// </summary>
    public enum EnemyBehaviours
    {
        Idle = 0,
        Patrol,
        ChasePlayer,
        MeleeAttackPlayer,
        RangeAttackPlayer,
        FleeFromPlayer,
        Knockback,
        Dead
    }

    public enum TreeBossBehaviours
    {
        Sleep = 0,
        Awaking,
        Idle,
        SlamAttack,
        SweepRight,
        SweepLeft,
        RootAttack,
        DropAttack,
        Dead
    }

    public enum FinalBossBehaviours
    {
        Sleep = 0,
        Awaking,
        Idle,
        Walk,
        Dash,
        ScanLaser,
        Projectile,
        MagicScythe,
        Dead
    }

    public enum Axis
    {
        AxisX = 0,
        AxisY,
        AxisZ
    }

    public enum KeyType
    {
        KeyBook = 0,
        KeyGateSouth,
        KeyGateNorth,
        KeyGarden,
        KeyCastle,
        KeyFinalBoss
    }

    public enum GateType
    {
        None = 0,
        GraveyardBookGate,
        GraveyardMiddleAreaSouthGate,
        GraveyardMiddleAreaNorthGate,
        GraveyardGardenGate,
        GraveyardCastleGate,
        CastleFinalBossDoor
    }

    public enum FadeType
    {
        None = 0,
        FadeIn,
        FadeOut
    }

    public enum Level
    {
        MainMenu = 0,
        Intro,
        Graveyard,
        Castle,
        Outro
    }

    public enum Barrier
    {
        None = 0,
        Sword,
        Book,
        MiddleArena,
        Garden,
        FinalBoss
    }
}