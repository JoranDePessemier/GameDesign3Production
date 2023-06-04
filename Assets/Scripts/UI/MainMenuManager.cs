using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private float _fadeTime = 1f;

    [SerializeField]
    private float _scaleTime;

    private void Start()
    {
        SetTimeScale(0f);
    }

    private void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void FadeInObject(GameObject gameObject)
    {
        LeanTween.alpha(gameObject, 1, _fadeTime).setEase(LeanTweenType.easeInOutSine);
    }

    public void FadeOutObject(GameObject gameObject)
    {
        LeanTween.alpha(gameObject, 0, _fadeTime).setEase(LeanTweenType.easeInOutSine);
    }

    public void ScaleDownObject(GameObject gameObject)
    {
        LeanTween.scale(gameObject,Vector3.zero,_scaleTime).setEase(LeanTweenType.easeInBack);
    }

    public void ScaleUpObject(GameObject gameObject)
    {
        LeanTween.scale(gameObject, Vector3.one, _scaleTime).setEase(LeanTweenType.easeOutBack);
    }


}
