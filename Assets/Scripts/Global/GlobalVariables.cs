namespace CursedWoods
{
    public struct GlobalVariables
    {
        #region Collision tags

        public const string ENEMY_TAG = "Enemy";
        public const string MELEE_WEAPON_TAG = "MeleeWeapon";
        public const string PROJECTILE_TAG = "Projectile";
        public const string PLAYER_TAG = "Player";

        public const int PLAYER_LAYER = 8;
        public const int ENEMY_LAYER = 10;
        public const int PLAYER_MELEE_LAYER = 12;

        #endregion Collision tags

        #region Input tags

        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";
        public const string HORIZONTAL_RS = "HorizontalRS";
        public const string VERTICAL_RS = "VerticalRS";
        public const string DASH = "Dash";
        public const string ATTACK = "Attack";
        public const string SPELLCAST = "Spellcast";
        public const string INTERACT = "Interact";
        public const string OPEN_SPELLMENU = "OpenSpellMenu";
        public const string CHANGE_CONTROL_TYPE = "ChangeControlType";

        #endregion Input tags

        #region Animation tags

        public const string UNIQUE_ANIM_VALUE = "UniqueAnimValue";

        #endregion Animation tags

        #region Animation values

        public const int PLAYER_ANIM_NULL = 0;
        public const int PLAYER_ANIM_MELEE = 1;
        public const int PLAYER_ANIM_SPELLCAST = 2;
        public const int PLAYER_ANIM_DASH = 3;
        public const int PLAYER_ANIM_INTERACT = 4;
        public const int PLAYER_ANIM_DEATH = 5;

        public const int ENEMY_ANIM_NULL = 0;
        public const int ENEMY_ANIM_MELEE_ATTACK = 1;
        public const int ENEMY_ANIM_FLEE = 2;
        public const int ENEMY_ANIM_STAGGER = 3;
        public const int ENEMY_ANIM_DEATH = 4;
        public const int ENEMY_ANIM_RANGED_ATTACK = 5;

        #endregion Animation values
    }
}