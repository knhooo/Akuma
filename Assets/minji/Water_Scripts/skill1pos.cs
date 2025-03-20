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
        Debug.Log("스킬발동");
        //공격주기
        float attackRate = 3;
        //발사체 생성갯수
        int count = 5;
        //발사체 사이의 각도
        float intervalAngle = 360 / count;
        //가중되는 각도(항상 같은 위치로 발사하지 않도록 설정
        float weightAngle = 0f;
        while (true)
        {
            for (int i = 0; i < count; ++i)
            {
                //발사체 생성
                GameObject clone = Instantiate(waterskill1, transform.position, Quaternion.identity);

                //발사체 이동 방향(각도)
                float angle = weightAngle + intervalAngle * i;
                //발사체 이동 방향(벡터)
                //Cos(각도)라디안 단위의 각도 표현을 위해 pi/180을 곱함
                float x = Mathf.Cos(angle * Mathf.Deg2Rad);
                //sin(각도)라디안 단위의 각도 표현을 위해 pi/180을 곱함
                float y = Mathf.Sin(angle * Mathf.Deg2Rad);

                //발사체 이동 방향 설정
                clone.GetComponent<Water>().Move(new Vector2(x, y));
            }
            //발사체가 생성되는 시작 각도 설정을 위한변수
            weightAngle += 1;
            //3초마다 미사일 발사
            yield return new WaitForSeconds(attackRate);
        }

    }
}
