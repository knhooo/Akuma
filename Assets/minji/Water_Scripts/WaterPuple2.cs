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
        StartCoroutine(Waterpp());
    }


    void Update()
    {
        transform.Translate(dirNo * Speed * Time.deltaTime); //�ǽð� ��ġ �̵�
    }

    //IEnumerator waterPstay() //����� �ڷ�ƾ
    //{
    //    yield return new WaitForSeconds(2); //2�ʵ��� ���� ���ǵ�� ���ư�
    //    while (Speed > 0)
    //    {
    //        Speed -= Time.deltaTime; //������ ������ ���ݾ� ���ǵ� ����
    //    }
    //    yield return new WaitForSeconds(3);//�����ϸ� 3�ʰ� �ӹ���
    //    wSoundManager.instance.tWaterP(); //����� ������ �Ҹ�
    //    Destroy(gameObject); //����
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Monster") || collision.CompareTag("Boss")) //���� �� ������ �浹���� ��
    //    {
    //        Speed = 0; //��� ����
    //    }
    //}

    IEnumerator Waterpp()
    {
        yield return new WaitForSeconds(1);
        waterPstay();
        Destroy(gameObject);
    }

    public void waterPstay()
    {
        //�߻�ü ��������
        int count = 15;
        //�߻�ü ������ ����
        float intervalAngle = 360 / count;
        //���ߵǴ� ����(�׻� ���� ��ġ�� �߻����� �ʵ��� ����
        float weightAngle = 0f;

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
}

