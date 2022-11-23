using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public class ParticleSpawner : DisposableSingleton<ParticleSpawner>
    {
        protected override void Initialize()
        {
            
        }
        
        // TODO: 파티클 재생이 끝나면 Pool로 Return
    }
}