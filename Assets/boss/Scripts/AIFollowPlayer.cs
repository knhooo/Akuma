using UnityEngine;

public class AIFollowPlayer : MonoBehaviour
{
    public Transform player;  // 플레이어 위치
    public float moveSpeed = 5f;  // 이동 속도
    public float stoppingDistance = 1f;  // 목표와의 최소 거리

    private SpriteRenderer spriteRenderer;  // 스프라이트 렌더러

    void Start()
    {
        // 스프라이트 렌더러 초기화
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 플레이어와의 거리 계산
        float distance = Vector3.Distance(transform.position, player.position);

        // 플레이어가 일정 거리 내에 있을 때만 추적
        if (distance > stoppingDistance)
        {
            // 플레이어 방향으로 이동
            Vector3 direction = (player.position - transform.position).normalized;

            // 2D에서 Z 축을 고정하여 이동
            direction.z = 0;  // Z 값을 0으로 고정

            transform.position += direction * moveSpeed * Time.deltaTime;
        }

        // 보스가 플레이어를 따라가면서 좌우 반전 처리
        // 플레이어의 위치에 따라 보스가 왼쪽이나 오른쪽으로 반전
        if (player.position.x > transform.position.x)
        {
            // 플레이어가 보스의 오른쪽에 있으면 스프라이트를 정상적으로 유지
            spriteRenderer.flipX = false;
        }
        else if (player.position.x < transform.position.x)
        {
            // 플레이어가 보스의 왼쪽에 있으면 스프라이트를 반전
            spriteRenderer.flipX = true;
        }
    }
}
