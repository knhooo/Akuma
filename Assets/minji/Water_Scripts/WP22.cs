using UnityEngine;
using System.Collections;

public class WP22 : Player //�������� ��ũ��Ʈ
{
    public float wspeedUp = 5f; //������ �ӵ�
    public float wkeepSpeed = 3f; //�ӵ� ����

    Animator ani; //�ִϸ��̼� ��ü ����

    public GameObject wExposion; //�� ����Ʈ


    public GameObject waterbullet; //���̻��� ��ü ����
    public GameObject waterPbullet; //���̻��� (�����) ��ü ����
    public GameObject waterbulletU; //���̻��� ��ü ����
    public GameObject waterbulletR; //���̻��� ��ü ����
    public GameObject waterbulletL; //���̻��� ��ü ����
    public GameObject waterbulletD; //���̻��� ��ü ����

    public Transform pos1 = null;
    public Transform pos2 = null;



    void Start()
    {
        ani = GetComponent<Animator>(); //�ִϸ��̼� ��������
        StartCoroutine("skill1"); //�ڵ����� Ȱ��ȭ
    }


    void Update()
    {
        //������
        if (exp >= maxExp)
        {
            level += 1;
            Debug.Log($"������! ���� ���� : {level}"); //Ȯ�ο�
            exp -= maxExp; //���� ����ġ �̿�
            attack += 10; //�ɷ�ġ ����� �ӽ÷� ���ص�.
            maxHp += 20;
            hp += 20;
            speed += 0.1f;
            wspeedUp += 0.1f;
            wkeepSpeed += 0.1f;

            if (level >= 20)
            {
                level = 20;
            }
        }

        //����Ű�� ���� �������� x, y��ǥ
        float moveX = speed * Time.deltaTime * Input.GetAxis("Horizontal");
        float moveY = speed * Time.deltaTime * Input.GetAxis("Vertical");


        //�¿� �̵�
        if (Input.GetAxis("Horizontal") <= -0.2) //�������� �̵��� ��
        {
            ani.SetBool("walk", true); //walk ��� Ȱ��
            transform.localScale = new Vector3(-1f, 1f, 1f); //ĳ���� �¿����

            if (Input.GetKey(KeyCode.LeftShift)) //�������� �̵��ϸ鼭 ����Ʈ
            {
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                ani.SetBool("surf", true); //�뽬 ��� Ȱ��ȭ

                speed = wspeedUp; //���ǵ� ����
            }
            else //����Ʈ �� ��
            {
                ani.SetBool("walk", true); //�ȴ� ��� �ٽ� Ȱ��ȭ
                ani.SetBool("surf", false);
                speed = wkeepSpeed; //����� �ӵ� �����ӵ��� ����ֱ�
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
                wSoundManager.instance.pWaterP();
                ani.SetBool("walk", false);
                ani.SetBool("atk2", true);
                GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("atk2", false);
            }
        }
        else if (Input.GetAxis("Horizontal") >= 0.2) //���������� �̵��� ��
        {
            ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
            transform.localScale = new Vector3(1f, 1f, 1f); //ĳ���� ������ ���

            if (Input.GetKey(KeyCode.LeftShift)) //���������� �̵��ϸ鼭 ����Ʈ
            {
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                ani.SetBool("surf", true); //�뽬 ��� Ȱ��ȭ

                speed = wspeedUp; //���ǵ� ����
            }
            else
            {
                ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
                ani.SetBool("surf", false); //�뽬 ��� ��Ȱ��ȭ
                speed = wkeepSpeed; //����� �ӵ� �����ӵ��� ����ֱ�
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
                wSoundManager.instance.pWaterP();
                ani.SetBool("walk", false);
                ani.SetBool("atk2", true);
                GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("atk2", false);
            }


        }
        else if (Input.GetAxis("Horizontal") == 0.0f) //�������� ��
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
                wSoundManager.instance.pWaterP();
                if (transform.localScale.x == 1f)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    ani.SetBool("atk2", true);
                    GameObject go1 = Instantiate(waterPbullet, pos1.position, Quaternion.identity);
                }
                else if (transform.localScale.x == -1f)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    ani.SetBool("atk2", true);
                    GameObject go1 = Instantiate(waterPbullet, pos2.position, Quaternion.identity);
                }

            }
            else
            {
                ani.SetBool("atk2", false);
            }
        }


        //ĳ���� ��ǥ
        transform.Translate(moveX, moveY, 0);

        if (hp <= 0)
        {
            ani.SetBool("death", true);
        }

    }

    IEnumerator skill1() //10�� �� <�ӽ÷� ���ص�. ù��° �ڵ� ��ų �ߵ�
    {
        yield return new WaitForSeconds(10);
        while (true)
        {
            wSoundManager.instance.pWater();
            Instantiate(waterbulletU, transform.position, Quaternion.identity);
            Instantiate(waterbulletR, transform.position, Quaternion.identity);
            Instantiate(waterbulletL, transform.position, Quaternion.identity);
            Instantiate(waterbulletD, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(2);
        }

    }

    public override void TakeDamage(int dmg) //������ �Դ� �޼��� �������̵�
    {
        hp -= dmg;
        Instantiate(wExposion, transform.position, Quaternion.identity);
    }

    public override void GetExperience(int ex) //����ġ �ø��� �޼��� �������̵�
    {
        exp += ex; //����ġ ���
    }

}

