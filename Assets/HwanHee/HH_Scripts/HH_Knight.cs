using System.Collections;
using System.ComponentModel.Design.Serialization;
using TMPro.SpriteAssetUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class HH_Knight : MonoBehaviour
{
    [SerializeField]
    private GameObject sword;
    [SerializeField]
    private int hp = 100;
    [SerializeField]
    private int attack = 10;
    public int Attack { get { return attack; } }

    enum Dir { left, right }
    private Dir dir = Dir.right;
    bool isAttacking = false;
    bool isDefending = false;
    bool canTakeDamage = true;

    enum KnightState { Idle, Run, Attack, Defend, TakeDmg, Die }
    KnightState state = KnightState.Idle;

    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Rigidbody2D rigid;
    private Vector2 inputVec;

    [SerializeField]
    private int speed = 3;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(state == KnightState.Die) 
        {
            DIE();
            return;                
        }

        HandleKnightInput();
    }

    private void FixedUpdate()
    {
        if (state == KnightState.Die || state == KnightState.Defend)
        {
            return;
        }

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    private void LateUpdate()
    {
        if (inputVec.x != 0)
        {
            dir = Dir.left;
            spriteRenderer.flipX = inputVec.x < 0;

        }

        if (!spriteRenderer.flipX)
        {
            dir = Dir.right;


        }

        if (state == KnightState.Die || state == KnightState.Defend)
        {
            return;
        }

        anim.SetFloat("Speed", inputVec.magnitude);
        if (inputVec.magnitude != 0)
            state = KnightState.Run;
        else
            state = KnightState.Idle;

    }

    private void HandleKnightInput()
    {
        if (state == KnightState.Die)
            return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        // 방어 끝
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            anim.speed = 1f;
            state = KnightState.Idle;
            anim.SetBool("Defend", false);
            isDefending = false;
        }

        if (state != KnightState.Defend && anim.speed == 0)
        {
            anim.speed = 1f;
        }

        if (state == KnightState.Defend)
            return;

        // 좌클릭 -> 공격
        if (Input.GetMouseButtonDown(0) && !isAttacking)
        {
            if (state != KnightState.Attack)
            {
                isAttacking = true;
                state = KnightState.Attack;
                anim.SetBool("Attack", true);
            }
        }

        // 좌쉬프트 -> 방어
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (state != KnightState.Defend)
            {
                state = KnightState.Defend;
                anim.SetBool("Defend", true);
                if (anim.GetBool("Attack"))
                    anim.SetBool("Attack", false);

            }
        }
    }


    private void DIE()
    {
        state = KnightState.Die;
        anim.SetTrigger("Die"); 
    }

    public void TakeDamage(int dmg)
    {
        if (state == KnightState.Defend || state == KnightState.Defend || !canTakeDamage)
            return;

        SetTakeDamage();

        hp -= dmg;
        if(hp <= 0) 
            state = KnightState.Die;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Monster") && canTakeDamage)
        //{
        //    SetTakeDamage();
        //}
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.CompareTag("Monster") && canTakeDamage)
        //{
        //    SetTakeDamage();
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.CompareTag("Monster"))
        //{
        //    TakeDamageFinish();
        //}
    }


    private void SetTakeDamage()
    {
        if (state == KnightState.Defend || state == KnightState.Die)
            return;

        state = KnightState.TakeDmg;
        anim.SetBool("TakeDmg", true);
        canTakeDamage = false;
    }

    private void TakeDamageFinish()
    {
        if (canTakeDamage)
            return;

        Debug.Log("DamageFinish");
        anim.SetBool("TakeDmg", false);
        canTakeDamage = true;
        state = KnightState.Idle;
    }

    // 애니메이션 이벤트용 함수
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

    private void InctivateSword() { sword.SetActive(false); }


    private void AnimationStop() { anim.speed = 0f; }


    private void AttackOver()
    {
        isAttacking = false;
        state = KnightState.Idle;
        anim.SetBool("Attack", false);
    }
}
