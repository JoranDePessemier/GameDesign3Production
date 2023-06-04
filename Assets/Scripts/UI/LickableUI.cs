using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LickableUI : MonoBehaviour
{
    private PlayerLicking _lickScript;

    [SerializeField]
    private Image _lickingImage;

    [SerializeField]
    private float _tweeningTime;

    private void Awake()
    {
        _lickScript = FindObjectOfType<PlayerLicking>();
        ResetIcon(this,EventArgs.Empty);
    }

    private void OnEnable()
    {
        _lickScript.StartedHolding += SetIcon;
        _lickScript.StoppedHolding += ResetIcon;
    }

    private void OnDisable()
    {
        _lickScript.StartedHolding -= SetIcon;
        _lickScript.StoppedHolding -= ResetIcon;
    }

    private void SetIcon(object sender, EventArgs e)
    {
        _lickingImage.sprite = _lickScript.HoldingObject.UIIcon;
        LeanTween.scale(_lickingImage.gameObject, Vector2.one, _tweeningTime).setEase(LeanTweenType.easeOutElastic);
    }

    private void ResetIcon(object sender, EventArgs e)
    {
        LeanTween.scale(_lickingImage.gameObject, Vector2.zero, _tweeningTime).setEase(LeanTweenType.easeInBack);
    }
}
