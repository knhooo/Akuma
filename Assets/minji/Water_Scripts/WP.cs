using UnityEngine;
using System.Collections;

public class WP : MonoBehaviour
{
    public float wmoveSpeed = 3f; //�����̴� �ӵ�
    public float wspeedUp = 5f; //������ �ӵ�
    public float wkeepSpeed = 3f; //�ӵ� ����

    Animator ani; //�ִϸ��̼� ��ü ����
    public int wPower = 10; //�ֹ��� 
    public int wHP = 100; //ü�� (20�� ����)
    public int wLevel = 1; //����
    public int wEx = 0; //����ġ




    public GameObject waterbullet; //���̻��� ��ü ����
    public GameObject waterskill1; //���̻��� ��ü ����

    public Transform pos1 = null;
    public Transform pos2 = null;


    void Start()
    {
        ani = GetComponent<Animator>(); //�ִϸ��̼� ��������
        //StartCoroutine(CheckConditionCoroutine());
    }

    //IEnumerator CheckConditionCoroutine()
    //{
    //    while (true) // ��� �ݺ�
    //    {
    //        if (wLevel >= 5)
    //        {
    //            StartCoroutine(skill1());
    //            yield break;
    //        }
    //        yield return new WaitForSeconds(1f); // 1�ʸ��� �˻�
    //    }
    //}

    void Update()
    {
        //����Ű�� ���� �������� x, y��ǥ
        float moveX = wmoveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float moveY = wmoveSpeed * Time.deltaTime * Input.GetAxis("Vertical");


        //�¿� �̵�
        if (Input.GetAxis("Horizontal") <= -0.2) //�������� �̵��� ��
        {
            ani.SetBool("walk", true); //walk ��� Ȱ��
            transform.localScale = new Vector3(-1f, 1f, 1f); //ĳ���� �¿����

            if (Input.GetKey(KeyCode.LeftShift)) //�������� �̵��ϸ鼭 Shift���� ��
            {
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                ani.SetBool("surf", true); //�뽬 ��� Ȱ��ȭ

                wmoveSpeed = wspeedUp; //���ǵ� ����
            }
            else //Shift�� �� ��
            {
                ani.SetBool("walk", true); //�ȴ� ��� �ٽ� Ȱ��ȭ
                ani.SetBool("surf", false);
                wmoveSpeed = wkeepSpeed; //����� �ӵ� �����ӵ��� ����ֱ�
            }

            if (Input.GetMouseButtonDown(0)) //�������� �̵��ϸ鼭 ���콺 ���� ��ư ���� ��
            {
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
        }
        else if (Input.GetAxis("Horizontal") >= 0.2) //���������� �̵��� ��
        {
            ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
            transform.localScale = new Vector3(1f, 1f, 1f); //ĳ���� ������ ���

            if (Input.GetKey(KeyCode.LeftShift)) //���������� �̵��ϸ鼭 Shift
            {
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                ani.SetBool("surf", true); //�뽬 ��� Ȱ��ȭ

                wmoveSpeed = wspeedUp; //���ǵ� ����
            }
            else
            {
                ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
                ani.SetBool("surf", false); //�뽬 ��� ��Ȱ��ȭ
                wmoveSpeed = wkeepSpeed; //����� �ӵ� �����ӵ��� ����ֱ�
            }

            if (Input.GetMouseButtonDown(0)) //���������� �̵��ϸ鼭 ���콺 ��Ŭ��
            {
                ani.SetBool("sp_atk", true); //���� ��� Ȱ��ȭ
                GameObject go = Instantiate(waterbullet, pos1.position, Quaternion.identity); //pos1���� �̻��� �߻�
                Destroy(go, 5); //5�� �� ����
            }
            else
            {
                ani.SetBool("sp_atk", false); //���ݸ�� ��Ȱ��ȭ
            }


        }
        else if (Input.GetAxis("Horizontal") == 0.0f) //�������� ��
        {
            ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
        }

        //Shift���� ��
        if (Input.GetKey(KeyCode.LeftShift))
        {
            ani.SetBool("surf", true);
            wmoveSpeed = wspeedUp; //���ǵ� ����
        }
        else
        {
            ani.SetBool("surf", false);
            wmoveSpeed = wkeepSpeed; //����� �ӵ� �����ӵ��� ����ֱ�
        }

        //���콺 ��Ŭ��
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

        //ĳ���� ��ǥ
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
    //    //�����ֱ�
    //    float attackRate = 3;
    //    //�߻�ü ��������
    //    int count = 5;
    //    //�߻�ü ������ ����
    //    float intervalAngle = 360 / count;
    //    //���ߵǴ� ����(�׻� ���� ��ġ�� �߻����� �ʵ��� ����
    //    float weightAngle = 0f;
    //    while (true)
    //    {
    //        for (int i = 0; i < count; ++i)
    //        {
    //            //�߻�ü ����
    //            GameObject clone = Instantiate(waterskill1, transform.position, Quaternion.identity);

    //            //�߻�ü �̵� ����(����)
    //            float angle = weightAngle + intervalAngle * i;
    //            //�߻�ü �̵� ����(����)
    //            //Cos(����)���� ������ ���� ǥ���� ���� pi/180�� ����
    //            float x = Mathf.Cos(angle * Mathf.Deg2Rad);
    //            //sin(����)���� ������ ���� ǥ���� ���� pi/180�� ����
    //            float y = Mathf.Sin(angle * Mathf.Deg2Rad);

    //            //�߻�ü �̵� ���� ����
    //            clone.GetComponent<Water>().Move(new Vector2(x, y));
    //        }
    //        //�߻�ü�� �����Ǵ� ���� ���� ������ ���Ѻ���
    //        weightAngle += 1;
    //        //3�ʸ��� �̻��� �߻�
    //        yield return new WaitForSeconds(attackRate);
    //    }

    //}
}
