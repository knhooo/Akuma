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




    public GameObject waterbullet; //���̻��� ��ü ����
    public GameObject waterbulletU; //���̻��� ��ü ����
    public GameObject waterbulletR; //���̻��� ��ü ����
    public GameObject waterbulletL; //���̻��� ��ü ����
    public GameObject waterbulletD; //���̻��� ��ü ����

    public Transform pos1 = null;
    public Transform pos2 = null;



    void Start()
    {
        ani = GetComponent<Animator>(); //�ִϸ��̼� ��������
        StartCoroutine("skill1");
    }


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

    IEnumerator skill1()
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

}
