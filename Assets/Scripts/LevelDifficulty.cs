using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDifficulty : MonoBehaviour
{
    public GameObject Panel;

    private void Start()
    {
        Panel.SetActive(false);
    }

    public void SelectDifficulty()
    {
        Panel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void StartGameEasy()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void StartGameNormal()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void StartGameHard()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }
}
