using UnityEngine;
using System.Collections;

public class WP : MonoBehaviour
{
    public float wmoveSpeed = 3f; //움직이는 속도
    public float wspeedUp = 5f; //증가된 속도
    public float wkeepSpeed = 3f; //속도 저장

    Animator ani; //애니메이션 객체 선언
    public int wPower = 10; //주문력 
    public int wHP = 100; //체력 (20씩 증가)
    public int wLevel = 1; //레벨
    public int wEx = 0; //경험치




    public GameObject waterbullet; //물미사일 객체 선언
    public GameObject waterskill1; //물미사일 객체 선언

    public Transform pos1 = null;
    public Transform pos2 = null;


    void Start()
    {
        ani = GetComponent<Animator>(); //애니메이션 가져오기
        //StartCoroutine(CheckConditionCoroutine());
    }

    //IEnumerator CheckConditionCoroutine()
    //{
    //    while (true) // 계속 반복
    //    {
    //        if (wLevel >= 5)
    //        {
    //            StartCoroutine(skill1());
    //            yield break;
    //        }
    //        yield return new WaitForSeconds(1f); // 1초마다 검사
    //    }
    //}

    void Update()
    {
        //방향키에 따른 물마법사 x, y좌표
        float moveX = wmoveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float moveY = wmoveSpeed * Time.deltaTime * Input.GetAxis("Vertical");


        //좌우 이동
        if (Input.GetAxis("Horizontal") <= -0.2) //왼쪽으로 이동할 때
        {
            ani.SetBool("walk", true); //walk 모션 활성
            transform.localScale = new Vector3(-1f, 1f, 1f); //캐릭터 좌우반전

            if (Input.GetKey(KeyCode.LeftShift)) //왼쪽으로 이동하면서 Shift누를 때
            {
                ani.SetBool("walk", false); //걷는 모션 비활성화
                ani.SetBool("surf", true); //대쉬 모션 활성화

                wmoveSpeed = wspeedUp; //스피드 증가
            }
            else //Shift를 땔 때
            {
                ani.SetBool("walk", true); //걷는 모션 다시 활성화
                ani.SetBool("surf", false);
                wmoveSpeed = wkeepSpeed; //저장된 속도 원래속도에 집어넣기
            }

            if (Input.GetMouseButtonDown(0)) //왼쪽으로 이동하면서 마우스 왼쪽 버튼 누를 때
            {
                ani.SetBool("sp_atk", true); //공격 모션 활성화
                ani.SetBool("walk", false); //걷는 모션 비활성화
                GameObject go = Instantiate(waterbullet, pos2.position, Quaternion.identity); //pos2에서 미사일 발사
                Destroy(go, 5); //5초 뒤 삭제

            }
            else //마우스 왼쪽버튼을 안누르고 있을 때
            {
                ani.SetBool("walk", true); //걷는 모션 활성화
                ani.SetBool("sp_atk", false);
            }
        }
        else if (Input.GetAxis("Horizontal") >= 0.2) //오른쪽으로 이동할 때
        {
            ani.SetBool("walk", true); //걷는 모션 활성화
            transform.localScale = new Vector3(1f, 1f, 1f); //캐릭터 오른쪽 모습

            if (Input.GetKey(KeyCode.LeftShift)) //오른쪽으로 이동하면서 Shift
            {
                ani.SetBool("walk", false); //걷는 모션 비활성화
                ani.SetBool("surf", true); //대쉬 모션 활성화

                wmoveSpeed = wspeedUp; //스피드 증가
            }
            else
            {
                ani.SetBool("walk", true); //걷는 모션 활성화
                ani.SetBool("surf", false); //대쉬 모션 비활성화
                wmoveSpeed = wkeepSpeed; //저장된 속도 원래속도에 집어넣기
            }

            if (Input.GetMouseButtonDown(0)) //오른쪽으로 이동하면서 마우스 좌클릭
            {
                ani.SetBool("sp_atk", true); //공격 모션 활성화
                GameObject go = Instantiate(waterbullet, pos1.position, Quaternion.identity); //pos1에서 미사일 발사
                Destroy(go, 5); //5초 뒤 삭제
            }
            else
            {
                ani.SetBool("sp_atk", false); //공격모션 비활성화
            }


        }
        else if (Input.GetAxis("Horizontal") == 0.0f) //멈춰있을 때
        {
            ani.SetBool("walk", false); //걷는 모션 비활성화
        }

        //Shift누를 때
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ani.SetBool("surf", true);
            wmoveSpeed = wspeedUp; //스피드 증가
        }
        else
        {
            ani.SetBool("surf", false);
            wmoveSpeed = wkeepSpeed; //저장된 속도 원래속도에 집어넣기
        }

        //마우스 좌클릭
        if (Input.GetMouseButtonDown(0))
        {
            if (transform.localScale.x == 1f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                ani.SetBool("sp_atk", true);
                GameObject go = Instantiate(waterbullet, pos1.position, Quaternion.identity);
                Destroy(go, 5);
            }
            else if (transform.localScale.x == -1f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                ani.SetBool("sp_atk", true);
                GameObject go = Instantiate(waterbullet, pos2.position, Quaternion.identity);
                Destroy(go, 5);
            }

        }
        else
        {
            ani.SetBool("sp_atk", false);
        }

        //캐릭터 좌표
        transform.Translate(moveX, moveY, 0);

        if (wHP == 0)
        {
            ani.SetBool("death", true);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            ani.SetBool("takehit", true);
            wHP -= 10;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            ani.SetBool("takehit", false);
        }
    }

    //public void ExUp(int ex)
    //{
    //    wEx += ex;

    //    if (wEx >= 50)
    //    {
    //        wLevel = 2;
    //    }
    //    else if (wEx >= 100 && wEx < 200)
    //    {
    //        wLevel = 3;
    //    }
    //    else if (wEx >= 200 && wEx < 300)
    //    {
    //        wLevel = 4;
    //    }
    //    else if (wEx >= 300)
    //    {
    //        wLevel = 5;
    //    }
    //}

    //IEnumerator skill1()
    //{
    //    //공격주기
    //    float attackRate = 3;
    //    //발사체 생성갯수
    //    int count = 5;
    //    //발사체 사이의 각도
    //    float intervalAngle = 360 / count;
    //    //가중되는 각도(항상 같은 위치로 발사하지 않도록 설정
    //    float weightAngle = 0f;
    //    while (true)
    //    {
    //        for (int i = 0; i < count; ++i)
    //        {
    //            //발사체 생성
    //            GameObject clone = Instantiate(waterskill1, transform.position, Quaternion.identity);

    //            //발사체 이동 방향(각도)
    //            float angle = weightAngle + intervalAngle * i;
    //            //발사체 이동 방향(벡터)
    //            //Cos(각도)라디안 단위의 각도 표현을 위해 pi/180을 곱함
    //            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
    //            //sin(각도)라디안 단위의 각도 표현을 위해 pi/180을 곱함
    //            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

    //            //발사체 이동 방향 설정
    //            clone.GetComponent<Water>().Move(new Vector2(x, y));
    //        }
    //        //발사체가 생성되는 시작 각도 설정을 위한변수
    //        weightAngle += 1;
    //        //3초마다 미사일 발사
    //        yield return new WaitForSeconds(attackRate);
    //    }

    //}
}
