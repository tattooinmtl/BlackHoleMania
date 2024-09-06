using UnityEngine;

public class ExtraGameController : MonoBehaviour
{
    public BlackHole blackHole;
    public GameObject[] fxPrefabs;

    private int previousBlackHoleLevel = 0;

    void Update()
    {
        if (blackHole != null)
        {
            int currentBlackHoleLevel = blackHole.upgradeCount + 1;

            if (currentBlackHoleLevel != previousBlackHoleLevel)
            {
                if (currentBlackHoleLevel >= 3 && currentBlackHoleLevel % 3 == 0)
                {
                    int fxIndex = (currentBlackHoleLevel / 3) - 1;
                    fxIndex = Mathf.Clamp(fxIndex, 0, fxPrefabs.Length - 1);

                    ApplyFXToBlackHole(fxIndex);
                    IncreaseBlackHoleDamage();
                }

                previousBlackHoleLevel = currentBlackHoleLevel;
            }
        }
    }

    void ApplyFXToBlackHole(int fxIndex)
{
    if (fxPrefabs.Length > fxIndex && fxPrefabs[fxIndex] != null)
    {
        // Instantiate the FX and make it a child of the Black Hole
        GameObject newFX = Instantiate(fxPrefabs[fxIndex], blackHole.transform.position, Quaternion.identity);
        newFX.transform.parent = blackHole.transform; 
    }
    else
    {
        Debug.LogError("Invalid FX index or missing prefab in ExtraGameController!");
    }
}

    void IncreaseBlackHoleDamage()
    {
        blackHole.baseDamage += 10; // Increase Black Hole damage 
    }
}