namespace CursedWoods
{
    public class HitscanMagicBeam : HitscanBase
    {
        protected override void Awake()
        {
            base.Awake();
            Init(true, 5f, 25f);
        }
    }
}