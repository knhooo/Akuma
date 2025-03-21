
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class HH_Monster : MonoBehaviour
{
    [SerializeField]
    protected int maxHp = 50;
    [SerializeField]
    protected int hp = 50;
    [SerializeField]
    protected int attack = 10;
    [SerializeField]
    protected float speed = 2f;
    [SerializeField]
    protected float attackRange = 3f;
    [SerializeField]
    protected int exp = 10;
    [SerializeField]
    GameObject item;
    [SerializeField]
    float dropRate = 10f;

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

    protected void OnEnable()
    {
        hp = maxHp;

        state = State.Run;
        anim.SetBool("Run", true);
        anim.SetBool("Attack", false);
        anim.SetBool("TakeHit", false);
        anim.ResetTrigger("Death");
    }

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected void Update()
    {
        if (state == State.Death)
            return;

        dirToPlayer = Vector3.Normalize(player.transform.position - transform.position);

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
        // 나한테 와서 충돌했어 -> 이러면 못움직이게 해야됨 /??
        if (state == State.Death)
            return;

        if (isCollisionStay)
        {
            Player _player = player.GetComponent<Player>();
            transform.Translate(-dirToPlayer * _player.Speed * Time.fixedDeltaTime);
            Debug.Log("dirToPlayer: " + dirToPlayer);
        }

        if (state == State.Attack)
            return;

        if (state == State.TakeHit)
        {
            transform.Translate(-dirToPlayer * knockBackSpeed * Time.fixedDeltaTime);
        }

        if (!isCollisionStay && state == State.Run)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
    }

    protected void LateUpdate()
    {
        if (state == State.Death)
            return;
        spriteRenderer.flipX = player.transform.position.x < rigid.position.x;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.Death)
            return;

        if (collision.gameObject.CompareTag("Player"))
        {
            isCollisionStay = true;
        }

        if (collision.CompareTag("PlayerAttack"))
        {
            isTakeHitOver = false;

            hp -= player.GetComponent<Player>().Attack;
            if (hp <= 0)
            {
                Death();
                return;
            }
            state = State.TakeHit;
            anim.SetBool("Run", false);
            anim.SetBool("Attack", false);
            anim.SetBool("TakeHit", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isCollisionStay = false;
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
        if (isTakeHitOver)
        {
            state = State.Run;
            anim.SetBool("TakeHit", false);
            anim.SetBool("Run", true);
        }
    }

    private void Death()
    {
        anim.SetBool("TakeHit", false);
        anim.SetBool("Death", true);
        state = State.Death;

        player.GetComponent<Player>().Exp += exp;
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

        if (Random.Range(0, 100) < dropRate)
            Instantiate(item, transform.position, Quaternion.identity);
    }

    protected void SetTakeHitOver()
    {
        isTakeHitOver = true;
    }
}
