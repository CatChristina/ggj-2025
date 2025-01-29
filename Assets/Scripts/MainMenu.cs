using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject tutorialMenu;

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToggleTutorialMenu()
    {
        tutorialMenu.SetActive(!tutorialMenu.activeInHierarchy);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
