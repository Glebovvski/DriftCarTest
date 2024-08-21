using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Data", menuName = "Audio/AudioData", order = 1)]
public class AudioData : ScriptableObject
{
    public List<SoundData> SoundData;
    public List<MusicData> MusicData;
}

[Serializable]
public class SoundData
{
    public Sounds Sounds;
    public AudioClip Clip;
}

[Serializable]
public class MusicData
{
    public Music Music;
    public AudioClip Clip;
}
