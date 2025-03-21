using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class HH_WizardProjectile : MonoBehaviour
{
    [SerializeField]
    float speed = 3;
    [SerializeField]
    int attack = 0;
    public int Attack {  get { return attack; } set { attack = value; } }

    bool isDestroy = false;
    protected GameObject player;

    Animator anim;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isDestroy)
            return;

        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isDestroy = true;
            anim.SetTrigger("Destroy");

            player.GetComponent<Player>().TakeDamage(attack);
        }
    }

    // 애니메이션 이벤트용
    void DestroyProj()
    {
        Destroy(gameObject);
    }
}
