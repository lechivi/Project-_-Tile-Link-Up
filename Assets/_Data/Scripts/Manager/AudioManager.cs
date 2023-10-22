using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager<AudioManager>
{
    private float bgmFadeSpeedRate = CONST.BGM_FADE_SPEED_RATE_HIGH;
    //Next BGM name, SE name
    private string nextBGMName;
    private string nextSFXName;

    //Is the highlightBackground music fading out?
    private bool isFadeOut = false;

    //Separate audio sources for BGM and SE
    public AudioSource AttachBGMSource;
    public AudioSource AttachSFXSource;

    //Keep All Audio
    private Dictionary<string, AudioClip> bgmDic, sfxDic;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.AttachBGMSource == null )
            this.AttachBGMSource = transform.Find("BgmSource").GetComponent<AudioSource>();   
        
        if (this.AttachSFXSource == null )
            this.AttachSFXSource = transform.Find("SfxSource").GetComponent<AudioSource>();
    }
    protected override void Awake()
    {
        base.Awake();
        //Load all SE & BGM files from resource folder
        this.bgmDic = new Dictionary<string, AudioClip>();
        this.sfxDic = new Dictionary<string, AudioClip>();

        object[] bgmList = Resources.LoadAll("Audio/BGM");
        object[] sfxList = Resources.LoadAll("Audio/SFX");

        foreach (AudioClip bgm in bgmList)
        {
            this.bgmDic[bgm.name] = bgm;
        }
        foreach (AudioClip sfx in sfxList)
        {
            this.sfxDic[sfx.name] = sfx;
        }
    }

    private void Start()
    {
        this.AttachBGMSource.volume = PlayerPrefs.GetFloat(CONST.BGM_VOLUME_KEY, CONST.BGM_VOLUME_DEFAULT);
        this.AttachSFXSource.volume = PlayerPrefs.GetFloat(CONST.SFX_VOLUME_KEY, CONST.SFX_VOLUME_DEFAULT);
    }

    public void PlaySFX(AudioClip audio)
    {
        this.AttachSFXSource.PlayOneShot(audio);
    }

    public void PlaySFX(string sfxName, float delay = 0.0f)
    {
        if (!this.sfxDic.ContainsKey(sfxName))
        {
            Debug.Log(sfxName + "There is no SFX named");
            return;
        }

        this.nextSFXName = sfxName;
        Invoke("DelayPlaySFX", delay);
    }

    private void DelayPlaySFX()
    {
        this.AttachSFXSource.PlayOneShot(this.sfxDic[nextSFXName] as AudioClip);
    }

    public void PlayBGM(string bgmName, float fadeSpeedRate = CONST.BGM_FADE_SPEED_RATE_HIGH)
    {
        if (!this.bgmDic.ContainsKey(bgmName))
        {
            Debug.Log(bgmName + "There is no BGM named");
            return;
        }

        //If BGM is not currently playing, play it as is
        if (!this.AttachBGMSource.isPlaying)
        {
            this.nextBGMName = "";
            this.AttachBGMSource.clip = this.bgmDic[bgmName] as AudioClip;
            this.AttachBGMSource.Play();
        }
        //When a different BGM is playing, fade out the BGM that is playing before playing the next one.
        //Through when the same BGM is playing
        else if (this.AttachBGMSource.clip.name != bgmName)
        {
            this.nextBGMName = bgmName;
            FadeOutBGM(fadeSpeedRate);
        }

    }

    public void FadeOutBGM(float fadeSpeedRate = CONST.BGM_FADE_SPEED_RATE_LOW)
    {
        this.bgmFadeSpeedRate = fadeSpeedRate;
        this.isFadeOut = true;
    }

    private void Update()
    {
        if (!this.isFadeOut)
        {
            return;
        }

        //Gradually lower the volume, and when the volume reaches 0
        //return the volume and play the next song
        this.AttachBGMSource.volume -= Time.deltaTime * this.bgmFadeSpeedRate;
        if (this.AttachBGMSource.volume <= 0)
        {
            this.AttachBGMSource.Stop();
            this.AttachBGMSource.volume = PlayerPrefs.GetFloat(CONST.BGM_VOLUME_KEY, CONST.BGM_VOLUME_DEFAULT);
            this.isFadeOut = false;

            if (!string.IsNullOrEmpty(this.nextBGMName))
            {
                PlayBGM(this.nextBGMName);
            }
        }
    }

    public void ChangeBGMVolume(float BGMVolume)
    {
        this.AttachBGMSource.volume = BGMVolume;
        PlayerPrefs.SetFloat(CONST.BGM_VOLUME_KEY, BGMVolume);
    }

    public void ChangeSFXVolume(float SFXVolume)
    {
        this.AttachSFXSource.volume = SFXVolume;
        PlayerPrefs.SetFloat(CONST.SFX_VOLUME_KEY, SFXVolume);
    }
}
