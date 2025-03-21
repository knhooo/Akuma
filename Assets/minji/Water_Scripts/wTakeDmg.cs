using UnityEngine;
using System.Collections;

public class wTakeDmg : MonoBehaviour
{

    void Start()
    {
        StartCoroutine("TDeffect");
    }

    IEnumerator TDeffect()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject); 
    }
}
