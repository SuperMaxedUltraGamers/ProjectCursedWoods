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
            // TODO: Set progress values to default values.
            SceneManager.LoadScene(GlobalVariables.GRAVEYARD);
        }

        public void LoadGameButton()
        {
            // TODO: Load progress values from file.
            // TODO: load correct scene.
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