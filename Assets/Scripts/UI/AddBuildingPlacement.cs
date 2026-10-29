using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddBuildingPlacement : MonoBehaviour
{
    private List<GameObject> BuildingButtons;
    public int WidthBetweenBuildings = 0;

    public void Start()
    {
        BuildingButtons = new();
    }

    public void Add(GameObject BuildingButton)
    {
        var i = BuildingButtons.Count;
        var buildingButton = Instantiate(BuildingButton, transform);
        buildingButton.transform.position = new Vector3(transform.position.x + WidthBetweenBuildings * i, transform.position.y, transform.position.z);
        BuildingButtons.Add(buildingButton);
    }
}
