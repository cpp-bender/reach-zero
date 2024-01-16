using DG.Tweening.Core.Enums;
using DG.Tweening;
using UnityEngine;

public class ReferenceManager : SingletonMonoBehaviour<ReferenceManager>
{
    public int movingStoneCount;
    public GameObject stoneParticle;

    private GridManager gridManager;
    private ParticleController particleController;

    protected override void Awake()
    {
        base.Awake();
        InitDOTween();
    }

    private void Start()
    {
        particleController = GetComponent<ParticleController>();
        stoneParticle = particleController.particleCircle.gameObject;
        gridManager = FindObjectOfType<GridManager>();
    }

    public void InitDOTween()
    {
        //Default All DOTween Global Settings
        DOTween.Init(true, true, LogBehaviour.Default);
        DOTween.defaultAutoPlay = AutoPlay.None;
        DOTween.maxSmoothUnscaledTime = .15f;
        DOTween.nestedTweenFailureBehaviour = NestedTweenFailureBehaviour.TryToPreserveSequence;
        DOTween.showUnityEditorReport = false;
        DOTween.timeScale = 1f;
        DOTween.useSafeMode = true;
        DOTween.useSmoothDeltaTime = false;
        DOTween.SetTweensCapacity(200, 50);

        //Default All DOTween Tween Settings
        DOTween.defaultAutoKill = false;
        DOTween.defaultEaseOvershootOrAmplitude = 1.70158f;
        DOTween.defaultEasePeriod = 0f;
        DOTween.defaultEaseType = Ease.Linear;
        DOTween.defaultLoopType = LoopType.Restart;
        DOTween.defaultRecyclable = false;
        DOTween.defaultTimeScaleIndependent = false;
        DOTween.defaultUpdateType = UpdateType.Normal;
    }

    public void CheckGameOver()
    {
        bool isGameComplete = true;
        bool isGameFail = true;

        //Success check
        for (int i = 0; i < gridManager.virtualGrid.Count; i++)
        {
            if (gridManager.virtualGrid[i].currentObj != null)
            {
                isGameComplete = false;
            }
        }

        //Fail check
        for (int i = 0; i < gridManager.virtualGrid.Count; i++)
        {
            if (movingStoneCount > 0 && gridManager.virtualGrid[i].currentObj != null)
            {
                isGameFail = false;
            }
        }

        if (isGameComplete)
        {
            GameManager.instance.LevelComplete();
        }

        else if(isGameFail)
        {
            StartCoroutine(particleController.FailParticle());
        }
    }
}
