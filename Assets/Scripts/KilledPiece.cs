using UnityEngine;
using UnityEngine.UI;

public class KilledPiece : MonoBehaviour
{
    public bool falling;
    private RectTransform _rect;
    private Animator _anim;

    [SerializeField]
    private Text earnedPoint;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }
        
    public void Initialize(Vector2 start, int earnedPoints)
    {
        falling = true;

        earnedPoint.text = earnedPoints.ToString();
        _rect = GetComponent<RectTransform>();
        _rect.anchoredPosition = start;
        
        this.gameObject.SetActive(true);
        _anim.Play("Anim_Score_Pop", -1, 0f);
        //Debug.Log("CurrentAnimationTime = " + _anim.GetCurrentAnimatorStateInfo(0).normalizedTime);
    }

    // Update is called once per frame
    private void Update()
    {
        if (!falling)
            return;

        if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.Anim_Score_Pop") ||
            !(_anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f))
            return;
        
        falling = false;
        this.gameObject.SetActive(false);
    }
}
