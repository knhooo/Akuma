using UnityEngine;
using System.Collections;

public class WaterPuple2 : MonoBehaviour
{
    public float Speed = 2f;
    Vector2 dir;
    Vector2 dirNo;


    void Start()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        dir = mousePos - transform.position;
        dirNo = dir.normalized;
        StartCoroutine("waterPstay");
    }


    void Update()
    {

        transform.Translate(dirNo * Speed * Time.deltaTime);
    }

    IEnumerator waterPstay()
    {
        yield return new WaitForSeconds(2);
        while (Speed > 0)
        {
            Speed -= Time.deltaTime;
        }
        yield return new WaitForSeconds(2);
        wSoundManager.instance.tWaterP();
        Destroy(gameObject);
    }

}

