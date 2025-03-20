using UnityEngine;

public class Water : MonoBehaviour
{
    public float Speed = 3f; //���̻��� ���ǵ�
    Vector2 dir; //�̻����� ������ ���� ���ϱ�
    Vector2 dirNo; //�̻��� ���⸸ ����

    void Start()
    {
        //���콺 ��ġ
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        dir = mousePos - transform.position;
        dirNo = dir.normalized; //�̻��� ���� ����
    }


    void Update()
    {
        //�̻��� ��ǥ
        transform.Translate(dirNo * Speed * Time.deltaTime);
    }

    //�̻��� �浹 ��
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //���� �浹 ��
        if(collision.CompareTag("Enemy"))
        {
            Destroy(gameObject); //�̻��� ����
            Destroy(collision.gameObject); //�� ����
        }
    }

    //ī�޶� ������ ���� �� �̻��� ����
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
