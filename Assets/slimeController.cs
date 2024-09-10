using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class slimeController : MonoBehaviour
{
    Animator animator;
    PlayerInput playerInput;
    InputAction moveAction;
    // CapsuleCollider collider;
    private float speed = 0f;
    public float normal_speed;
    public float shift_speed;

    // Start is called before the first frame update
    void Start()
    {
        // collider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed = shift_speed;
        }
        else
        {
            speed = normal_speed;
        }
        if(Input.GetMouseButton(0)) // left mouse button
        {
            animator.SetBool("is_attack_1", true);
        }
        else
        {
            animator.SetBool("is_attack_1", false);
        }
    }

    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, direction.y, 0) * Time.deltaTime * speed;
        if(direction.magnitude > 0)
        {
            animator.SetBool("is_run", true);
            // transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
        }
        else
        {
            animator.SetBool("is_run", false);
        }
    }
}
