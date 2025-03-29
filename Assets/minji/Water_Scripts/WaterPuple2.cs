using UnityEngine;
using System.Collections;

public class WaterPuple2 : MonoBehaviour
{
    public float Speed = 5f; //����� �ӵ�
    Vector2 dir;
    Vector2 dirNo;
    public GameObject wps;


    void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //���콺�� Ŭ���� ��ġ ����

        dir = mousePos - transform.position; //���콺�� Ŭ���� ��ġ�� ���ϴ� ���� ����
        dirNo = dir.normalized; //���콺�� Ŭ���� ��ġ ����Ű�� ����
    }


    void Update()
    {
        transform.Translate(dirNo * Speed * Time.deltaTime); //�ǽð� ��ġ �̵�
    }

    public void waterPstay()
    {
        //�߻�ü ��������
        int count = 15;
        //�߻�ü ������ ����
        float intervalAngle = 360 / count;
        //���ߵǴ� ����(�׻� ���� ��ġ�� �߻����� �ʵ��� ����
        float weightAngle = 0f;
        wSoundManager.instance.tWaterP();

        //�� ���·� ����ϴ� �߻�ü ����(count ���� ��ŭ)
        for (int i = 0; i < count; ++i)
        {
            //�߻�ü ����
            GameObject clone = Instantiate(wps, transform.position, Quaternion.identity);

            //�߻�ü �̵� ����(����)
            float angle = weightAngle + intervalAngle * i;
            //�߻�ü �̵� ����(����)
            //Cos(����)���� ������ ���� ǥ���� ���� pi/180�� ����
            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
            //sin(����)���� ������ ���� ǥ���� ���� pi/180�� ����
            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

            //�߻�ü �̵� ���� ����
            clone.GetComponent<waterPupleSmall>().Move(new Vector2(x, y));
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") || collision.CompareTag("Boss"))
        {
            waterPstay();
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

