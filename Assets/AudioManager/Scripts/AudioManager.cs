using System;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public enum PlaybackMode
    {
        Chronologically,
        Randomized
    }

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
    private Queue<AudioFile> _backgroudMusicQueue;
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

    public void PlaySFX(string audioName)
    {
        AudioClip clip = GetSFXAudio(audioName);
        _soundfxAudioSource.PlayOneShot(clip);
    }

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
    #endregion
}
