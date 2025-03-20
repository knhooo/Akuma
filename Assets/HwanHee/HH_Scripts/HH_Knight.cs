using System.Collections;
using System.ComponentModel.Design.Serialization;
using NUnit.Framework.Constraints;
using TMPro.SpriteAssetUtilities;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class HH_Knight : MonoBehaviour
{
    [SerializeField]
    int hp = 100;
    [SerializeField]
    int attack = 10;
    [SerializeField]
    int speed = 3;
    [SerializeField]
    GameObject sword_right;
    [SerializeField]
    GameObject sword_left;
    [SerializeField]
    GameObject shield;
    [SerializeField]
    float shieldTime = 3.0f;
    [SerializeField]
    float shieldCoolTime = 5.0f;
    [SerializeField]
    GameObject blood;
    [SerializeField]
    protected Material takeHitMaterial;

    public int Attack { get { return attack; } set { attack = value; } }
    public int Hp { get { return hp; } set { hp = value; } }
    public int Speed { get { return speed; } }

    enum Dir { left, right }
    Dir dir = Dir.right;

    enum KnightState { Attack, Defend, Death }
    KnightState state = KnightState.Attack;

    SpriteRenderer spriteRenderer;
    Animator anim;
    Rigidbody2D rigid;

    Material originalMaterial;
    Coroutine shieldCoroutine;
    Vector2 inputVec;

    float shieldTimer = 0f;
    float shieldCoolTimer = 0f;

    void Awake()
    {
        shieldCoolTimer = shieldCoolTime;

        originalMaterial = GetComponent<SpriteRenderer>().material;
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (state == KnightState.Death)
        {
            return;
        }

        switch (state)
        {
            case KnightState.Attack:
                StateAttack();
                break;
            case KnightState.Defend:
                Defend();
                break;
        }

        HandleKnightInput();
    }

    void FixedUpdate()
    {
        if (state == KnightState.Death || state == KnightState.Defend)
        {
            return;
        }

        Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
        rigid.MovePosition(rigid.position + nextVec);
    }

    void LateUpdate()
    {
        if (state == KnightState.Death)
        {
            return;
        }

        if (inputVec.x != 0)
        {
            dir = Dir.left;
            spriteRenderer.flipX = inputVec.x < 0;
        }

        if (!spriteRenderer.flipX)
        {
            dir = Dir.right;
        }
    }

    void StateAttack()
    {
        shieldCoolTimer += Time.deltaTime;
        if (shieldCoolTimer >= shieldCoolTime)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                shieldCoolTimer = 0f;

                if (shieldCoroutine != null)
                    StopCoroutine(shieldCoroutine);
                shieldCoroutine = StartCoroutine(SetShieldAlpha());
                shield.SetActive(true);
                SetAlpha(1f);

                state = KnightState.Defend;
                anim.SetBool("Attack", false);
                anim.SetBool("Defend", true);

            }
        }
    }

    void Defend()
    {
        shieldTimer += Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.LeftShift) || shieldTimer >= shieldTime)
        {
            shieldTimer = 0f;
            anim.speed = 1f;

            StopCoroutine(SetShieldAlpha());
            shield.SetActive(false);

            state = KnightState.Attack;
            anim.SetBool("Defend", false);
            anim.SetBool("Attack", true);
        }
    }

    void HandleKnightInput()
    {
        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        if (state != KnightState.Defend && anim.speed == 0)
        {
            anim.speed = 1f;
        }

        if (state == KnightState.Defend)
            return;
    }

    public void TakeDamage(int dmg)
    {
        if (state == KnightState.Defend || state == KnightState.Death)
            return;

        hp -= dmg;
        if (hp <= 0)
        {
            state = KnightState.Death;
            anim.SetTrigger("Death");
            return;
        }

        StartCoroutine(FlashWhite());

        Vector3 bloodPos = transform.position;
        bloodPos.y = transform.position.y - 0.92f;
        Instantiate(blood, bloodPos, Quaternion.Euler(0, 0, 0));
    }

    protected IEnumerator FlashWhite()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = takeHitMaterial;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.material = originalMaterial;
    }

    IEnumerator SetShieldAlpha()
    {
        float t = 0f;
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;
            t = timer / shieldTime;
            SetAlpha(Mathf.Lerp(1f, 0.2f, t));
            yield return null;
        }
    }

    void SetAlpha(float alpha)
    {
        Color _color = shield.GetComponent<SpriteRenderer>().color;
        _color.a = Mathf.Clamp01(alpha);
        shield.GetComponent<SpriteRenderer>().color = _color;
    }

    // 애니메이션 이벤트용 함수
    void ActivateSword()
    {
        if (dir == Dir.left)
        {
            sword_left.SetActive(true);
        }
        else
        {
            sword_right.SetActive(true);
        }
    }

    void InctivateSword()
    {
        sword_right.SetActive(false);
        sword_left.SetActive(false);
    }

    void AnimationStop() { anim.speed = 0f; }
}
