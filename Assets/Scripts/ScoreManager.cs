using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField]
    private Text scoreValue;
    [SerializeField] 
    private Text moveValue;
    [SerializeField] 
    private GameObject comboObject;

    private Animator _anim;

    private int _currentScore = 0;
    private int _comboCount = 0;

    private void Start()
    {
        comboObject.SetActive(false);
        ResetPoint();
    }

    public void AddPoint(int value)
    {
        _currentScore += value;
        StartCoroutine(Count(_currentScore, _currentScore - value));
    }

    private IEnumerator Count(float targetValue, float currentValue)
    {
        float duration = 0.4f; // time required to count
        float offset = (targetValue - currentValue) / duration;
        
        while (currentValue < targetValue)
        {
            currentValue += offset * Time.deltaTime;
            SetScoreText(currentValue);
            yield return null;
        }

        currentValue = targetValue;
        SetScoreText(currentValue);
    }

    private void ResetPoint()
    {
        _currentScore = 0;
        SetScoreText(_currentScore);
    }

    private void SetScoreText(float value)
    {
        scoreValue.text = $"{Mathf.Ceil(value):#,###}";
    }


    public void UpdateCombo(int combo, int moves)
    {
        _comboCount = combo;

        comboObject.GetComponent<Text>().text = (_comboCount > 0) ? "+" + _comboCount.ToString() : ":(";
        comboObject.SetActive(true);
        
        _anim = comboObject.GetComponent<Animator>();
        _anim.Play("Anim_Combo_Pop", -1, 0f);
        
        StartCoroutine(ComboFade());        

        moveValue.text = moves.ToString();
    }

    private IEnumerator ComboFade()
    {
        yield return new WaitForSeconds(1f); // wait for 1s before disappearing
        comboObject.SetActive(false);
    }

}
