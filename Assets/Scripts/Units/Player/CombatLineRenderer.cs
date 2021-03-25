using UnityEngine;

namespace CursedWoods
{
    public class CombatLineRenderer : MonoBehaviour
    {
        [SerializeField]
        private LayerMask raycastMask;
        private LineRenderer combatLine;
        private bool useCombatLine;
        private float maxDrawDistance = 30f;
        private CharController charController;

        private void Awake()
        {
            combatLine = GetComponent<LineRenderer>();
            combatLine.enabled = false;
            charController = GetComponent<CharController>();
        }

        private void OnEnable()
        {
            //GameMan.Instance.CharController.ControlTypeChanged += ToggleLineRenderer;
            charController.ControlTypeChanged += ToggleLineRenderer;
        }

        private void OnDisable()
        {
            /*
            if (GameMan.Instance != null)
            {
                GameMan.Instance.CharController.ControlTypeChanged -= ToggleLineRenderer;

            }
            else
            {
                //print("GameMan was null, couldn't unsubscribe CombatLineRenderer from event.");
            }
            */

            charController.ControlTypeChanged -= ToggleLineRenderer;
        }

        private void Update()
        {
            if (useCombatLine)
            {
                DrawLineRenderer();
            }
        }

        private void ToggleLineRenderer()
        {
            if (useCombatLine)
            {
                useCombatLine = false;
                combatLine.enabled = false;
            }
            else
            {
                useCombatLine = true;
                combatLine.enabled = true;
            }
        }

        private void DrawLineRenderer()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDrawDistance, raycastMask))
            {
                combatLine.SetPosition(1, new Vector3(0f, 0f, hit.distance));
            }
            else
            {
                //Debug.DrawLine(transform.position, transform.position + (transform.forward * rayMaxDistance), Color.green, 1f, false);
                combatLine.SetPosition(1, new Vector3(0f, 0f, maxDrawDistance));
            }
        }
    }
}