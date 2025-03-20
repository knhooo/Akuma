using UnityEngine;

public class ArcherAnim : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    public float moveSpeed = 5f; // �̵� �ӵ�
    public bool allowVerticalMovement = true; // ���� �̵� ����

    public GameObject arrowPrefab; // ȭ�� ������
    public Transform firePoint; // ȭ���� ������ ��ġ
    public float arrowSpeed = 10f; // ȭ�� �ӵ�

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
        if (Input.GetMouseButtonDown(1)) // ��Ŭ�� ���� (���� ����)
        {
            animator.SetTrigger("1Attack");
        }
        else if (Input.GetMouseButtonDown(0)) // ��Ŭ�� ���� (���Ÿ� ȭ��)
        {
            animator.SetTrigger("2Attack");
            Invoke("ShootArrow", 0.4f); // 0.3초 뒤에 화살 발사
        }
    }

    // �ִϸ��̼� �̺�Ʈ���� ȣ��� �Լ�
    public void ShootArrow()
    {
        Debug.Log("ShootArrow 함수 실행됨!"); // 실행 확인용 로그
        if (arrowPrefab != null && firePoint != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float direction = transform.localScale.x; // ĳ���� ���� (��/��)
                rb.linearVelocity = new Vector2(direction * arrowSpeed, 0); // ���⿡ ���� �߻�
            }
        }
    }
}
