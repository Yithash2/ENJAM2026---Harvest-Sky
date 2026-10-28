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
                 Debug.Log("1");
                 return false;
             }

             int i = (int)((int)(obj.transform.position.x - Bound1.x) / SnapAmount);
             int j = (int)((int)(obj.transform.position.x - Bound1.x) / SnapAmount);

             if (!_grid[i, j].Usable)
             {
                 Debug.Log("2");
                 return false;
             }

             List<Node> PlaceUsed = new List<Node>();

             _grid[i, j].Occupied = true;
             PlaceUsed.Add(_grid[i, j]);

             for (int w = -1; w < 2; w++)
                 for (int h = -1; h < 2; h++)
                     if (InBound(i-w, j-h) && (w != 0 && h != 0))
                     {
                         if (!_grid[i - w, j - h].Usable)
                         {
                             Debug.Log("3");
                             return false;
                         }
                         
                         _grid[i - w, j - h].Blocked = true;
                         PlaceUsed.Add(_grid[i - w, j - h]);
                     }

             obj.PlacesUsed = PlaceUsed;
             ObjectsOnGrid.Add(obj);
             return true;
         }

         private bool InBound(Vector3 position)
         {
             return Bound1.x < position.x && Bound2.x > position.x && Bound1.y < position.y && Bound2.y > position.y;
         }

         private bool InBound(int x,int y)
         {
             return 0 < x && size.x > x && 0 < y && size.y > y;
         }
         
         private void OnValidate()
         {
             int i = (int)((int)Mathf.Abs(Bound1.x - Bound2.x) / SnapAmount);
             int j = (int)((int)Mathf.Abs(Bound1.y - Bound2.y) / SnapAmount);
             size = new Vector2Int(i, j);
             _grid = new Node[i,j];
             for(int h = 0; h<i; h++)
                 for(int w = 0; w<j; w++)
                     _grid[h,w] = new Node();
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
                     if (_grid[i, j].Occupied)
                        Gizmos.color = Color.black;
                     else if (_grid[i, j].Blocked)
                         Gizmos.color = Color.red;
                     else
                         Gizmos.color = Color.blue;
                     
                     Gizmos.DrawWireCube(new Vector3(x,transform.position.y,y),(Vector3.one + Vector3.down) * SnapAmount);
                     j++;
                 }

                 i++;
                 j = 0;
             }
         }
     }   
}

