using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public float lifetime = 3f;
    private Vector2 direction;

    public void Initialize(Vector2 dir)
    {
        direction = dir.normalized;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += (Vector3)direction * speed * Time.deltaTime;
    }

    //충돌 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어와 충돌했을 때
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(10);
            }
            Destroy(gameObject);  // 총알 제거
        }
    }
}
