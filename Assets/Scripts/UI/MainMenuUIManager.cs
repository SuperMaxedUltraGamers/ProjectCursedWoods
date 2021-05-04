using UnityEngine;
using UnityEngine.UI;
using CursedWoods.SaveSystem;

namespace CursedWoods.UI
{
    public class MainMenuUIManager : OptionsManager
    {
        [SerializeField]
        private GameObject mainMenu;

        [SerializeField]
        private Button loadGameButton = null;

        [SerializeField]
        private GameObject controlsMenu;

        [SerializeField]
        private GameObject quitMenu;

        protected override void Awake()
        {
            base.Awake();
            mainMenu.SetActive(true);
        }

        protected override void Start()
        {
            base.Start();
            loadGameButton.interactable = GameMan.Instance.SaveSystem.SaveFileExist(SaveUtils.AUTOSAVE_SAVE_SLOT);
            eventSystem.SetSelectedGameObject(null);
        }

        private void Update()
        {
            
        }

        public void NewGameButton()
        {
            PlayButtonSFX();
            GameMan.Instance.NewGame();
        }

        public void LoadGameButton()
        {
            PlayButtonSFX();
            // TODO: Create loadmenu where we can choose saveslot if we want to have more than one savefile.
            GameMan.Instance.LoadGame(SaveUtils.AUTOSAVE_SAVE_SLOT);
        }

        public void ControlsMenuButton()
        {
            eventSystem.SetSelectedGameObject(null);
            controlsMenu.SetActive(true);
            mainMenu.SetActive(false);
            PlayButtonSFX();
        }

        public void QuitMenuButton()
        {
            eventSystem.SetSelectedGameObject(null);
            quitMenu.SetActive(true);
            mainMenu.SetActive(false);
            PlayButtonSFX();
        }

        public void OptionsMenuButton()
        {
            eventSystem.SetSelectedGameObject(null);
            optionsMenu.SetActive(true);
            mainMenu.SetActive(false);
            PlayButtonSFX();
        }

        public void BackOutToMainMenu()
        {
            eventSystem.SetSelectedGameObject(null);
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            quitMenu.SetActive(false);
            controlsMenu.SetActive(false);
            Settings.Instance.SavePlayerPrefs();
            PlayButtonSFX();
        }

        public void QuitButton()
        {
            Application.Quit();
        }
    }
}