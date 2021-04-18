using UnityEngine;
using UnityEngine.SceneManagement;
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
        private GameObject optionsMenu;

        [SerializeField]
        private GameObject quitMenu;

        private void Awake()
        {
            mainMenu.SetActive(true);
        }

        private void Start()
        {
            loadGameButton.interactable = GameMan.Instance.SaveSystem.SaveFileExist(SaveUtils.AUTOSAVE_SAVE_SLOT);
        }

        public void NewGameButton()
        {
            GameMan.Instance.NewGame();
        }

        public void LoadGameButton()
        {
            // TODO: Create loadmenu where we can choose saveslot if we want to have more than one savefile.
            GameMan.Instance.LoadGame(SaveUtils.AUTOSAVE_SAVE_SLOT);
        }

        public void ControlsMenuButton()
        {
            controlsMenu.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void QuitMenuButton()
        {
            quitMenu.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void OptionsMenuButton()
        {
            optionsMenu.SetActive(true);
            mainMenu.SetActive(false);
        }

        public void BackOutToMainMenu()
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
            quitMenu.SetActive(false);
            controlsMenu.SetActive(false);
        }

        public void QuitButton()
        {
            Application.Quit();
        }
    }
}