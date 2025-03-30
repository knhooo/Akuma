using System.Collections;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UIElements;

public class HH_CameraShake : MonoBehaviour
{
    [SerializeField]
    private float shakeTime;
    [SerializeField]
    private float shakeIntensity;
    private float bossTimer;

    bool isCameraShake = false;

    GameObject player;
    Vector3 startPos;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        bossTimer += Time.deltaTime;
        if (bossTimer >= 300f && !isCameraShake)
        {
            isCameraShake = true;
            startPos = transform.position;
            OnShakeCamera(0.75f, 0.5f);
            GameObject canvas = GameObject.Find("Canvas");
            canvas.GetComponent<UIManager>().FillBossHP();
        }
    }

    public void OnShakeCamera(float _shakeTime = 1f, float _shakeIntensity = 0.1f)
    {
        shakeTime = _shakeTime;
        shakeIntensity = _shakeIntensity;

        StartCoroutine(ShakeByPosition());
    }

    IEnumerator ShakeByPosition()
    {
        while (shakeTime > 0f)
        {
            Vector3 shakePos = new Vector3(player.transform.position.x, player.transform.position.y, startPos.z);
            transform.position = shakePos + Random.insideUnitSphere * shakeIntensity;
            shakeTime = Mathf.Max(shakeTime - Time.deltaTime, 0f);
            yield return null;
        }
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, startPos.z);
    }
}