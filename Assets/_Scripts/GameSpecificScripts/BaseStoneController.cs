using UnityEngine;
using TMPro;

public class BaseStoneController : MonoBehaviour
{
    public GameObject textObj;
    public int textCount = 1;
    public Canvas canvas;
    public GameObject frame;

    private char sign = '+';
    private int count = 1;

    public void SetText(char sign, int count)
    {
        this.sign = sign;
        this.count = count;
        this.textCount = count;
        if (sign == '+')
        {
            textCount *= 1;
        }
        else if (sign == '-')
        {
            textCount *= -1;
        }
        textObj.GetComponent<TextMeshProUGUI>().text = $"{sign}{count}";
    }

    public void SetText(int difference)
    {

        if (Mathf.Sign(difference) == 1)
        {
            sign = '+';
            count = Mathf.Abs(difference);
            textCount = count;
        }
        else if (Mathf.Sign(difference) == -1)
        {
            sign = '-';
            count = Mathf.Abs(difference);
            textCount = -count;
        }
        textObj.GetComponent<TextMeshProUGUI>().text = $"{sign}{count}";
    }

    public void TurnOffAllRenderers()
    {
        var allMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < allMeshRenderers.Length; i++)
        {
            allMeshRenderers[i].enabled = false;
        }

        GetComponent<MeshRenderer>().enabled = false;
        canvas.enabled = false;
    }

    public void TurnOnAllRenderers()
    {
        var allMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < allMeshRenderers.Length; i++)
        {
            allMeshRenderers[i].enabled = true;
        }

        GetComponent<MeshRenderer>().enabled = true;
        canvas.enabled = true;
    }
}
