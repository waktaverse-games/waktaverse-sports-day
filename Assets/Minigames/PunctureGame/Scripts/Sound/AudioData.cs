using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    [System.Serializable]
    public class AudioData
    {
        [Required] public AudioClip clip;
        [Range(0f, 1f)] public float volume;
    }
}