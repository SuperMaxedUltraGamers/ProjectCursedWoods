namespace CursedWoods
{
    public class MeleeUnlock : InteractionBase
    {
        public override float Interaction()
        {
            base.Interaction();
            Destroy(gameObject);
            return animTime;
        }
    }
}