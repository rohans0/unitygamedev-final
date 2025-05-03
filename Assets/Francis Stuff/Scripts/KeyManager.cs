using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour
{
    public int keys;
    public static KeyManager Instance;

    void Awake()
    {
        Instance = this;
    }
}
