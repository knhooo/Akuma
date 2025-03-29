using UnityEngine;

public class waterPupleSmall : MonoBehaviour
{
    public float Speed = 5f;
    Vector2 vec2 = Vector2.down;
    public GameObject wbsmll;
    void Update()
    {
        transform.Translate(vec2 * Speed * Time.deltaTime);
    }

    public void Move(Vector2 vec)
    {
        vec2 = vec;
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster")|| collision.CompareTag("Boss"))
        {
            wSoundManager.instance.tWater();
            Instantiate(wbsmll, transform.position, Quaternion.identity);
        }
    }
}
