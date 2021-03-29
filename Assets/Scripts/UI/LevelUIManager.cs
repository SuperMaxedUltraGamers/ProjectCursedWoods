using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

namespace CursedWoods.UI
{
    public class LevelUIManager : OptionsManager
    {
        [SerializeField]
        private SpellCaster playerSC;

        [SerializeField]
        private EventSystem eventSystem;

        [SerializeField]
        private GameObject spellMenu;

        [SerializeField]
        private GameObject spellMenuPointer;
        private Image pointerImg;

        [SerializeField]
        private Image[] spellMenuSpellGraphics;
        private int previousGraphicIndex;

        [SerializeField]
        private GameObject interactPromt;
        private TextMeshProUGUI interactText;

        [SerializeField]
        private GameObject playerHealthBar;

        private bool isPaused = false;

        [SerializeField]
        private GameObject pauseMenu;

        [SerializeField]
        private GameObject controlsMenu;

        [SerializeField]
        private GameObject quitMenu;

        [SerializeField]
        private GameObject optionsMenu;

        private void Awake()
        {
            pointerImg = spellMenuPointer.GetComponent<Image>();
            interactText = interactPromt.GetComponentInChildren<TextMeshProUGUI>();
            interactText.text = "";
            interactPromt.SetActive(false);
        }

        private void OnEnable()
        {
            playerSC.SpellMenuTransIn += SpellMenuTransIn;
            playerSC.SpellMenuTransOut += SpellMenuTransOut;
            playerSC.SelectionMoved += UpdateSpellMenu;
        }

        private void OnDisable()
        {
            playerSC.SpellMenuTransIn -= SpellMenuTransIn;
            playerSC.SpellMenuTransOut -= SpellMenuTransOut;
            playerSC.SelectionMoved -= UpdateSpellMenu;
        }

        private void Update()
        {
            if (Input.GetButtonDown(GlobalVariables.PAUSE))
            {
                TogglePauseMenu();
            }
        }

        private void SpellMenuTransIn(float transparency)
        {
            spellMenu.SetActive(true);

            for (int i = 0; i < spellMenuSpellGraphics.Length; i++)
            {
                if (i == previousGraphicIndex && previousGraphicIndex != 0)
                {
                    SetImageAlpha(spellMenuSpellGraphics[i], transparency * 2f);
                }
                else
                {
                    SetImageAlpha(spellMenuSpellGraphics[i], transparency);
                }
            }

            SetImageAlpha(pointerImg, transparency * 2f);
        }

        private void SpellMenuTransOut(float transparency)
        {
            if (transparency < 0.01f)
            {
                spellMenu.SetActive(false);
            }
            else
            {
                foreach (Image image in spellMenuSpellGraphics)
                {
                    SetImageAlpha(image, transparency);
                }

                SetImageAlpha(pointerImg, transparency);
            }
        }

        private void UpdateSpellMenu(Vector2 inputDir, int spellMenuGraphicIndex)
        {
            // Rotation of selector.
            float rot = Mathf.Atan2(-inputDir.x, inputDir.y);
            spellMenuPointer.transform.rotation = Quaternion.Euler(0f, 0f, rot * Mathf.Rad2Deg);

            if (spellMenuGraphicIndex != previousGraphicIndex)
            {
                SetImageAlpha(spellMenuSpellGraphics[previousGraphicIndex], 0.5f);
            }

            SetImageAlpha(spellMenuSpellGraphics[spellMenuGraphicIndex], 1f);
            previousGraphicIndex = spellMenuGraphicIndex;
        }

        private void SetImageAlpha(Image img, float alpha)
        {
            Color tempColor = img.color;
            tempColor.a = alpha;
            img.color = tempColor;
        }

        public void SetInteractPromtVisibility(bool visible, string text)
        {
            interactText.text = text;
            interactPromt.SetActive(visible);
        }

        public void ResumeButton()
        {
            TogglePauseMenu();
        }

        public void LoadGameButton()
        {
            SceneManager.LoadScene(GlobalVariables.GRAVEYARD);
        }

        public void ControlsMenu()
        {
            controlsMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }

        public void OptionsButton()
        {
            optionsMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }

        public void QuitMenuButton()
        {
            quitMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }

        public void MainMenuButton()
        {
            SceneManager.LoadScene(GlobalVariables.MAIN_MENU);
        }

        public void QuitButton()
        {
            Application.Quit();
        }

        public void BackOutToPauseMenu()
        {
            pauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
            quitMenu.SetActive(false);
            controlsMenu.SetActive(false);
        }

        private void TogglePauseMenu()
        {
            if (isPaused)
            {
                isPaused = false;
                GameMan.Instance.CharController.IgnoreControl = false;
                Time.timeScale = 1f;
                eventSystem.SetSelectedGameObject(null);
                optionsMenu.SetActive(false);
                quitMenu.SetActive(false);
                controlsMenu.SetActive(false);
                pauseMenu.SetActive(false);
            }
            else
            {
                isPaused = true;
                GameMan.Instance.CharController.IgnoreControl = true;
                Time.timeScale = 0f;
                pauseMenu.SetActive(true);
                interactPromt.SetActive(false);
            }
        }
    }
}