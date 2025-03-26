using UnityEngine;

public class wBulletD2 : MonoBehaviour
{
    public float Speed = 4.0f;
    public GameObject wbd;

    void Update()
    {
        transform.Translate(Vector2.down * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") || collision.CompareTag("Boss"))
        {
            wSoundManager.instance.tWater();
            Instantiate(wbd, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

