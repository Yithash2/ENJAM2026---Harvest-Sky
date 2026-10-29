using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AddBuildingPlacement : MonoBehaviour
{
    private List<GameObject> BuildingButtons;
    public int WidthBetweenBuildings = 0;

    public void Add(GameObject BuildingButton)
    {
        var i = BuildingButtons.Count;
        var buildingButton = Instantiate(BuildingButton, transform);
        buildingButton.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + WidthBetweenBuildings * i);
        BuildingButtons.Add(buildingButton);
    }
}
