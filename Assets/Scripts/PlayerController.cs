using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 2.0f;

    private CharacterController controller;
    private GameController gameController;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        gameController = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
    }

    private void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
            gameObject.transform.forward = move;
    }

    private void OnTriggerEnter(Collider other)
    {
        gameController.CheckPlayerDirection(other);
    }
}
