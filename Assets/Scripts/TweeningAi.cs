using UnityEngine;
using DG.Tweening;

/*
 * A test script testing animation curves and Sine curevs as movement
 */

public class TweeningAi : MonoBehaviour
{
    [SerializeField] private AnimationCurve animCurve;

    [SerializeField] private float timeDevider = 2f;

    private float theTime;

    private float currentTime;

    private float value = 0f;

    private void Start()
    {
        var lastKey = animCurve[animCurve.length - 1];
        theTime = lastKey.time;
        currentTime = theTime;
    }

    private void Update()
    {
        MoveWithAnimationCurve();
    }

    private void MoveWithAnimationCurve()
    {
        currentTime -= Time.fixedDeltaTime * 50;

        if (currentTime > 0)
        {
            value = animCurve.Evaluate(theTime - currentTime);
            transform.Translate(new Vector3(value / timeDevider, 0f));
        }
        if (currentTime <= 0)
        {
            currentTime = theTime;
        }
    }
}