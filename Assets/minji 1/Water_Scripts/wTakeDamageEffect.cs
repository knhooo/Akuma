using UnityEngine;
using System.Collections;

public class wTakeDamageEffect : MonoBehaviour
{
    //물마법사 피격시 이펙트
    void Start()
    {
        StartCoroutine("TDeffect");
    }

    IEnumerator TDeffect()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject); //이펙트 애니메이션 끝난 뒤 사라지게 하기
    }
}
