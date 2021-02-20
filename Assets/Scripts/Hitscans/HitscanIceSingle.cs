namespace CursedWoods
{
    public class HitscanIceSingle : HitscanBase
    {
        protected override void Awake()
        {
            base.Awake();
            Init(false, 1f, 50f);
        }
    }
}