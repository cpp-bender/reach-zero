using UnityEngine;
using DG.Tweening;

public class StoneController : MonoBehaviour, IStoneController
{
    private Vector3 offset;
    private Vector3 velocity;
    private Vector3 startPos;
    private BoxCollider selfCollider;
    private Tween moveTween;

    private void Awake()
    {
        selfCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        velocity = Vector3.zero;
        startPos = transform.position;
    }

    private void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();
        selfCollider.enabled = false;
    }

    private void OnMouseDrag()
    {
        Vector3 desiredPos = GetMouseWorldPos() + offset;
        Vector3 currentPos = transform.position;
        transform.position = Vector3.SmoothDamp(currentPos, desiredPos, ref velocity, .1f);
    }

    private void OnMouseUp()
    {
        Vector3 origin = Camera.main.transform.position;
        var dir = GetMouseWorldPos() - Camera.main.transform.position;
        Ray ray = new Ray(origin, dir);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.transform.tag == Tags.DRAGGABLE)
            {
                var hitObject = raycastHit.transform.gameObject;
                transform.position = new Vector3(hitObject.transform.position.x, 1f, hitObject.transform.position.z);
                raycastHit.collider.enabled = false;
            }
        }
        else
        {
            DoMoveToStartPos();
        }
    }

    private void DoMoveToStartPos()
    {
        if (moveTween != null)
        {
            moveTween.Kill(false);
        }
        moveTween = transform.DOMove(startPos, .5f)
        .OnStart(delegate
        {

        })
        .OnComplete(delegate
        {
            selfCollider.enabled = true;
        })
        .Play();
    }

    private Vector3 GetMouseWorldPos()
    {
        var mouseScreenPos = Input.mousePosition;
        mouseScreenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPos);
    }
}
