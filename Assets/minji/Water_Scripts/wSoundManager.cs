using UnityEngine;
using System.Collections;

public class wSoundManager : MonoBehaviour
{
    public static wSoundManager instance;
    private AudioSource wAudio;       // 단발 사운드 전용 AudioSource
    private AudioSource loopAudio;    // 반복 사운드 전용 AudioSource

    public AudioClip pwp;
    public AudioClip twp;
    public AudioClip pw;
    public AudioClip tw;
    public AudioClip s;

    private Coroutine fadeCoroutine; // 현재 실행 중인 페이드 아웃 코루틴 추적

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        // 단발성 재생용 AudioSource
        wAudio = gameObject.AddComponent<AudioSource>();

        // 반복 재생용 AudioSource
        loopAudio = gameObject.AddComponent<AudioSource>();
        loopAudio.loop = true; // 반복 재생 설정
    }

    // 단발성 사운드 재생
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

    // 반복 사운드 시작 (surfS)
    public void surfS()
    {
        if (!loopAudio.isPlaying) // 중복 실행 방지
        {
            loopAudio.clip = s;
            loopAudio.volume = 0.1f; // 볼륨 초기화
            loopAudio.Play();
        }
    }

    // 반복 사운드 종료 (페이드 아웃)
    public void surfEnd()
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine); // 기존 코루틴 중지
        fadeCoroutine = StartCoroutine(FadeOut(loopAudio, 1f));
    }

    // 오디오 서서히 끄기 (부드러운 페이드 아웃)
    IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / duration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // 볼륨 초기화
    }
}
