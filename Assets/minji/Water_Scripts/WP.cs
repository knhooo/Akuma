using UnityEngine;
using System.Collections;

public class WP : Player //물마법사 스크립트
{
    public float wspeedUp = 5f; //증가된 속도
    public float wkeepSpeed = 3f; //속도 저장

    Animator ani; //애니메이션 객체 선언




    public GameObject waterbullet; //물미사일 객체 선언
    public GameObject waterbulletU; //물미사일 객체 선언
    public GameObject waterbulletR; //물미사일 객체 선언
    public GameObject waterbulletL; //물미사일 객체 선언
    public GameObject waterbulletD; //물미사일 객체 선언

    public Transform pos1 = null;
    public Transform pos2 = null;



    void Start()
    {
        ani = GetComponent<Animator>(); //애니메이션 가져오기
        StartCoroutine("skill1");
    }


    void Update()
    {
        
        //방향키에 따른 물마법사 x, y좌표
        float moveX = speed * Time.deltaTime * Input.GetAxis("Horizontal");
        float moveY = speed * Time.deltaTime * Input.GetAxis("Vertical");


        //좌우 이동
        if (Input.GetAxis("Horizontal") <= -0.2) //왼쪽으로 이동할 때
        {
            ani.SetBool("walk", true); //walk 모션 활성
            transform.localScale = new Vector3(-1f, 1f, 1f); //캐릭터 좌우반전

            if (Input.GetKey(KeyCode.LeftShift)) //왼쪽으로 이동하면서 Shift누를 때
            {
                ani.SetBool("walk", false); //걷는 모션 비활성화
                ani.SetBool("surf", true); //대쉬 모션 활성화

                speed = wspeedUp; //스피드 증가
            }
            else //Shift를 땔 때
            {
                ani.SetBool("walk", true); //걷는 모션 다시 활성화
                ani.SetBool("surf", false);
                speed = wkeepSpeed; //저장된 속도 원래속도에 집어넣기
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

                speed = wspeedUp; //스피드 증가
            }
            else
            {
                ani.SetBool("walk", true); //걷는 모션 활성화
                ani.SetBool("surf", false); //대쉬 모션 비활성화
                speed = wkeepSpeed; //저장된 속도 원래속도에 집어넣기
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
            speed = wspeedUp; //스피드 증가
        }
        else
        {
            ani.SetBool("surf", false);
            speed = wkeepSpeed; //저장된 속도 원래속도에 집어넣기
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

        if (hp <= 0)
        {
            ani.SetBool("death", true);
        }

    }

    IEnumerator skill1() //10초 뒤 <임시로 정해둠. 첫번째 자동 스킬 발동
    {
        yield return new WaitForSeconds(10);
        while (true)
        {
            Instantiate(waterbulletU, transform.position, Quaternion.identity);
            Instantiate(waterbulletR, transform.position, Quaternion.identity);
            Instantiate(waterbulletL, transform.position, Quaternion.identity);
            Instantiate(waterbulletD, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(2);
        }
        
    }

    public override void TakeDamage(int dmg) //데미지 입는 메서드 오버라이딩
    {
        hp -= dmg;
    }

}
