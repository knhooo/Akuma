using UnityEngine;
using System.Collections;
using System;

public class WP : Player //�������� ��ũ��Ʈ
{

    Animator ani; //�ִϸ��̼� ��ü ����
    private Rigidbody2D rb;

    public GameObject wExposion; //�� ����Ʈ

    public GameObject waterbullet; //���̻��� ��ü ����
    public GameObject waterPbullet; //���̻��� (�����) ��ü ����
    public GameObject waterbulletU; //���̻��� ��ü ����
    public GameObject waterbulletR; //���̻��� ��ü ����
    public GameObject waterbulletL; //���̻��� ��ü ����
    public GameObject waterbulletD; //���̻��� ��ü ����

    public float wspeed;
    public float surfing=1f;
    public float surfingTime = 0f;
    private bool isDashing=false;
    private bool speedzero = false;

    public Transform pos1 = null;
    public Transform pos2 = null;

    void Start()
    {
        ani = GetComponent<Animator>(); //�ִϸ��̼� ��������
        StartCoroutine("skill1"); //�ڵ����� Ȱ��ȭ
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

        //�뽬���϶�
        if (isDashing)
        {
            surfing -= Time.deltaTime;

            // �뽬 ���� ����
            if (surfing <= 0)
            {
                isDashing = false;
            }
        }

        //�뽬�� ������
        if (!isDashing)
        {
            canUseDash = true;
            speed = wspeed;
            wSoundManager.instance.surfEnd();
            ani.SetBool("surf", false);
        }


        //������
        if (exp >= maxExp)
        {
            level += 1;
            exp -= maxExp; //���� ����ġ �̿�
            maxExp += 10; //max ����ġ ���

            if (level >= 20)
            {
                level = 20;
            }
        }

        //����Ű�� ���� �������� x, y��ǥ
        float moveX = speed * Time.deltaTime * Input.GetAxis("Horizontal");
        float moveY = speed * Time.deltaTime * Input.GetAxis("Vertical");

        //�����̵�
        if (Input.GetAxis("Vertical") <= -0.2 && Input.GetAxis("Horizontal") <= -0.1) //�Ʒ�, ����
        {
            ani.SetBool("walk", true); //walk ��� Ȱ��
            transform.localScale = new Vector3(-1f, 1f, 1f); //ĳ���� �¿����

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
                        surfingTime = surfing; // �뽬 �ð� �ʱ�ȭ
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

            if (Input.GetMouseButtonDown(0)) //�Ʒ��� �̵��ϸ鼭 ���콺 ���� ��ư ���� ��
            {
                wSoundManager.instance.pWater();
                ani.SetBool("sp_atk", true); //���� ��� Ȱ��ȭ
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                GameObject go = Instantiate(waterbullet, pos2.position, Quaternion.identity); //pos2���� �̻��� �߻�
                Destroy(go, 5); //5�� �� ����

            }
            else //���콺 ���ʹ�ư�� �ȴ����� ���� ��
            {
                ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
                ani.SetBool("sp_atk", false);
            }

            if (Input.GetMouseButtonDown(1)) //�Ʒ��� �����鼭 ��Ŭ��
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
        else if (Input.GetAxis("Vertical") <= -0.2 && Input.GetAxis("Horizontal") >= 0.1) //�Ʒ�, ������
        {
            ani.SetBool("walk", true); //walk ��� Ȱ��
            transform.localScale = new Vector3(1f, 1f, 1f); //ĳ���� �¿����

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
                        surfingTime = surfing; // �뽬 �ð� �ʱ�ȭ
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

            if (Input.GetMouseButtonDown(0)) //�Ʒ��� �̵��ϸ鼭 ���콺 ���� ��ư ���� ��
            {
                wSoundManager.instance.pWater();
                ani.SetBool("sp_atk", true); //���� ��� Ȱ��ȭ
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                GameObject go = Instantiate(waterbullet, pos2.position, Quaternion.identity); //pos2���� �̻��� �߻�
                Destroy(go, 5); //5�� �� ����

            }
            else //���콺 ���ʹ�ư�� �ȴ����� ���� ��
            {
                ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
                ani.SetBool("sp_atk", false);
            }

            if (Input.GetMouseButtonDown(1)) //�Ʒ��� �����鼭 ��Ŭ��
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
        else if (Input.GetAxis("Vertical") >= 0.2 && Input.GetAxis("Horizontal") <= -0.1) //��, ��
        {
            ani.SetBool("walk", true); //walk ��� Ȱ��
            transform.localScale = new Vector3(-1f, 1f, 1f); //ĳ���� �¿����

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
                        surfingTime = surfing; // �뽬 �ð� �ʱ�ȭ
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

            if (Input.GetMouseButtonDown(0)) //�ö󰡸鼭 ���콺 ���� ��ư ���� ��
            {
                wSoundManager.instance.pWater();
                ani.SetBool("sp_atk", true); //���� ��� Ȱ��ȭ
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                GameObject go = Instantiate(waterbullet, pos2.position, Quaternion.identity); //pos2���� �̻��� �߻�
                Destroy(go, 5); //5�� �� ����

            }
            else //���콺 ���ʹ�ư�� �ȴ����� ���� ��
            {
                ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
                ani.SetBool("sp_atk", false);
            }

            if (Input.GetMouseButtonDown(1)) //���� �����鼭 ��Ŭ��
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
        else if (Input.GetAxis("Vertical") >= 0.2 && Input.GetAxis("Horizontal") >= 0.1) //��, ��
        {
            ani.SetBool("walk", true); //walk ��� Ȱ��
            transform.localScale = new Vector3(1f, 1f, 1f); //ĳ���� �¿����
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
                        surfingTime = surfing; // �뽬 �ð� �ʱ�ȭ
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
            if (Input.GetMouseButtonDown(0)) //�ö󰡸鼭 ���콺 ���� ��ư ���� ��
            {
                wSoundManager.instance.pWater();
                ani.SetBool("sp_atk", true); //���� ��� Ȱ��ȭ
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                GameObject go = Instantiate(waterbullet, pos2.position, Quaternion.identity); //pos2���� �̻��� �߻�
                Destroy(go, 5); //5�� �� ����

            }
            else //���콺 ���ʹ�ư�� �ȴ����� ���� ��
            {
                ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
                ani.SetBool("sp_atk", false);
            }

            if (Input.GetMouseButtonDown(1)) //���� �����鼭 ��Ŭ��
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

        //�¿� �̵�
        if (Input.GetAxis("Horizontal") <= -0.2 && Input.GetAxis("Vertical") == 0) //�������� �̵��� ��
        {
            ani.SetBool("walk", true); //walk ��� Ȱ��
            transform.localScale = new Vector3(-1f, 1f, 1f); //ĳ���� �¿����

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
                        surfingTime = surfing; // �뽬 �ð� �ʱ�ȭ
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


            if (Input.GetMouseButtonDown(0)) //�������� �̵��ϸ鼭 ���콺 ���� ��ư ���� ��
            {
                wSoundManager.instance.pWater();
                ani.SetBool("sp_atk", true); //���� ��� Ȱ��ȭ
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                GameObject go = Instantiate(waterbullet, pos2.position, Quaternion.identity); //pos2���� �̻��� �߻�
                Destroy(go, 5); //5�� �� ����

            }
            else //���콺 ���ʹ�ư�� �ȴ����� ���� ��
            {
                ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
                ani.SetBool("sp_atk", false);
            }

            if (Input.GetMouseButtonDown(1)) //�������� �����鼭 ��Ŭ��
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
        else if (Input.GetAxis("Horizontal") >= 0.2 && Input.GetAxis("Vertical") == 0) //���������� �̵��� ��
        {
            ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
            transform.localScale = new Vector3(1f, 1f, 1f); //ĳ���� ������ ���

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
                        surfingTime = surfing; // �뽬 �ð� �ʱ�ȭ
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

            if (Input.GetMouseButtonDown(0)) //���������� �̵��ϸ鼭 ���콺 ��Ŭ��
            {
                wSoundManager.instance.pWater();
                ani.SetBool("walk", false);
                ani.SetBool("sp_atk", true); //���� ��� Ȱ��ȭ
                GameObject go = Instantiate(waterbullet, pos1.position, Quaternion.identity); //pos1���� �̻��� �߻�
                Destroy(go, 5); //5�� �� ����
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("sp_atk", false); //���ݸ�� ��Ȱ��ȭ
            }

            if (Input.GetMouseButtonDown(1)) //���������� �����鼭 ��Ŭ��
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
        else if (Input.GetAxis("Horizontal") == 0.0f && Input.GetAxis("Vertical") == 0.0f) //�������� ��
        {
            ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ

            //���缭 ���콺 ��Ŭ�� ���� ��
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

            //���缭 ��Ŭ��
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

        if (Input.GetAxis("Vertical") <= -0.1f && Input.GetAxis("Horizontal") == 0.0f) //�Ʒ��θ�
        {
            ani.SetBool("walk", true); //walk ��� Ȱ��

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
                        surfingTime = surfing; // �뽬 �ð� �ʱ�ȭ
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
            if (Input.GetMouseButtonDown(0)) //�Ʒ��� �̵��ϸ鼭 ���콺 ���� ��ư ���� ��
            {
                wSoundManager.instance.pWater();
                ani.SetBool("sp_atk", true); //���� ��� Ȱ��ȭ
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                GameObject go = Instantiate(waterbullet, pos2.position, Quaternion.identity); //pos2���� �̻��� �߻�
                Destroy(go, 5); //5�� �� ����

            }
            else //���콺 ���ʹ�ư�� �ȴ����� ���� ��
            {
                ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
                ani.SetBool("sp_atk", false);
            }

            if (Input.GetMouseButtonDown(1)) //�Ʒ��� �����鼭 ��Ŭ��
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
        if (Input.GetAxis("Vertical") >= 0.1f && Input.GetAxis("Horizontal") == 0.0f) //���θ�
        {
            ani.SetBool("walk", true); //walk ��� Ȱ��

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
                        surfingTime = surfing; // �뽬 �ð� �ʱ�ȭ
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
            if (Input.GetMouseButtonDown(0)) //�Ʒ��� �̵��ϸ鼭 ���콺 ���� ��ư ���� ��
            {
                wSoundManager.instance.pWater();
                ani.SetBool("sp_atk", true); //���� ��� Ȱ��ȭ
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                GameObject go = Instantiate(waterbullet, pos2.position, Quaternion.identity); //pos2���� �̻��� �߻�
                Destroy(go, 5); //5�� �� ����

            }
            else //���콺 ���ʹ�ư�� �ȴ����� ���� ��
            {
                ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
                ani.SetBool("sp_atk", false);
            }

            if (Input.GetMouseButtonDown(1)) //�Ʒ��� �����鼭 ��Ŭ��
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


        //ĳ���� ��ǥ
        transform.Translate(moveX, moveY, 0);

        if (hp <= 0)
        {
            hp = 0;
            ani.SetBool("death", true);
        }

    }

    IEnumerator skill1() //�ڵ� ��ų �ߵ�
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

    public override void TakeDamage(int dmg) //������ �Դ� �޼��� �������̵�
    {
        base.TakeDamage(dmg);

        if (!GodMode)
            hp -= dmg;
        Instantiate(wExposion, transform.position, Quaternion.identity);
    }

    public override void GetExperience(int ex) //����ġ �ø��� �޼��� �������̵�
    {
        exp += ex; //����ġ ���
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

