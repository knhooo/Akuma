using UnityEngine;
using System.Collections;

public class wTakeDamageEffect : MonoBehaviour
{
    //�������� �ǰݽ� ����Ʈ
    void Start()
    {
        StartCoroutine("TDeffect");
    }

    IEnumerator TDeffect()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject); //����Ʈ �ִϸ��̼� ���� �� ������� �ϱ�
    }
}
