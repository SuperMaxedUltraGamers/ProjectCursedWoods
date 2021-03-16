namespace CursedWoods
{
    public struct GlobalVariables
    {
        #region Collision tags

        public const string ENEMY_TAG = "Enemy";
        public const string MELEE_WEAPON_TAG = "MeleeWeapon";
        public const string PROJECTILE_TAG = "Projectile";

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

        public const string PLAYER_ANIM_TORSO_ANIM_VALUE = "TorsoAnimValue";

        #endregion Animation tags

        #region Animation values

        public const int PLAYER_ANIM_NULL = 0;
        public const int PLAYER_ANIM_MELEE = 1;
        public const int PLAYER_ANIM_SPELLCAST = 2;
        public const int PLAYER_ANIM_DEATH = 3;

        #endregion Animation values
    }
}