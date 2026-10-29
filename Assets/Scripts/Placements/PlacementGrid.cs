using System;
using UnityEngine;
using Random = UnityEngine.Random;


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
             
             int layer = ValidObjectDirectPlacement(obj,out List<Node> placeUsed, out int numberOfOccupiedNodeHitOnTheSameLayer);

             if (layer < 0) return false;
             
             if (numberOfOccupiedNodeHitOnTheSameLayer < (obj.TotalNbrOfNode/2f))
                 obj.BadSupport = true;
             
             obj.layerPlaced = layer;
             obj.gameObject.layer = PlacementManager.Instance.PlaceLayer.value;
             
             foreach (var node in placeUsed)
             {
                 node.Objects.Add(obj);
             }
             ObjectsOnGrid.Add(obj);
             return true;
         }

         public int ValidObjectDirectPlacement(PlacementObject obj,out List<Node> placesUsed, out int numberOfOccupiedNodeHitOnTheSameLayer)
         {
             placesUsed = new List<Node>();
             
             var min = obj.collider.bounds.min;
             var max = obj.collider.bounds.max;

             var minIndex = WorldToGridIndex(min, true);
             var maxIndex = WorldToGridIndex(new Vector3(
                 max.x - 1f * SnapAmount,
                 max.y,
                 max.z - 1f * SnapAmount
             ), true);
             numberOfOccupiedNodeHitOnTheSameLayer = 0;
             int layer = 0;
             
             for(int i = minIndex.x; i <= maxIndex.x; i++)
             for (int j = minIndex.y; j <= maxIndex.y; j++)
             {
                 
                 if (_grid[i, j].Occupied)//Si l'espace est occuper
                 {
                     
                     if(_grid[i, j].Objects[^1].IsRuined)
                         if (_grid[i, j].Objects[^1].LayerOnTop() > layer)
                         {
                             layer = _grid[i, j].Objects[^1].LayerOnTop();
                             numberOfOccupiedNodeHitOnTheSameLayer = 1;
                             placesUsed = new List<Node>();
                         }
                         else if (_grid[i, j].Objects[^1].LayerOnTop() == layer)
                         {
                             numberOfOccupiedNodeHitOnTheSameLayer++;
                             placesUsed.Add(_grid[i, j]);
                         }
                 }
                 else if(layer == 0)
                 {
                     numberOfOccupiedNodeHitOnTheSameLayer++;
                     placesUsed.Add(_grid[i, j]);
                 }
                 
                 Debug.DrawRay(GridToWorld(i,j), Vector3.up);
             }
         
             if (numberOfOccupiedNodeHitOnTheSameLayer >= obj.TotalNbrOfNode/4f)
             {
                 return layer;
             }
             else
             {
                 placesUsed = new List<Node>();
                 return -1;
             }
             
         }

         public Vector2 GridToWorld(int i, int j)
         {
             var centerOfFirst = new Vector2(Bound1.x + SnapAmount / 2,Bound1.y + SnapAmount / 2);
             return centerOfFirst + new Vector2(i, j) * SnapAmount;
         }

         public Vector2Int WorldToGridIndex(Vector3 worldPos, bool correct = false)
         {
             int i = Mathf.Abs(Mathf.FloorToInt((worldPos.x - Bound1.x) / SnapAmount));
             int j = Mathf.Abs(Mathf.FloorToInt((worldPos.z - Bound1.y) / SnapAmount));

             if (correct)
             {
                 i = 0 > i ? 0 : i >= size.x ? size.x-1 : i;
                 j = 0 > j ? 0 : j >= size.y ? size.y-1 : j;
             }
             
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

         public Tuple<Vector2Int,Node> FindRdmNodeInView() // Trouve une case au hasard qui regarde le ciel
         {
             List<Tuple<Vector2Int,Node>> usable = new List<Tuple<Vector2Int,Node>>();

             int i = 0;
             int j = 0;
             foreach (var node in _grid)
             {
                 if(node.Occupied && !node.Objects[^1].IsRuined) continue;
                 usable.Add(new Tuple<Vector2Int, Node>(new Vector2Int(i,j),node));

                 j++;
                 if (j >= size.y)
                 {
                     j = 0;
                     i++;
                 }
             }

             List<Tuple<Vector2Int,Node>> valid = new List<Tuple<Vector2Int,Node>>();
             foreach (var node in usable)
             {
                 if (CheckNodeExteriorView(node.Item2, GridToWorld(node.Item1.x, node.Item1.y)))
                 {
                     valid.Add(node);
                 }
             }

             int rdm = Random.Range(0, valid.Count - 1);

             return valid[rdm];
         }

         public bool CheckNodeExteriorView(Node node, Vector3 position)
         {
             /*Physics.Raycast(position, Vector3.forward, 30);
             Physics.Raycast(position, Vector3.right, 30);
             Physics.Raycast(position, -Vector3.forward, 30);
             Physics.Raycast(position, -Vector3.right, 30);*/
             
             bool b = Physics.Raycast(position, new Vector3(Mathf.Cos(Mathf.PI/6f), Mathf.Sin(Mathf.PI/6f), 0), 30);
             b = b||Physics.Raycast(position, new Vector3(0, Mathf.Sin(Mathf.PI/6f), Mathf.Cos(Mathf.PI/6f)), 30);
             b=b||Physics.Raycast(position, new Vector3(-Mathf.Cos(Mathf.PI/6f), Mathf.Sin(Mathf.PI/6f), 0), 30);
             b=b||Physics.Raycast(position, new Vector3(0, Mathf.Sin(Mathf.PI/6f), -Mathf.Cos(Mathf.PI/6f)), 30);

             return b;
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

