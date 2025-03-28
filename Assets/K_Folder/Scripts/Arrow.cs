using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime); // 자동 제거
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 무시
        if (other.CompareTag("Player")) return;

        if (other.CompareTag("Monster"))
        {
            Debug.Log("화살 몬스터에 명중!");

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
