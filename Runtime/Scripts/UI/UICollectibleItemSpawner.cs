using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RoyoGames.UI
{
    public class UICollectibleItemSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private UICollectibleItemObject itemPrefab;
        [SerializeField] private int maxSpawnCount = 20;
        [SerializeField] private float spawnDuration = 0.5f;
        [SerializeField] private float spawnRadius = 0;

        [Header("Spray Animation Settings")]
        [SerializeField] private SprayDirections sprayDirection = SprayDirections.Random;
        [SerializeField] private float sprayDuration = 1;
        [SerializeField] private float minSprayRadius = 50;
        [SerializeField] private float maxSprayRadius = 100;
        [SerializeField] private float sprayConeAngle = 90;

        [SerializeField]
        private AnimationCurve sprayCurve = new AnimationCurve(
            new Keyframe(0, 0, 0, 2),
            new Keyframe(1, 1, 0, 0));


        [Header("Move Animation Settings")]
        [SerializeField] private RectTransform target;
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private SpringDirections springDirection;

        [SerializeField]
        private AnimationCurve springCurve = new AnimationCurve(
            new Keyframe(0, 0, 0, 5),
            new Keyframe(0.5f, 1, 0, 0),
            new Keyframe(1, 0, -5, 0));

        [SerializeField] private float minSpringAmplitude = -100;
        [SerializeField] private float maxSpringAmplitude = 100;

        [SerializeField]
        private AnimationCurve moveCurve = new AnimationCurve(
            new Keyframe(0, 0, 0, 2),
            new Keyframe(1, 1, 0, 0));

        public SprayDirections SprayDirection
        {
            get => sprayDirection;
            set => sprayDirection = value;
        }

        public float SprayDuration
        {
            get => sprayDuration;
            set => sprayDuration = value;
        }

        public float MinSprayRadius
        {
            get => minSprayRadius;
            set => minSprayRadius = value;
        }

        public float MaxSprayRadius
        {
            get => maxSprayRadius;
            set => maxSprayRadius = value;
        }

        public float SprayConeAngle
        {
            get => sprayConeAngle;
            set => sprayConeAngle = value;
        }

        public AnimationCurve SprayCurve
        {
            get => sprayCurve;
            set => sprayCurve = value;
        }

        public RectTransform Target
        {
            get => target;
            set => target = value;
        }

        public float MoveDuration
        {
            get => moveDuration;
            set => moveDuration = value;
        }

        public SpringDirections SpringDirection
        {
            get => springDirection;
            set => springDirection = value;
        }

        public AnimationCurve SpringCurve
        {
            get => springCurve;
            set => springCurve = value;
        }

        public float MinSpringAmplitude
        {
            get => minSpringAmplitude;
            set => minSpringAmplitude = value;
        }

        public float MaxSpringAmplitude
        {
            get => maxSpringAmplitude;
            set => maxSpringAmplitude = value;
        }

        public AnimationCurve MoveCurve
        {
            get => moveCurve;
            set => moveCurve = value;
        }

        public bool IsPlaying { get; private set; }

        protected Stack<UICollectibleItemObject> pool = new Stack<UICollectibleItemObject>();
        protected List<UICollectibleItemObject> spawnedItems = new List<UICollectibleItemObject>();

        private Action<UICollectibleItemObject> onArrivedItemEvent;
        private Action<UICollectibleItemObject> onSpawnedItemEvent;
        private Action onCompletedEvent;

        public enum SprayDirections
        {
            None,
            Random,
            Up,
            Down,
            Left,
            Right
        }

        public enum SpringDirections
        {
            Horizontal,
            Vertical
        }

        protected virtual void Awake()
        {
            PreLoad();
        }

        protected virtual void PreLoad()
        {
            for (int i = 0; i < maxSpawnCount; i++)
            {
                UICollectibleItemObject item = Instantiate(itemPrefab, transform);
                DespawnRaw(item);
            }
        }

        protected virtual void DespawnRaw(UICollectibleItemObject item)
        {
            item.transform.localPosition = Vector3.zero;
            item.gameObject.SetActive(false);
            pool.Push(item);
        }

        public virtual void OnSpawnedItem(Action<UICollectibleItemObject> onSpawnedItem)
        {
            onSpawnedItemEvent = onSpawnedItem;
        }

        public virtual void OnArrivedItem(Action<UICollectibleItemObject> onArrivedItem)
        {
            onArrivedItemEvent = onArrivedItem;
        }

        public virtual void OnCompleted(Action onCompleted)
        {
            onCompletedEvent = onCompleted;
        }

        public virtual int Play(int spawnCount)
        {
            if (IsPlaying)
                Stop();

            IsPlaying = true;
            spawnCount = Mathf.Min(spawnCount, maxSpawnCount);

            StartCoroutine(IPLay(spawnCount));

            return spawnCount;
        }

        protected IEnumerator IPLay(int particleCount)
        {
            WaitForSeconds spawnDelay = new WaitForSeconds(spawnDuration / particleCount);

            for (int i = 0; i < particleCount; i++)
            {
                UICollectibleItemObject item = Spawn();
                item.transform.position = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle.normalized * spawnRadius;
                item.Play(this);
                onSpawnedItemEvent?.Invoke(item);
                yield return spawnDelay;
            }
        }

        public IEnumerator WaitForCompletion()
        {
            while (IsPlaying)
            {
                yield return null;
            }
        }

        protected virtual UICollectibleItemObject Spawn()
        {
            UICollectibleItemObject item = pool.Pop();
            item.gameObject.SetActive(true);
            spawnedItems.Add(item);
            item.transform.SetAsFirstSibling();
            return item;
        }

        internal void Despawn(UICollectibleItemObject item)
        {
            spawnedItems.Remove(item);
            DespawnRaw(item);

            if(spawnedItems.Count == 0)
            {
                IsPlaying = false;
                onCompletedEvent?.Invoke();
            }
        }

        public virtual void Stop()
        {
            if (!IsPlaying)
                return;

            for (int i = 0; i < spawnedItems.Count; i++)
            {
                UICollectibleItemObject item = spawnedItems[i];
                DespawnRaw(item);
            }

            spawnedItems.Clear();
            IsPlaying = false;

            StopAllCoroutines();
        }

        internal void ArrivedItem(UICollectibleItemObject item)
        {
            onArrivedItemEvent?.Invoke(item);
        }
    }
}