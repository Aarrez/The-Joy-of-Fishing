using UnityEngine;
using DG.Tweening;

public class TweeningAi : MonoBehaviour
{
    [SerializeField] private AnimationCurve animCurve;

    private void Start()
    {
        Debug.Log(animCurve.length);
        transform.DOMoveX(4, 3).SetEase(Ease.InOutBack).SetLoops(-1);
    }
}