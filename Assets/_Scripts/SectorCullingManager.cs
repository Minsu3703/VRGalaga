using UnityEngine;

public class SectorCullingManager : MonoBehaviour
{
    public struct Coord
    {
        public int x, y, z;
    }

    public struct Sector
    {
        public Coord coord;
        public Transform root;
    }

    [SerializeField] private float _sectorWidth = 400f;
    private Sector[,,] _sectors = new Sector[3,3,3];
    [SerializeField] private Transform _target;
    [SerializeField] LayerMask _targetMask;

    private void Start()
    {
        InitSectors();
    }
    private void InitSectors()
    {
        for (int i = 0; i < _sectors.GetLength(0); i++)
        {
            for (int j = 0; j < _sectors.GetLength(1); j++)
            {
                for (int k = 0; k < _sectors.GetLength(2); k++)
                {
                    Sector sector = new Sector();
                    sector.coord = new Coord { x = i - _sectors.GetLength(0) / 2, y = j - _sectors.GetLength(1) / 2, z = k - _sectors.GetLength(2) / 2 };
                    sector.root = new GameObject($"Sector_{sector.coord.x}_{sector.coord.y}_{sector.coord.z}").transform;
                    sector.root.SetParent(transform);
                    //sector.root.position = new Vector3(i, j, k) * _sectorWidth;
                    sector.root.position = new Vector3(sector.coord.x, sector.coord.y, sector.coord.z) * _sectorWidth;
                    _sectors[i, j, k] = sector;
                }
            }
        }
    }

    private void Update()
    {
        if (_target.position.z >= _sectorWidth)
        {
            RollSectorZMinus();
        }
    }

    /// <summary>
    /// 플레이어가 Z+ 방향의 다음 섹터로 넘어갈때 모든 섹터를 Z- 방향으로 회전 
    /// </summary>
    private void RollSectorZMinus()
    {
        for (int i = 0; i < _sectors.GetLength(0); i++)
        {
            for (int j = 0; j < _sectors.GetLength(1); j++)
            {
                _sectors[i, j, 0].root.gameObject.SetActive(false);

                for (int k = 0; k < _sectors.GetLength(2) - 1; k++)
                {
                    _sectors[i, j, k] = _sectors[i, j, k + 1];
                    _sectors[i, j, k].root.position = new Vector3(i, j, k) * _sectorWidth;
                }

                Sector prevSector = _sectors[i, j, _sectors.GetLength(2) - 2];
                Sector sector = new Sector();
                sector.coord = new Coord { x = prevSector.coord.x, y = prevSector.coord.y, z = prevSector.coord.z + 1 };
                sector.root = transform.Find($"Sector_{sector.coord.x}_{sector.coord.y}_{sector.coord.z}") ?? new GameObject($"Sector_{sector.coord.x}_{sector.coord.y}_{sector.coord.z}").transform;
                sector.root.SetParent(transform);
                _sectors[i, j, _sectors.GetLength(2) - 1] = sector;
                _sectors[i, j, _sectors.GetLength(2) - 1].root.position = new Vector3(i, j, _sectors.GetLength(2) - 1) * _sectorWidth;
            }
        }

        _target.position = new Vector3(_target.position.x, _target.position.y, _target.position.z - _sectorWidth);
        Debug.Log($"Fucosed sector center : {_sectors[1,1,1].coord.x}, {_sectors[1, 1, 1].coord.y}, {_sectors[1, 1, 1].coord.z}");
    }
}
