using UnityEngine;
using System.Collections;

public class WaterPuple2 : MonoBehaviour
{
    public float Speed = 5f; //물방울 속도
    Vector2 dir;
    Vector2 dirNo;
    public GameObject wps;


    void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition); //마우스로 클릭한 위치 추출

        dir = mousePos - transform.position; //마우스로 클릭한 위치로 향하는 벡터 추출
        dirNo = dir.normalized; //마우스로 클릭한 위치 가리키는 방향
        StartCoroutine(Waterpp());
    }


    void Update()
    {
        transform.Translate(dirNo * Speed * Time.deltaTime); //실시간 위치 이동
    }

    //IEnumerator waterPstay() //물방울 코루틴
    //{
    //    yield return new WaitForSeconds(2); //2초동안 정상 스피드로 날아감
    //    while (Speed > 0)
    //    {
    //        Speed -= Time.deltaTime; //정지할 때까지 조금씩 스피드 감소
    //    }
    //    yield return new WaitForSeconds(3);//정지하면 3초간 머무름
    //    wSoundManager.instance.tWaterP(); //물방울 터지는 소리
    //    Destroy(gameObject); //삭제
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Monster") || collision.CompareTag("Boss")) //몬스터 및 보스와 충돌했을 때
    //    {
    //        Speed = 0; //즉시 정지
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
        //발사체 생성갯수
        int count = 15;
        //발사체 사이의 각도
        float intervalAngle = 360 / count;
        //가중되는 각도(항상 같은 위치로 발사하지 않도록 설정
        float weightAngle = 0f;

        //원 형태로 방사하는 발사체 생성(count 갯수 만큼)

            for (int i = 0; i < count; ++i)
            {
                //발사체 생성
                GameObject clone = Instantiate(wps, transform.position, Quaternion.identity);

                //발사체 이동 방향(각도)
                float angle = weightAngle + intervalAngle * i;
                //발사체 이동 방향(벡터)
                //Cos(각도)라디안 단위의 각도 표현을 위해 pi/180을 곱함
                float x = Mathf.Cos(angle * Mathf.Deg2Rad);
                //sin(각도)라디안 단위의 각도 표현을 위해 pi/180을 곱함
                float y = Mathf.Sin(angle * Mathf.Deg2Rad);

                //발사체 이동 방향 설정
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

