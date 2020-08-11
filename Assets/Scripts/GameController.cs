using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private LevelGenerator levelGenerator;
    private GameObject levels;
    private GameObject player;

    private int gridSize;
    private int fullGridSize;
    private int initialLevel;
    private int actualLevel;

    private void Start()
    {
        levelGenerator = gameObject.GetComponent<LevelGenerator>();
        levels = levelGenerator.GetLevels();
        player = GameObject.FindGameObjectWithTag(Tags.Player);

        //GenerateDirections();

        gridSize = levelGenerator.gridSize;
        fullGridSize = gridSize * gridSize;
        initialLevel = (fullGridSize / 2) + 1;
    }

    private void Update()
    {
        // TODO: Remove this Input
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetLevel();
        }
    }

    private int GetLevel()
    {
        Vector3 playerPosition = player.transform.position;

        int x = Mathf.RoundToInt(playerPosition.x / levelGenerator.GetGroundWidth());
        int z = Mathf.RoundToInt(playerPosition.z / levelGenerator.GetGroundHeight());

        //Debug.Log(playerPosition + "\n" + x + "\n" + z + "\n" + initialLevel + "\n" + gridSize);

        return initialLevel + x + (z * gridSize);
    }

    public void CheckPlayerDirection(Collider other)
    {
        string playerDirection = other.name;

        int actualLevel = GetLevel();
        Transform levelTransform = levels.transform.GetChild(actualLevel - 1);

        string rightDirection = GetDirection(levelTransform);

        if (playerDirection == rightDirection)
        {
            Debug.Log("ACERTÔ MIZERÁVIO");
        }
        else
        {
            Debug.Log("(É um país da oropa) ERROU!");
        }
    }

    private void GenerateDirections()
    {
        Transform levelsTransform = levels.transform;

        //int levelCount = levelsTransform.childCount;
        //Debug.Log("Level Count: " + levelCount);

        foreach (Transform levelTransform in levelsTransform)
        {
            GetDirection(levelTransform);
        }
    }

    private string GetDirection(Transform levelTransform)
    {
        int objectCount = levelTransform.childCount - 1;

        int cactusCount = 0;
        int rockCount = 0;

        foreach (Transform objectTransform in levelTransform)
        {
            string objectName = objectTransform.name.Trim().Replace("(Clone)", "");
            //Debug.Log(objectTransform + " -> " + objectName);
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
        Debug.Log(levelTransform + "\nObjects count: " + objectCount + "\nCactus count: " + cactusCount + "\nRock count: " + rockCount);

        int logic = GetLogic(objectCount, cactusCount, rockCount);
        /*
        Debug.Log("Lógica " + logic);

        Debug.Log(levelTransform + "\n" +
            "Objects count: " + objectCount + "\n" +
            "Cactus count: " + cactusCount + "\n" +
            "Rock count: " + rockCount);
        */

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
