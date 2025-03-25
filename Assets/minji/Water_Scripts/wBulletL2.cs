using UnityEngine;

public class wBulletL2 : MonoBehaviour
{
    public float Speed = 4.0f;
    public GameObject wbl;

    void Update()
    {
        transform.Translate(Vector2.left * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            wSoundManager.instance.tWater();
            Instantiate(wbl, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
