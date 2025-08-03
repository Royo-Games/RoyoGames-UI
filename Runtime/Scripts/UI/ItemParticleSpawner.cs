using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RoyoGames.UI
{
    public class ItemParticleSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private ItemParticleObject itemPrefab;
        [SerializeField] private int maxSpawnCount = 30;
        [SerializeField] private float spawnDuration = 0.7f;
        [SerializeField] private float spawnRadius = 0;

        [Header("Spray Animation Settings")]
        [SerializeField] private SprayDirections sprayDirection = SprayDirections.Random;
        [SerializeField] private float sprayDuration = 0.5f;
        [SerializeField] private float minSprayRadius = 200;
        [SerializeField] private float maxSprayRadius = 250;
        [SerializeField] private float sprayConeAngle = 90;
        [SerializeField] private CurveType sprayCurveType = CurveType.OutQuad;
        [SerializeField] private AnimationCurve sprayCurve;

        [Header("Move Animation Settings")]
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private CurveType moveCurveType = CurveType.InQuad;
        [SerializeField] private AnimationCurve moveCurve;
        [SerializeField] private SpringDirections springDirection = SpringDirections.None;
        [SerializeField] private CurveType springCurveType = CurveType.HalfSine;
        [SerializeField] private AnimationCurve springCurve;
        [SerializeField] private float minSpringAmplitude = 0;
        [SerializeField] private float maxSpringAmplitude = 0;

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

        public CurveType SprayCurveType
        {
            get => sprayCurveType;
            set
            {
                if (sprayCurveType != value)
                {
                    sprayCurve = AnimationCurveCreator.Create(value);
                    sprayCurveType = value;
                }
            }
        }

        public AnimationCurve SprayCurve
        {
            get => sprayCurve;
            set => sprayCurve = value;
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

        public CurveType SpringCurveType
        {
            get => springCurveType;
            set
            {
                if (springCurveType != value)
                {
                    springCurve = AnimationCurveCreator.Create(value);
                    springCurveType = value;
                }
            }
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

        public CurveType MoveCurveType
        {
            get => moveCurveType;
            set
            {
                if (moveCurveType != value)
                {
                    moveCurve = AnimationCurveCreator.Create(value);
                    moveCurveType = value;
                }
            }
        }

        public AnimationCurve MoveCurve
        {
            get => moveCurve;
            set => moveCurve = value;
        }

        public bool IsPlaying { get; private set; }

        protected Stack<ItemParticleObject> pool = new Stack<ItemParticleObject>();
        protected List<ItemParticleObject> spawnedItems = new List<ItemParticleObject>();

        private Action<ItemParticleObject> onArrivedItemEvent;
        private Action<ItemParticleObject> onSpawnedItemEvent;
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
            None,
            Horizontal,
            Vertical
        }

        protected virtual ItemParticleObject NewInstance()
        {
            ItemParticleObject item = Instantiate(itemPrefab, transform);
            DespawnRaw(item);
            return item;
        }

        protected virtual void DespawnRaw(ItemParticleObject item)
        {
            item.transform.localPosition = Vector3.zero;
            item.gameObject.SetActive(false);
            pool.Push(item);
        }

        public virtual void OnSpawnedItem(Action<ItemParticleObject> onSpawnedItem)
        {
            onSpawnedItemEvent = onSpawnedItem;
        }

        public virtual void OnArrivedItem(Action<ItemParticleObject> onArrivedItem)
        {
            onArrivedItemEvent = onArrivedItem;
        }

        public virtual void OnCompleted(Action onCompleted)
        {
            onCompletedEvent = onCompleted;
        }

        public virtual int Play(int spawnCount, Vector2 targetPos)
        {
            if (IsPlaying)
                Stop();

            IsPlaying = true;
            spawnCount = Mathf.Min(spawnCount, maxSpawnCount);

            StartCoroutine(IPLay(spawnCount, targetPos));

            return spawnCount;
        }

        protected IEnumerator IPLay(int particleCount, Vector2 targetPos)
        {
            WaitForSeconds spawnDelay = new WaitForSeconds(spawnDuration / particleCount);

            for (int i = 0; i < particleCount; i++)
            {
                ItemParticleObject item = Spawn();
                item.transform.position = (Vector2)transform.position + UnityEngine.Random.insideUnitCircle.normalized * spawnRadius;
                item.Play(this, targetPos);
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

        protected virtual ItemParticleObject Spawn()
        {
            if (pool.Count == 0)
                NewInstance();

            ItemParticleObject item = pool.Pop();
            item.gameObject.SetActive(true);
            spawnedItems.Add(item);
            item.transform.SetAsFirstSibling();
            return item;
        }

        internal void Despawn(ItemParticleObject item)
        {
            spawnedItems.Remove(item);
            DespawnRaw(item);

            if (spawnedItems.Count == 0)
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
                ItemParticleObject item = spawnedItems[i];
                DespawnRaw(item);
            }

            spawnedItems.Clear();
            IsPlaying = false;

            StopAllCoroutines();
        }

        internal void ArrivedItem(ItemParticleObject item)
        {
            onArrivedItemEvent?.Invoke(item);
        }
    }
}