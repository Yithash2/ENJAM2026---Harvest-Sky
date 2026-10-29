using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public int Resources;

    private void Awake()
    {
        if (Instance != this && Instance != null)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
    }

    public void AddResources(int amount)
    {
        Resources += amount;
    }

    public void UseResources(int amount)
    {
        Resources -= amount;
    }

    public bool CompareResources(int comparison)
    {
        return Resources >= comparison;
    }
}
