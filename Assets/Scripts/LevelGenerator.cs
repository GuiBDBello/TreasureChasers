using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject ground;
    public GameObject[] objects;
    public int gridSize;

    private GameObject points;
    private GameObject[] instantiatedObjects;
    private int groundWidth = 100;
    private int groundHeight = 75;

    private void Start()
    {
        CreateLevels(gridSize);
    }

    private void Update()
    {
        // TODO: Remove this Input
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void CreateLevels(int gridSize)
    {
        GameObject levels = new GameObject();
        levels.transform.position = new Vector3(0.0f, 0.0f, 0.0f);
        levels.name = "Levels";

        int size = (int) Mathf.Sqrt(gridSize);

        Vector3 levelPosition;

        int i = 1;
        while (i <= size)
        {
            int halfSize = (int)Mathf.Round((gridSize / 2));
            int x = -halfSize * groundWidth;
            int z = -halfSize * groundHeight;

            for (int h = 0; h < gridSize; h++)
            {
                for (int w = 0; w < gridSize; w++)
                {
                    levelPosition = new Vector3(x, 0.0f, z);
                    //Debug.Log(levelPosition);

                    GameObject level = CreateLevel(levelPosition);
                    //level.transform.position = levelPosition;
                    level.name = "Level " + i;
                    level.transform.parent = levels.transform;

                    x += groundWidth;
                    i++;
                }
                x = -halfSize * groundWidth;
                z += groundHeight;
            }
        }
    }

    private GameObject CreateLevel(Vector3 levelPosition)
    {
        GameObject level = new GameObject();

        GameObject instantiatedGround = Instantiate(ground, levelPosition, Quaternion.identity);
        instantiatedGround.transform.parent = level.transform;

        points = instantiatedGround.transform.GetChild(0).gameObject;
        Debug.Log("points");
        Debug.Log(points.transform.position);

        int objectQuantity = Random.Range(1, 4);
        Debug.Log("objectQuantity: " + objectQuantity);

        instantiatedObjects = CreateObjects(objectQuantity);

        foreach (GameObject instantiatedObject in instantiatedObjects)
        {
            instantiatedObject.transform.parent = level.transform;
        }

        return level;
    }

    private GameObject[] CreateObjects(int objectQuantity)
    {
        GameObject[] instantiatedObjects = new GameObject[objectQuantity];
        Transform[] pointArray = points.GetComponentsInChildren<Transform>();

        for (int j = 0; j < objectQuantity; j++)
        {
            int pointPosition = Random.Range(1, pointArray.Length);
            int objectPosition = Random.Range(0, objects.Length);

            Collider[] colliders = Physics.OverlapSphere(pointArray[pointPosition].position, 1f);
            Debug.Log("Colliders Length: " + colliders.Length);

            foreach(Collider collider in colliders)
            {
                Debug.Log(collider);
            }

            while (colliders.Length > 1)
            {
                pointPosition = Random.Range(1, pointArray.Length);
                colliders = Physics.OverlapSphere(pointArray[pointPosition].position, 1f);
            }

            float y = 0;
            if (ChanceOf(40))
                y = Random.Range(0f, 360f);

            pointArray[pointPosition].rotation = Quaternion.Euler(0, y, 0);
            instantiatedObjects[j] = Instantiate(objects[objectPosition], pointArray[pointPosition].position, pointArray[pointPosition].rotation);
        }

        return instantiatedObjects;
    }

    private bool ChanceOf(int percent)
    {
        return Random.Range(0f, 100f) < percent;
    }
}
