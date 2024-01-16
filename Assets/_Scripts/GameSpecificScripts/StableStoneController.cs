using UnityEngine;

[SelectionBase]
public class StableStoneController : BaseStoneController
{
    private void Start()
    {
        TurnOffAlllFrames();
    }

    private void TurnOffAlllFrames()
    {
        frame.GetComponent<SphereFrameController>().SwitchRightFrame(false);
        frame.GetComponent<SphereFrameController>().SwitchLeftFrame(false);
        frame.GetComponent<SphereFrameController>().SwitchUpFrame(false);
        frame.GetComponent<SphereFrameController>().SwitchDownFrame(false);
    }
}
