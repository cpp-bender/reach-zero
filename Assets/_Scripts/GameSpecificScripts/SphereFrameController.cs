using DG.Tweening;
using UnityEngine;

public class SphereFrameController : BaseFrameController
{
    public GameObject rightFrame;
    public GameObject leftFrame;
    public GameObject upFrame;
    public GameObject downFrame;

    public void SwitchRightFrame(bool on)
    {
        rightFrame.gameObject.SetActive(on);
    }

    public void SwitchLeftFrame(bool on)
    {
        leftFrame.gameObject.SetActive(on);
    }

    public void SwitchUpFrame(bool on)
    {
        upFrame.gameObject.SetActive(on);
    }

    public void SwitchDownFrame(bool on)
    {
        downFrame.gameObject.SetActive(on);
    }
}
