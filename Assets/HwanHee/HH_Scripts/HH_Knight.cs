using System.Collections;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class HH_Knight : MonoBehaviour
{
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private int Hp = 100;
    [SerializeField]
    private int Attack = 10;

    enum Dir { left, right }
    private Dir dir = Dir.right;

    enum KnightState { Idle, Run, Attack, Defend, Die }
    KnightState state = KnightState.Idle;

    private SpriteRenderer spriteRenderer;
    private Animator ani;
    private float nextAttackTime = 0f;
    private float attackCooldown = 0.11f;

    [SerializeField]
    private int speed = 3;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Hp <= 0)
        {
            DIE();
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (Time.time >= nextAttackTime)
            {
                nextAttackTime = Time.time + attackCooldown;
            }

            if (state != KnightState.Attack)
            {
                state = KnightState.Attack;
            }
        }

        else if (Input.GetMouseButtonUp(0) && state == KnightState.Attack)
        {
            state = KnightState.Idle;
            ani.SetBool("Attack", false);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (state != KnightState.Defend)
            {
                state = KnightState.Defend;
            }
        }

        else if (Input.GetKeyUp(KeyCode.LeftShift) && state == KnightState.Defend)
        {
            ani.speed = 1f;
            state = KnightState.Idle;
            ani.SetBool("Defend", false);
        }

        switch (state)
        {
            case KnightState.Idle:
                ani.SetBool("Run", false);
                break;
            case KnightState.Run:
                ani.SetBool("Run", true);
                break;
            case KnightState.Attack:
                ani.SetBool("Attack", true);
                break;
            case KnightState.Defend:
                ani.SetBool("Defend", true);
                break;
        }
    }

    private void FixedUpdate()
    {
        float moveX = speed * Time.deltaTime * Input.GetAxisRaw("Horizontal");
        float moveY = speed * Time.deltaTime * Input.GetAxisRaw("Vertical");

        if (moveX > 0.01f)
        {
            if (spriteRenderer.flipX == true)
            {
                dir = Dir.right;
                spriteRenderer.flipX = false;
            }
        }

        else if (moveX < -0.01f)
        {
            if (spriteRenderer.flipX == false)
            {
                dir = Dir.left;
                sword.transform.rotation = Quaternion.Euler(0, 180f, 0);
                spriteRenderer.flipX = true;
            }
        }

        if (state == KnightState.Defend)
            return;

        if (moveX != 0 || moveY != 0)

        {
            state = KnightState.Run;
        }

        else
        {
            state = KnightState.Idle;
        }

        transform.Translate(moveX, moveY, 0);
    }


    private void ActivateSword()
    {
        sword.SetActive(true);

        if (dir == Dir.left)
        {
            sword.transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else
        {
            sword.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    private void InctivateSword()
    {
        sword.SetActive(false);
    }

    private void AnimationStop()
    {
        ani.speed = 0f;
    }

    private void DIE()
    {
        state = KnightState.Die;
        ani.SetBool("Die", true);
    }
}
