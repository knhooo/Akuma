using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 3f; //�����̴� �ӵ�
    public float speedUp = 5f; //������ �ӵ�
    public float keepSpeed = 3f; //�ӵ� ����

    Animator ani; //�ִϸ��̼� ��ü ����
    public int power = 0; //�ֹ���
    public int HP = 100; //ü��
    public GameObject waterbullet; //���̻��� ��ü ����
    public Transform pos1 = null;
    public Transform pos2 = null;


    void Start()
    {
        ani = GetComponent<Animator>(); //�ִϸ��̼� ��������
    }

    void Update()
    {
        //����Ű�� ���� �������� x, y��ǥ
        float moveX = moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float moveY = moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");


        //�¿� �̵�
        if(Input.GetAxis("Horizontal")<=-0.2) //�������� �̵��� ��
        {
            ani.SetBool("walk", true); //walk ��� Ȱ��
            transform.localScale = new Vector3(-1f, 1f, 1f); //ĳ���� �¿����

            if(Input.GetKey(KeyCode.LeftShift)) //�������� �̵��ϸ鼭 Shift���� ��
            {
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                ani.SetBool("surf", true); //�뽬 ��� Ȱ��ȭ

                moveSpeed = speedUp; //���ǵ� ����
            }
            else //Shift�� �� ��
            {
                ani.SetBool("walk", true); //�ȴ� ��� �ٽ� Ȱ��ȭ
                ani.SetBool("surf", false);
                moveSpeed = keepSpeed; //����� �ӵ� �����ӵ��� ����ֱ�
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
        else if (Input.GetAxis("Horizontal")>=0.2) //���������� �̵��� ��
        {
            ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
            transform.localScale = new Vector3(1f, 1f, 1f); //ĳ���� ������ ���

            if (Input.GetKey(KeyCode.LeftShift)) //���������� �̵��ϸ鼭 Shift
            {
                ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
                ani.SetBool("surf", true); //�뽬 ��� Ȱ��ȭ

                moveSpeed = speedUp; //���ǵ� ����
            }
            else
            {
                ani.SetBool("walk", true); //�ȴ� ��� Ȱ��ȭ
                ani.SetBool("surf", false); //�뽬 ��� ��Ȱ��ȭ
                moveSpeed = keepSpeed; //����� �ӵ� �����ӵ��� ����ֱ�
            }

            if (Input.GetMouseButtonDown(0)) //���������� �̵��ϸ鼭 ���콺 ��Ŭ��
            {
                ani.SetBool("sp_atk", true); //���� ��� Ȱ��ȭ
                GameObject go=Instantiate(waterbullet, pos1.position, Quaternion.identity); //pos1���� �̻��� �߻�
                Destroy(go, 5); //5�� �� ����
            }
            else
            {
                ani.SetBool("sp_atk", false); //���ݸ�� ��Ȱ��ȭ
            }


        }
        else if(Input.GetAxis("Horizontal") == 0.0f) //�������� ��
        {
            ani.SetBool("walk", false); //�ȴ� ��� ��Ȱ��ȭ
        }

        //Shift���� ��
        if(Input.GetKey(KeyCode.LeftShift))
        {
            ani.SetBool("surf", true);
            moveSpeed = speedUp; //���ǵ� ����
        }
        else
        {
            ani.SetBool("surf", false);
            moveSpeed = keepSpeed; //����� �ӵ� �����ӵ��� ����ֱ�
        }

        //���콺 ��Ŭ��
        if(Input.GetMouseButtonDown(0))
        {
            if(transform.localScale.x==1f)
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

        if(HP==0)
        {
            ani.SetBool("death", true);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            ani.SetBool("takehit", true);
            HP -= 10;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            ani.SetBool("takehit", false);
        }
    }
}
