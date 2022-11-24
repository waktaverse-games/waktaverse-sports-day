using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    public enum ParticleType
    {
        Tread, Break
    }
    
    public class ParticleSpawner : MonoBehaviour
    {
        [SerializeField] private ParticlePool particlePool;

        public void PlayParticle(ParticleType type, Vector3 pos)
        {
            var particleName = type switch
            {
                ParticleType.Tread => "Tread",
                ParticleType.Break => "Break",
                _ => "Tread"
            };
            PlayParticle(particleName, pos);
        }
        public void PlayParticle(ParticleType type, Transform parent)
        {
            var particleName = type switch
            {
                ParticleType.Tread => "Tread",
                ParticleType.Break => "Break",
                _ => "Tread"
            };
            PlayParticle(particleName, parent);
        }

        private async void PlayParticle(string name, Vector3 pos)
        {
            var particle = particlePool.GetFromPool(name);
            particle.transform.position = pos;
            particle.Play();
            while (particle.isPlaying)
            {
                await Task.Yield();
            }
            particlePool.TakeToPool(name, particle);
        }
        private async void PlayParticle(string name, Transform parent)
        {
            var particle = particlePool.GetFromPool(name);
            particle.transform.SetParent(parent);
            particle.Play();
            while (particle.isPlaying)
            {
                await Task.Yield();
            }
            particle.transform.SetParent(particlePool.transform);
            particlePool.TakeToPool(name, particle);
        }
        
        // TODO: 파티클 재생이 끝나면 Pool로 Return
    }
}