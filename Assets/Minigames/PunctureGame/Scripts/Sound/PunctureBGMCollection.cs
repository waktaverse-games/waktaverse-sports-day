using SharedLibs;

namespace GameHeaven.PunctureGame
{
    public enum PunctureBGMType
    {
        Default
    }

    public class PunctureBGMCollection : BGMCollection<PunctureBGMType>
    {
        private void Start()
        {
            SetVolume(SoundManager.Instance.BGMVolume);
        }

        private void OnEnable()
        {
            SoundManager.Instance.OnBGMVolumeChanged += SetVolume;
        }

        private void OnDisable()
        {
            SoundManager.Instance.OnBGMVolumeChanged -= SetVolume;
        }
    }
}