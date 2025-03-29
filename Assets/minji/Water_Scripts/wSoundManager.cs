using UnityEngine;
using System.Collections;

public class wSoundManager : MonoBehaviour
{
    public static wSoundManager instance;
    private AudioSource wAudio;       // �ܹ� ���� ���� AudioSource
    private AudioSource loopAudio;    // �ݺ� ���� ���� AudioSource

    public AudioClip pwp;
    public AudioClip twp;
    public AudioClip pw;
    public AudioClip tw;
    public AudioClip s;

    private Coroutine fadeCoroutine; // ���� ���� ���� ���̵� �ƿ� �ڷ�ƾ ����

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // �ܹ߼� ����� AudioSource
        wAudio = gameObject.AddComponent<AudioSource>();

        // �ݺ� ����� AudioSource
        loopAudio = gameObject.AddComponent<AudioSource>();
        loopAudio.loop = true; // �ݺ� ��� ����
    }

    // �ܹ߼� ���� ���
    public void pWater()
    {
        wAudio.PlayOneShot(pw);
    }

    public void tWater()
    {
        wAudio.PlayOneShot(tw);
    }

    public void pWaterP()
    {
        wAudio.PlayOneShot(pwp);
    }

    public void tWaterP()
    {
        wAudio.PlayOneShot(twp);
    }

    // �ݺ� ���� ���� (surfS)
    public void surfS()
    {
        if (!loopAudio.isPlaying) // �ߺ� ���� ����
        {
            loopAudio.clip = s;
            loopAudio.volume = 0.1f; // ���� �ʱ�ȭ
            loopAudio.Play();
        }
    }

    // �ݺ� ���� ���� (���̵� �ƿ�)
    public void surfEnd()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine); // ���� �ڷ�ƾ ����
        fadeCoroutine = StartCoroutine(FadeOut(loopAudio, 1f));
    }

    // ����� ������ ���� (�ε巯�� ���̵� �ƿ�)
    IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // ���� �ʱ�ȭ
    }
}
