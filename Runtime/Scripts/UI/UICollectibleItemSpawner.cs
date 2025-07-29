using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RoyoGames.UI
{
    public class UICollectibleItemSpawner : MonoBehaviour
    {
        [Header("Spawn Settings")]
        public UICollectibleItemObject ItemPrefab;
        public int MaxSpawnCount = 20;
        public float SpawnInterval = 0.1f;
        public float SpawnRadius = 0;

        [Header("Spray Animation Settings")]
        public SprayTypes SprayType = SprayTypes.Random;
        public float SprayDuration = 5;
        public float SprayRadius = 1;
        public AnimationCurve SprayCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Move Animation Settings")]
        public RectTransform Target;
        public float MoveDuration = 5;
        public SpringDirections SpringDirection;
        public float SpringAmplitude = 1;
        public AnimationCurve MoveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        public bool IsPlaying { get; private set; }

        protected Queue<UICollectibleItemObject> pool = new Queue<UICollectibleItemObject>();
        protected List<UICollectibleItemObject> spawnedItems = new List<UICollectibleItemObject>();

        public enum SprayTypes
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
            LeftRight,
            UpDown
        }

        protected virtual void Awake()
        {
            PreLoad();
        }

        protected virtual void PreLoad()
        {
            for (int i = 0; i < MaxSpawnCount; i++)
            {
                UICollectibleItemObject item = Instantiate(ItemPrefab, transform);
                DespawnRaw(item);
            }
        }

        protected virtual void DespawnRaw(UICollectibleItemObject item)
        {
            item.transform.localPosition = Vector3.zero;
            item.gameObject.SetActive(false);
            pool.Enqueue(item);
        }

        public virtual void Play()
        {
            Play(MaxSpawnCount, null);
        }

        public virtual int Play(int spawnCount, System.Action onArriwed)
        {
            if (IsPlaying)
                Stop();

            IsPlaying = true;
            spawnCount = Mathf.Min(spawnCount, MaxSpawnCount);

            StartCoroutine(IPLay(spawnCount));

            return spawnCount;
        }

        protected IEnumerator IPLay(int particleCount)
        {
            WaitForSeconds spawnDelay = new WaitForSeconds(SpawnInterval);

            for (int i = 0; i < particleCount; i++)
            {
                UICollectibleItemObject item = Spawn();
                item.transform.position = (Vector2)transform.position + Random.insideUnitCircle.normalized * SpawnRadius;
                item.Play(this);
                yield return spawnDelay;
            }
        }

        protected virtual UICollectibleItemObject Spawn()
        {
            UICollectibleItemObject item = pool.Dequeue();
            item.gameObject.SetActive(true);
            spawnedItems.Add(item);
            return item;
        }

        internal void Despawn(UICollectibleItemObject item)
        {
            spawnedItems.Remove(item);
            DespawnRaw(item);
        }

        public virtual void Stop()
        {
            for (int i = 0; i < spawnedItems.Count; i++)
            {
                UICollectibleItemObject item = spawnedItems[i];
                DespawnRaw(item);
            }

            spawnedItems.Clear();
            IsPlaying = false;

            StopAllCoroutines();
        }
    }
}