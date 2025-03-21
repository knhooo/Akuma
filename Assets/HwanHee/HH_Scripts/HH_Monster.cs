
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class HH_Monster : MonoBehaviour
{
    [SerializeField]
    protected int hp = 50;
    [SerializeField]
    protected int attack = 10;
    [SerializeField]
    protected float speed = 2f;
    [SerializeField]
    protected float attackRange = 2f;
    [SerializeField]
    protected int exp = 10;

    protected enum State { Run, Attack, TakeHit, Death }
    protected State state = State.Run;

    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected Animator anim;

    protected GameObject player;

    protected float distanceToPlayer;
    protected float knockBackSpeed = 1f;

    protected bool isTakeHitOver = true;
    protected bool isCollisionStay = false;

    protected Vector3 dirToPlayer;

    protected void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected void Update()
    {
        if (state == State.Death)
            return;

        distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

        switch (state)
        {
            case State.Run:
                Run();
                break;
            case State.Attack:
                Attack();
                break;
            case State.TakeHit:
                TakeHit();
                break;
            default:
                break;
        }
    }

    protected void FixedUpdate()
    {
        // 플레이어한테 밀리면서 맞았을 때 -> player이속 + 넉백 이속
        if (isCollisionStay && state != State.TakeHit)
        {
            Player _player = player.GetComponent<Player>();
            transform.Translate(-dirToPlayer * (knockBackSpeed + _player.Speed) * Time.fixedDeltaTime);
        }

        // 넉백
        else if (!isCollisionStay && state == State.TakeHit)
        {
            transform.Translate(-dirToPlayer * knockBackSpeed * Time.fixedDeltaTime);
        }

        if (state != State.Run || state == State.TakeHit)
            return;

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
            dirToPlayer = Vector3.Normalize(player.transform.position - transform.position);
        }
    }

    protected void LateUpdate()
    {
        spriteRenderer.flipX = player.transform.position.x < rigid.position.x;
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollisionStay = true;
        }
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollisionStay = false;
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.Death)
            return;

        if (collision.CompareTag("PlayerAttack"))
        {
            isTakeHitOver = false;
            hp -= player.GetComponent<Player>().Attack;
            state = State.TakeHit;
            anim.SetBool("Run", false);
            anim.SetBool("Attack", false);
            anim.SetBool("TakeHit", true);
        }
    }

    protected virtual void Run()
    {
        // 공격범위 들어올 경우
        if (distanceToPlayer <= attackRange)
        {
            anim.SetBool("Run", false);
            anim.SetBool("Attack", true);
            state = State.Attack;
        }
    }

    protected virtual void Attack()
    {
        // 멀어졌을 경우
        if (distanceToPlayer > attackRange)
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Run", true);
            state = State.Run;
        }
    }

    protected void TakeHit()
    {
        if (hp <= 0)
        {
            anim.SetBool("TakeHit", false);
            anim.SetBool("Death", true);
            state = State.Death;

            player.GetComponent<Player>().Exp += exp;
        }

        if (isTakeHitOver)
        {
            state = State.Run;
            anim.SetBool("TakeHit", false);
            anim.SetBool("Run", true);
        }
    }

    // 애니메이션 이벤트용
    protected virtual void AttackPlayer()
    {
        if (distanceToPlayer <= attackRange)
        {
            Player _player = player.GetComponent<Player>();
            _player.TakeDamage(attack);
        }
    }

    protected void DestroyMonster()
    {
        gameObject.SetActive(false);
    }

    protected void SetTakeHitOver()
    {
        isTakeHitOver = true;
    }
}
