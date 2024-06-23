using System;
using System.Threading.Tasks;
using Managers;
using Services.DependencyInjection;
using UnityEngine;

namespace Grid_Manager
{
    public class GridManager : MonoBehaviour, IGridManager, IDependencyProvider
    {
        [SerializeField] private float cellSize = 1f;
        
        private Vector3[,] grid;
        private bool[,] occupiedCell;
        private float offsetX, offsetZ;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public float CellSize => cellSize;
        
        [Provide]
        public IGridManager ProviderGridManager()
        {
            return this;
        }
        
        private void Start()
        {
            CreateGrid();
        }
        
        private void CreateGrid()
        {
            var mapSize = GetComponent<Renderer>().bounds.size;
            var planePosition = transform.position;
            
            Width = Mathf.FloorToInt(mapSize.x / CellSize);
            Height = Mathf.FloorToInt(mapSize.z / CellSize);
            
            /*var offsetX = (mapSize.x - (Width * CellSize)) / 2;
            var offsetZ = (mapSize.z - (Width * CellSize)) / 2;*/
            
            /*var offsetX = planePosition.x - mapSize.x / 2 + CellSize / 2;
            var offsetZ = planePosition.z - mapSize.z / 2 + CellSize / 2;*/
            
            offsetX = planePosition.x - (mapSize.x / 2) + (CellSize / 2);
            offsetZ = planePosition.z - (mapSize.z / 2) + (CellSize / 2);
            
            grid = new Vector3[Width, Height];
            occupiedCell = new bool[Width, Height];
            
            for (var x = 0; x < Width; x++)
            {
                for (var z = 0; z < Height; z++)
                {
                    var xPos = x * cellSize + offsetX;
                    var zPos = z * cellSize + offsetZ;
                    grid[x, z] = new Vector3(xPos, 0, zPos);
                    occupiedCell[x, z] = false;
                }
            }
            
            Initializer.Instance.CreateGridCompletion.SetResult(true);
        }

        public Vector3 GetNearestGridPosition(Vector3 worldPosition)
        {
            var x = Mathf.RoundToInt((worldPosition.x - offsetX) / CellSize);
            var z = Mathf.RoundToInt((worldPosition.z - offsetZ) / CellSize);
            x = Mathf.Clamp(x, 0, Width - 1);
            z = Mathf.Clamp(z, 0, Height - 1);
            
            return grid[x, z];
        }
        
        public Vector3[,] GetGrid()
        {
            return grid;
        }

        public void SetOccupiedCell(Vector3 position)
        {
            var x = Mathf.RoundToInt((position.x - offsetX) / CellSize);
            var z = Mathf.RoundToInt((position.z - offsetZ) / CellSize);
            
            occupiedCell[x, z] = true;
        }

        public bool IsValidPlacement(Vector3 position)
        {
            var x = Mathf.RoundToInt((position.x - offsetX) / CellSize);
            var z = Mathf.RoundToInt((position.z - offsetZ) / CellSize);
            
            if (x < 0 || x >= Width || z < 0 || z >= Height)
            {
                return false;
            }
            
            if (occupiedCell[x, z])
            {
                return false;
            }
            
            /*if (grid[x, z].IsPath)
            {
                return false;
            }
            */

            return true;
        } 
    }
}
