using System.Collections.Generic;
using UnityEngine;

namespace Ancinia.Consts;

public class AudioStrings
{
    #region BGM
    public const string TorvusBogLoopKey = "TorvusBogLoop";
    private const string TorvusBogLoopFile = "TorvusBogLoop";
    #endregion BGM

    private Dictionary<string, AudioClip> _dict;

    public AudioStrings(SceneChanger sc)
    {
        _dict = new Dictionary<string, AudioClip>();
        var tmpAudio = new Dictionary<string, string>();
        tmpAudio.Add(TorvusBogLoopKey, TorvusBogLoopFile);

        foreach (var pair in tmpAudio)
        {
            _dict.Add(pair.Key, sc.AbAcMat.LoadAsset<AudioClip>(pair.Value));
            Object.DontDestroyOnLoad(_dict[pair.Key]);
        }
    }

    public AudioClip Get(string key)
    {
        return _dict[key];
    }
}