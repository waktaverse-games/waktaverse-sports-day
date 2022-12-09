using System.Threading.Tasks;
using UnityEngine;

public static class SoundUtil
{
    public static async Task CrossFadePlay(this AudioSource source, AudioClip targetClip, float changeSpeed, float delay = 0.0f)
    {
        if (source == null) return;
        if (!source.isPlaying) return;
        if (targetClip == null) return;

        var targetVolume = source.volume;
        var halfChangeSpeed = changeSpeed / 2.0f;

        while (source.volume > 0.02f)
        {
            source.volume -= halfChangeSpeed * Time.deltaTime;
            await Task.Yield();
        }
        source.Stop();
        if (delay > 0.0f) await Task.Delay((int) (delay * 1000));
        source.clip = targetClip;
        source.Play();
        while (source.volume < targetVolume)
        {
            source.volume += halfChangeSpeed * Time.deltaTime;
            await Task.Yield();
        }
        source.volume = targetVolume;
    }
}