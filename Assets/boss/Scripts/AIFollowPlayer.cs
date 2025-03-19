using UnityEngine;

public class AIFollowPlayer : MonoBehaviour
{
    public Transform player;  // �÷��̾� ��ġ
    public float moveSpeed = 5f;  // �̵� �ӵ�
    public float stoppingDistance = 1f;  // ��ǥ���� �ּ� �Ÿ�

    private SpriteRenderer spriteRenderer;  // ��������Ʈ ������

    void Start()
    {
        // ��������Ʈ ������ �ʱ�ȭ
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // �÷��̾���� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, player.position);

        // �÷��̾ ���� �Ÿ� ���� ���� ���� ����
        if (distance > stoppingDistance)
        {
            // �÷��̾� �������� �̵�
            Vector3 direction = (player.position - transform.position).normalized;

            // 2D���� Z ���� �����Ͽ� �̵�
            direction.z = 0;  // Z ���� 0���� ����

            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // ������ �÷��̾ ���󰡸鼭 �¿� ���� ó��
        // �÷��̾��� ��ġ�� ���� ������ �����̳� ���������� ����
        if (player.position.x > transform.position.x)
        {
            // �÷��̾ ������ �����ʿ� ������ ��������Ʈ�� ���������� ����
            spriteRenderer.flipX = false;
        }
        else if (player.position.x < transform.position.x)
        {
            // �÷��̾ ������ ���ʿ� ������ ��������Ʈ�� ����
            spriteRenderer.flipX = true;
        }
    }
}
