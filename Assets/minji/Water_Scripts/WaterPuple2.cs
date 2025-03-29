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
    }


    void Update()
    {
        transform.Translate(dirNo * Speed * Time.deltaTime); //실시간 위치 이동
    }

    public void waterPstay()
    {
        //발사체 생성갯수
        int count = 15;
        //발사체 사이의 각도
        float intervalAngle = 360 / count;
        //가중되는 각도(항상 같은 위치로 발사하지 않도록 설정
        float weightAngle = 0f;
        wSoundManager.instance.tWaterP();

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

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}

