using UnityEngine;

public class wBulletU2 : MonoBehaviour
{
    public float Speed = 4.0f;
    public GameObject wbu;

    void Update()
    {
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") || collision.CompareTag("Boss"))
        {
            wSoundManager.instance.tWater();
            Instantiate(wbu, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
