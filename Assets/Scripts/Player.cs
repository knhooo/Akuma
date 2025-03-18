using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    Animator ani;
    public int power = 0;
    public int HP = 100;

    
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = moveSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float moveY = moveSpeed * Time.deltaTime * Input.GetAxis("Vertical");

        if(Input.GetAxis("Horizontal")<=-0.2)
        {
            ani.SetBool("walk", true);
            transform.localScale = new Vector3(-1f, 1f, 1f);

            if(Input.GetKey(KeyCode.LeftShift))
            {
                ani.SetBool("walk", false);
                ani.SetBool("surf", true);
                moveSpeed = 10f;
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("surf", false);
                moveSpeed = 5f;
            }

            if (Input.GetMouseButton(0))
            {
                if (transform.localScale.x == 1f)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    ani.SetBool("sp_atk", true);
                    ani.SetBool("walk", false);
                }
                else if (transform.localScale.x == -1f)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    ani.SetBool("sp_atk", true);
                    ani.SetBool("walk", false);
                }

            }
            else
            {
                ani.SetBool("sp_atk", false);
                ani.SetBool("walk", true);
            }
        }
        else if (Input.GetAxis("Horizontal")>=0.2)
        {
            ani.SetBool("walk", true);
            transform.localScale = new Vector3(1f, 1f, 1f);

            if (Input.GetKey(KeyCode.LeftShift))
            {
                ani.SetBool("walk", false);
                ani.SetBool("surf", true);
                moveSpeed = 10f;
            }
            else
            {
                ani.SetBool("walk", true);
                ani.SetBool("surf", false);
                moveSpeed = 5f;
            }

            if (Input.GetMouseButton(0))
            {
                if (transform.localScale.x == 1f)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                    ani.SetBool("sp_atk", true);
                }
                else if (transform.localScale.x == -1f)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                    ani.SetBool("sp_atk", true);
                }

            }
            else
            {
                ani.SetBool("sp_atk", false);
            }

        }
        else if(Input.GetAxis("Horizontal") == 0.0f)
        {
            ani.SetBool("walk", false);
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            ani.SetBool("surf", true);
            moveSpeed = 10f;
        }
        else
        {
            ani.SetBool("surf", false);
            moveSpeed = 5f;
        }

        if(Input.GetMouseButton(0))
        {
            if(transform.localScale.x==1f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                ani.SetBool("sp_atk", true);
            }
            else if (transform.localScale.x == -1f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                ani.SetBool("sp_atk", true);
            }
            
        }
        else
        {
            ani.SetBool("sp_atk", false);
        }

            transform.Translate(moveX, moveY, 0);

    }
}
