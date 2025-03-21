using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    public Collider2D effectCollider;
    public DotStackManager dotStackManager; // DotStackManager 참조
    public GameObject DotStackManagerPrefabs;
    void Start()
    {
        if (dotStackManager == null)
        {
            GameObject dotStackManagerObject = Instantiate(DotStackManagerPrefabs);
            dotStackManager = dotStackManagerObject.GetComponent<DotStackManager>();
        }
    }


    void OnDrawGizmos()
    {
        if (effectCollider != null)
        {
            Gizmos.color = Color.green; // 색상 설정 (원하는 색으로 변경 가능)
            Gizmos.DrawWireCube(effectCollider.bounds.center, effectCollider.bounds.size); // Collider의 크기와 위치에 맞게 그리기
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            // 플레이어에게 도트 스택을 관리하는 DotStackManager에 요청
            dotStackManager.OnPlayerEnter(other); // DotStackManager에 플레이어가 들어왔음을 알림
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            // 플레이어에게 도트 스택을 관리하는 DotStackManager에 요청
            dotStackManager.OnPlayerStay(other); // DotStackManager에 플레이어가 머무는 중임을 알림
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            // 플레이어가 콜라이더를 벗어나면 DotStackManager에 알림
            dotStackManager.OnPlayerExit(other); // DotStackManager에 플레이어가 나갔음을 알림
        }
    }

    void Update()
    {
        dotStackManager.UpdateStackTimers(); // DotStackManager에서 스택 타이머 갱신
    }
}
