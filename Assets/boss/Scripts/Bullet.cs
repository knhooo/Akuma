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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 태그와 충돌했을 때
        if (collision.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(10);
            }
            Destroy(gameObject);
        }
    }
}
