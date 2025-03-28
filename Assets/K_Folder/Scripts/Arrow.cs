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
        // 플레이어는 무시
        if (other.CompareTag("Player")) return;

        // 몬스터와 보스 모두 처리
        if (other.CompareTag("Monster") || other.CompareTag("Boss"))
        {
            Debug.Log("화살 적중 대상: " + other.name);

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
