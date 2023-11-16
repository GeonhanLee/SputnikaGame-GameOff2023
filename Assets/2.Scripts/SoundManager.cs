using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private float _masterVolumeLerpSpeed = 2f;
    [SerializeField, Range(0,1)] private float _minMasterVolume = 0.3f;
    [SerializeField] private AudioSource _BGM;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private AudioSource _mergeSound;

    private float _bgmInitialVolume;
    private float _shootInitialVolume;
    private float _mergeInitialVolume;

    private float _masterVolume;
    [SerializeField] private float _masterTargetVolume;

    //Singleton
    private static SoundManager _instance;
    public static SoundManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<SoundManager>();
            }
            return _instance;
        }
    }
    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    private void Start()
    {
        _masterVolume = 1f;
        _masterTargetVolume = 1f;

        _bgmInitialVolume = _BGM.volume;
        _shootInitialVolume = _shootSound.volume;
        _mergeInitialVolume = _mergeSound.volume;
    }

    private void Update()
    {
        _masterVolume = Mathf.Lerp(_masterVolume, _masterTargetVolume, Time.unscaledDeltaTime * _masterVolumeLerpSpeed);

        _BGM.volume = _bgmInitialVolume * _masterVolume;
        _shootSound.volume = _shootInitialVolume * _masterVolume;
        _mergeSound.volume = _mergeInitialVolume * _masterVolume;
    }

    public void SetMasterVolume(float vol)
    {
        _masterTargetVolume = Mathf.Clamp(vol, _minMasterVolume, 1);
    }

    public void PlayShootSound()
    {
        _shootSound.Play();
    }
    public void PlayMergeSound()
    {
        _mergeSound.Play();
    }
}
