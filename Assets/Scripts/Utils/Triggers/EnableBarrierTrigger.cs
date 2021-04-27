using UnityEngine;

namespace CursedWoods.Utils
{
    public class EnableBarrierTrigger : MonoBehaviour
    {
        [SerializeField]
        private Barrier barrier = Barrier.None;
        private int barrierIndex;
        private Level levelBarrierIsIn = Level.Graveyard;

        private void Awake()
        {
            switch (barrier)
            {
                case Barrier.Sword:
                    barrierIndex = GlobalVariables.GRAVEYARD_SWORD_BARRIER;
                    levelBarrierIsIn = Level.Graveyard;
                    break;
                case Barrier.Book:
                    barrierIndex = GlobalVariables.GRAVEYARD_BOOK_BARRIER;
                    levelBarrierIsIn = Level.Graveyard;
                    break;
                case Barrier.MiddleArena:
                    barrierIndex = GlobalVariables.GRAVEYARD_MIDDLE_BARRIER;
                    levelBarrierIsIn = Level.Graveyard;
                    break;
                case Barrier.Garden:
                    barrierIndex = GlobalVariables.GRAVEYARD_GARDEN_BARRIER;
                    levelBarrierIsIn = Level.Graveyard;
                    break;
                case Barrier.FinalBoss:
                    barrierIndex = GlobalVariables.CASTLE_FINAL_BOSS_BARRIER;
                    levelBarrierIsIn = Level.Castle;
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            switch (levelBarrierIsIn)
            {
                case Level.Graveyard:
                    GameMan.Instance.GraveyardManager.EnableBarrier(barrierIndex);
                    break;
                case Level.Castle:
                    GameMan.Instance.CastleManager.EnableBarrier(barrierIndex);
                    break;
            }

            gameObject.SetActive(false);
        }
    }
}