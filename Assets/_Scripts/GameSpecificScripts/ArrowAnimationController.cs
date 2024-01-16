using UnityEngine;
using DG.Tweening;

public class ArrowAnimationController : MonoBehaviour
{
    public Vector3 moveDirection;
    public Vector3 scaleValue;
    public float speed;

    private MeshRenderer selfRenderer;
    private Sequence animationSequence;
    private Tween idleMoveTween;
    private Tween toucheMoveTween;
    private Tween touchScaleTween;
    private Tween touchColorTween;

    private Vector3 startLocalPos;
    private Vector3 startScale;
    private Color startColor;

    private void Awake()
    {
        selfRenderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        startLocalPos = transform.localPosition;
        startScale = transform.localScale;
        startColor = selfRenderer.material.color;

        IdleArrowAnimation();
    }

    private Tween GetIdleMoveTween()
    {
        idleMoveTween = transform.DOLocalMove(moveDirection, 1)
        .SetId("idlemovetween")
        .SetEase(Ease.Linear);

        return idleMoveTween;
    }

    private Tween GetTouchMoveTween()
    {
        toucheMoveTween = transform.DOLocalMove(moveDirection, speed)
         .SetId("touchmovetween")
         .SetEase(Ease.Linear);

        return toucheMoveTween;
    }

    private Tween GetTouchScaleTween()
    {
        touchScaleTween = transform.DOScale(scaleValue, speed)
        .SetId("touchscaletween")
        .SetEase(Ease.Linear);

        return touchScaleTween;
    }

    private Tween GetTouchColorTween()
    {
        touchColorTween = selfRenderer.material.DOColor(Color.red, speed)
        .SetId("touchcolortween")
        .SetEase(Ease.Linear);

        return touchColorTween;
    }

    public void IdleArrowAnimation()
    {
        animationSequence.Kill(false);

        transform.localPosition = startLocalPos;
        transform.localScale = startScale;
        selfRenderer.material.color = startColor;

        animationSequence = DOTween.Sequence().SetAutoKill(true).SetRecyclable(true).SetLoops(-1, LoopType.Yoyo);

        animationSequence.Append(GetIdleMoveTween());
        animationSequence.Play();
    }

    public void OnTouchArrowAnimation()
    {
        animationSequence.Kill(false);

        transform.localPosition = startLocalPos;
        transform.localScale = startScale;
        selfRenderer.material.color = startColor;

        animationSequence = DOTween.Sequence().SetAutoKill(true).SetRecyclable(true).SetLoops(-1, LoopType.Yoyo);

        animationSequence.Append(GetTouchMoveTween()).Join(GetTouchScaleTween()).Join(GetTouchColorTween());
        animationSequence.Play();
    }
}
