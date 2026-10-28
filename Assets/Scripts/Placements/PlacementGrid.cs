using System;
using UnityEngine;


namespace Placements
{
    using System.Collections.Generic;
     public class PlacementGrid : MonoBehaviour
     {
         [Range(0.1f, 2f)] public float SnapAmount = 0.5f;
         public Vector2 Bound1;
         public Vector2 Bound2;
         private Vector2Int size;

         [SerializeField]private Node[,] _grid;

         private List<PlacementObject> ObjectsOnGrid;

         public bool PlaceObject(PlacementObject obj)
         {
             if (!InBound(obj.transform.position))
             {
                 return false;
             }
             
             bool b = ValidObjectDirectPlacement(obj,out List<Node> placeUsed);

             if (!b) return false;
             
             obj.PlacesUsed = placeUsed;
             foreach (var node in placeUsed)
             {
				print("fbbkbkbkbkbk");
                 node.Objects.Add(obj);
                 print(node.Objects.Count);
             }
             ObjectsOnGrid.Add(obj);
             return true;
         }

         public bool ValidObjectDirectPlacement(PlacementObject obj,out List<Node> placesUsed)
         {
             placesUsed = new List<Node>();
             
             var min = obj.collider.bounds.min;
             var max = obj.collider.bounds.max;

             var minIndex = WorldToGridIndex(min);
             var maxIndex = WorldToGridIndex(new Vector3(
                 max.x - 1f * SnapAmount,
                 max.y,
                 max.z - 1f * SnapAmount
             ));

             int numberOfOccupiedNodeHitOnTheSameLayer = 0;
             int layer = 0;
             
             for(int i = minIndex.x; i <= maxIndex.x; i++)
             for (int j = minIndex.y; j <= maxIndex.y; j++)
             {
                 
                 if (_grid[i, j].Occupied)//Si l'espace est occuper
                 {
                     
                     if(_grid[i, j].Objects[^1].SupportsOtherObjects)
                         if (_grid[i, j].Objects[^1].LayerOnTop() > layer)
                         {
                             print($"{new Vector2Int(i,j)} : {_grid[i, j].Objects[^1].LayerOnTop()}");
                             layer = _grid[i, j].Objects[^1].LayerOnTop();
                             numberOfOccupiedNodeHitOnTheSameLayer = 1;
                             placesUsed = new List<Node>();
                         }
                         else if (_grid[i, j].Objects[^1].LayerOnTop() == layer)
                         {
                             print($"{new Vector2Int(i,j)} : {numberOfOccupiedNodeHitOnTheSameLayer}");
                             numberOfOccupiedNodeHitOnTheSameLayer++;
                             placesUsed.Add(_grid[i, j]);
                         }
                 }
                 else if(layer == 0)
                 {
                     numberOfOccupiedNodeHitOnTheSameLayer++;
                     placesUsed.Add(_grid[i, j]);
                 }
             }
         
             if (numberOfOccupiedNodeHitOnTheSameLayer >= obj.totalNbrOfNode/4f)
             {
                 obj.layerPlaced = layer;
                 if (numberOfOccupiedNodeHitOnTheSameLayer < (obj.totalNbrOfNode/2f))
                     obj.BadSupport = true;
                 return true;
             }
             else
             {
                 placesUsed = new List<Node>();
                 return false;
             }
             
         }

         public Vector2 GridToWorld(int i, int j)
         {
             var centerOfFirst = new Vector2(Bound1.x + SnapAmount / 2,Bound1.y + SnapAmount / 2);
             return centerOfFirst + new Vector2(i, j) * SnapAmount;
         }

         public Vector2Int WorldToGridIndex(Vector3 worldPos)
         {
             int i = Mathf.Abs(Mathf.FloorToInt((worldPos.x - Bound1.x) / SnapAmount));
             int j = Mathf.Abs(Mathf.FloorToInt((worldPos.z - Bound1.y) / SnapAmount));
             return new Vector2Int(i, j);
         }
         
         private bool InBound(Vector3 position)
         {
             return Bound1.x < position.x && Bound2.x > position.x && Bound1.y < position.z && Bound2.y > position.z;
         }

         private bool InBound(int x,int y)
         {
             return 0 < x && size.x > x && 0 < y && size.y > y;
         }
         
         private void OnValidate()
         {

             size = WorldToGridIndex(new Vector3(Bound2.x,0,Bound2.y));
             _grid = new Node[size.x,size.y];
             for(int h = 0; h<size.x; h++)
                 for(int w = 0; w<size.y; w++)
                     _grid[h,w] = new Node();
             ObjectsOnGrid = new List<PlacementObject>();
         }

         private void OnDrawGizmos()
         {
             if (_grid is null)
                 return; 
             
             int i = 0, j = 0;
             for (float x = Bound1.x + SnapAmount; x < Bound2.x; x += SnapAmount)
             {
                 for (float y = Bound1.y + SnapAmount; y < Bound2.y; y += SnapAmount)
                 {
                     float d = 0.01f;
                     if (_grid[i, j].Occupied)
                     {
                         d += 0.02f;
                        Gizmos.color = Color.black;
                         
                     }
                     else
                         Gizmos.color = Color.blue;
                     
                     Gizmos.DrawWireCube(new Vector3(x,transform.position.y+d,y),(Vector3.one + Vector3.down) * SnapAmount);
                     j++;
                 }

                 i++;
                 j = 0;
             }
         }
     }   
}

