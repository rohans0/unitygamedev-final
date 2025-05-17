using System.Collections.Generic;
using UnityEngine;

public class GuardSpawner : MonoBehaviour
{
    const int guardCount = 3;
    [SerializeField] GameObject guardPrefab;
    [SerializeField] GameObject[] guards = new GameObject[guardCount];

    [SerializeField] Vector2 spawnBoundsX;
    [SerializeField] Vector2 spawnBoundsY;

    void Update()
    {
        for (int i = 0; i < guardCount; i++)
        {
            if (guards[i] == null) guards[i] = spawnGuard();
        }
    }

    GameObject spawnGuard()
    {
        float x = Random.Range(spawnBoundsX.x, spawnBoundsX.y);
        float y = Random.Range(spawnBoundsY.x, spawnBoundsY.y);
        GameObject guard = Instantiate(guardPrefab);
        guard.transform.position = new Vector2(x, y);
        guard.GetComponent<LavaGuard>().currentState = LavaGuard.GuardState.Chase;
        return guard;
    }
}
