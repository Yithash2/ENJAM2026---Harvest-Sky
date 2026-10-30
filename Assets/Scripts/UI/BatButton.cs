using System;
using Placements;
using UnityEngine;
using UnityEngine.UI;

public class BatButton : MonoBehaviour
{
    
    public int Ressources;
    public GameObject prefab;
    [SerializeField] private Button _button;

    void Update()
    {
        if (_button != null) _button.interactable = ResourceManager.Instance.CompareResources(Ressources);
    }
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
