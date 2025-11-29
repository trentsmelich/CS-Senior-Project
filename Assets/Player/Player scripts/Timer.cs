using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private float timeElapsed;   // total time passed (seconds)
    private bool isRunning = true;
    public TextMeshProUGUI displayTimeCounter;
    public TextMeshProUGUI displayFinalTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            timeElapsed += Time.deltaTime;   // Add time each frame
            FixedUpdate();
        }

        if (!isRunning)
        {
            FixedUpdate();
        }
    }

    private void FixedUpdate()
    {
        // Update the display every fixed frame
        int minutes = Mathf.FloorToInt(timeElapsed / 60F);
        int seconds = Mathf.FloorToInt(timeElapsed - minutes * 60);
        displayTimeCounter.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        displayFinalTime.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
