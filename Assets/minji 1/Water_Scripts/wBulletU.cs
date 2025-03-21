using UnityEngine;

public class wBulletU : MonoBehaviour
{
    public float Speed = 4.0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.up * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {

            //미사일 삭제
            Destroy(gameObject);

        }
    }

    private void OnBecameInvisible()
    {
        //자기 자신 지우기
        Destroy(gameObject);
    }
}
