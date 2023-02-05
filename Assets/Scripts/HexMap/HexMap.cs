using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public static HexMap main;

    public HexCell cellPrefab;

    public List<HexCell> cells = new List<HexCell>();
    public Dictionary<Vector3Int, HexCell> cellDict = new Dictionary<Vector3Int, HexCell>();

    [Header("Game Variables")]
    [Range(0,4)]public int revealRadius;
    [Range(0, 1)] public float
        RockProbability,
        ForestProbability,
        AquiferProbability;

    public Vector4 mapExtends;

    private void Awake()
    {
        main = this;
    }
    private void Start()
    {
        cells= new List<HexCell>();
        cellDict= new Dictionary<Vector3Int, HexCell>();
        RevealFromCoordinates(Vector3Int.zero);

        cellDict[Vector3Int.zero].biome = Biome.Plains;
        cellDict[Vector3Int.zero].isClaimed = true;
        cellDict[Vector3Int.zero].isThrone = true;
    }

    public void RevealFromCoordinates(Vector3Int coordinates)
    {
        List<HexCell> newCells= new List<HexCell>();
        List<Vector3Int> revealCoordinates = new List<Vector3Int>() { coordinates };
        //Get All Coordinates
        #region ADD REVEAL COORDINATES by hand because im not going to figure that shit out now
        if (revealRadius >= 1)
        {
            revealCoordinates.Add(coordinates + new Vector3Int(0, -1, 1));
            revealCoordinates.Add(coordinates + new Vector3Int(1, -1, 0));
            revealCoordinates.Add(coordinates + new Vector3Int(1, 0, -1));
            revealCoordinates.Add(coordinates + new Vector3Int(0, 1, -1));
            revealCoordinates.Add(coordinates + new Vector3Int(-1, 1, 0));
            revealCoordinates.Add(coordinates + new Vector3Int(-1, 0, 1));
        }
        if (revealRadius >= 2)
        {
            revealCoordinates.Add(coordinates + new Vector3Int(0, -2, 2));
            revealCoordinates.Add(coordinates + new Vector3Int(1, -2, 1));
            revealCoordinates.Add(coordinates + new Vector3Int(2, -2, 0));
            revealCoordinates.Add(coordinates + new Vector3Int(2, -1, -1));
            revealCoordinates.Add(coordinates + new Vector3Int(2, 0, -2));
            revealCoordinates.Add(coordinates + new Vector3Int(1, 1, -2));
            revealCoordinates.Add(coordinates + new Vector3Int(0, 2, -2));
            revealCoordinates.Add(coordinates + new Vector3Int(-1, 2, -1));
            revealCoordinates.Add(coordinates + new Vector3Int(-2, 2, 0));
            revealCoordinates.Add(coordinates + new Vector3Int(-2, 1, 1));
            revealCoordinates.Add(coordinates + new Vector3Int(-2, 0, 2));
            revealCoordinates.Add(coordinates + new Vector3Int(-1, -1, 2));
        }
        if (revealRadius >= 3)
        {
            revealCoordinates.Add(coordinates + new Vector3Int(0, -3, 3));
            revealCoordinates.Add(coordinates + new Vector3Int(1, -3, 2));
            revealCoordinates.Add(coordinates + new Vector3Int(2, -3, 1));
            revealCoordinates.Add(coordinates + new Vector3Int(3, -3, 0));
            revealCoordinates.Add(coordinates + new Vector3Int(3, -2, -1));
            revealCoordinates.Add(coordinates + new Vector3Int(3, -1, -2));
            revealCoordinates.Add(coordinates + new Vector3Int(3, 0, -3));
            revealCoordinates.Add(coordinates + new Vector3Int(2, 1, -3));
            revealCoordinates.Add(coordinates + new Vector3Int(1, 2, -3));
            revealCoordinates.Add(coordinates + new Vector3Int(0, 3, -3));
            revealCoordinates.Add(coordinates + new Vector3Int(-1, 3, -2));
            revealCoordinates.Add(coordinates + new Vector3Int(-2, 3, -1));
            revealCoordinates.Add(coordinates + new Vector3Int(-3, 3, 0));
            revealCoordinates.Add(coordinates + new Vector3Int(-3, 2, 1));
            revealCoordinates.Add(coordinates + new Vector3Int(-3, 1, 2));
            revealCoordinates.Add(coordinates + new Vector3Int(-3, 0, 3));
            revealCoordinates.Add(coordinates + new Vector3Int(-2, -1, 3));
            revealCoordinates.Add(coordinates + new Vector3Int(-1, -2, 3));
        }
        if (revealRadius >= 4)
        {
            revealCoordinates.Add(coordinates + new Vector3Int(0, -4, 4));
            revealCoordinates.Add(coordinates + new Vector3Int(1, -4, 3));
            revealCoordinates.Add(coordinates + new Vector3Int(2, -4, 2));
            revealCoordinates.Add(coordinates + new Vector3Int(3, -4, 1));
            revealCoordinates.Add(coordinates + new Vector3Int(4, -4, 0));
            revealCoordinates.Add(coordinates + new Vector3Int(4, -3, -1));

            revealCoordinates.Add(coordinates + new Vector3Int(4, -2, -2));
            revealCoordinates.Add(coordinates + new Vector3Int(4, -1, -3));
            revealCoordinates.Add(coordinates + new Vector3Int(4, 0, -4));
            revealCoordinates.Add(coordinates + new Vector3Int(3, 1, -4));
            revealCoordinates.Add(coordinates + new Vector3Int(2, 2, -4));
            revealCoordinates.Add(coordinates + new Vector3Int(1, 3, -4));

            revealCoordinates.Add(coordinates + new Vector3Int(0, 4, -4));
            revealCoordinates.Add(coordinates + new Vector3Int(-1, 4, -3));
            revealCoordinates.Add(coordinates + new Vector3Int(-2, 4, -2));
            revealCoordinates.Add(coordinates + new Vector3Int(-3, 4, -1));
            revealCoordinates.Add(coordinates + new Vector3Int(-4, 4, 0));
            revealCoordinates.Add(coordinates + new Vector3Int(-4, 3, 1));

            revealCoordinates.Add(coordinates + new Vector3Int(-4, 2, 2));
            revealCoordinates.Add(coordinates + new Vector3Int(-4, 1, 3));
            revealCoordinates.Add(coordinates + new Vector3Int(-4, 0, 4));
            revealCoordinates.Add(coordinates + new Vector3Int(-3, -1, 4));
            revealCoordinates.Add(coordinates + new Vector3Int(-2, -2, 4));
            revealCoordinates.Add(coordinates + new Vector3Int(-1, -3, 4));
        }
        #endregion

        //Loop through all coordinates
        //Check if they are already created, else create them
        foreach (Vector3Int r in revealCoordinates)
        {
            HexCell c = null;
            if (cellDict.TryGetValue(r, out c))
            {
                //Nothing Yet
            }
            else
            {
                newCells.Add(CreateCellAt(r));
            }
        }
        foreach (HexCell cell in newCells)
        {
            Biome biome = Biome.Plains;
            Dictionary<Biome, float> biomes = new Dictionary<Biome, float>();
            float specialProbability = RockProbability + ForestProbability + AquiferProbability;
            biomes.Add(Biome.Plains, 1 - specialProbability);
            biomes.Add(Biome.Rock, RockProbability);
            biomes.Add(Biome.Forest, ForestProbability);
            biomes.Add(Biome.Aquifer, AquiferProbability);

            float roll = Random.Range(0, specialProbability > 1 ? specialProbability : 1);

            foreach (Biome b in biomes.Keys)
            {
                if (roll<=biomes[b])
                {
                    biome = b;
                    break;
                }
                roll -= biomes[b];
            }

            cell.SetBiome(biome);

            if (cell.transform.position.x < mapExtends.x) mapExtends.x = cell.transform.position.x;
            if (cell.transform.position.x > mapExtends.y) mapExtends.y = cell.transform.position.x;
            if (cell.transform.position.y < mapExtends.z) mapExtends.z = cell.transform.position.y;
            if (cell.transform.position.y > mapExtends.w) mapExtends.w = cell.transform.position.y;

        }
    }

    public HexCell CreateCellAt(Vector3Int c)
    {
        //float
        //    x = offsetPosition.x,
        //    y = offsetPosition.y;
        //Vector3 position;
        //position.x = (x + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f);
        //position.y = y * (HexMetrics.outerRadius * 1.5f);
        //position.z = 0f;

        Vector3 position= Vector3.zero;
        position.x = (c.x+0.5f*c.z) * (HexMetrics.innerRadius*2);
        position.y = c.z * (HexMetrics.outerRadius * 1.5f);

        HexCoordinates hexCoordinates = new HexCoordinates(c.x,c.z);
        //Vector3Int hexCoordinatesVect = hexCoordinates.AsVector3Int();

        HexCell cell = Instantiate(cellPrefab, transform);
        cell.name = "Cell at " + hexCoordinates.ToString();
        cell.transform.localPosition = position;
        cell.coordinates = hexCoordinates;
        cells.Add(cell);
        cellDict.Add(c, cell);

        return cell;
    }
}


//List<Vector3Int> revealCoordinates = new List<Vector3Int>() { coordinates };
////Get All Coordinates
//#region ADD REVEAL COORDINATES by hand because im not going to figure that shit out now
//revealCoordinates.Add(coordinates + new Vector3Int(0, -1, 1));
//revealCoordinates.Add(coordinates + new Vector3Int(1, -1, 0));
//revealCoordinates.Add(coordinates + new Vector3Int(1, 0, -1));
//revealCoordinates.Add(coordinates + new Vector3Int(0, 1, -1));
//revealCoordinates.Add(coordinates + new Vector3Int(-1, 1, 0));
//revealCoordinates.Add(coordinates + new Vector3Int(-1, 0, 1));

//revealCoordinates.Add(coordinates + new Vector3Int(0, -2, 2));
//revealCoordinates.Add(coordinates + new Vector3Int(1, -2, 1));
//revealCoordinates.Add(coordinates + new Vector3Int(2, -2, 0));
//revealCoordinates.Add(coordinates + new Vector3Int(2, -1, -1));
//revealCoordinates.Add(coordinates + new Vector3Int(2, 0, -2));
//revealCoordinates.Add(coordinates + new Vector3Int(1, 1, -2));
//revealCoordinates.Add(coordinates + new Vector3Int(0, 2, -2));
//revealCoordinates.Add(coordinates + new Vector3Int(-1, 2, -1));
//revealCoordinates.Add(coordinates + new Vector3Int(-2, 2, 0));
//revealCoordinates.Add(coordinates + new Vector3Int(-2, 1, 1));
//revealCoordinates.Add(coordinates + new Vector3Int(-2, 0, 2));
//revealCoordinates.Add(coordinates + new Vector3Int(-1, -1, 2));

//revealCoordinates.Add(coordinates + new Vector3Int(0, -3, 3));
//revealCoordinates.Add(coordinates + new Vector3Int(1, -3, 2));
//revealCoordinates.Add(coordinates + new Vector3Int(2, -3, 1));
//revealCoordinates.Add(coordinates + new Vector3Int(3, -3, 1));
//revealCoordinates.Add(coordinates + new Vector3Int(3, -3, 0));
//revealCoordinates.Add(coordinates + new Vector3Int(3, -2, -1));
//revealCoordinates.Add(coordinates + new Vector3Int(3, -1, -2));
//revealCoordinates.Add(coordinates + new Vector3Int(3, 0, -3));
//revealCoordinates.Add(coordinates + new Vector3Int(2, 1, -3));
//revealCoordinates.Add(coordinates + new Vector3Int(1, 2, -3));
//revealCoordinates.Add(coordinates + new Vector3Int(0, 3, -3));
//revealCoordinates.Add(coordinates + new Vector3Int(-1, 3, -2));
//revealCoordinates.Add(coordinates + new Vector3Int(-2, 3, -1));
//revealCoordinates.Add(coordinates + new Vector3Int(-3, 3, 0));
//revealCoordinates.Add(coordinates + new Vector3Int(-3, 2, 1));
//revealCoordinates.Add(coordinates + new Vector3Int(-3, 1, 2));
//revealCoordinates.Add(coordinates + new Vector3Int(-3, 0, 3));
//revealCoordinates.Add(coordinates + new Vector3Int(-2, -1, 3));
//revealCoordinates.Add(coordinates + new Vector3Int(-1,-2,3));
//#endregion

////Loop through all coordinates
////Check if they are already created, else create them
//foreach (Vector3Int r in revealCoordinates)
//{
//    HexCell c = null;
//    if (cellDict.TryGetValue(r,out c))
//    {

//    }
//    else
//    {
//        CreateCellAt(r);
//    }
//}