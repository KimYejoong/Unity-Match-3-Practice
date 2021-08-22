using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{

    [SerializeField]
    Slider timer;

    [SerializeField]
    GameObject gameOverPanel;

    float TimeElapsed;
    float TimeStarted;

    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTime(float timeElapsed, float timeMax)
    {
        timer.value = timeElapsed / timeMax;
    }

    public void GameEnd()
    {
        gameOverPanel.SetActive(true);
    }
}
