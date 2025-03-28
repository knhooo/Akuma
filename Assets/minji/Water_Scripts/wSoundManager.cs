using UnityEngine;
using System.Collections;
public class wSoundManager : MonoBehaviour
{

    public static wSoundManager instance;
    AudioSource myAudio;
    public AudioClip pwp;
    public AudioClip twp;
    public AudioClip pw;
    public AudioClip tw;
    public AudioClip s;


    private void Awake()
    {
        if (wSoundManager.instance == null)
        {
            wSoundManager.instance = this;
        }
    }

    void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    public void pWater()
    {
        myAudio.loop = false;
        myAudio.PlayOneShot(pw);
    }

    public void tWater()
    {
        myAudio.PlayOneShot(tw);
    }


    public void pWaterP()
    {
        myAudio.PlayOneShot(pwp);
    }

    public void tWaterP()
    {
        myAudio.PlayOneShot(twp);
    }
    public void surfS()
    {
        myAudio.loop = true;
        myAudio.PlayOneShot(s);
    }

    public void surfEnd()
    {
        StartCoroutine(FadeOut(myAudio,1f));
    }

    IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;
        myAudio.loop = false;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume; // º¼·ý ÃÊ±âÈ­
    }
}
