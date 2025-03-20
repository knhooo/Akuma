using UnityEngine;
using System.Collections;

public class skill1pos : MonoBehaviour
{
    public GameObject waterskill1;

    void Start()
    {
        StartCoroutine(skill1());
    }

    IEnumerator skill1()
    {
        Debug.Log("��ų�ߵ�");
        //�����ֱ�
        float attackRate = 3;
        //�߻�ü ��������
        int count = 5;
        //�߻�ü ������ ����
        float intervalAngle = 360 / count;
        //���ߵǴ� ����(�׻� ���� ��ġ�� �߻����� �ʵ��� ����
        float weightAngle = 0f;
        while (true)
        {
            for (int i = 0; i < count; ++i)
            {
                //�߻�ü ����
                GameObject clone = Instantiate(waterskill1, transform.position, Quaternion.identity);

                //�߻�ü �̵� ����(����)
                float angle = weightAngle + intervalAngle * i;
                //�߻�ü �̵� ����(����)
                //Cos(����)���� ������ ���� ǥ���� ���� pi/180�� ����
                float x = Mathf.Cos(angle * Mathf.Deg2Rad);
                //sin(����)���� ������ ���� ǥ���� ���� pi/180�� ����
                float y = Mathf.Sin(angle * Mathf.Deg2Rad);

                //�߻�ü �̵� ���� ����
                clone.GetComponent<Water>().Move(new Vector2(x, y));
            }
            //�߻�ü�� �����Ǵ� ���� ���� ������ ���Ѻ���
            weightAngle += 1;
            //3�ʸ��� �̻��� �߻�
            yield return new WaitForSeconds(attackRate);
        }

    }
}
