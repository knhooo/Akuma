using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpell : MonoBehaviour
{
    private Animator animator;  // 애니메이터
    private float animationLength;  // 애니메이션 길이

    public GameObject explosionEffectPrefab; // 화염 이펙트 프리팹
    public Transform explosionPoint;         // 폭발이 발생할 위치
    public float damage = 20f; // 폭발 데미지
    public float explosionRadius = 1f; // 폭발 반경
    public float explosionDuration = 0.5f; // 폭발 지속 시간
    public Collider2D effectCollider; //Effect의 Collider를 할당

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
            // 화염 이펙트 소환
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

        // 화염 이펙트 생성 (폭발 지점에 소환)
        GameObject explosionEffect = Instantiate(explosionEffectPrefab, explosionPoint.position, Quaternion.identity);

        // Collider 크기 기반으로 폭발 반경 조정 (조금 더 크게 설정)
        if (effectCollider != null)
        {
            explosionRadius = (effectCollider.bounds.size.x / 2f) * 1.2f;  // 기존 크기보다 20% 더 크게
        }

        // 폭발 즉시 플레이어가 범위 내에 있으면 데미지 입히기
        ApplyExplosionDamage(explosionRadius);

        // 일정 시간 후 화염 이펙트 삭제
        Destroy(explosionEffect, 5f);
        isExplosionTriggered = false;
    }

    // 폭발 범위 내의 플레이어에게 즉시 데미지 적용
    void ApplyExplosionDamage(float explosionRadius)
    {
        Collider2D[] hitPlayers = Physics2D.OverlapBoxAll(explosionPoint.position, effectCollider.bounds.size * 1.5f, 0f);

        foreach (var player in hitPlayers)
        {
            if (player.CompareTag("Player"))
            {
                player.GetComponent<Player>().TakeDamage(Mathf.RoundToInt(damage));
                Debug.Log($"💣 {player.name} 폭발 데미지 적용! 데미지 : {damage}");
            }
        }
    }
}
