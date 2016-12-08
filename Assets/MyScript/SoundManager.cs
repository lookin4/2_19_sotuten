using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{


    public enum BGM
    {
        Main = 0,
    }


    public enum SE
    {
        Shoot = 0,
        Crash,
        Score,
    }


    static SoundManager instance_;
    static public SoundManager Instance
    {
        get { return instance_; }
    }


    [SerializeField]
    AudioSource[] bgmSources;

    [SerializeField]
    AudioSource[] seSources;


    //List<AudioSource> sources = new List<AudioSource>();


    void Awake()
    {
        instance_ = this;
    }


    void Start()
    {
        //AudioSource[] audioSources = GetComponents<AudioSource>();

        //for (int i = 0; i < audioSources.Length; i++)
        //{
        //    sources.Add(audioSources[i]);

        //}

    }


    void Update()
    {

    }


    public float GetBgmTime(BGM bgm)
    {
        return bgmSources[(int)bgm].time;
    }


    public void PlaySE(SE se)
    {
        seSources[(int)se].Play();

        //// int -> enum
        //SE s = (SE)System.Enum.ToObject(typeof(SE), 1);

    }


    public void PlayBGM(BGM bgm)
    {
        bgmSources[(int)bgm].Play();

    }


}
