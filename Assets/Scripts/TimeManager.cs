using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private Slider timer;
    [SerializeField] 
    private GameObject gameOverPanel;

    // Start is called before the first frame update
    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void SetTime(float timeRemaining, float timeMax)
    {
        timer.value = timeRemaining / timeMax;
    }

    public float GetTime()
    {
        return timer.value;
    }

    public void GameEnd()
    {
        gameOverPanel.SetActive(true);
    }
}
