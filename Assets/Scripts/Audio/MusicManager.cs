using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    private Music _currentPlayingMusic;

    private Music _previousPlayingMusic;

    [SerializeField]
    private Music[] _musicInGame;

    [SerializeField]
    private Music _menuMusic;

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
        foreach (var music in _musicInGame)
        {
            CreateSource(music);
        }
        CreateSource(_menuMusic);
        _menuMusic.source.Play();
        _currentPlayingMusic = _menuMusic;
    }

    private void CreateSource(Music music)
    {
        music.source = this.AddComponent<AudioSource>();
        music.source.volume = music.Volume;
        music.source.clip = music.Clip;
        music.source.playOnAwake = false;
        music.source.loop = music.Looping;
    }

    public void StartNewSong() => StartNewSong(false);

    public void StartNewSong(bool menuMusic)
    {
        _previousPlayingMusic = _currentPlayingMusic;

        StopCoroutine(CountToNewSong());

        if(!menuMusic)
        {
            while (_currentPlayingMusic == _previousPlayingMusic)
            {
                _currentPlayingMusic = _musicInGame[UnityEngine.Random.Range(0, _musicInGame.Length)];
            }
            StartCoroutine(CountToNewSong());
        }
        else
        {
            _currentPlayingMusic = _menuMusic;
        }


        StartCoroutine(FadeIn(_currentPlayingMusic));
        if(_previousPlayingMusic != null)
        {
            StartCoroutine(FadeOut(_previousPlayingMusic));
        }

    }

    private IEnumerator CountToNewSong()
    {
        yield return new WaitForSecondsRealtime(_currentPlayingMusic.EndTimeSeconds);
        StartNewSong();
    }

    private IEnumerator FadeOut(Music music)
    {
        while(music.source.volume > 0)
        {
            music.source.volume = Mathf.MoveTowards(music.source.volume,0, _fadeSpeed * Time.unscaledDeltaTime);
            yield return 0;
        }
        music.source.Stop();
    }

    private IEnumerator FadeIn(Music music)
    {
        music.source.Play();
        music.source.volume = 0;
        while (music.source.volume < music.Volume)
        {
            music.source.volume = Mathf.MoveTowards(music.source.volume, music.Volume, _fadeSpeed * Time.unscaledDeltaTime);
            yield return 0;
        }
    }
}
