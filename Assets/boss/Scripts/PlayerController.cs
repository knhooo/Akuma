using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �̵� �ӵ�

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Rigidbody2D ������Ʈ ��������
    }

    void Update()
    {
        // �Է� �� �޾ƿ��� (WASD �Ǵ� ����Ű)
        float moveX = Input.GetAxisRaw("Horizontal"); // �¿� �̵� (-1 ~ 1)
        float moveY = Input.GetAxisRaw("Vertical");   // ���� �̵� (-1 ~ 1)

        // �밢�� �̵� �ӵ� ���� (����ȭ)
        moveInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Rigidbody�� �̿��� �̵� ó��
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
