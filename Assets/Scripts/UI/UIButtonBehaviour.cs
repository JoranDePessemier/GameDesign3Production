using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIButtonBehaviour : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Button button;

    [SerializeField]
    private float _scaleSize = 1.2f;

    [SerializeField]
    private float _scaleTime = 0.5f;

    public void OnDeselect(BaseEventData eventData)
    {
        LeanTween.scale(gameObject, Vector3.one, _scaleTime).setEase(LeanTweenType.easeOutBack).setIgnoreTimeScale(true);
    }


    public void OnSelect(BaseEventData eventData)
    {
        LeanTween.scale(gameObject, Vector3.one * _scaleSize, _scaleTime).setEase(LeanTweenType.easeInBack).setIgnoreTimeScale(true);
        GlobalAudioManager.Instance.PlaySound("ButtonSwoosh");
    }

    
}
