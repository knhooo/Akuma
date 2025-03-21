using Unity.VisualScripting;
using UnityEngine;

public class Item_Heal : MonoBehaviour
{
    [SerializeField]
    int healAmount = 10;

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
        }
    }
}
