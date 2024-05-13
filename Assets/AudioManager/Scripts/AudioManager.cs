using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Playback mode for the background audio.
    /// </summary>
    public enum PlaybackMode
    {
        Chronologically,
        Randomized
    }

    /// <summary>
    /// Struct containing information about audio files.
    /// </summary>
    [Serializable]
    public struct AudioFile
    {
        public string Name;
        public AudioClip Clip;
    }


    #region Public bg
    [Header("Background music")]
    [Space]

    [SerializeField]
    private AudioSource _backgroundAudioSource;

    [Space]
    [Tooltip("Playback mode for the background audio. Chronologically will play the audioclips in the collection's order. Randomized will randomize the audioclip order.")]
    [SerializeField]
    private PlaybackMode _playbackMode;

    [Tooltip("Time between two audio clips in seconds.")]
    [SerializeField, Range(0, 60)]
    private uint _interval;

    [Space]
    [SerializeField]
    private AudioFile[] _backgroundMusic;
    #endregion

    #region Private bg
    private Queue<AudioFile> _backgroundMusicQueue = new Queue<AudioFile>();

    private bool _inInterval;
    #endregion



    #region Public sfx
    [Header("Sound fx")]
    [SerializeField]
    private AudioSource _soundfxAudioSource;

    [SerializeField]
    private AudioFile[] _sfx;
    #endregion




    #region Background music
    /// <summary>
    /// Sets the volume to the given value.
    /// </summary>
    /// <param name="volume">Volume value reaching from 0 to 100.</param>
    public void SetBackgroundVolume(uint volume) => SetVolume(volume, _backgroundAudioSource);

    /// <summary>
    /// Returns the volume value.
    /// </summary>
    /// <returns>Volume value.</returns>
    public float GetBackgroundVolume() => _backgroundAudioSource.volume;

    // Remove this and make it a generic function: ExecuteAfterDelay(interval, function)
    IEnumerator PlayNextBackgroundClipAfterDelay()
    {
        yield return new WaitForSeconds(_interval);
        PlayNextBackgroundAudioClip();
    }

    /// <summary>
    /// Plays the next audio clip in the queue.
    /// </summary>
    public void PlayNextBackgroundAudioClip()
    {
        AudioFile file = _backgroundMusicQueue.Dequeue();
        _backgroundMusicQueue.Enqueue(file);

        _backgroundAudioSource.clip = file.Clip;
        _backgroundAudioSource.Play();
        _inInterval = false;
    }
    #endregion


    #region Soundfx
    /// <summary>
    /// Sets the volume to the given value.
    /// </summary>
    /// <param name="volume">Volume value reaching from 0 to 100.</param>
    public void SetSFXVolume(uint volume) => SetVolume(volume, _soundfxAudioSource);

    /// <summary>
    /// Returns the volume value.
    /// </summary>
    /// <returns>Volume value.</returns>
    public float GetSFXVolume() => _soundfxAudioSource.volume;

    /// <summary>
    /// Plays the given sfx audio clip.
    /// </summary>
    /// <param name="audioName">Name of the audio clip that should be played.</param>
    public void PlaySFX(string audioName)
    {
        AudioClip clip = GetSFXAudio(audioName);
        _soundfxAudioSource.PlayOneShot(clip);
    }

    /// <summary>
    /// Retrieves the audio clip with the given name.
    /// </summary>
    /// <param name="audioName">Name of the corresponding clip that should be returned.</param>
    /// <returns>Returns the audio clip from the audio file with the corresponding name.</returns>
    private AudioClip GetSFXAudio(string audioName) => GetAudioClip(audioName, _sfx);
    #endregion




    #region Common Helper
    /// <summary>
    /// Sets the volume for a given audio source.
    /// </summary>
    /// <param name="volume">Volume variable reaching from 0 to 100.</param>
    /// <param name="audioSource">Audio source to set the volume on.</param>
    private void SetVolume(uint volume, AudioSource audioSource)
    {
        float processedVolume = Mathf.Min(100, volume) * 0.01f;
        audioSource.volume = processedVolume;
    }

    /// <summary>
    /// Returns the AudioClip with the given name from the supplied collection.
    /// </summary>
    /// <param name="audioName">Name of the audio file.</param>
    /// <param name="audioFiles">Collection of audio files.</param>
    /// <returns>The AudioClip in case it exists. Will throw an error otherwise.</returns>
    private AudioClip GetAudioClip(string audioName, AudioFile[] audioFiles) {
        AudioFile file = Array.Find(audioFiles, audio => audio.Name == audioName);

        if (file.Equals(default(AudioFile)))
            Debug.LogError($"Audio file '{audioName}' not found. Check if the name is spelled correctly or the audioFile is placed in the right collection.");

        return file.Clip;
    }

    /// <summary>
    /// Shuffles the content of the given collection.
    /// </summary>
    /// <typeparam name="T">Type of the collection.</typeparam>
    /// <param name="list">Collection that will be shuffled.</param>
    /// <returns>A shuffled version of the collection.</returns>
    private List<T> ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
        return list;
    }
    #endregion


    /// <summary>
    /// Intialization method for the audio manager.
    /// </summary>
    private void Initialize()
    {
        if(_playbackMode == PlaybackMode.Chronologically)
        {
            foreach (AudioFile file in _backgroundMusic)
            {
                _backgroundMusicQueue.Enqueue(file);
            }
        }
        else if(_playbackMode == PlaybackMode.Randomized)
        {
            // Shuffle the list of audio files
            List<AudioFile> shuffledList = ShuffleList(new List<AudioFile>(_backgroundMusic));
            

            // Enqueue shuffled audio files
            foreach (AudioFile file in shuffledList)
            {
                _backgroundMusicQueue.Enqueue(file);
            }
        }

        PlayNextBackgroundAudioClip();
    }

    private void Update()
    {
        if (!_backgroundAudioSource.isPlaying && !_inInterval)
        {
            StartCoroutine(PlayNextBackgroundClipAfterDelay());
            _inInterval = true;
        }
    }

    public void Start()
    {
        Initialize();
    }
}
