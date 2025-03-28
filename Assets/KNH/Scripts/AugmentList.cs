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
        if (codeList.Contains(code))//기존에 선택된 증강인 경우
        {
            codeList.Add(code);//리스트에 추가
            //텍스트 활성화
            Texts[code].SetActive(true);
            int count = codeList.Count(n => n == code);
            Texts[code].GetComponent<Text>().text = "+" + count;
            switch (code)
            {
                case 0://최대 체력 증가
                    break;
                case 1://공격력 증가
                    break;
                case 2://이동속도 증가
                    break;
            }
        }
        else//새로 선택된 증강인 경우
        {
            codeList.Add(code);//리스트에 추가
            //이미지 활성화
            Images[code].SetActive(true);
            Images[code].GetComponent<Image>().sprite = sprites[code];
        }
    }

    public void SetCode(int num)
    {
        code = num;
    }
}
