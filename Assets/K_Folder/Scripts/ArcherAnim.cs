using UnityEngine;

public class ArcherAnim : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public float moveSpeed = 5f; // �̵� �ӵ�
    public bool allowVerticalMovement = true; // ���� �̵� ����

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // �߷� ���� (Y�� �̵� �����ϵ��� ����)
    }

    void Update()
    {
        HandleMovement();
        HandleAttackInput();
    }

    void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = allowVerticalMovement ? Input.GetAxis("Vertical") : 0f; // ��/�Ʒ� �Է� �ޱ�

        rb.linearVelocity = new Vector2(moveX * moveSpeed, moveY * moveSpeed); // X, Y �̵�

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
