using System;
using System.Threading.Tasks;
using Managers;
using Services.DependencyInjection;
using UnityEngine;
using UnityEngine.Serialization;

namespace Grid_Manager
{
    public class TDGridManager : MonoBehaviour, IGridManager, IDependencyProvider
    {
        [SerializeField] private float setCellSize = 1f;
        
        private Vector3[,] m_Grid;
        private bool[,] m_OccupiedCell;
        private float m_OffsetX, m_OffsetZ;

        public int width { get; private set; }
        public int height { get; private set; }
        public float cellSize
        {
            get
            {
                return setCellSize;
            }
        }

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
            
            width = Mathf.FloorToInt(mapSize.x / cellSize);
            height = Mathf.FloorToInt(mapSize.z / cellSize);
            
            /*var offsetX = (mapSize.x - (Width * CellSize)) / 2;
            var offsetZ = (mapSize.z - (Width * CellSize)) / 2;*/
            
            /*var offsetX = planePosition.x - mapSize.x / 2 + CellSize / 2;
            var offsetZ = planePosition.z - mapSize.z / 2 + CellSize / 2;*/
            
            m_OffsetX = planePosition.x - (mapSize.x / 2) + (cellSize / 2);
            m_OffsetZ = planePosition.z - (mapSize.z / 2) + (cellSize / 2);
            
            m_Grid = new Vector3[width, height];
            m_OccupiedCell = new bool[width, height];
            
            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < height; z++)
                {
                    var xPos = x * cellSize + m_OffsetX;
                    var zPos = z * cellSize + m_OffsetZ;
                    m_Grid[x, z] = new Vector3(xPos, 0, zPos);
                    m_OccupiedCell[x, z] = false;
                }
            }
            
            TDInitializeModel.api.createGridCompletion.SetResult(true);
        }

        public Vector3 GetNearestGridPosition(Vector3 worldPosition)
        {
            var x = Mathf.RoundToInt((worldPosition.x - m_OffsetX) / cellSize);
            var z = Mathf.RoundToInt((worldPosition.z - m_OffsetZ) / cellSize);
            x = Mathf.Clamp(x, 0, width - 1);
            z = Mathf.Clamp(z, 0, height - 1);
            
            return m_Grid[x, z];
        }
        
        public Vector3[,] GetGrid()
        {
            return m_Grid;
        }

        public void SetOccupiedCell(Vector3 position)
        {
            var x = Mathf.RoundToInt((position.x - m_OffsetX) / cellSize);
            var z = Mathf.RoundToInt((position.z - m_OffsetZ) / cellSize);
            
            m_OccupiedCell[x, z] = true;
        }

        public bool IsValidPlacement(Vector3 position)
        {
            var x = Mathf.RoundToInt((position.x - m_OffsetX) / cellSize);
            var z = Mathf.RoundToInt((position.z - m_OffsetZ) / cellSize);
            
            if (x < 0 || x >= width || z < 0 || z >= height)
            {
                return false;
            }
            
            if (m_OccupiedCell[x, z])
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
