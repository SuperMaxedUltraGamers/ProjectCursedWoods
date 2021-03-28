using UnityEngine;
using UnityEngine.EventSystems;

namespace CursedWoods.UI
{
    public class SetEventSystemFirstActive : MonoBehaviour
    {
        [SerializeField]
        private EventSystem eventSystem;

        private void OnEnable()
        {
            eventSystem.SetSelectedGameObject(gameObject);
        }
    }
}