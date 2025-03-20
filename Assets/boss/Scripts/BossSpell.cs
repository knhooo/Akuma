using System.Collections;
using UnityEngine;

public class BossSpell : MonoBehaviour
{
    private Animator animator;  // 애니메이터
    private float animationLength;  // 애니메이션 길이

    public GameObject explosionEffectPrefab; // 폭발 이펙트 프리팹
    public Transform explosionPoint;         // 폭발이 발생할 위치
    public float damage = 20f; // 폭발 데미지
    public float explosionRadius = 1f; // 폭발 반경
    public float explosionDuration = 0.5f; // 폭발 지속 시간
    public Collider2D effectCollider; //Effect의 Collider를 할당

    public float damageOverTime = 10f;       // 도트 데미지
    public float dotDuration = 3f;           // 도트 지속 시간
    public float dotInterval = 1f;           // 도트 타격 주기

    private bool isExplosionTriggered = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        // 애니메이션 길이를 가져오기 위한 방법
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        // 애니메이션 클립이 존재한다면 길이 가져오기
        if (clipInfo.Length > 0)
        {
            animationLength = clipInfo[0].clip.length;
            Destroy(gameObject, animationLength);  // 애니메이션 길이 후 오브젝트 삭제
        }
    }

    // 애니메이션 이벤트로 호출되는 메서드
    public void OnExplosionEffectTriggered()
    {
        if (!isExplosionTriggered)
        {
            isExplosionTriggered = true;
            // 폭발 이펙트 소환
            SpawnExplosionEffect();
        }
    }

    void SpawnExplosionEffect()
    {
        if (explosionPoint == null)
        {
            Debug.LogError("ExplosionPoint가 null입니다. 폭발 효과를 생성할 수 없습니다.");
            return; // explosionPoint가 null이면 더 이상 진행하지 않음
        }

        // 폭발 이펙트 생성 (폭발 지점에 소환)
        GameObject explosionEffect = Instantiate(explosionEffectPrefab, explosionPoint.position, Quaternion.identity);

        // Effect_1의 Collider 크기를 explosionRadius에 반영
        if (effectCollider != null)
        {
            // Collider의 크기만큼 explosionRadius 설정
            explosionRadius = effectCollider.bounds.size.x / 2f;
        }

        // 폭발 즉시 플레이어가 범위 내에 있으면 데미지 입히기
        ApplyExplosionDamage();

        // 생성된 폭발 이펙트를 3초 후 삭제
        Destroy(explosionEffect, 3f);
        isExplosionTriggered = false;

        // 폭발 후 이펙트 위에 도트딜 적용
        StartCoroutine(ApplyDotDamage(explosionEffect.transform.position));
    }

    // 폭발 범위 내의 플레이어에게 즉시 데미지를 적용
    void ApplyExplosionDamage()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(explosionPoint.position, explosionRadius);  // 폭발 범위 내의 모든 플레이어를 감지

        foreach (var player in hitPlayers)
        {
            if (player.CompareTag("Player"))
            {
                // 플레이어에게 폭발 데미지 적용
                // player.GetComponent<Player>().TakeDamage(damage);
                Debug.Log("💣 폭발 데미지 적용");
            }
        }
    }

    // 플레이어가 폭발 범위에 있을 때 도트 데미지 적용
    IEnumerator ApplyDotToPlayer(Collider2D player)
    {
        float elapsedTime = 0f;

        while (elapsedTime < dotDuration)
        {
            // 도트 데미지 적용 (Player.cs에 적절한 데미지 처리 함수 추가 필요)
            //player.GetComponent<Player>().TakeDamage(damageOverTime * dotInterval);
            Debug.Log("🔥 도트 데미지 적용");

            elapsedTime += dotInterval;
            yield return new WaitForSeconds(dotInterval);
        }
    }

    // 도트 딜 적용 함수
    IEnumerator ApplyDotDamage(Vector3 explosionPosition)
    {
        // 도트 범위를 설정 (폭발 반경을 콜라이더 범위로 설정)
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(explosionPosition, explosionRadius);

        foreach (var player in hitPlayers)
        {
            if (player.CompareTag("Player"))
            {
                // 플레이어에게 도트 데미지 적용
                StartCoroutine(ApplyDotToPlayer(player));
            }
        }

        // 도트 지속 시간 후에 사라짐
        yield return new WaitForSeconds(dotDuration);
        Destroy(gameObject);  // 이펙트 삭제
    }        
}
