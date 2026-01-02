using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
  [System.Serializable]
  public struct SpawnableObject
  {

    public GameObject prefab;
    [Range(0f, 1f)]
    public float spawnChance;
    public int poolSize;
  }

  public SpawnableObject[] objects;

  public float minSpawnRate = 0.5f;
  public float maxSpawnRate = 1f;

  private Dictionary<GameObject, List<GameObject>> pools = new Dictionary<GameObject, List<GameObject>>();

    private void Awake()
    {
        foreach(var obj in objects)
        {
            List<GameObject> pool = new List<GameObject>();
            for(int i = 0; i < obj.poolSize; i++)
            {
                GameObject instance = Instantiate(obj.prefab);
                instance.SetActive(false);
                pool.Add(instance);
            }
            pools.Add(obj.prefab, pool);
        }
    }

    private void OnEnable()
  {

    Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));
  }

  private void OnDisable()
  {

    CancelInvoke();
    HideAllObstacles();
  }

  private void Spawn()
  {

    float spawnChance = Random.value;

    foreach (var obj in objects)
    {

      if (spawnChance < obj.spawnChance)
      {
        int randomIndex = Random.Range(0, objects.Length);
        var selectedObj = objects[randomIndex];

        GameObject obstacle = GetObjectFromPool(obj.prefab);
        if(obstacle != null)
                {
                    obstacle.transform.position = transform.position;
                    obstacle.SetActive(true);
                }

                break;
      }

      spawnChance -= obj.spawnChance;
    }

    Invoke(nameof(Spawn), Random.Range(minSpawnRate, maxSpawnRate));

  }
  private GameObject GetObjectFromPool(GameObject prefab)
    {
        foreach(GameObject obj in pools[prefab])
        {
            if(!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return null;
          
    }

    private void HideAllObstacles()
    {
        foreach(var pool in pools.Values)
        {
            foreach(var obj in pool)
            {
                obj.SetActive(false);
            }
        }
    }
}
