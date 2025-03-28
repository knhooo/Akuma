using Unity.Cinemachine;
using UnityEngine;

public class HH_CameraShake : MonoBehaviour
{
    public static HH_CameraShake instance;
    private CinemachineImpulseSource impulseSource;

    void Start()
    {
        instance = this;
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void CameraShakeShow()
    {
        if (impulseSource != null)
        {
            impulseSource.GenerateImpulse();
        }
    }
}
