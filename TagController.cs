using UnityEngine;

public class TagController : MonoBehaviour
{
    // ... (Your other TagController code, like dictionaries for tag values etc.) ...

    public static TagController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetValueForTag(string tag)
    {
        // ... (Your logic to get value based on tag) ...
        // Example: 
        switch (tag)
        {
            case "Creep":
                return 10;
            case "Building":
                return 50;
            default:
                return 1;
        }
    }

    public void IncrementTagCount(string tag)
    {
        // ... (Your logic to increment tag count) ...
        // Example: You might have a dictionary to track tag counts
        // if (tagCounts.ContainsKey(tag))
        // {
        //     tagCounts[tag]++;
        // }
        // else
        // {
        //     tagCounts[tag] = 1;
        // }
    }

    // ... (Your other TagController code) ...
}