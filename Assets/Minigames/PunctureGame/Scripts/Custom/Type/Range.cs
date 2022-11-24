using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameHeaven.PunctureGame
{
    [Serializable]
    public sealed class Range
    {
        [SerializeField] [HorizontalGroup] private float min;
        [SerializeField] [HorizontalGroup] private float max;

        public float Min => min;
        public float Max => max;

        public (float min, float max) GetRange()
        {
            return (min, max);
        }
    }

    [Serializable]
    public sealed class IntRange
    {
        [SerializeField] [HorizontalGroup] private int min;
        [SerializeField] [HorizontalGroup] private int max;

        public int Min => min;
        public int Max => max;

        public (int min, int max) GetRange()
        {
            return (min, max);
        }
    }

    [Serializable]
    public sealed class PingPongRange
    {
        [SerializeField] [BoxGroup("PingPong Range")] [HorizontalGroup("PingPong Range/Value")]
        private int min;

        [SerializeField] [BoxGroup("PingPong Range")] [HorizontalGroup("PingPong Range/Value")]
        private int max;

        [SerializeField] [BoxGroup("PingPong Range")] [ShowIf(nameof(startCustomIndex))]
        private int startIndex;

        [SerializeField] [BoxGroup("PingPong Range")] [ReadOnly]
        private int pingPongValue;

        [SerializeField] private bool startCustomIndex;
        [SerializeField] private bool isReversed;

        private bool isInitialized;

        public int Min => min;
        public int Max => max;

        public (int min, int max) GetRange()
        {
            return (min, max);
        }

        public int GetPingPongValue()
        {
            if (!isInitialized)
            {
                isInitialized = true;
                pingPongValue = startCustomIndex ? startIndex : isReversed ? max - 1 : min;

                return pingPongValue;
            }

            pingPongValue += isReversed ? -1 : 1;

            if ((!isReversed && pingPongValue >= max - 1) || (isReversed && pingPongValue <= min))
                isReversed = !isReversed;

            return pingPongValue;
        }

        private int AddIndex()
        {
            return isReversed ? -1 : 1;
        }
    }
}