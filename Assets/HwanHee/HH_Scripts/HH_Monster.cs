
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class HH_Monster : MonoBehaviour
{
    [SerializeField]
    int hp = 50;
    [SerializeField]
    int attack = 10;
    [SerializeField]
    float speed = 2;
    [SerializeField]
    float attackRange = 2f;

    enum State { Run, Attack, TakeHit, Death }
    State state = State.Run;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private GameObject player;

    float distanceToPlayer;
    float knockBackSpeed = 1f;

    bool isAttackOver = true;
    bool isTakeHitOver = true;
    bool isCollisionStay = false;

    Vector3 dir;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
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

    private void FixedUpdate()
    {
        // 플레이어한테 뒤로 밀려남
        if (isCollisionStay && state != State.TakeHit)
        {
            HH_Knight _player = player.GetComponent<HH_Knight>();
            transform.Translate(-dir * (knockBackSpeed + _player.Speed) * Time.fixedDeltaTime);
        }

        // 넉백
        if (isCollisionStay && state == State.TakeHit)
        {
            HH_Knight _player = player.GetComponent<HH_Knight>();
            transform.Translate(-dir * (knockBackSpeed + _player.Speed) * Time.fixedDeltaTime);
        }

        // 넉백
        else if (!isCollisionStay && state == State.TakeHit)
        {
            transform.Translate(-dir * knockBackSpeed * Time.fixedDeltaTime);
        }

        if (state != State.Run || state == State.TakeHit)
            return;

        else
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.fixedDeltaTime);
            dir = Vector3.Normalize(player.transform.position - transform.position);
        }
    }

    private void LateUpdate()
    {
        spriteRenderer.flipX = player.transform.position.x < rigid.position.x;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollisionStay = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollisionStay = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.Death)
            return;

        if (collision.CompareTag("PlayerAttack"))
        {
            isAttackOver = true;
            isTakeHitOver = false;
            hp -= player.GetComponent<HH_Knight>().Attack;
            state = State.TakeHit;
            anim.SetBool("Run", false);
            anim.SetBool("Attack", false);
            anim.SetBool("TakeHit", true);
        }
    }

    private void Run()
    {
        // 공격범위 들어올 경우
        if (distanceToPlayer <= attackRange)
        {
            isAttackOver = false;
            anim.SetBool("Run", false);
            anim.SetBool("Attack", true);
            state = State.Attack;
        }
    }

    private void Attack()
    {
        if (!player)
        {
            anim.SetBool("Run", true);
            anim.SetBool("Attack", false);
            return;
        }

        // 멀어졌을 경우
        if (distanceToPlayer > attackRange && isAttackOver)
        {
            anim.SetBool("Attack", false);
            anim.SetBool("Run", true);
            state = State.Run;
        }
    }

    private void TakeHit()
    {
        if (!player)
        {
            anim.SetBool("TakeHit", false);
            anim.SetBool("Run", true);
            state = State.Run;
            return;
        }

        if (hp <= 0)
        {
            anim.SetBool("TakeHit", false);
            anim.SetBool("Death", true);
            state = State.Death;
        }

        if (isTakeHitOver)
        {
            state = State.Run;
            anim.SetBool("TakeHit", false);
            anim.SetBool("Run", true);
        }
    }

    // 애니메이션 이벤트용
    private void AttackPlayer()
    {
        if (distanceToPlayer <= attackRange)
        {
            HH_Knight _player = player.GetComponent<HH_Knight>();
            _player.TakeDamage(attack);
        }
    }

    private void SetAttcakOver()
    {
        isAttackOver = true;
    }

    private void DestroyMonster()
    {
        gameObject.SetActive(false);
    }

    private void SetTakeHitOver()
    {
        isTakeHitOver = true;
    }
}
