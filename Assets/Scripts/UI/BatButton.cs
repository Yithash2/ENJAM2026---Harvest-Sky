using System;
using Placements;
using UnityEngine;

public class BatButton : MonoBehaviour
{
    
    public int Ressources;
    public GameObject prefab;
    
    public void Cliked()
    {
        // Si il y a assez de ressources

        if (PlacementManager.Instance.CurrentObjectInHand)
        {
            Destroy(PlacementManager.Instance.CurrentObjectInHand.gameObject);
            PlacementManager.Instance.CurrentObjectInHand = null;
        }
        var go = Instantiate(prefab);
        PlacementManager.Instance.CurrentObjectInHand = go.GetComponent<PlacementObject>();;
    }

    public void Delete()
    {
        if (PlacementManager.Instance.CurrentObjectInHand)
        {
            Destroy(PlacementManager.Instance.CurrentObjectInHand.gameObject);
            PlacementManager.Instance.CurrentObjectInHand = null;
            
        }
    }
}
