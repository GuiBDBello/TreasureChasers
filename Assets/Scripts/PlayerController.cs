using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 2.0f;

    private CharacterController controller;
    private GameController gameController;
    private Animator animator;

    private void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        controller.center = new Vector3(controller.center.x, 5f, controller.center.z);
        controller.radius = 1.5f;
        controller.height = 10f;
        gameController = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
            gameObject.transform.forward = move;

        Animate(move);
    }

    private void OnTriggerEnter(Collider other)
    {
        gameController.CheckPlayerDirection(other);
    }

    private void Animate(Vector3 movement)
    {
        if (movement.magnitude > 0)
        {
            animator.SetBool(Animations.IsMoving, true);
            //gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.LookRotation(movement), 0.1f);
            // break dance
            gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, Quaternion.LookRotation(movement), 0.1f);
        }
        else animator.SetBool(Animations.IsMoving, false);
    }
}
