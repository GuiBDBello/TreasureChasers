using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 2.0f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;

    private LevelGenerator levelGenerator;

    private int gridSize;
    private int fullGridSize;
    private int initialLevel;
    private int actualLevel;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        levelGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelGenerator>();

        gridSize = levelGenerator.gridSize;
        fullGridSize = gridSize * gridSize;
        initialLevel = (fullGridSize / 2) + 1;
        actualLevel = getLevel();
    }

    private void Update()
    {
        move();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colidiu com " + other.name);

        switch (other.name)
        {
            case "North":
                break;
            case "South":
                break;
            case "East":
                break;
            case "West":
                break;
        }
    }

    private void move()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private int getLevel()
    {
        Vector3 playerPosition = gameObject.transform.position;

        int x = Mathf.RoundToInt(playerPosition.x / levelGenerator.getGroundWidth());
        int z = Mathf.RoundToInt(playerPosition.z / levelGenerator.getGroundHeight());

        return initialLevel + x + (z * gridSize);
    }
}
