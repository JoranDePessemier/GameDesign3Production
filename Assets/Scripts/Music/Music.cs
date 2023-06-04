using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Music : MonoBehaviour
{
    [SerializeField]
    private string _name;

    public string Name => _name;

    [SerializeField]
    private AudioClip _clip;

    public AudioClip Clip => _clip;

    [SerializeField]
    private float _volume;

    public float Volume => _volume;

    [SerializeField]
    private float _endTimeSeconds;

    public float EndTimeSeconds => _endTimeSeconds;

    public AudioSource source { get; set; }


}
