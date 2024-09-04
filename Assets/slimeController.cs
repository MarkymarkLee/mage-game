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
    private bool isDead = false;
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
        if(isDead)
        {
            if(Input.GetKey("r"))
            {
                animator.SetBool("is_revive", true);
                isDead = false;
                animator.SetBool("is_dead", false);
            }
            else
            {
                animator.SetBool("is_revive", false);
            }
            return;
        }
        MovePlayer();
        if(Input.GetKey("space"))
        {
            animator.SetBool("is_jump", true);
        }
        else
        {
            animator.SetBool("is_jump", false);
        }
        if(Input.GetKey("q"))
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
        transform.position += new Vector3(direction.x, direction.y, 0) * Time.deltaTime;
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
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "enemy")
        {
            isDead = true;
            animator.SetBool("is_dead", true);
        }
    }
}
