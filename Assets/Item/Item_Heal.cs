using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class Item_Heal : MonoBehaviour
{
    [SerializeField]
    int healAmount = 10;
    [SerializeField]
    GameObject logPrefab;

    private void Awake()
    {
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player _player = collision.GetComponent<Player>();

            _player.Hp += healAmount;
            if (_player.Hp > _player.MaxHp)
            {
                _player.Hp = _player.MaxHp;
            }
            //로그 띄우기
            Vector3 vec = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y + 1, 0);
            GameObject log = Instantiate(logPrefab, vec, Quaternion.identity);
            log.transform.SetParent(collision.gameObject.transform);
            log.GetComponent<LogText>().SetHpLog(healAmount);

            //아이템 삭제
            Destroy(gameObject, 0.5f);
        }
    }
}
