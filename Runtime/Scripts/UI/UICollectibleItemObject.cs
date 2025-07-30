using UnityEngine;
using static RoyoGames.UI.UICollectibleItemSpawner;

namespace RoyoGames.UI
{
    public class UICollectibleItemObject : MonoBehaviour
    {
        private UICollectibleItemSpawner spawner;

        private State state;
        private Vector2 startPos;
        private Vector2 targetPos;
        private float elapsedTime;
        private float springAmplitude;

        private enum State
        {
            Spray,
            Move
        }

        internal void Play(UICollectibleItemSpawner spawner)
        {
            this.spawner = spawner;

            if (spawner.SprayDirection != SprayDirections.None)
                BeginSpray();
            else
                BeginMove();

            OnSpawn();
        }

        protected virtual void Update()
        {
            switch (state)
            {
                case State.Spray:
                    UpdateSpray();
                    break;

                case State.Move:
                    UpdateMove();
                    break;
            }
        }

        private void UpdateSpray()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < spawner.SprayDuration)
            {
                float sprayTime = elapsedTime / spawner.SprayDuration;
                float f = spawner.SprayCurve.Evaluate(sprayTime);

                transform.position = Vector2.Lerp(startPos, targetPos, f);
            }
            else
            {
                BeginMove();
            }
        }

        private void UpdateMove()
        {
            elapsedTime += Time.deltaTime;

            if (elapsedTime < spawner.MoveDuration)
            {
                float t = elapsedTime / spawner.MoveDuration;
                float f = spawner.MoveCurve.Evaluate(t);

                Vector2 pos = Vector2.Lerp(startPos, targetPos, f);
                Vector2 springDir = spawner.SpringDirection == SpringDirections.Horizontal ? Vector2.right : Vector2.up;

                //pos += springDir * Mathf.Sin(t * Mathf.PI) * springAmplitude;
                pos += springDir * spawner.SpringCurve.Evaluate(t) * springAmplitude;

                transform.position = pos;
            }
            else
            {
                Arrived();
                Despawn();
            }
        }

        private void BeginSpray()
        {
            elapsedTime = 0;
            state = State.Spray;
            startPos = transform.position;
            targetPos = transform.position + (GetDirection() * Random.Range(spawner.MinSprayRadius, spawner.MaxSprayRadius));
        }

        private void BeginMove()
        {
            startPos = transform.position;
            elapsedTime = 0;
            targetPos = spawner.Target.TransformPoint(spawner.Target.rect.center);
            springAmplitude = Random.Range(spawner.MinSpringAmplitude, spawner.MaxSpringAmplitude);
            state = State.Move;
        }

        protected virtual void OnSpawn()
        {
        }

        protected virtual void Despawn()
        {
            spawner.Despawn(this);
        }

        protected virtual void Arrived()
        {
            spawner.ArrivedItem(this);
        }

        private Vector3 RandomConeDirection(Vector3 baseDirection, float coneAngle)
        {
            float halfAngle = coneAngle * 0.5f;
            float randomOffset = Random.Range(-halfAngle, halfAngle);
            return Quaternion.Euler(0f, 0f, randomOffset) * baseDirection;
        }

        private Vector3 GetDirection()
        {
            switch (spawner.SprayDirection)
            {
                case SprayDirections.Up:
                    return RandomConeDirection(Vector3.up, spawner.SprayConeAngle);

                case SprayDirections.Down:
                    return RandomConeDirection(Vector3.down, spawner.SprayConeAngle);

                case SprayDirections.Left:
                    return RandomConeDirection(Vector3.left, spawner.SprayConeAngle);

                case SprayDirections.Right:
                    return RandomConeDirection(Vector3.right, spawner.SprayConeAngle);

                case SprayDirections.Random:
                    return Random.insideUnitCircle.normalized;

                default:
                    return Vector3.zero;
            }
        }
    }
}
