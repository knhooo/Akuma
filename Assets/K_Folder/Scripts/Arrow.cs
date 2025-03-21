using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime); // �ð� ������ ����
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� ����
        if (other.CompareTag("Player")) return;

        // ���ʹ� ������ ����
        if (other.CompareTag("Monster"))
        {
            Debug.Log("ȭ�� ���Ϳ� ����!");
            Destroy(gameObject);
        }

        // �� �� ������ ������Ʈ�� �浹 �� ���� (���� ����)
        // else if (!other.isTrigger)
        // {
        //     Destroy(gameObject);
        // }
    }
}
