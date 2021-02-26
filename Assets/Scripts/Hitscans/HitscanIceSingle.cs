namespace CursedWoods
{
    public class HitscanIceSingle : HitscanBase
    {
        protected override void Awake()
        {
            Init(false, 1f, 50f);
            base.Awake();
        }
    }
}