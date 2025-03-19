using UnityEngine;

public class BossSpell : MonoBehaviour
{
    private Animator animator;  // �ִϸ�����
    private float animationLength;  // �ִϸ��̼� ����

    void Start()
    {
        animator = GetComponent<Animator>();

        // �ִϸ��̼� ���̸� �������� ���� ���
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);

        // �ִϸ��̼� Ŭ���� �����Ѵٸ� ���� ��������
        if (clipInfo.Length > 0)
        {
            animationLength = clipInfo[0].clip.length;
            Destroy(gameObject, animationLength);  // �ִϸ��̼� ���� �� ������Ʈ ����
        }
    }
}
