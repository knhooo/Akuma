using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DotStackManager : MonoBehaviour
{
    public float damageOverTime = 2.5f;     // 초당 도트 데미지
    public float dotInterval = 1f;          // 도트 데미지 적용 간격 (초)
    public float dotStackInterval = 0.5f;   // 도트 스택 증가 간격 (딜레이)
    public float dotStackDuration = 3f;     // 1 스택의 유지시간 (초)
    public int maxDotStacks = int.MaxValue; // 무제한

    private Dictionary<Player, int> dotStacks = new Dictionary<Player, int>(); // 플레이어별 스택 관리
    private Dictionary<Player, Coroutine> activeDots = new Dictionary<Player, Coroutine>();
    private Dictionary<Player, List<float>> stackTimers = new Dictionary<Player, List<float>>();  // 각 플레이어의 도트 스택 타이머 관리


    // 도트 스택을 증가시키는 코루틴 (각각 일정 시간마다 증가)
    public IEnumerator IncreaseDotStackOverTime(Player player)
    {
        while (isPlayerInCollider(player))
        {
            Debug.Log($"🔄 {player.name} 스택 증가 코루틴 실행 중... (현재 스택: {dotStacks[player]})");
            // 스택을 증가시킴
            if (dotStacks[player] < maxDotStacks) // 최대 스택을 넘지 않도록
            {
                dotStacks[player]++;
                stackTimers[player].Add(dotStackDuration); // 새 스택에 대한 타이머 추가

                // 도트 스택 증가 로그
                Debug.Log($"🔥 {player.name} 도트 스택 증가: {dotStacks[player]}");

                // 도트 스택이 증가된 후 자동으로 데미지를 적용
                if (!activeDots.ContainsKey(player) || activeDots[player] == null)
                {
                    activeDots[player] = StartCoroutine(ApplyDotToPlayer(player)); // 데미지 적용 시작
                }

                // 일정 시간마다 도트 스택 증가
                yield return new WaitForSeconds(dotStackInterval);
            }
            else
            {
                Debug.Log($"🔥 {player.name} 도트 스택이 최대치에 도달했습니다: {dotStacks[player]}");
            }

        }
        Debug.Log($"⛔ {player.name} 코루틴 종료됨");
        // 범위에서 벗어나면 코루틴 종료
        activeDots.Remove(player);
    }

    // 도트 스택에 맞게 플레이어에게 데미지를 주는 코루틴
    private IEnumerator ApplyDotToPlayer(Player player)
    {
        // 초당 피해량 계산
        float dps = damageOverTime / dotInterval;

        while (dotStacks.ContainsKey(player) && dotStacks[player] > 0)
        {
            // 스택에 맞는 총 피해를 계산
            float totalDamage = damageOverTime * dotStacks[player];

            // 피해 적용
            player.TakeDamage(Mathf.RoundToInt(totalDamage));
            Debug.Log($"🔥 {player.name} 도트 데미지 적용 ({dotStacks[player]}스택, 초당 {dps}피해, 총 {totalDamage} 피해)");

            // 도트 데미지를 주고 일정 간격으로 대기
            yield return new WaitForSeconds(dotInterval);
        }

        // 도트 데미지를 다 받은 후 더 이상 데미지가 적용되지 않도록 제거
        activeDots.Remove(player);
    }

    // 범위에서 벗어나면 타이머 및 스택 갱신
    public void UpdateStackTimers()
    {
        // 타이머 갱신 (Dictionary의 키를 안전하게 순회하는 방법)
        List<Player> playersToUpdate = new List<Player>(dotStacks.Keys); // 복사본 생성

        foreach (var player in playersToUpdate)
        {
            List<float> playerTimers = stackTimers[player];
            for (int i = playerTimers.Count - 1; i >= 0; i--)
            {
                playerTimers[i] -= Time.deltaTime; // 남은 시간 감소 (매 프레임마다 갱신)

                // 타이머가 0 이하로 떨어지면 해당 스택 제거
                if (playerTimers[i] <= 0)
                {
                    playerTimers.RemoveAt(i);
                    dotStacks[player]--;
                    Debug.Log($"❄️ {player.name} 도트 스택 감소: {dotStacks[player]}");
                }
            }
        }
    }

    // 플레이어가 새로운 도트 스택을 받을 때 초기화
    public void InitializePlayer(Player player)
    {
        if (!dotStacks.ContainsKey(player))
        {
            dotStacks[player] = 0;
            stackTimers[player] = new List<float>(); // 스택 타이머 초기화
        }
    }

    // 콜라이더에 플레이어가 들어왔을 때, 초기화하고 도트 스택을 증가시키는 코루틴 시작
    public void OnPlayerEnter(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            InitializePlayer(player);

            // 스택을 증가시키는 코루틴 시작
            if (!activeDots.ContainsKey(player) || activeDots[player] == null)
            {
                activeDots[player] = StartCoroutine(IncreaseDotStackOverTime(player));
            }
        }
    }

    // 콜라이더에 플레이어가 머물 때, 초기화하고 도트 스택을 증가시키는 코루틴 시작
    public void OnPlayerStay(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            InitializePlayer(player);

            // 스택을 증가시키는 코루틴 시작
            if (!activeDots.ContainsKey(player) || activeDots[player] == null)
            {
                activeDots[player] = StartCoroutine(IncreaseDotStackOverTime(player));
            }
        }
    }

    // 콜라이더에 플레이어가 나갔을 때, 스택을 멈추고 로그 출력
    public void OnPlayerExit(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            if (dotStacks.ContainsKey(player))
            {
                // 범위를 벗어나도 도트 스택은 유지됨
                Debug.Log($"❄️ {player.name} 콜라이더에서 벗어남, 도트 스택 유지");

                // 스택 증가 코루틴을 중단하지만 스택 타이머는 유지
                if (activeDots.ContainsKey(player) && activeDots[player] != null)
                {
                    StopCoroutine(activeDots[player]);
                    activeDots.Remove(player);
                }
            }
        }
    }

    // 플레이어가 콜라이더 범위에 있는지 체크    
    private bool isPlayerInCollider(Player player)
    {
        bool inCollider = true; // 현재는 무조건 true 반환하는 상태

        //Debug.Log($"📍 {player.name} isPlayerInCollider: {inCollider}");

        return inCollider;
    }

    public void Update()
    {
        UpdateStackTimers(); // 초마다 타이머 갱신
    }
}
