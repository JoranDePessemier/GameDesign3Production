using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[System.Serializable]
public class Music
{
    [SerializeField]
    private string _name;

    public string Name => _name;

    [SerializeField]
    private AudioClip _clip;

    public AudioClip Clip => _clip;

    [SerializeField]
    [Range(0f, 1f)]
    private float _volume = 1f;

    public float Volume => _volume;

    [SerializeField]
    private float _endTimeSeconds;

    [SerializeField]
    private bool _looping;

    public bool Looping => _looping;

    public float EndTimeSeconds => _endTimeSeconds;

    public AudioSource source { get; set; }


}
