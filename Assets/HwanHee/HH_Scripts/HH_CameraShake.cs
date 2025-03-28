using System.Collections;
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
    private void Update()
    {
        bossTimer += Time.deltaTime;
        if (bossTimer >= 300f && !isCameraShake)
        {
            isCameraShake = true;
            OnShakeCamera(0.75f, 0.5f);
        }
    }

    public void OnShakeCamera(float _shakeTime = 1f, float _shakeIntensity = 0.1f)
    {
        shakeTime = _shakeTime;
        shakeIntensity = _shakeIntensity;

        StopCoroutine(ShakeByPosition());
        StartCoroutine(ShakeByPosition());
    }

    IEnumerator ShakeByPosition()
    {
        Vector3 startPos = transform.position;
        while (shakeTime > 0f)
        {
            transform.position = startPos + Random.insideUnitSphere * shakeIntensity;
            shakeTime -= Time.deltaTime;
            yield return null;
        }
        transform.position = startPos;
    }
}
