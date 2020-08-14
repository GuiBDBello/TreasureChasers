using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject treasure;

    private LevelGenerator levelGenerator;
    private GameObject levels;
    private GameObject player;
    private Camera mainCamera;
    private UIController uiController;

    private int gridSize;
    private int initialLevel;
    private int actualLevel;

    private int levelsCompleted;
    private int levelsToWin;

    private int treasureMoney;
    private Text treasureValueText;

    private void Start()
    {
        levelGenerator = gameObject.GetComponent<LevelGenerator>();
        levels = levelGenerator.GetLevels();
        player = GameObject.FindGameObjectWithTag(Tags.Player);
        mainCamera = Camera.main;

        gridSize = levelGenerator.gridSize;
        initialLevel = (gridSize * gridSize / 2);

        levelsCompleted = 0;
        levelsToWin = Mathf.RoundToInt(gridSize / 2);

        uiController = GameObject.FindGameObjectWithTag(Tags.UI).GetComponent<UIController>();

        treasureMoney = PlayerPrefs.GetInt("treasureMoney");
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            treasureValueText = GameObject.FindGameObjectWithTag(Tags.Score).GetComponent<Text>();
            treasureValueText.text = "$ " + treasureMoney + ",00";
        }
    }

    private void Update()
    {
        if (levelsCompleted >= levelsToWin)
            TreasureFound();
    }

    public bool GetGameOver()
    {
        return !player.GetComponent<PlayerController>().enabled;
    }

    public void CheckPlayerDirection(Collider other)
    {
        Transform levelTransform = levels.transform.GetChild(GetLevelIndex());

        if (other.name == GetRightDirection(levelTransform))
            NextLevel(other.name, levelTransform.GetChild(0).position);
        else
        {
            string message = "You got lost in the Desert. You found your way back to the camp.";
            GameOver(message);
        }
    }

    private int GetLevelIndex()
    {
        int levelPositionX = Mathf.RoundToInt(player.transform.position.x / levelGenerator.GetGroundWidth());
        int levelPositionZ = Mathf.RoundToInt(player.transform.position.z / levelGenerator.GetGroundHeight());

        return initialLevel + levelPositionX + (levelPositionZ * gridSize);
    }

    private void NextLevel(string direction, Vector3 levelPosition)
    {
        Vector3 mainCameraPosition = mainCamera.transform.position;
        Vector3 playerPosition = player.transform.position;

        switch (direction)
        {
            case Directions.North:
                mainCameraPosition.z += levelGenerator.GetGroundHeight();
                playerPosition.z = levelPosition.z + levelGenerator.GetGroundHeight();
                playerPosition.x = levelPosition.x;
                break;
            case Directions.South:
                mainCameraPosition.z -= levelGenerator.GetGroundHeight();
                playerPosition.z = levelPosition.z - levelGenerator.GetGroundHeight();
                playerPosition.x = levelPosition.x;
                break;
            case Directions.East:
                mainCameraPosition.x += levelGenerator.GetGroundWidth();
                playerPosition.x = levelPosition.x + levelGenerator.GetGroundWidth();
                playerPosition.z = levelPosition.z;
                break;
            case Directions.West:
                mainCameraPosition.x -= levelGenerator.GetGroundWidth();
                playerPosition.x = levelPosition.x - levelGenerator.GetGroundWidth();
                playerPosition.z = levelPosition.z;
                break;
        }
        CharacterController playerCharacterController = player.GetComponent<CharacterController>();
        mainCamera.transform.position = mainCameraPosition;
        playerCharacterController.enabled = false;
        player.transform.position = playerPosition;
        playerCharacterController.enabled = true;

        levelsCompleted++;
    }

    private void TreasureFound()
    {
        levelsCompleted = 0;
        player.GetComponent<PlayerController>().enabled = false;
        uiController.Panel.transform.GetChild(0).GetComponent<Text>().text = "Congratulations, you have found a Treasure.";
        uiController.Panel.SetActive(true);
        Vector3 position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z + 5);
        Instantiate(treasure, position, Quaternion.identity);

        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 1:
                treasureMoney = PlayerPrefs.GetInt("treasureMoney") + 10000;
                PlayerPrefs.SetInt("treasureMoney", treasureMoney);
                break;
            case 2:
                treasureMoney = PlayerPrefs.GetInt("treasureMoney") + 25000;
                PlayerPrefs.SetInt("treasureMoney", treasureMoney);
                break;
            case 3:
                treasureMoney = PlayerPrefs.GetInt("treasureMoney") + 50000;
                PlayerPrefs.SetInt("treasureMoney", treasureMoney);
                break;
        }
    }

    public void GameOver(string message)
    {
        player.GetComponent<PlayerController>().enabled = false;
        uiController.Panel.transform.GetChild(0).GetComponent<Text>().text = message;
        uiController.Panel.SetActive(true);
    }

    private string GetRightDirection(Transform levelTransform)
    {
        int objectCount = levelTransform.childCount - 1;

        int cactusCount = 0;
        int rockCount = 0;

        foreach (Transform objectTransform in levelTransform)
        {
            string objectName = objectTransform.name.Trim().Replace("(Clone)", "");
            switch (objectName)
            {
                case Objects.Cactus:
                    cactusCount++;
                    break;
                case Objects.Rock:
                    rockCount++;
                    break;
                default:
                    break;
            }
        }

        int logic = GetLogic(objectCount, cactusCount, rockCount);
        switch (logic % 4)
        {
            case 0:
                return Directions.North;
            case 1:
                return Directions.South;
            case 2:
                return Directions.East;
            case 3:
                return Directions.West;
        }
        // Só pra não dar erro de compilação, nunca vai chegar aqui;
        return "";
    }

    private int GetLogic(int objectCount, int cactusCount, int rockCount)
    {
        if (objectCount == 1)
        {
            if (cactusCount > rockCount)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else if (objectCount == 2)
        {
            if (cactusCount == rockCount)
            {
                return 2;
            }
            else if (cactusCount > 0)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }
        else if (objectCount == 3)
        {
            if (cactusCount == 3)
            {
                return 5;
            }
            else if (rockCount == 3)
            {
                return 6;
            }
            else if (cactusCount > rockCount)
            {
                return 7;
            }
            else
            {
                return 8;
            }
        }
        else
        {
            if (cactusCount == rockCount)
            {
                return 9;
            }
            else if (cactusCount > rockCount)
            {
                return 10;
            }
            else
            {
                return 11;
            }
        }
    }
}
