using RoyoGames.UI;
using System.Collections;
using TMPro;
using UnityEngine;

public class CollectibleItemSpawnerExample : MonoBehaviour
{
    public UICollectibleItemSpawner spawner;
    public TextMeshProUGUI goldText;

    private float golds;

    public void SpawnGolds()
    {
        StartCoroutine(ISpawn());
    }

    private IEnumerator ISpawn()
    {
        spawner.Play(20);

        spawner.OnSpawnedItem((item) => 
        {
            Debug.Log("item spawned");
        });

        spawner.OnArrivedItem((item) =>
        {
            golds++;
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
