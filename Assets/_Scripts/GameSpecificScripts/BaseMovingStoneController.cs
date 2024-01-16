using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class BaseMovingStoneController : BaseStoneController
{
    public List<GameObject> arrows;

    protected List<GameObject> currentNeighbours;
    protected Vector3 offset;
    protected Vector3 mobileOffset;
    protected Vector3 velocity;
    protected Vector3 startPos;
    protected BoxCollider selfCollider;
    protected Tween moveTween;
    protected GridManager gridManager;
    protected ReferenceManager referenceManager;

    private List<VectorXZ> possibleGridPos;
    private List<GameObject> neighbours;
    private Vector3 desiredPos;
    private Vector3 snapPos;

    protected virtual void Awake()
    {
        selfCollider = GetComponent<BoxCollider>();
        snapPos = Vector3.zero;
        desiredPos = Vector3.zero;
        mobileOffset = new Vector3(-1f, 0f, 2f);
    }

    protected virtual void Start()
    {
        velocity = Vector3.zero;
        startPos = transform.position;
        gridManager = FindObjectOfType<GridManager>();
        referenceManager = FindObjectOfType<ReferenceManager>();
    }

    protected virtual void OnMouseDown()
    {
        offset = transform.position - GetMouseWorldPos();

        selfCollider.enabled = false; ;

        PlayArrowTouchAnimation();
    }

    protected virtual void OnMouseDrag()
    {
        possibleGridPos = gridManager.GetAllPossibleNeighbourGrid();

        snapPos = gridManager.GetNearestPossibleGridPos(GetMouseWorldPos() + offset + mobileOffset, possibleGridPos);

        if (snapPos == Vector3.zero)
        {
            desiredPos = GetMouseWorldPos() + offset + mobileOffset;
            desiredPos = new Vector3(desiredPos.x, 0f, desiredPos.z);
            transform.position = desiredPos;
            TurnOnArrows();
            TurnOffFrames();
        }
        else
        {
            snapPos = new Vector3(snapPos.x, 0f, snapPos.z);
            transform.position = snapPos;
            HandleArrows();
            TurnOnFrames();
        }
    }

    protected virtual void OnMouseUp()
    {
        TurnOffFrames();
        Vector3 origin = Camera.main.transform.position;
        var dir = transform.position - Camera.main.transform.position;
        Ray ray = new Ray(origin, dir);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            var hitPos = raycastHit.transform.position;
            var virtualGrid = gridManager.WorldToGrid(hitPos);
            int neighboursCount = gridManager.GetNeighboursCount(virtualGrid.x, virtualGrid.z);
            var neighbours = gridManager.GetNeighboursObject(virtualGrid.x, virtualGrid.z);

            if (raycastHit.transform.tag == Tags.DRAGGABLE && virtualGrid.currentObj == null && neighboursCount > 0)
            {
                // If slot is empty, draggable and has at least one neihgbour, put it in there
                var hitObject = raycastHit.transform.gameObject;
                transform.position = new Vector3(hitObject.transform.position.x, 0f, hitObject.transform.position.z);
                virtualGrid.SetObject(gameObject);

                StartCoroutine(ExecuteDropAnimation(neighbours));
            }
            else
            {
                DoMoveToStartPos();
            }
        }
        else
        {
            DoMoveToStartPos();
        }

        PlayArrowIdleAnimation();
    }

    protected abstract IEnumerator ExecuteDropAnimation(List<GameObject> neighbours);

    private void DoMoveToStartPos()
    {
        if (moveTween != null)
        {
            moveTween.Kill(false);
        }
        moveTween = transform.DOMove(startPos, .5f)
            .SetAutoKill(true)
            .SetRecyclable(true)
            .OnStart(delegate
            {

            })
            .OnComplete(delegate
            {
                moveTween = null;
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

    private void HandleArrows()
    {
        Vector3 diffVec = Vector3.zero;

        int x = gridManager.WorldToGrid(transform.position).x;
        int z = gridManager.WorldToGrid(transform.position).z;
        var neighbours = gridManager.GetNeighboursObject(x, z);
        var turnOnArrows = new List<GameObject>();

        for (int i = 0; i < neighbours.Count; i++)
        {
            diffVec = transform.position - neighbours[i].transform.position;
            diffVec = new Vector3(diffVec.x, 0f, diffVec.z);
            if (diffVec.x > 0f && diffVec.z == 0f)
            {
                turnOnArrows.Add(arrows[2].gameObject);
            }
            else if (diffVec.x < 0f && diffVec.z == 0f)
            {
                turnOnArrows.Add(arrows[0].gameObject);
            }
            else if (diffVec.x == 0f && diffVec.z > 0f)
            {
                turnOnArrows.Add(arrows[1].gameObject);
            }
            else if (diffVec.x == 0f && diffVec.z < 0f)
            {
                turnOnArrows.Add(arrows[3].gameObject);
            }
        }

        for (int i = 0; i < arrows.Count; i++)
        {
            if (turnOnArrows.Contains(arrows[i]))
            {
                arrows[i].gameObject.SetActive(true);
            }
            else
            {
                arrows[i].gameObject.SetActive(false);
            }
        }
    }

    private void TurnOnFrames()
    {
        TurnOffFrames();

        Vector3 diffVec = Vector3.zero;
        var closestFrames = new List<GameObject>();

        int x = gridManager.WorldToGrid(transform.position).x;
        int z = gridManager.WorldToGrid(transform.position).z;

        var neighbours = gridManager.GetNeighboursObject(x, z);

        for (int i = 0; i < neighbours.Count; i++)
        {
            diffVec = transform.position - neighbours[i].transform.position;
            diffVec = new Vector3(diffVec.x, 0f, diffVec.z);

            var currentFrame = neighbours[i].GetComponent<BaseStoneController>().frame.GetComponent<SphereFrameController>();
            currentFrame.gameObject.SetActive(true);

            if (diffVec.x > 0f && diffVec.z == 0f)
            {
                currentFrame.SwitchRightFrame(false);
                currentFrame.SwitchLeftFrame(true);
                currentFrame.SwitchUpFrame(true);
                currentFrame.SwitchDownFrame(true);
            }
            else if (diffVec.x < 0f && diffVec.z == 0f)
            {
                currentFrame.SwitchRightFrame(true);
                currentFrame.SwitchLeftFrame(false);
                currentFrame.SwitchUpFrame(true);
                currentFrame.SwitchDownFrame(true);
            }
            else if (diffVec.x == 0f && diffVec.z > 0f)
            {
                currentFrame.SwitchRightFrame(true);
                currentFrame.SwitchLeftFrame(true);
                currentFrame.SwitchUpFrame(false);
                currentFrame.SwitchDownFrame(true);
            }
            else if (diffVec.x == 0f && diffVec.z < 0f)
            {
                currentFrame.SwitchRightFrame(true);
                currentFrame.SwitchLeftFrame(true);
                currentFrame.SwitchUpFrame(true);
                currentFrame.SwitchDownFrame(false);
            }
        }
    }

    private void TurnOffFrames()
    {
        Vector3 diffVec = Vector3.zero;
        var closestFrames = new List<GameObject>();

        int x = gridManager.WorldToGrid(transform.position).x;
        int z = gridManager.WorldToGrid(transform.position).z;

        var neighbours = gridManager.GetAllGridObjects();

        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].GetComponent<BaseStoneController>().frame == null)
            {
                return;
            }

            var currentFrame = neighbours[i].GetComponent<BaseStoneController>().frame.GetComponent<SphereFrameController>();

            currentFrame.SwitchRightFrame(false);
            currentFrame.SwitchLeftFrame(false);
            currentFrame.SwitchUpFrame(false);
            currentFrame.SwitchDownFrame(false);
        }

    }

    public void TurnOffArrows()
    {
        for (int i = 0; i < arrows.Count; i++)
        {
            arrows[i].gameObject.SetActive(false);
        }
    }

    public void TurnOnArrows()
    {
        TurnOffArrows();
        for (int i = 0; i < arrows.Count; i++)
        {
            arrows[i].gameObject.SetActive(true);
        }
    }

    private void PlayArrowTouchAnimation()
    {
        for (int i = 0; i < arrows.Count; i++)
        {
            arrows[i].GetComponent<ArrowAnimationController>().OnTouchArrowAnimation();
        }
    }

    private void PlayArrowIdleAnimation()
    {
        for (int i = 0; i < arrows.Count; i++)
        {
            arrows[i].GetComponent<ArrowAnimationController>().IdleArrowAnimation();
        }
    }
}
