using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[SelectionBase]
public class RectangleMovingStoneController : BaseMovingStoneController
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
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
        Tween tween3;

        var movingObjects = new List<GameObject>();
        var particle = referenceManager.stoneParticle;
        movingObjects.Add(gameObject);

        TurnOffArrows();


        for (int i = 0; i < neighbours.Count - 1; i++)
        {
            var movingObject = Instantiate(gameObject, transform.position, Quaternion.Euler(-90f, 0f, 0f));
            movingObject.GetComponent<BaseMovingStoneController>().TurnOffArrows();
            movingObjects.Add(movingObject);
        }

        for (int i = 0; i < neighbours.Count; i++)
        {
            Vector3 currentMovingObjPos = movingObjects[i].transform.position;

            var dir = -(neighbours[i].transform.position - currentMovingObjPos).normalized;
            dir = new Vector3(dir.x, 0f, dir.z);

            tween0 = movingObjects[i].transform.DOMove(dir * 10f, .3f)
            .SetEase(Ease.Linear);

            tween1 = movingObjects[i].transform.DOMove(neighbours[i].transform.position, .2f)
                        .SetEase(Ease.InBack);

            tween2 = movingObjects[i].transform.DOScale(Vector3.zero, .2f)
                .SetEase(Ease.InBack);

            tween3 = neighbours[i].transform.DOScale(new Vector3(2f, 2f, .5f), .2f)
                .SetEase(Ease.InBack)
                .SetLoops(2, LoopType.Yoyo);

            int movingStoneCount = movingObjects[i].GetComponent<BaseStoneController>().textCount;
            int currentNeighbourCount = neighbours[i].GetComponent<BaseStoneController>().textCount;
            int difference = movingStoneCount + currentNeighbourCount;

            tween0
                .OnStart(delegate
                {
                    if (difference == 0)
                    {
                        Instantiate(particle, neighbours[i].transform.position, Quaternion.identity);
                    }

                    tween1.Play();
                    tween2.Play();
                    tween3.Play();
                })
                .OnComplete(delegate
                {
                    Taptic.Medium();

                    if (difference == 0)
                    {
                        gridManager.WorldToGrid(currentMovingObjPos).currentObj = null;
                        gridManager.WorldToGrid(neighbours[i].transform.position).currentObj = null;
                        movingObjects[i].GetComponent<BaseStoneController>().TurnOffAllRenderers();
                        neighbours[i].GetComponent<BaseStoneController>().TurnOffAllRenderers();
                    }
                    else
                    {
                        gridManager.WorldToGrid(currentMovingObjPos).currentObj = null;
                        movingObjects[i].GetComponent<BaseStoneController>().TurnOffAllRenderers();
                        neighbours[i].GetComponent<BaseStoneController>().SetText(difference);
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