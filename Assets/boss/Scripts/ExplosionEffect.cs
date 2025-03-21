using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionEffect : MonoBehaviour
{
    private float damageOverTime;
    private float dotDuration;
    private float dotInterval;

    private HashSet<Player> playersInEffect = new HashSet<Player>(); // 도트 딜 적용할 플레이어 리스트

    public void SetDamageParams(float dotDamage, float duration, float interval)
    {
        damageOverTime = dotDamage;
        dotDuration = duration;
        dotInterval = interval;

        StartCoroutine(DotDamageCoroutine()); // 지속적으로 도트 딜 적용
        Destroy(gameObject, dotDuration + 0.5f); // 도트 끝난 후 효과 삭제
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                playersInEffect.Add(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                playersInEffect.Remove(player);
            }
        }
    }

    private IEnumerator DotDamageCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < dotDuration)
        {
            foreach (var player in playersInEffect)
            {
                if (player != null)
                {
                    player.TakeDamage(Mathf.RoundToInt(damageOverTime * dotInterval));
                    Debug.Log($"🔥 {player.name}에게 도트 데미지 적용");
                }
            }

            elapsedTime += dotInterval;
            yield return new WaitForSeconds(dotInterval);
        }
    }
}
