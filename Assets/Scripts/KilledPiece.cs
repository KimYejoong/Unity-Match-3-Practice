using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KilledPiece : MonoBehaviour
{
    public bool falling;

    float speed = 16f;
    float gravity = 32f;
    Vector2 moveDir;
    RectTransform rect;
    Image img;
    Animator anim;

    [SerializeField]
    Text EarnedPoint;

        
    public void Initialize(Vector2 start, int earnedPoints)
    {
       
        falling = true;

        moveDir = Vector2.up;
        moveDir.x = Random.Range(-1.0f, 1.0f);
        moveDir *= speed / 2;

        EarnedPoint.text = earnedPoints.ToString();
        //img = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
        //img.sprite = piece;
        rect.anchoredPosition = start;

        anim = GetComponent<Animator>();
        anim.Play("Anim_Score_Pop", -1, 0f);
        Debug.Log("CurrentAnimationTime = " + anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        this.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!falling)
            return;
        //moveDir.y -= Time.deltaTime * gravity;
        //moveDir.x = Mathf.Lerp(moveDir.x, 0, Time.deltaTime);
        //rect.anchoredPosition += moveDir * Time.deltaTime * speed;
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Anim_Score_Pop") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) {
        // if (rect.position.x < -32f || rect.position.x > Screen.width + 32f || rect.position.y < -32f || rect.position.y > Screen.height + 32f)
            falling = false;
            this.gameObject.SetActive(false);
            //Debug.Log("Animation End");

        }
    }
}
