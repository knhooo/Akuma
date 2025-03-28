using UnityEngine;
using System.Collections;

public class WaterPuple2 : MonoBehaviour
{
    public float Speed = 2f; //물방울 속도
    Vector2 dir;
    Vector2 dirNo;

    void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //마우스로 클릭한 위치 추출

        dir = mousePos - transform.position; //마우스로 클릭한 위치로 향하는 벡터 추출
        dirNo = dir.normalized; //마우스로 클릭한 위치 가리키는 방향
        StartCoroutine("waterPstay"); //물방울 코루틴 시작
    }


    void Update()
    {
        transform.Translate(dirNo * Speed * Time.deltaTime); //실시간 위치 이동
    }

    IEnumerator waterPstay() //물방울 코루틴
    {
        yield return new WaitForSeconds(2); //2초동안 정상 스피드로 날아감
        while (Speed > 0)
        {
            Speed -= Time.deltaTime; //정지할 때까지 조금씩 스피드 감소
        }
        yield return new WaitForSeconds(3);//정지하면 3초간 머무름
        wSoundManager.instance.tWaterP(); //물방울 터지는 소리
        Destroy(gameObject); //삭제
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster") || collision.CompareTag("Boss")) //몬스터 및 보스와 충돌했을 때
        {
            Speed = 0; //즉시 정지
        }
    }
}

