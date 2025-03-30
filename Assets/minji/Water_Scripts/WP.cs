using UnityEngine;
using System.Collections;
using System;

public class WP : Player //물마법사 스크립트
{

    Animator ani; //애니메이션 객체 선언
    private Rigidbody2D rb;

    public GameObject wExposion; //피 이펙트

    public GameObject waterbullet; //물미사일 객체 선언
    public GameObject waterPbullet; //물미사일 (보라색) 객체 선언
    public GameObject waterbulletU; //물미사일 객체 선언
    public GameObject waterbulletR; //물미사일 객체 선언
    public GameObject waterbulletL; //물미사일 객체 선언
    public GameObject waterbulletD; //물미사일 객체 선언

    public float wspeed;
    public float surfing=1f;
    public float surfingTime = 0f;
    private bool isDashing=false;
    private bool speedzero = false;

    public Transform pos1 = null;
    public Transform pos2 = null;

    void Start()
    {
        ani = GetComponent<Animator>(); //애니메이션 가져오기
        StartCoroutine("skill1"); //자동공격 활성화
        dashCoolTimer = dashCoolTime;
        skillCoolTimer = skillCoolTime;
        wspeed = speed;
        rb = GetComponent<Rigidbody2D>();

    }


    protected override void Update()
    {
        if (GameManager.instance.isAugementActive)
            return;

        base.Update();

        if (!ani.GetBool("surf"))
        {
            wspeed = speed;
            surfing = 1f;
            if (dashCoolTimer < dashCoolTime) dashCoolTimer += Time.deltaTime;
            if (dashCoolTimer >= dashCoolTime) canUseDash = false;

        }
        if (!ani.GetBool("atk2"))
        {
            if (skillCoolTimer < skillCoolTime) skillCoolTimer += Time.deltaTime;
            if (skillCoolTimer >= skillCoolTime) canUseSkill = false;

        }

        //대쉬중일때
        if (isDashing)
        {
            surfing -= Time.deltaTime;

            // 대쉬 종료 조건
            if (surfing <= 0)
            {
                isDashing = false;
            }
        }

        //대쉬가 끝나면
        if (!isDashing)
        {
            canUseDash = true;
            speed = wspeed;
            wSoundManager.instance.surfEnd();
            ani.SetBool("surf", false);
        }


        //레벨업
        if (exp >= maxExp)
        {
            level += 1;
            exp -= maxExp; //남은 경험치 이월
            maxExp += 10; //max 경험치 상승

            if (level >= 20)
            {
                level = 20;
            }
        }

        //방향키에 따른 물마법사 x, y좌표
        float moveX = speed * Time.deltaTime * Input.GetAxis("Horizontal");
        float moveY = speed * Time.deltaTime * Input.GetAxis("Vertical");

        //상하이동
        if (Input.GetAxis("Vertical") <= -0.2 && Input.GetAxis("Horizontal") <= -0.1) //아래, 왼쪽
        {
            ani.SetBool("walk", true); //walk 모션 활성
            transform.localScale = new Vector3(-1f, 1f, 1f); //캐릭터 좌우반전

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (dashCoolTimer >= dashCoolTime)
                {
                    if (!isDashClick)
                        isDashClick = true;
                    wSoundManager.instance.surfS();
                    canUseDash = false;
                    dashCoolTimer = 0f;
                    speed += 2f;
                    ani.SetBool("walk", false);
                    ani.SetBool("surf", true);

                    if (!isDashing)
                    {
                        isDashing = true;
                        surfingTime = surfing; // 대쉬 시간 초기화
                    }
                }
                else if (dashCoolTimer < dashCoolTime && !canUseDash)
                    canUseDash = true;
               
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = wspeed;
                wSoundManager.instance.surfEnd();
                ani.SetBool("walk", true);
                ani.SetBool("surf", false);
            }

            if (Input.GetMouseButtonDown(0)) //아래로 이동하면서 마우스 왼쪽 버튼 누를 때
            {
                wSoundManager.instance.pWater();
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

            if (Input.GetMouseButtonDown(1)) //아래로 걸으면서 우클릭
            {
                if (skillCoolTimer >= skillCoolTime)
                {
                    if (!isSkillClick)
                        isSkillClick = true;
                    wSoundManager.instance.pWaterP();
                    canUseSkill = false;
                    skillCoolTimer = 0f;
                    ani.SetBool("walk", false);
                    ani.SetBool("atk2", true);
                    GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
                }
                else if (skillCoolTimer < skillCoolTime && !canUseSkill)
                    canUseSkill = true;
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("atk2", false);
            }
        }
        else if (Input.GetAxis("Vertical") <= -0.2 && Input.GetAxis("Horizontal") >= 0.1) //아래, 오른쪽
        {
            ani.SetBool("walk", true); //walk 모션 활성
            transform.localScale = new Vector3(1f, 1f, 1f); //캐릭터 좌우반전

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (dashCoolTimer >= dashCoolTime)
                {
                    if (!isDashClick)
                        isDashClick = true;
                    wSoundManager.instance.surfS();
                    canUseDash = false;
                    dashCoolTimer = 0f;
                    speed += 2f;
                    ani.SetBool("walk", false);
                    ani.SetBool("surf", true);

                    if (!isDashing)
                    {
                        isDashing = true;
                        surfingTime = surfing; // 대쉬 시간 초기화
                    }

                }
                else if (dashCoolTimer < dashCoolTime && !canUseDash)
                    canUseDash = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = wspeed;
                wSoundManager.instance.surfEnd();
                ani.SetBool("walk", true);
                ani.SetBool("surf", false);
            }

            if (Input.GetMouseButtonDown(0)) //아래로 이동하면서 마우스 왼쪽 버튼 누를 때
            {
                wSoundManager.instance.pWater();
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

            if (Input.GetMouseButtonDown(1)) //아래로 걸으면서 우클릭
            {
                if (skillCoolTimer >= skillCoolTime)
                {
                    if (!isSkillClick)
                        isSkillClick = true;
                    wSoundManager.instance.pWaterP();
                    canUseSkill = false;
                    skillCoolTimer = 0f;
                    ani.SetBool("walk", false);
                    ani.SetBool("atk2", true);
                    GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
                }
                else if (skillCoolTimer < skillCoolTime && !canUseSkill)
                    canUseSkill = true;
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("atk2", false);
            }
        }
        else if (Input.GetAxis("Vertical") >= 0.2 && Input.GetAxis("Horizontal") <= -0.1) //위, 왼
        {
            ani.SetBool("walk", true); //walk 모션 활성
            transform.localScale = new Vector3(-1f, 1f, 1f); //캐릭터 좌우반전

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (dashCoolTimer >= dashCoolTime)
                {
                    if (!isDashClick)
                        isDashClick = true;
                    wSoundManager.instance.surfS();
                    canUseDash = false;
                    dashCoolTimer = 0f;
                    speed += 2f;
                    ani.SetBool("walk", false);
                    ani.SetBool("surf", true);

                    if (!isDashing)
                    {
                        isDashing = true;
                        surfingTime = surfing; // 대쉬 시간 초기화
                    }

                }
                else if (dashCoolTimer < dashCoolTime && !canUseDash)
                    canUseDash = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = wspeed;
                wSoundManager.instance.surfEnd();
                ani.SetBool("walk", true);
                ani.SetBool("surf", false);
            }

            if (Input.GetMouseButtonDown(0)) //올라가면서 마우스 왼쪽 버튼 누를 때
            {
                wSoundManager.instance.pWater();
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

            if (Input.GetMouseButtonDown(1)) //위로 걸으면서 우클릭
            {
                if (skillCoolTimer >= skillCoolTime)
                {
                    if (!isSkillClick)
                        isSkillClick = true;
                    wSoundManager.instance.pWaterP();
                    canUseSkill = false;
                    skillCoolTimer = 0f;
                    ani.SetBool("walk", false);
                    ani.SetBool("atk2", true);
                    GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
                }
                else if (skillCoolTimer < skillCoolTime && !canUseSkill)
                    canUseSkill = true;
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("atk2", false);
            }
        }
        else if (Input.GetAxis("Vertical") >= 0.2 && Input.GetAxis("Horizontal") >= 0.1) //위, 오
        {
            ani.SetBool("walk", true); //walk 모션 활성
            transform.localScale = new Vector3(1f, 1f, 1f); //캐릭터 좌우반전
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (dashCoolTimer >= dashCoolTime)
                {
                    if (!isDashClick)
                        isDashClick = true;
                    wSoundManager.instance.surfS();
                    canUseDash = false;
                    dashCoolTimer = 0f;
                    speed += 2f;
                    ani.SetBool("walk", false);
                    ani.SetBool("surf", true);

                    if (!isDashing)
                    {
                        isDashing = true;
                        surfingTime = surfing; // 대쉬 시간 초기화
                    }

                }
                else if (dashCoolTimer < dashCoolTime && !canUseDash)
                    canUseDash = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = wspeed;
                wSoundManager.instance.surfEnd();
                ani.SetBool("walk", true);
                ani.SetBool("surf", false);
            }
            if (Input.GetMouseButtonDown(0)) //올라가면서 마우스 왼쪽 버튼 누를 때
            {
                wSoundManager.instance.pWater();
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

            if (Input.GetMouseButtonDown(1)) //위로 걸으면서 우클릭
            {
                if (skillCoolTimer >= skillCoolTime)
                {
                    if (!isSkillClick)
                        isSkillClick = true;
                    wSoundManager.instance.pWaterP();
                    canUseSkill = false;
                    skillCoolTimer = 0f;
                    ani.SetBool("walk", false);
                    ani.SetBool("atk2", true);
                    GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
                }
                else if (skillCoolTimer < skillCoolTime && !canUseSkill)
                    canUseSkill = true;
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("atk2", false);
            }
        }

        //좌우 이동
        if (Input.GetAxis("Horizontal") <= -0.2 && Input.GetAxis("Vertical") == 0) //왼쪽으로 이동할 때
        {
            ani.SetBool("walk", true); //walk 모션 활성
            transform.localScale = new Vector3(-1f, 1f, 1f); //캐릭터 좌우반전

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (dashCoolTimer >= dashCoolTime)
                {
                    if (!isDashClick)
                        isDashClick = true;
                    wSoundManager.instance.surfS();
                    canUseDash = false;
                    dashCoolTimer = 0f;
                    speed += 2f;
                    ani.SetBool("walk", false);
                    ani.SetBool("surf", true);


                    if (!isDashing)
                    {
                        isDashing = true;
                        surfingTime = surfing; // 대쉬 시간 초기화
                    }

                }
                else if (dashCoolTimer < dashCoolTime && !canUseDash)
                    canUseDash = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = wspeed;
                wSoundManager.instance.surfEnd();
                ani.SetBool("walk", true);
                ani.SetBool("surf", false);
            }


            if (Input.GetMouseButtonDown(0)) //왼쪽으로 이동하면서 마우스 왼쪽 버튼 누를 때
            {
                wSoundManager.instance.pWater();
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

            if (Input.GetMouseButtonDown(1)) //왼쪽으로 걸으면서 우클릭
            {
                if (skillCoolTimer >= skillCoolTime)
                {
                    if (!isSkillClick)
                        isSkillClick = true;
                    wSoundManager.instance.pWaterP();
                    canUseSkill = false;
                    skillCoolTimer = 0f;
                    ani.SetBool("walk", false);
                    ani.SetBool("atk2", true);
                    GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
                }
                else if (skillCoolTimer < skillCoolTime && !canUseSkill)
                    canUseSkill = true;
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("atk2", false);
            }
        }
        else if (Input.GetAxis("Horizontal") >= 0.2 && Input.GetAxis("Vertical") == 0) //오른쪽으로 이동할 때
        {
            ani.SetBool("walk", true); //걷는 모션 활성화
            transform.localScale = new Vector3(1f, 1f, 1f); //캐릭터 오른쪽 모습

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (dashCoolTimer >= dashCoolTime)
                {
                    if (!isDashClick)
                        isDashClick = true;
                    wSoundManager.instance.surfS();
                    canUseDash = false;
                    dashCoolTimer = 0f;
                    speed += 2f;

                    ani.SetBool("walk", false);
                    ani.SetBool("surf", true);

                    if (!isDashing)
                    {
                        isDashing = true;
                        surfingTime = surfing; // 대쉬 시간 초기화
                    }

                }
                else if (dashCoolTimer < dashCoolTime && !canUseDash)
                    canUseDash = true;

            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = wspeed;
                wSoundManager.instance.surfEnd();
                ani.SetBool("walk", true);
                ani.SetBool("surf", false);
            }

            if (Input.GetMouseButtonDown(0)) //오른쪽으로 이동하면서 마우스 좌클릭
            {
                wSoundManager.instance.pWater();
                ani.SetBool("walk", false);
                ani.SetBool("sp_atk", true); //공격 모션 활성화
                GameObject go = Instantiate(waterbullet, pos1.position, Quaternion.identity); //pos1에서 미사일 발사
                Destroy(go, 5); //5초 뒤 삭제
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("sp_atk", false); //공격모션 비활성화
            }

            if (Input.GetMouseButtonDown(1)) //오른쪽으로 걸으면서 우클릭
            {
                if (skillCoolTimer >= skillCoolTime)
                {
                    if (!isSkillClick)
                        isSkillClick = true;
                    wSoundManager.instance.pWaterP();
                    canUseSkill = false;
                    skillCoolTimer = 0f;
                    ani.SetBool("walk", false);
                    ani.SetBool("atk2", true);
                    GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
                }
                else if (skillCoolTimer < skillCoolTime && !canUseSkill)
                    canUseSkill = true;
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("atk2", false);
            }


        }
        else if (Input.GetAxis("Horizontal") == 0.0f && Input.GetAxis("Vertical") == 0.0f) //멈춰있을 때
        {
            ani.SetBool("walk", false); //걷는 모션 비활성화

            //멈춰서 마우스 좌클릭 누를 때
            if (Input.GetMouseButtonDown(0))
            {
                wSoundManager.instance.pWater();
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

            //멈춰서 우클릭
            if (Input.GetMouseButtonDown(1))
            {
                if (skillCoolTimer >= skillCoolTime)
                {
                    wSoundManager.instance.pWaterP();
                    if (!isSkillClick)
                        isSkillClick = true;
                    if (transform.localScale.x == 1f)
                    {
                        canUseSkill = false;
                        skillCoolTimer = 0f;
                        transform.localScale = new Vector3(1f, 1f, 1f);
                        ani.SetBool("atk2", true);
                        GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
                    }
                    else if (transform.localScale.x == -1f)
                    {
                        canUseSkill = false;
                        skillCoolTimer = 0f;
                        transform.localScale = new Vector3(-1f, 1f, 1f);
                        ani.SetBool("atk2", true);
                        GameObject go1 = Instantiate(waterPbullet, pos2.position, Quaternion.identity);
                    }
                }
                else if (skillCoolTimer < skillCoolTime && !canUseSkill)
                    canUseSkill = true;

            }
            else
            {
                ani.SetBool("atk2", false);
            }
        }

        if (Input.GetAxis("Vertical") <= -0.1f && Input.GetAxis("Horizontal") == 0.0f) //아래로만
        {
            ani.SetBool("walk", true); //walk 모션 활성

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (dashCoolTimer >= dashCoolTime)
                {
                    if (!isDashClick)
                        isDashClick = true;
                    wSoundManager.instance.surfS();
                    canUseDash = false;
                    dashCoolTimer = 0f;
                    speed += 2f;
                    ani.SetBool("walk", false);
                    ani.SetBool("surf", true);

                    if (!isDashing)
                    {
                        isDashing = true;
                        surfingTime = surfing; // 대쉬 시간 초기화
                    }


                }
                else if (dashCoolTimer < dashCoolTime && !canUseDash)
                    canUseDash = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = wspeed;
                wSoundManager.instance.surfEnd();
                ani.SetBool("walk", true);
                ani.SetBool("surf", false);
            }
            if (Input.GetMouseButtonDown(0)) //아래로 이동하면서 마우스 왼쪽 버튼 누를 때
            {
                wSoundManager.instance.pWater();
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

            if (Input.GetMouseButtonDown(1)) //아래로 걸으면서 우클릭
            {
                if (skillCoolTimer >= skillCoolTime)
                {
                    if (!isSkillClick)
                        isSkillClick = true;
                    wSoundManager.instance.pWaterP();
                    canUseSkill = false;
                    skillCoolTimer = 0f;
                    ani.SetBool("walk", false);
                    ani.SetBool("atk2", true);
                    GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
                }
                else if (skillCoolTimer < skillCoolTime && !canUseSkill)
                    canUseSkill = true;
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("atk2", false);
            }
        }
        if (Input.GetAxis("Vertical") >= 0.1f && Input.GetAxis("Horizontal") == 0.0f) //위로만
        {
            ani.SetBool("walk", true); //walk 모션 활성

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (dashCoolTimer >= dashCoolTime)
                {
                    if (!isDashClick)
                        isDashClick = true;
                    wSoundManager.instance.surfS();
                    canUseDash = false;
                    dashCoolTimer = 0f;
                    speed += 2f;
                    ani.SetBool("walk", false);
                    ani.SetBool("surf", true);

                    if (!isDashing)
                    {
                        isDashing = true;
                        surfingTime = surfing; // 대쉬 시간 초기화
                    }


                }
                else if (dashCoolTimer < dashCoolTime && !canUseDash)
                    canUseDash = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                speed = wspeed;
                wSoundManager.instance.surfEnd();
                ani.SetBool("walk", true);
                ani.SetBool("surf", false);
            }
            if (Input.GetMouseButtonDown(0)) //아래로 이동하면서 마우스 왼쪽 버튼 누를 때
            {
                wSoundManager.instance.pWater();
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

            if (Input.GetMouseButtonDown(1)) //아래로 걸으면서 우클릭
            {
                if (skillCoolTimer >= skillCoolTime)
                {
                    if (!isSkillClick)
                        isSkillClick = true;
                    wSoundManager.instance.pWaterP();
                    canUseSkill = false;
                    skillCoolTimer = 0f;
                    ani.SetBool("walk", false);
                    ani.SetBool("atk2", true);
                    GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
                }
                else if (skillCoolTimer < skillCoolTime && !canUseSkill)
                    canUseSkill = true;
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("atk2", false);
            }
        }


        //캐릭터 좌표
        transform.Translate(moveX, moveY, 0);

        if (hp <= 0)
        {
            hp = 0;
            ani.SetBool("death", true);
        }

    }

    IEnumerator skill1() //자동 스킬 발동
    {
        while (true)
        {
            Instantiate(waterbulletU, transform.position, Quaternion.identity);
            Instantiate(waterbulletR, transform.position, Quaternion.identity);
            Instantiate(waterbulletL, transform.position, Quaternion.identity);
            Instantiate(waterbulletD, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(1.5f);
        }

    }

    public override void TakeDamage(int dmg) //데미지 입는 메서드 오버라이딩
    {
        base.TakeDamage(dmg);

        if (!GodMode)
            hp -= dmg;
        Instantiate(wExposion, transform.position, Quaternion.identity);
    }

    public override void GetExperience(int ex) //경험치 올리는 메서드 오버라이딩
    {
        exp += ex; //경험치 상승
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster")||collision.gameObject.CompareTag("Ground"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Monster") || collision.gameObject.CompareTag("Ground"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
}

