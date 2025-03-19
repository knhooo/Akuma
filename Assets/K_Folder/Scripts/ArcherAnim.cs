using UnityEngine;

public class ArcherAnim : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public float moveSpeed = 5f; // 이동 속도
    public bool allowVerticalMovement = true; // 상하 이동 여부

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // 중력 제거 (Y축 이동 가능하도록 설정)
    }

    void Update()
    {
        HandleMovement();
        HandleAttackInput();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = allowVerticalMovement ? Input.GetAxis("Vertical") : 0f; // 위/아래 입력 받기

        rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed); // X, Y 이동

        animator.SetBool("isMoving", moveX != 0 || moveY != 0);

        if (moveX > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveX < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("1Attack");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("2Attack");
        }
    }
}
