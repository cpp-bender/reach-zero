using UnityEngine;
using DG.Tweening;

public class TestSphereController : MonoBehaviour
{
    public Sequence seq;

    private Tween moveTween;
    private Tween scaleTween;

    private void Start()
    {
        moveTween = transform.DOMoveY(10f, 1f);

        scaleTween = transform.DOScale(2f, 1f);

        moveTween
            .OnStart(delegate 
            {
                scaleTween.Play();
            })
            .Play();
    }
}
