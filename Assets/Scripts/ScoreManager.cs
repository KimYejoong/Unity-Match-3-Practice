﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    Text scoreValue;

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

    public void UpdateCombo(int combo)
    {
        comboCount = combo;
        if (comboCount > 1) {
            ComboObject.GetComponent<Text>().text = "+" + (comboCount - 1).ToString();
            ComboObject.SetActive(true);

            anim = ComboObject.GetComponent<Animator>();
            anim.SetTrigger("Pop");

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
    }

    IEnumerator ComboFade()
    {
        yield return new WaitForSeconds(1f);
        ComboObject.SetActive(false);
    }

}
