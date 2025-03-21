using UnityEngine;

public class wBulletR2 : MonoBehaviour
{
    public float Speed = 4.0f;
    public GameObject wbr;

    void Update()
    {
        transform.Translate(Vector2.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            Instantiate(wbr, transform.position, Quaternion.identity);
            Destroy(gameObject);

        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
