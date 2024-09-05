using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerController : MonoBehaviour
{
    Animator animator;
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction fireAction;
    InputAction dashAction;
    InputAction interactAction;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        fireAction = playerInput.actions.FindAction("Fire");
        dashAction = playerInput.actions.FindAction("Dash");
        interactAction = playerInput.actions.FindAction("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, direction.y, 0) * Time.deltaTime * speed;
    }
}
