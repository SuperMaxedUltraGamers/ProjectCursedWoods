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
        private UnitBase playerUnit;

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
        private bool isGameOver = false;

        [SerializeField]
        private GameObject pauseMenu;

        [SerializeField]
        private GameObject controlsMenu;

        [SerializeField]
        private GameObject quitMenu;

        [SerializeField]
        private GameObject optionsMenu;

        [SerializeField]
        private GameObject gameOverMenu;

        [SerializeField]
        private Image displayInfoTextBG = null;
        private TextMeshProUGUI displayInfoText;

        private void Awake()
        {
            pointerImg = spellMenuPointer.GetComponent<Image>();
            interactText = interactPromt.GetComponentInChildren<TextMeshProUGUI>();
            playerUnit = playerSC.GetComponent<CharController>();
            interactText.text = "";
            interactPromt.SetActive(false);
            displayInfoText = displayInfoTextBG.GetComponentInChildren<TextMeshProUGUI>();
            displayInfoText.text = "";
        }

        private void OnEnable()
        {
            playerSC.SpellMenuTransIn += SpellMenuTransIn;
            playerSC.SpellMenuTransOut += SpellMenuTransOut;
            playerSC.SelectionMoved += UpdateSpellMenu;
            playerUnit.HealthChanged += OpenGameOverScreen;
        }

        private void OnDisable()
        {
            playerSC.SpellMenuTransIn -= SpellMenuTransIn;
            playerSC.SpellMenuTransOut -= SpellMenuTransOut;
            playerSC.SelectionMoved -= UpdateSpellMenu;
            playerUnit.HealthChanged -= OpenGameOverScreen;
        }

        private void Update()
        {
            if (Input.GetButtonDown(GlobalVariables.PAUSE))
            {
                TogglePauseMenu();
            }
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
            GameMan.Instance.LoadGame();
        }

        public void ControlsMenu()
        {
            eventSystem.SetSelectedGameObject(null);
            controlsMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }

        public void OptionsButton()
        {
            eventSystem.SetSelectedGameObject(null);
            optionsMenu.SetActive(true);
            pauseMenu.SetActive(false);
        }

        public void QuitMenuButton()
        {
            eventSystem.SetSelectedGameObject(null);
            quitMenu.SetActive(true);
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(false);
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
            eventSystem.SetSelectedGameObject(null);
            if (isGameOver)
            {
                BackOutToGameOverMenu();
            }
            else
            {
                pauseMenu.SetActive(true);
                optionsMenu.SetActive(false);
                quitMenu.SetActive(false);
                controlsMenu.SetActive(false);
            }
        }

        public void BackOutToGameOverMenu()
        {
            gameOverMenu.SetActive(true);
            quitMenu.SetActive(false);
        }

        public void RestartLevelButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void DisplayInfoText(string text, float transparency)
        {
            if (transparency >= 1f)
            {
                displayInfoText.text = text;
                displayInfoTextBG.gameObject.SetActive(true);
            }
            else if (transparency < 0.01f)
            {
                displayInfoTextBG.gameObject.SetActive(false);
            }

            SetImageAlpha(displayInfoTextBG, transparency);
            Color textColor = displayInfoText.color;
            textColor.a = transparency;
            displayInfoText.color = textColor;
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

        private void TogglePauseMenu()
        {
            if (!isGameOver)
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

        private void OpenGameOverScreen(int currentHealth, int maxHealth)
        {
            if (currentHealth <= 0 && !isGameOver)
            {
                isGameOver = true;
                eventSystem.SetSelectedGameObject(null);
                optionsMenu.SetActive(false);
                quitMenu.SetActive(false);
                controlsMenu.SetActive(false);
                pauseMenu.SetActive(false);
                spellMenu.SetActive(false);
                gameOverMenu.SetActive(true);
            }
        }
    }
}