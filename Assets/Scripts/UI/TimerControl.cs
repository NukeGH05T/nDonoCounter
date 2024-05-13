using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerControl : MonoBehaviour
{
    [Header("Controls")] public bool isPaused = true;
    [Header("Animation")] public float animationDuration = 1f;
    public float textSizeIncrease = 3f;
    
    [Header("References")]
    public TMP_Text donothonTimer;
    public TMP_InputField addTimeText;
    public TMP_InputField setTimeText;
    public TMP_Text startButtonText;

    [SerializeField] private float timer = 5000;
    [SerializeField] private float autosaveInterval = 300;
    private int hours;
    private int minutes;
    private int seconds;
    private float originalFontSize;

    void Start()
    {
        // Store the original text size
        originalFontSize = donothonTimer.fontSize;

        if (!GetSavedTime().Equals(""))
        {
            timer = float.Parse(GetSavedTime());
        }
        
        InvokeRepeating(nameof(SaveTimerTime), autosaveInterval, autosaveInterval);
    }
    
    void Update()
    {
        if (isPaused) return;
        
        timer -= Time.deltaTime;

        // Calculate time
        hours = Mathf.FloorToInt(timer / 3600);
        minutes = Mathf.FloorToInt((timer - (hours * 3600)) / 60);
        seconds = Mathf.FloorToInt(timer - (hours * 3600) - (minutes * 60));

        // Update time text
        donothonTimer.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void StartTimer()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            startButtonText.text = "START";
        }
        else
        {
            startButtonText.text = "PAUSE";
        }
    }
    
    public void SetTime()
    {
        string[] parts = setTimeText.text.Split(new char[] { 'h', 'm', 's' }, System.StringSplitOptions.RemoveEmptyEntries);
        int addHours = 0, addMinutes = 0, addSeconds = 0;

        if (parts.Length >= 1)
            int.TryParse(parts[0], out addHours);
        if (parts.Length >= 2)
            int.TryParse(parts[1], out addMinutes);
        if (parts.Length >= 3)
            int.TryParse(parts[2], out addSeconds);

        timer = addHours * 3600 + addMinutes * 60 + addSeconds;
        
        //Updating Text
        hours = Mathf.FloorToInt(timer / 3600);
        minutes = Mathf.FloorToInt((timer - (hours * 3600)) / 60);
        seconds = Mathf.FloorToInt(timer - (hours * 3600) - (minutes * 60));

        donothonTimer.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");
    }
    
    public void AddTime()
    {
        string[] parts = addTimeText.text.Split(new char[] { 'h', 'm', 's' }, System.StringSplitOptions.RemoveEmptyEntries);
        int addHours = 0, addMinutes = 0, addSeconds = 0;

        if (parts.Length >= 1)
            int.TryParse(parts[0], out addHours);
        if (parts.Length >= 2)
            int.TryParse(parts[1], out addMinutes);
        if (parts.Length >= 3)
            int.TryParse(parts[2], out addSeconds);

        StartCoroutine(AnimateTimerAddition(addHours * 3600 + addMinutes * 60 + addSeconds));
    }

    public void AddTimeFromString(string time)
    {
        string[] parts = time.Split(new char[] { 'h', 'm', 's' }, System.StringSplitOptions.RemoveEmptyEntries);
        int addHours = 0, addMinutes = 0, addSeconds = 0;

        if (parts.Length >= 1)
            int.TryParse(parts[0], out addHours);
        if (parts.Length >= 2)
            int.TryParse(parts[1], out addMinutes);
        if (parts.Length >= 3)
            int.TryParse(parts[2], out addSeconds);

        StartCoroutine(AnimateTimerAddition(addHours * 3600 + addMinutes * 60 + addSeconds));
    }
    
    public void SaveTimerTime()
    {
        PlayerPrefs.SetString("TIME_LEFT", timer + "");
    }

    public string GetSavedTime()
    {
        return PlayerPrefs.GetString("TIME_LEFT", "");
    }
    
    // Coroutine to animate the timer addition
    private IEnumerator AnimateTimerAddition(float timeToAdd)
    {
        float elapsedTime = 0f;
        float startTimer = timer;
        float endTimer = timer + timeToAdd;

        while (elapsedTime < animationDuration)
        {
            timer = Mathf.Lerp(startTimer, endTimer, elapsedTime / animationDuration);

            // Calculate time
            hours = Mathf.FloorToInt(timer / 3600);
            minutes = Mathf.FloorToInt((timer - (hours * 3600)) / 60);
            seconds = Mathf.FloorToInt(timer - (hours * 3600) - (minutes * 60));

            // Update time text
            donothonTimer.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

            // Interpolate text size
            float newSize = Mathf.Lerp(donothonTimer.fontSize, originalFontSize + textSizeIncrease, elapsedTime / animationDuration);
            donothonTimer.fontSize = (int)newSize;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure timer is exactly at the end value
        timer = endTimer;

        // Update time text
        donothonTimer.text = hours.ToString("00") + ":" + minutes.ToString("00") + ":" + seconds.ToString("00");

        // Reset text size
        donothonTimer.fontSize = originalFontSize;
    }
}
