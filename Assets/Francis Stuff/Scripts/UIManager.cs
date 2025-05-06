using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI keyCountText;
    public GameObject KeyManager;

    
    private void UpdateUI()
    {
        // Get the KeyManager component from the KeyManager GameObject
        KeyManager keyManager = KeyManager.GetComponent<KeyManager>();
        keyCountText.text = "Keys: " + keyManager.keys.ToString();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
}
