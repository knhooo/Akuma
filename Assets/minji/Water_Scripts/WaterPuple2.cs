using UnityEngine;
using System.Collections;

public class WaterPuple2 : MonoBehaviour
{
    public float Speed = 2f; //����� �ӵ�
    Vector2 dir;
    Vector2 dirNo;

    void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //���콺�� Ŭ���� ��ġ ����

        dir = mousePos - transform.position; //���콺�� Ŭ���� ��ġ�� ���ϴ� ���� ����
        dirNo = dir.normalized; //���콺�� Ŭ���� ��ġ ����Ű�� ����
        StartCoroutine("waterPstay"); //����� �ڷ�ƾ ����
    }


    void Update()
    {
        transform.Translate(dirNo * Speed * Time.deltaTime); //�ǽð� ��ġ �̵�
    }

    IEnumerator waterPstay() //����� �ڷ�ƾ
    {
        yield return new WaitForSeconds(2); //2�ʵ��� ���� ���ǵ�� ���ư�
        while (Speed > 0)
        {
            Speed -= Time.deltaTime; //������ ������ ���ݾ� ���ǵ� ����
        }
        yield return new WaitForSeconds(3);//�����ϸ� 3�ʰ� �ӹ���
        wSoundManager.instance.tWaterP(); //����� ������ �Ҹ�
        Destroy(gameObject); //����
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") || collision.CompareTag("Boss")) //���� �� ������ �浹���� ��
        {
            Speed = 0; //��� ����
        }
    }
}

