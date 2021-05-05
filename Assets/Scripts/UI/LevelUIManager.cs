using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using CursedWoods.SaveSystem;
using CursedWoods.Utils;

namespace CursedWoods.UI
{
    public class LevelUIManager : OptionsManager
    {
        [SerializeField]
        private SpellCaster playerSC;
        private UnitBase playerUnit;

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
        private Button pauseMenuLoadButton = null;

        [SerializeField]
        private GameObject controlsMenu;

        [SerializeField]
        private GameObject[] controlsGraphics;

        [SerializeField]
        private GameObject quitMenu;

        [SerializeField]
        private GameObject gameOverMenu;
        [SerializeField]
        private Button gameOverMenuLoadButton = null;

        [SerializeField]
        private Image displayInfoTextBG = null;
        private TextMeshProUGUI displayInfoText;

        [SerializeField]
        private GameObject bossHealthBarGO = null;
        private BossHealthBar bossHealthBar = null;

        [SerializeField]
        private Image[] spellCooldownIcons;
        [SerializeField]
        private TextMeshProUGUI[] spellCooldownText;

        private Fader fader;

        protected override void Awake()
        {
            base.Awake();
            pointerImg = spellMenuPointer.GetComponent<Image>();
            interactText = interactPromt.GetComponentInChildren<TextMeshProUGUI>();
            playerUnit = playerSC.GetComponent<CharController>();
            interactText.text = "";
            interactPromt.SetActive(false);
            displayInfoText = displayInfoTextBG.GetComponentInChildren<TextMeshProUGUI>();
            displayInfoText.text = "";
            bossHealthBar = GetComponent<BossHealthBar>();
            fader = GetComponent<Fader>();

            /*
            int spellIconsAmount = spellCooldownIcons.Length;
            for (int i = 0; i < spellIconsAmount; i++)
            {
                spellCooldownText[i] = spellCooldownIcons[i].gameObject.GetComponentInChildren<TextMeshProUGUI>();
                spellCooldownText[i].text = "";
            }
            */
        }

        private void OnEnable()
        {
            playerSC.SpellMenuTransIn += SpellMenuTransIn;
            playerSC.SpellMenuTransOut += SpellMenuTransOut;
            playerSC.SelectionMoved += UpdateSpellMenu;
            playerSC.SpellCasted += SpellCooldownGraphic;
            playerUnit.HealthChanged += OpenGameOverScreen;
            PlayerManager.SpellUnlocked += ActivateSpellIcons;
        }

        private void OnDisable()
        {
            playerSC.SpellMenuTransIn -= SpellMenuTransIn;
            playerSC.SpellMenuTransOut -= SpellMenuTransOut;
            playerSC.SelectionMoved -= UpdateSpellMenu;
            playerSC.SpellCasted -= SpellCooldownGraphic;
            playerUnit.HealthChanged -= OpenGameOverScreen;
            PlayerManager.SpellUnlocked -= ActivateSpellIcons;
        }

        private void Update()
        {
            if (Input.GetButtonDown(GlobalVariables.PAUSE))
            {
                TogglePauseMenu();
            }
        }

        public void InitializeLevelUIManager(PlayerManager playerMan)
        {
            // Start from one because index 0 in Spells equals None.
            for (int i = 1; i < Enum.GetNames(typeof(Spells)).Length; i++)
            {
                if (playerMan.GetSpellLockStatus((Spells)i))
                {
                    spellMenuSpellGraphics[i].gameObject.SetActive(true);
                    spellCooldownIcons[i-1].gameObject.SetActive(true);
                }
                else
                {
                    spellMenuSpellGraphics[i].gameObject.SetActive(false);
                    spellCooldownIcons[i - 1].gameObject.SetActive(false);
                }
            }


        }

        public void ConfigureBossHealthBar(UnitBase currentBoss, string bossName, int currentHealth, int maxHealth)
        {
            bossHealthBarGO.SetActive(true);
            bossHealthBar.ConfigureBossHealthBar(currentBoss, bossName, currentHealth, maxHealth);
        }

        public void DisableBossHealthBar()
        {
            bossHealthBar.DisableBossHealthBar();
            bossHealthBarGO.SetActive(false);
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
            // TODO: Create loadmenu where we can choose saveslot if we want to have more than one savefile.
            PlayButtonSFX();
            GameMan.Instance.LoadGame(SaveUtils.AUTOSAVE_SAVE_SLOT);
        }

        public void ControlsMenu()
        {
            eventSystem.SetSelectedGameObject(null);
            controlsMenu.SetActive(true);
            pauseMenu.SetActive(false);
            PlayButtonSFX();
        }

        public void MouseAndKbButton()
        {
            controlsGraphics[0].SetActive(true);
            controlsGraphics[1].SetActive(false);
            PlayButtonSFX();
        }

        public void ControllerButton()
        {
            controlsGraphics[1].SetActive(true);
            controlsGraphics[0].SetActive(false);
            PlayButtonSFX();
        }

        public void OptionsButton()
        {
            eventSystem.SetSelectedGameObject(null);
            optionsMenu.SetActive(true);
            pauseMenu.SetActive(false);
            PlayButtonSFX();
        }

        public void QuitMenuButton()
        {
            eventSystem.SetSelectedGameObject(null);
            quitMenu.SetActive(true);
            pauseMenu.SetActive(false);
            gameOverMenu.SetActive(false);
            PlayButtonSFX();
        }

        public void MainMenuButton()
        {
            PlayButtonSFX();
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
                cameraOptions.SetActive(false);
                combatOptions.SetActive(false);
                audioOptions.SetActive(false);
                quitMenu.SetActive(false);
                controlsMenu.SetActive(false);
                PlayButtonSFX();
            }
        }

        public void BackOutToGameOverMenu()
        {
            gameOverMenu.SetActive(true);
            quitMenu.SetActive(false);
            PlayButtonSFX();
        }

        /*
        public void RestartLevelButton()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        */

        public void StartFade(FadeType fadeType)
        {
            fader.StartFade(fadeType);
        }

        public void StartFade(FadeType fadeType, Level levelToLoadAfterFade)
        {
            fader.StartFade(fadeType, levelToLoadAfterFade);
        }

        public void DisplayInfoText(string text, float transparency)
        {
            if (transparency > 0.01f)
            {
                displayInfoText.text = text;
                displayInfoTextBG.gameObject.SetActive(true);
            }
            else
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
                    Settings.Instance.SavePlayerPrefs();
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
                    pauseMenuLoadButton.interactable = GameMan.Instance.SaveSystem.SaveFileExist(SaveUtils.AUTOSAVE_SAVE_SLOT);
                    pauseMenu.SetActive(true);
                    interactPromt.SetActive(false);
                }
            }

            PlayButtonSFX();
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
                bossHealthBarGO.SetActive(false);
                gameOverMenuLoadButton.interactable = GameMan.Instance.SaveSystem.SaveFileExist(SaveUtils.AUTOSAVE_SAVE_SLOT);
                gameOverMenu.SetActive(true);
            }
        }

        private void SpellCooldownGraphic(Spells spell, float cooldownTime)
        {
            int index = 0;
            switch (spell)
            {
                case Spells.Fireball:
                    index = 0;
                    break;
                case Spells.Shockwave:
                    index = 1;
                    break;
                case Spells.IceRay:
                    index = 2;
                    break;
                case Spells.MagicBeam:
                    index = 3;
                    break;
            }

            StartCoroutine(SpellCooldownIconFill(index, cooldownTime));
        }

        private void ActivateSpellIcons(Spells spell)
        {
            // TODO: FUCKING AWFUL HARDCODED BULLSHIT
            switch (spell)
            {
                case Spells.Fireball:
                    spellMenuSpellGraphics[1].gameObject.SetActive(true);
                    spellCooldownIcons[0].gameObject.SetActive(true);
                    break;
                case Spells.Shockwave:
                    spellMenuSpellGraphics[2].gameObject.SetActive(true);
                    spellCooldownIcons[1].gameObject.SetActive(true);
                    break;
                case Spells.IceRay:
                    spellMenuSpellGraphics[3].gameObject.SetActive(true);
                    spellCooldownIcons[2].gameObject.SetActive(true);
                    break;
                case Spells.MagicBeam:
                    spellMenuSpellGraphics[4].gameObject.SetActive(true);
                    spellCooldownIcons[3].gameObject.SetActive(true);
                    break;
            }
        }

        private IEnumerator SpellCooldownIconFill(int iconIndex, float cooldownTime)
        {
            float elapsedTime = 0f;

            while (elapsedTime < cooldownTime)
            {
                elapsedTime += Time.deltaTime;
                spellCooldownIcons[iconIndex].fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / cooldownTime);
                spellCooldownText[iconIndex].text = (cooldownTime - elapsedTime).ToString("F1");
                yield return null;
            }

            spellCooldownText[iconIndex].text = "";
            spellCooldownIcons[iconIndex].fillAmount = 1f;
        }
    }
}