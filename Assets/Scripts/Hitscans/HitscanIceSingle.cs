namespace CursedWoods
{
    public class HitscanIceSingle : HitscanBase
    {
        protected override void Awake()
        {
            Init(false, 0.75f, 50f);
            base.Awake();
        }
    }
}