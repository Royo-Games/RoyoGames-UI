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

        private enum State
        {
            Spray,
            Move
        }

        internal void Play(UICollectibleItemSpawner spawner)
        {
            this.spawner = spawner;

            if (spawner.SprayType != SprayTypes.None)
                BeginSpray();
            else
                BeginMove();
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
                float t = elapsedTime / spawner.SprayDuration;
                float f = spawner.SprayCurve.Evaluate(t);

                transform.position = Vector2.Lerp(startPos, targetPos, f);
                transform.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, f);
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
                Vector2 springDir = spawner.SpringDirection == SpringDirections.LeftRight ? Vector2.right : Vector2.up;

                pos += springDir * Mathf.Sin(t * Mathf.PI) * spawner.SpringAmplitude;

                transform.position = pos;
            }
            else
            {
                Despawn();
            }
        }

        private void BeginSpray()
        {
            elapsedTime = 0;
            state = State.Spray;
            startPos = transform.position;
            transform.localScale = Vector2.zero;
            targetPos = transform.position + (GetDirection() * spawner.SprayRadius);
        }

        private void BeginMove()
        {
            startPos = transform.position;
            elapsedTime = 0;
            targetPos = spawner.Target.TransformPoint(spawner.Target.rect.center);
            state = State.Move;
        }

        protected virtual void OnSpawn()
        {
        }

        protected virtual void Despawn()
        {
            spawner.Despawn(this);
        }

        private Vector3 GetDirection()
        {
            switch (spawner.SprayType)
            {
                case SprayTypes.Up: return Vector3.up;
                case SprayTypes.Down: return Vector3.down;
                case SprayTypes.Left: return Vector3.left;
                case SprayTypes.Right: return Vector3.right;
                case SprayTypes.Random: return Random.insideUnitCircle.normalized;
                default:
                    return Vector3.zero;
            }
        }
    }
}
