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

        scoreValue.text = currentScore.ToString();
    }

    public void AddPoint(int value)
    {
        currentScore += value;
        StartCoroutine(Count(currentScore, currentScore - value));
    }

    IEnumerator Count(float target, float current)
    {
        float duration = 0.5f;
        float offset = (target - current) / duration;

        Debug.Log("offset = " + offset);

        while (current < target)
        {
            current += offset * Time.deltaTime;
            scoreValue.text = Mathf.Ceil(current).ToString();
            yield return null;
        }

        current = target;
        scoreValue.text = current.ToString();
    }

    public void ResetPoint()
    {
        currentScore = 0;
        scoreValue.text = currentScore.ToString();
    }

    public void UpdateCombo(int combo, int moves)
    {
        comboCount = combo;
        if (comboCount > 1) {
            ComboObject.GetComponent<Text>().text = "+" + (comboCount - 1).ToString();
            ComboObject.SetActive(true);

            anim = ComboObject.GetComponent<Animator>();
            anim.Play("Anim_Combo_Pop");

            StartCoroutine("ComboFade");
        }
        else if (comboCount == 0)
        {            
            ComboObject.GetComponent<Text>().text = ":(";
            ComboObject.SetActive(true);

            anim = ComboObject.GetComponent<Animator>();
            anim.SetTrigger("Pop");
            StartCoroutine("ComboFade");
        }

        moveValue.text = moves.ToString();
    }

    IEnumerator ComboFade()
    {
        yield return new WaitForSeconds(1f);
        ComboObject.SetActive(false);
    }

}
