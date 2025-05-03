using System.IO;
using UnityEngine;

public class MapParser : MonoBehaviour
{
    [SerializeField] private GameObject keyPrefab;
    [SerializeField] private GameObject guardPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private string path = null;
    

    enum a
    {
        WALL = '1',
        KEY,
        GUARD,
        // WA,
        // WALL = '1',
    };

    void Start()
    {
        if (path == null) path = "Assets/RohanStuff/LevelEditor/map2.txt";
        if (File.Exists(path))
        {
            using StreamReader reader = new(path);
            int charValue;
            int i = 0;
            while ((charValue = reader.Read()) != -1)
            {
                if (charValue < '0') continue;
                Vector2 pos = new(-.4f+((i%9)*.1f), .4f-((i/9)*.1f));
                switch ((a)(charValue))
                {
                    case a.WALL:
                        GameObject wall = Instantiate(wallPrefab, transform);
                        wall.transform.localPosition = pos;
                        wall.transform.localScale = new Vector2(.07f, .07f);
                        break;
                    case a.KEY:
                        Instantiate(keyPrefab, transform).transform.localPosition = pos;
                        break;
                    case a.GUARD:
                        Instantiate(guardPrefab, transform).transform.localPosition = pos;
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
