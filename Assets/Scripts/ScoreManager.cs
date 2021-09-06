using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    [Header("UI Elements")]
    [SerializeField]
    Text scoreValue;
    [SerializeField]
    Text moveValue;
    [SerializeField]
    GameObject ComboObject;

    Animator anim;

    int currentScore = 0;
    int comboCount = 0;

    private void Start()
    {
        ComboObject.SetActive(false);
        ResetPoint();
    }

    void Update()
    {        
    }

    public void AddPoint(int value)
    {
        currentScore += value;
        StartCoroutine(Count(currentScore, currentScore - value));
    }

    IEnumerator Count(float targetValue, float currentValue)
    {
        float duration = 0.4f; // time required to count
        float offset = (targetValue - currentValue) / duration;

        // Debug.Log("offset = " + offset);

        while (currentValue < targetValue)
        {
            currentValue += offset * Time.deltaTime;
            SetScoreText(currentValue);
            yield return null;
        }

        currentValue = targetValue;
        SetScoreText(currentValue);
    }

    public void ResetPoint()
    {
        currentScore = 0;
        SetScoreText(currentScore);
    }

    private void SetScoreText(float value)
    {
        scoreValue.text = string.Format("{0:#,###}", Mathf.Ceil(value));
    }


    public void UpdateCombo(int combo, int moves)
    {
        comboCount = combo;

        ComboObject.GetComponent<Text>().text = (comboCount > 0) ? "+" + comboCount.ToString() : ":(";
        ComboObject.SetActive(true);
        
        anim = ComboObject.GetComponent<Animator>();
        anim.Play("Anim_Combo_Pop", -1, 0f);
        
        StartCoroutine("ComboFade");        

        moveValue.text = moves.ToString();
    }

    IEnumerator ComboFade()
    {
        yield return new WaitForSeconds(1f); // wait for 1s before disappearing
        ComboObject.SetActive(false);
    }

}
