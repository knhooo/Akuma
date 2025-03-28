using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Heal : MonoBehaviour
{
    [SerializeField]
    int healAmount = 10;
    [SerializeField]
    GameObject logPrefab;
    [SerializeField]
    GameObject glowEffect1;
    [SerializeField]
    GameObject glowEffect2;

    private void Awake()
    {
        glowEffect1.SetActive(false);
        glowEffect2.SetActive(true);

        StartCoroutine(SwitchObjects());
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player _player = collision.GetComponent<Player>();
            healAmount = _player.MaxHp / 10;

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

    private IEnumerator SwitchObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.2f);
            glowEffect1.SetActive(!glowEffect1.activeSelf);
            glowEffect2.SetActive(!glowEffect2.activeSelf);
        }
    }
}
