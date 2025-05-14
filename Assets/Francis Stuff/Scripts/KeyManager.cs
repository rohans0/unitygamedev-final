using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public int keys;
    public static KeyManager Instance;
    public GameObject keyList;

    private void UpdateKeySprites() // FIX: instead update only when key is collected
    {
        for (int i = 0; i < keyList.transform.childCount; i++)
        {
            keyList.transform.GetChild(i).gameObject.SetActive(i < keys);
        }
    }

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UpdateKeySprites();   
    }
}
