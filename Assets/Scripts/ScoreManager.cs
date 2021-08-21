using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    Text scoreValue;
    Text comboText;

    int currentScore = 0;

    private void Start()
    {
        ResetPoint();        
    }

    public void AddPoint(int value)
    {
        currentScore += value;
        scoreValue.text = currentScore.ToString();
    }

    public void ResetPoint()
    {
        currentScore = 0;
        scoreValue.text = currentScore.ToString();
    }

}
