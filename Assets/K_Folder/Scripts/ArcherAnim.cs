using UnityEngine;

public class ArcherAnim : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public float moveSpeed = 5f;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
        HandleAttackInput();
        HandleIdleState();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveX * moveSpeed, rb.linearVelocity.y);

        if (moveX != 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        if (moveX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left Mouse Click for First Attack
        {
            animator.SetTrigger("1Attack");
            isAttacking = true;
        }
        else if (Input.GetMouseButtonDown(1)) // Right Mouse Click for Second Attack
        {
            animator.SetTrigger("2Attack");
            isAttacking = true;
        }
    }

    void HandleIdleState()
    {
        if (!isAttacking)
        {
            animator.ResetTrigger("1Attack");
            animator.ResetTrigger("2Attack");
        }
    }

    // Call this method in animation event to reset attack state after animation completes
    public void ResetAttack()
    {
        isAttacking = false;
        animator.ResetTrigger("1Attack");
        animator.ResetTrigger("2Attack");
    }
}
