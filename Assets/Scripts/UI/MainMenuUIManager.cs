using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CursedWoods.UI
{
    public class MainMenuUIManager : OptionsManager
    {
        [SerializeField]
        private GameObject mainMenu;

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

        public void NewGameButton()
        {
            GameMan.Instance.NewGame();
        }

        public void LoadGameButton()
        {
            GameMan.Instance.LoadGame();
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