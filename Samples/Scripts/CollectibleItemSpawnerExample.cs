using RoyoGames.UI;
using System.Collections;
using TMPro;
using UnityEngine;

public class CollectibleItemSpawnerExample : MonoBehaviour
{
    public ItemParticleSpawner spawner;
    public TextMeshProUGUI goldText;
    public RectTransform target;

    private float golds;

    public void SpawnGolds()
    {
        StartCoroutine(ISpawn());
    }

    private IEnumerator ISpawn()
    {
        float totalGolds = 1000;

        int spawnCount = spawner.Play((int)totalGolds, target.TransformPoint(target.rect.center));

        float goldIncrease = totalGolds / spawnCount;

        spawner.OnSpawnedItem((item) => 
        {
            Debug.Log("item spawned");
        });

        spawner.OnArrivedItem((item) =>
        {
            golds += goldIncrease;
            goldText.text = golds.ToString();
            Debug.Log("item arived");
        });

        spawner.OnCompleted(() =>
        {
            Debug.Log("Completed");
        });

        yield return spawner.WaitForCompletion();

        Debug.Log("Completed***");
    }
}
