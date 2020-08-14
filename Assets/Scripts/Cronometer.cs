using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cronometer : MonoBehaviour
{
    public Text timeText;

    private GameController gameController;
    private float levelTime;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                levelTime = 30f;
                break;
            case 2:
                levelTime = 25f;
                break;
            case 3:
                levelTime = 20f;
                break;
        }
    }

    private void Update()
    {
        if (!gameController.GetGameOver())
        {
            timeText.text = ("Time: " + Mathf.Round(levelTime)).Replace("0", "o");

            if (levelTime > 0)
                levelTime -= Time.deltaTime;
            else if (!timeText.text.Equals("Time's up"))
            {
                timeText.text = "Time's up";
                string message = "A sandstorm is coming. You have to go back to the camp.";
                gameController.GameOver(message);
            }
        }
    }
}
