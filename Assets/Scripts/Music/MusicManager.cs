using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private Music _currentPlayingMusic;

    private Music _previousPlayingMusic;

    [SerializeField]
    private Music[] _musicInGame;

    [SerializeField]
    private float _fadeSpeed;

    private void Awake()
    {
        if (MusicManager.Instance != null)
        {        
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        StartNewSong();
    }

    private void StartNewSong()
    {
        _previousPlayingMusic = _currentPlayingMusic;

        while(_currentPlayingMusic == _previousPlayingMusic)
        {
            _currentPlayingMusic = _musicInGame[UnityEngine.Random.Range(0, _musicInGame.Length)];
        }

        StartCoroutine(FadeIn(_currentPlayingMusic));
        StartCoroutine(FadeOut(_previousPlayingMusic));
        StartCoroutine(CountToNewSong());
    }

    private IEnumerator CountToNewSong()
    {
        
    }

    private IEnumerator FadeOut(Music music)
    {
        while(music.source.volume > 0)
        {
            music.source.volume = Mathf.MoveTowards(music.source.volume,0, _fadeSpeed * Time.unscaledDeltaTime);
            yield return 0;
        }
    }

    private IEnumerator FadeIn(Music music)
    {
        while (music.source.volume < music.Volume)
        {
            music.source.volume = Mathf.MoveTowards(music.source.volume, music.Volume, _fadeSpeed * Time.unscaledDeltaTime);
            yield return 0;
        }
    }
}
