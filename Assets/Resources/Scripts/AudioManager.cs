using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Audio manager that loads in all sounds from the Audio folder. Use the file names as arguments to play.
/// Requires an AudioChannel prefab with an audio source component in it.
/// </summary>
public class AudioManager : Singleton<AudioManager>
{

    // Use this to mute game during production
    public bool mute;

    private AudioSource musicChannel;
    private AudioSource soundChannel;
    private Dictionary<string, AudioClip> soundMap;

    protected override void Awake()
    {
        base.Awake();

        soundMap = new Dictionary<string, AudioClip>();

        musicChannel = Instantiate(Resources.Load<GameObject>("Prefabs/AudioChannel")).GetComponent<AudioSource>();
        musicChannel.transform.SetParent(transform);
        musicChannel.loop = true;
        soundChannel = Instantiate(Resources.Load<GameObject>("Prefabs/AudioChannel")).GetComponent<AudioSource>();
        soundChannel.transform.SetParent(transform);

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio");
        foreach (AudioClip clip in clips)
        {
            soundMap.Add(clip.name, clip);
        }

        ToggleMute(mute);

        //Kick off initial theme here
        //PlayMusicWithIntro("exampleIntro", "exampleLoop", .25f);
    }

    public void PlayMusic(string name, float volume)
    {
        musicChannel.clip = soundMap[name];
        musicChannel.volume = volume;
        musicChannel.Play();
    }

    public void PlayMusicWithIntro(string introName, string loopName, float volume)
    {
        PlayMusic(introName, volume);
        StartCoroutine(PlayMusicDelayed(loopName, volume, musicChannel.clip.length));
    }

    private IEnumerator PlayMusicDelayed(string name, float volume, float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        PlayMusic(name, volume);
    }

    public void PlaySound(string name)
    {
        AudioClip clip = soundMap[name];
        soundChannel.PlayOneShot(soundMap[name]);
    }

    public void PlaySound(string name, float volume)
    {
        soundChannel.PlayOneShot(soundMap[name], volume);
    }

    public void ToggleMute(bool mute)
    {
        if (mute)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

}
