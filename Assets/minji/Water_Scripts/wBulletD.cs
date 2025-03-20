using UnityEngine;

public class wBulletD : MonoBehaviour
{
    public float Speed = 4.0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.down * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {


            Destroy(gameObject);

        }
    }

    private void OnBecameInvisible()
    {

        Destroy(gameObject);
    }
}
