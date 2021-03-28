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
            Settings.CombatLineValueChange += LineRendererCheck;
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

            /*
            if (Settings.Instance != null)
            {
                Settings.CombatLineValueChange -= LineRendererCheck;
            }
            */

            Settings.CombatLineValueChange -= LineRendererCheck;
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
                //if (Settings.Instance.UseCombatLineRenderer)
                //{
                    combatLine.enabled = false;
                //}
            }
            else
            {

                useCombatLine = true;
                if (Settings.Instance.UseCombatLineRenderer)
                {
                    combatLine.enabled = true;
                }
            }
        }

        private void LineRendererCheck(bool isOn)
        {
            if (isOn)
            {
                if (useCombatLine)
                {
                    combatLine.enabled = true;
                }
            }
            else
            {
                if (useCombatLine)
                {
                    combatLine.enabled = false;
                }
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