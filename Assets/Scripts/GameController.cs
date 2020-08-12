using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private LevelGenerator levelGenerator;
    private GameObject levels;
    private GameObject player;
    private Camera mainCamera;

    private int gridSize;
    private int initialLevel;
    private int actualLevel;

    private void Start()
    {
        levelGenerator = gameObject.GetComponent<LevelGenerator>();
        levels = levelGenerator.GetLevels();
        player = GameObject.FindGameObjectWithTag(Tags.Player);
        mainCamera = Camera.main;

        gridSize = levelGenerator.gridSize;
        initialLevel = (gridSize * gridSize / 2);
    }

    public void CheckPlayerDirection(Collider other)
    {
        Transform levelTransform = levels.transform.GetChild(GetLevelIndex());

        if (other.name == GetRightDirection(levelTransform))
            NextLevel(other.name, levelTransform.GetChild(0).position);
        else
            GameOver();
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
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
