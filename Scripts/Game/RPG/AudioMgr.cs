using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Esa;
public class AudioMgr : Singleton<AudioMgr>
{
    public string path { get { _path = Application.streamingAssetsPath + folder + clipName; return _path; } }
    public string _path;
    public string folder = @"\Wav\BGM\";
    public string clipName = "Town";
    public string prefix = ".wav";
    public float volumeBGM = 0.75f;
    public float volumeSound = 1f;
    AudioSource[] src;
    public float volumeTotal = 0.75f;
    public bool volumeTotalOn = true;
    public bool playOnAwake = true;
    [Button]
    void SetupSource()
    {
        gameObject.AddComponent<AudioSource>();
        gameObject.AddComponent<AudioSource>();
    }
    void Start()
    {
        var btns = TransTool.GetComsScene<UnityEngine.UI.Button>();
        foreach (var btn in btns)
        {
            btn.onClick.AddListener(() => PlaySound("Click"));
        }

        src = GetComponents<AudioSource>();
        if (playOnAwake) PlayBGM(clipName);
    }
    public void PlayBGM(string name)
    {
        folder = @"\Wav\BGM\";
        StartCoroutine(PlayClip(0, name));
    }
    public void PlaySound(string name)
    {
        if (Battle.on && name == "Click") name += "2";
        folder = @"\Wav\Sound\";
        StartCoroutine(PlayClip(1, name));
    }
    IEnumerator PlayClip(int i, string name)
    {
        clipName = name + prefix;
        WWW w = new WWW(path);
        while (!w.isDone)
        {
            yield return 0;
        }
        src[i].clip = w.GetAudioClip();
        src[i].Play();
    }
    // Update is called once per frame
    void Update()
    {
        src[0].volume = (volumeTotalOn ? 1 : 0) * volumeTotal * volumeBGM;
        src[1].volume = (volumeTotalOn ? 1 : 0) * volumeTotal * volumeSound;
    }
}
