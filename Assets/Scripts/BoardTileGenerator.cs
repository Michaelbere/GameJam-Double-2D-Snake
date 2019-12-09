using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTileGenerator : MonoBehaviour
{

    public SnakeBodyScript snakeHead;

    private int[,] map;

    [Range(10, 30)] public int boardWidth = 20;
    [Range(10, 30)] public int boardHeight = 20;

    public List<HolePosition> holes = new List<HolePosition>();

    [System.Serializable]
    public struct HolePosition
    {
        public int x;
        public int y;
    }
    private System.Random rnd = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        initMap();
    }

    void initMap()
    {
        map = new int[boardWidth, boardHeight];
        // map init
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                if (holes.Exists((HolePosition hole) => hole.x == row && hole.y == col))
                {
                    map[row, col] = 0; // a 30X30 full map    
                }
                else
                {
                    map[row, col] = 1; // a 30X30 full map
                }
            }
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        // tile creation
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                if (map[row, col] == 1)
                {
                    GameObject newTile = Instantiate(Resources.Load("BoardTile")) as GameObject;
                    newTile.transform.parent = this.transform;
                    newTile.transform.localPosition = new Vector3(row, 0, col);
                }
            }
        }
    }

    public Vector3 getFreePosition(int zCoordinate)
    {
        Vector3 position = new Vector3(0, zCoordinate, 0);
        do
        {
            position.x = rnd.Next(0, map.GetLength(0));
            position.z = rnd.Next(0, map.GetLength(1));
        } while (!availablePosition(position));
        return position;
    }

    private bool availablePosition(Vector3 position)
    {
        SnakeBodyScript cur = this.snakeHead;
        while (true)
        {
            if (cur.GetComponent<Transform>().position.Equals(position))
            {
                return false;
            }
            if (cur.nextBodyPart)
            {
                cur = cur.nextBodyPart.GetComponent<SnakeBodyScript>();
            }
            else
            {
                break;
            }
        }
        return true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int getWidth()
    {
        return map.GetLength(0);
    }
    public int getHeight()
    {
        return map.GetLength(1);
    }

    public bool isHole(Vector3 position)
    {
        return this.map[(int)position.x, (int)position.z] == 0;
    }
}
