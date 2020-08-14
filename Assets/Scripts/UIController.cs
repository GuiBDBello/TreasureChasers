using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject Panel;

    private void Start()
    {
        Panel.SetActive(false);
    }

    public void ShowInstructions()
    {
        Panel.SetActive(true);
    }

    public void Return()
    {
        Panel.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
