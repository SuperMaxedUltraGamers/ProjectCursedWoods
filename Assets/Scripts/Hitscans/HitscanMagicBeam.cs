namespace CursedWoods
{
    public class HitscanMagicBeam : HitscanBase
    {
        protected override void Awake()
        {
            Init(true, 1000f, 25f);
            base.Awake();
        }
    }
}