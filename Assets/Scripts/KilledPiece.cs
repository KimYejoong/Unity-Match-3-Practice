using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KilledPiece : MonoBehaviour
{
    public bool falling;
    RectTransform rect;
    Animator anim;

    [SerializeField]
    Text EarnedPoint;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
        
    public void Initialize(Vector2 start, int earnedPoints)
    {
       
        falling = true;


        EarnedPoint.text = earnedPoints.ToString();
        rect = GetComponent<RectTransform>();
        rect.anchoredPosition = start;
        
        this.gameObject.SetActive(true);
        anim.Play("Anim_Score_Pop", -1, 0f);
        Debug.Log("CurrentAnimationTime = " + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!falling)
            return;
        
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Anim_Score_Pop") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {        
            falling = false;
            this.gameObject.SetActive(false);       
        }
    }
}
