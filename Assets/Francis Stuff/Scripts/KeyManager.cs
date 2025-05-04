using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public int keys;
    public static KeyManager Instance;
    public Image[] keyImages;

    private void UpdateKeySprites()
    {
        for (int i = 0; i < keyImages.Length; i++)
        {
            if (i < keys)
            {
                keyImages[i].enabled = true;
            }
            else
            {
                keyImages[i].enabled = false;
            }
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
