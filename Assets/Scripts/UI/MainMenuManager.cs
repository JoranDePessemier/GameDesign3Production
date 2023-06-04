using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private float _fadeTime = 1f;

    [SerializeField]
    private float _scaleTime;

    [SerializeField]
    private float _preventButtonSpamTime = 2f;

    [SerializeField]
    private UnityEvent _startScene;

    private void Start()
    {
        SetTimeScale(0f);
        _startScene.Invoke();
    }

    public void SetTimeScale(float scale)
    {
        Time.timeScale = scale;
    }

    public void FadeInObject(RectTransform gameObject)
    {
        LeanTween.alpha(gameObject,1,_fadeTime).setEase(LeanTweenType.easeInOutSine).setIgnoreTimeScale(true);
    }

    public void FadeOutObject(RectTransform gameObject)
    {
        LeanTween.alpha(gameObject, 0, _fadeTime).setEase(LeanTweenType.easeInOutSine).setIgnoreTimeScale(true);
    }

    public void ScaleDownObjectAndReloadScene(GameObject gameObject)
    {
        LeanTween.scale(gameObject, Vector3.zero, _scaleTime).setEase(LeanTweenType.easeInBack).setIgnoreTimeScale(true).setOnComplete(ReloadScene);
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ScaleDownObject(GameObject gameObject)
    {
        LeanTween.scale(gameObject,Vector3.zero,_scaleTime).setEase(LeanTweenType.easeInBack).setIgnoreTimeScale(true);
    }

    public void ScaleUpObject(GameObject gameObject)
    {
        LeanTween.scale(gameObject, Vector3.one, _scaleTime).setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(true).setDelay(_scaleTime);
    }

    public void SetSelectedButton(GameObject selected)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if(selected != null)
        {
            StartCoroutine(PreventSpam(selected));
        }


    }

    public void NoSelectedButton()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator PreventSpam(GameObject selected)
    {
        yield return new WaitForSecondsRealtime(_preventButtonSpamTime);
        EventSystem.current.SetSelectedGameObject(selected);
    }

    public void OpenLink(string link)
    {
        Application.OpenURL(link);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void SetScaleZero(GameObject gameObject)
    {
        gameObject.LeanScale(Vector3.zero, 0).setIgnoreTimeScale(true); ;
    }

    public void SetAlphaZero(RectTransform gameObject)
    {
        LeanTween.alpha(gameObject, 0, 0).setIgnoreTimeScale(true);
    }

    public void ResetSpawn()
    {
        RespawnTracker.Instance.RespawnChangedSinceStart = false;
    }
}