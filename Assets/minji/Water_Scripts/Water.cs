using UnityEngine;

public class Water : MonoBehaviour
{
    public float Speed = 3f; //물미사일 스피드
    Vector2 dir; //미사일이 가야할 방향 구하기
    Vector2 dirNo; //미사일 방향만 추출
    public GameObject p;
    

    void Start()
    {
        //마우스 위치
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        dir = mousePos - transform.position;
        dirNo = dir.normalized; //미사일 방향 추출
        
    }


    void Update()
    {
        //미사일 좌표
        transform.Translate(dirNo * Speed * Time.deltaTime);
    }

    public void Move(Vector2 vec)
    {
        dirNo = vec;
    }

    //미사일 충돌 시
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //적과 충돌 시
        if(collision.CompareTag("Enemy"))
        {
            Destroy(gameObject); //미사일 삭제
            Destroy(collision.gameObject); //적 삭제
            //p.gameObject.GetComponent<WP>().ExUp(50);
        }
    }

    //카메라 밖으로 나갈 시 미사일 삭제
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
