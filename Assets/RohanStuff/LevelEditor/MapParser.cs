using System.IO;
using UnityEngine;

public class MapParser : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject guardPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private string path = null;

    void Start()
    {
        if (path == null) path = "Assets/RohanStuff/LevelEditor/map.txt";
        if (File.Exists(path))
        {
            using StreamReader reader = new(path);
            int charValue;
            int i = 0;
            while ((charValue = reader.Read()) != -1)
            {
                char character = (char)charValue;
                switch (character)
                {
                    case ' ':
                    case '\n':
                        continue;
                    case '1':
                        GameObject wall = Instantiate(wallPrefab, transform);
                        wall.transform.localPosition = new Vector2(-.4f+((i%9)*.1f), .4f-((i/9)*.1f));
                        wall.transform.localScale = new Vector2(.07f, .07f);
                        break;
                    case '2':
                        Instantiate(keyPrefab, transform).transform.localPosition = new Vector2(-.4f+((i%9)*.1f), .4f-((i/9)*.1f));
                        break;
                    case '3':
                        Instantiate(guardPrefab, transform).transform.localPosition = new Vector2(-.4f+((i%9)*.1f), .4f-((i/9)*.1f));
                        break;
                    default:
                        break;
                }
                i++;
            }
        }
        else
        {
            Debug.LogError("File not found");
        }
    }
}
