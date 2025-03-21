using UnityEngine;

public class Arrow : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime); // 시간 지나면 제거
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어는 무시
        if (other.CompareTag("Player")) return;

        // 몬스터는 맞으면 제거
        if (other.CompareTag("Monster"))
        {
            Debug.Log("화살 몬스터에 명중!");
            Destroy(gameObject);
        }

        // 벽 등 나머지 오브젝트와 충돌 시 제거 (선택 사항)
        // else if (!other.isTrigger)
        // {
        //     Destroy(gameObject);
        // }
    }
}
