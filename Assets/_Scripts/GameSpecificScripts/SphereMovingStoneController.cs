using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;

[SelectionBase]
public class SphereMovingStoneController : BaseMovingStoneController
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        frame.gameObject.SetActive(false);
    }

    protected override void OnMouseDown()
    {
        base.OnMouseDown();
    }

    protected override void OnMouseDrag()
    {
        base.OnMouseDrag();
    }

    protected override void OnMouseUp()
    {
        base.OnMouseUp();
    }

    protected override IEnumerator ExecuteDropAnimation(List<GameObject> neighbours)
    {
        gridManager.SwitchGrid(false);

        currentNeighbours = neighbours;

        Tween tween0;
        Tween tween1;
        Tween tween2;

        var particle = referenceManager.stoneParticle;

        TurnOffArrows();

        for (int i = 0; i < neighbours.Count; i++)
        {
            Vector3 oldNeighbourPos = neighbours[i].transform.position;

            tween0 = neighbours[i].transform.DOMove(transform.position, .3f)
                .SetEase(Ease.InOutBack);

            tween1 = transform.DOScale(new Vector3(2f, 2f, .5f), .2f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.InOutBack);

            tween2 = neighbours[i].transform.DOScale(Vector3.zero, .2f)
                .SetEase(Ease.InOutBack);

            int movingStoneCount = neighbours[i].GetComponent<BaseStoneController>().textCount;
            int selfCount = transform.GetComponent<BaseStoneController>().textCount;
            int difference = movingStoneCount + selfCount;

            tween0
                .OnStart(delegate
                {
                    if (difference == 0)
                    {
                        Instantiate(particle, transform.position, Quaternion.identity);
                    }

                    tween1.Play();
                    tween2.Play();
                })
                .OnComplete(delegate
                {
                    Taptic.Medium();

                    gridManager.WorldToGrid(oldNeighbourPos).currentObj = null;
                    neighbours[i].GetComponent<BaseStoneController>().TurnOffAllRenderers();
                    GetComponent<BaseStoneController>().SetText(difference);

                    if (i == neighbours.Count - 1)
                    {
                        if (difference == 0)
                        {
                            gridManager.WorldToGrid(transform.position).currentObj = null;
                            GetComponent<BaseStoneController>().TurnOffAllRenderers();
                        }
                    }
                });

            tween0.Play();

            yield return tween0.WaitForCompletion();
        }

        gridManager.SwitchGrid(true);

        referenceManager.movingStoneCount--;

        referenceManager.CheckGameOver();

        yield return null;
    }
}
