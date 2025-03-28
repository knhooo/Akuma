using UnityEngine;

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
        myAudio.loop = false;
        myAudio.PlayOneShot(tw);
    }


    public void pWaterP()
    {
        myAudio.loop = false;
        myAudio.PlayOneShot(pwp);
    }

    public void tWaterP()
    {
        myAudio.loop = false;
        myAudio.PlayOneShot(twp);
    }
    public void surfS()
    {
        myAudio.loop = true;
        myAudio.PlayOneShot(s);
    }

    public void surfEnd()
    {
        myAudio.loop = false;
        myAudio.Stop();
    }

    void Update()
    {

    }
}
