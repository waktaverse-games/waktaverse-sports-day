using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using SharedLibs;
using UnityEngine;
using UnityEngine.Events;

namespace GameHeaven.UIUX
{
    [System.Serializable]
    public class UIPositionPoint
    {
        [SerializeField] private RectTransform pointTransform;
        [SerializeField] private float moveTime;
        [SerializeField] private float moveDelay;
        [SerializeField] private bool isSmooth;
        [SerializeField] private AudioClip endSfx;

        public Vector3 PointPosition => pointTransform.position;
        public float MoveTime => moveTime;
        public float MoveDelay => moveDelay;
        public bool IsSmooth => isSmooth;
        public AudioClip EndSfx => endSfx;
    }
    
    public class UIPositionMover : MonoBehaviour
    {
        [SerializeField] private RectTransform targetTransform;
        [SerializeField] private RectTransform pivotTransform;
        
        [SerializeField] private UIPositionPoint[] points;
        
        [SerializeField] private bool isScroll;
        
        [SerializeField] private UnityEvent onStartMove;
        [SerializeField] private UnityEvent onEndMove;

        [SerializeField] private AudioSource bgmPlayer;
        [SerializeField] private AudioSource sfxPlayer;

        private void Awake()
        {
            bgmPlayer.playOnAwake = false;

            bgmPlayer.volume = SoundManager.Instance.BGMVolume;
            sfxPlayer.volume = SoundManager.Instance.SFXVolume;
        }

        private void Start()
        {
            bgmPlayer.Play();
            StartCoroutine(StartMove());
        }

        private IEnumerator StartMove()
        {
            onStartMove?.Invoke();
            for (var i = 0; i < points.Length; i++)
            {
                var point = points[i];
                var isOver = false;
                StartCoroutine(MovePositionToPoint(point.PointPosition, point.MoveTime, point.MoveDelay,
                    () => { isOver = true;}, point.IsSmooth));
                yield return new WaitUntil(() => isOver);
                if (point.EndSfx)
                {
                    sfxPlayer.PlayOneShot(point.EndSfx);
                }
            }
            onEndMove?.Invoke();
        }
        
        private IEnumerator MovePositionToPoint(Vector3 point, float time, float delay, Action onEndMove, bool isSmooth = false)
        {
            var elapsedTime = 0f;
            var startPosition = targetTransform.position;
            var moveVec = isScroll ? (pivotTransform.position - point) : (point - pivotTransform.position);

            yield return new WaitForSeconds(delay);
            
            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                targetTransform.position = isSmooth
                    ? startPosition + moveVec * Mathf.SmoothStep(0, 1, elapsedTime / time)
                    : Vector3.Lerp(startPosition, startPosition + moveVec, elapsedTime / time);
                yield return null;
            }

            targetTransform.position = startPosition + moveVec;
            pivotTransform.position = point;
            onEndMove?.Invoke();
        }
    }
}