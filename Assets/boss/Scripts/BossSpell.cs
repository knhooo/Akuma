using UnityEngine;

public class BossSpell : MonoBehaviour
{
    private Animator animator;  // 애니메이터
    private float animationLength;  // 애니메이션 길이

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
}
