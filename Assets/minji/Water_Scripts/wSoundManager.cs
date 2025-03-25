using UnityEngine;

public class wSoundManager : MonoBehaviour
{

    public static wSoundManager instance;
    AudioSource myAudio;
    public AudioClip pwp;
    public AudioClip twp;
    public AudioClip pw;
    public AudioClip tw;


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

    void Update()
    {

    }
}
