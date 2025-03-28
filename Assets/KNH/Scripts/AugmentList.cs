using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class AugmentList : MonoBehaviour
{
    [SerializeField] GameObject augIcon;
    [SerializeField] GameObject[] Frames;
    [SerializeField] GameObject[] Images;
    [SerializeField] GameObject[] Texts;
    [SerializeField] Sprite[] sprites;
    int code;
    List<int> codeList = new List<int>();

    public void AddAugmentIcon()
    {
        if (codeList.Contains(code))//������ ���õ� ������ ���
        {
            codeList.Add(code);//����Ʈ�� �߰�
            //�ؽ�Ʈ Ȱ��ȭ
            Texts[code].SetActive(true);
            int count = codeList.Count(n => n == code);
            Texts[code].GetComponent<Text>().text = "+" + count;
            switch (code)
            {
                case 0://�ִ� ü�� ����
                    break;
                case 1://���ݷ� ����
                    break;
                case 2://�̵��ӵ� ����
                    break;
            }
        }
        else//���� ���õ� ������ ���
        {
            codeList.Add(code);//����Ʈ�� �߰�
            //�̹��� Ȱ��ȭ
            Images[code].SetActive(true);
            Images[code].GetComponent<Image>().sprite = sprites[code];
        }
    }

    public void SetCode(int num)
    {
        code = num;
    }
}
