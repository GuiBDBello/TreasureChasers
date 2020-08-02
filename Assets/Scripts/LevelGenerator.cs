using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject ground;
    public GameObject points;
    public GameObject[] objects;
    public int size;

    private Vector3 groundPosition = Vector3.zero;

    private void Start()
    {
        Instantiate(ground, groundPosition, Quaternion.identity);

        int objectQuantity = Random.Range(1, 4);
        Debug.Log(objectQuantity);

        Transform[] pointArray = points.GetComponentsInChildren<Transform>();

        for (int i = 0; i < objectQuantity; i++)
        {
            int pointPosition = Random.Range(1, pointArray.Length);
            int objectPosition = Random.Range(0, objects.Length);

            Collider[] colliders = Physics.OverlapSphere(pointArray[pointPosition].position, 1f);

            while (colliders.Length > 1)
            {
                pointPosition = Random.Range(1, pointArray.Length);
                colliders = Physics.OverlapSphere(pointArray[pointPosition].position, 1f);
            }

            float y = 0;
            if (ChanceOf(40))
                y = Random.Range(0f, 360f);

            pointArray[pointPosition].rotation = Quaternion.Euler(0, y, 0);
            Instantiate(objects[objectPosition], pointArray[pointPosition].position, pointArray[pointPosition].rotation);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private bool ChanceOf(int percent)
    {
        return Random.Range(0f, 100f) < percent;
    }
}
