using UnityEngine;

public class wSkill1 : MonoBehaviour
{

    public float w1Speed = 3f;
    Vector2 w1vec2 = Vector2.down;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(w1vec2 * w1Speed * Time.deltaTime);
    }

    public void Move(Vector2 vec)
    {
        w1vec2 = vec;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
