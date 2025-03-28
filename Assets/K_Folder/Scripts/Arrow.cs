using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime); // �ڵ� ����
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �÷��̾�� ����
        if (other.CompareTag("Player")) return;

        // ���Ϳ� ���� ��� ó��
        if (other.CompareTag("Monster") || other.CompareTag("Boss"))
        {
            Debug.Log("ȭ�� ���� ���: " + other.name);

            HH_Monster monster = other.GetComponent<HH_Monster>();
            if (monster != null)
            {
                GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
                int atk = playerObj.GetComponent<Player>().Attack;
                monster.SendMessage("TakeDamageFromArrow", atk, SendMessageOptions.DontRequireReceiver);
            }

            Destroy(gameObject);
        }
    }
}
