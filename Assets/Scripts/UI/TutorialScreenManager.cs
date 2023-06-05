using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreenManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform[] _tutorialObjects;

    [SerializeField]
    private float _fadeSpeed;

    private void Awake()
    {
        foreach(var tutorialObject in _tutorialObjects)
        {
            LeanTween.alpha(tutorialObject, 0, 0.1f).setIgnoreTimeScale(true).setEase(LeanTweenType.linear);
        }
    }

    public void FadeInObject(RectTransform rectObject)
    {
        LeanTween.alpha(rectObject, 1, _fadeSpeed).setEase(LeanTweenType.linear);
    }

    public void FadeOutObject(RectTransform rectObject)
    {
        LeanTween.alpha(rectObject, 0, _fadeSpeed).setEase(LeanTweenType.linear);
    }
}
