using UnityEngine;
using UnityEngine.UI;

public class TimeDisplay : MonoBehaviour
{
    Text time;
    float startTime;
    float ms;

    void Awake()
    {
        time = GetComponent<Text>();
        startTime = 0f;
    }

    void Update()
    {
        startTime += Time.deltaTime;
        ms = startTime;

        int min = Mathf.FloorToInt(ms / 60);
        int sec = Mathf.FloorToInt(ms % 60);
        time.text = string.Format("{0:D2}:{1:D2}", min, sec);
    }

    public float GetTime()
    {
        return ms;
    }
}
