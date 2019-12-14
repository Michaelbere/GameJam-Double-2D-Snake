using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardTileGenerator : MonoBehaviour
{

    public SnakeBodyScript snakeHead;
    public Transform renderingOffseter;

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

    public void initMapEditor()
    {
        map = new int[boardWidth, boardHeight];
        // map init
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                if (holes.Exists((HolePosition hole) => (Mathf.Abs(hole.x - row) <= 1 &&
                                                         Mathf.Abs(hole.y - col) <= 1)))
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
            DestroyImmediate(transform.GetChild(i).gameObject);
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

    void initMap()
    {
        renderingOffseter.position = new Vector3(-(boardWidth - 1) / 2, 0, -(boardHeight - 1) / 2);
        map = new int[boardWidth, boardHeight];
        // map init
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                if (holes.Exists((HolePosition hole) => (Mathf.Abs(hole.x - row) <= 1 &&
                                                         Mathf.Abs(hole.y - col) <= 1)))
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
        List<Vector3> centerNodes = new List<Vector3>();
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                if (map[row, col] == 1)
                {
                    GameObject newTile = Instantiate(Resources.Load("BoardTile")) as GameObject;
                    newTile.transform.parent = this.transform;
                    newTile.transform.localPosition = new Vector3(row, 0, col);
                    if (Mathf.Abs(row - map.GetLength(0) / 2) < 0.6 && Mathf.Abs(col - map.GetLength(1) / 2) < 0.6)
                    {
                        centerNodes.Add(newTile.transform.position);
                    }
                }
            }
        }
        if (centerNodes.Count != 0)
        {
            Vector3 average = new Vector3(centerNodes.Average(a => a.x), centerNodes.Average(a => a.y), centerNodes.Average(a => a.z)) / centerNodes.Count;
            Camera mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            Vector3 newCameraPosition = new Vector3(average.x, 21.5f * 0.8f, -34.67f * 0.8f);
            mainCamera.transform.position = newCameraPosition;
            mainCamera.transform.LookAt(Vector3.zero);
        }
        else
        {
            Debug.Log("Centering error");
        }
    }

    public Vector3 getFreePosition(int zCoordinate)
    {
        Vector3 position = new Vector3(0, zCoordinate, 0);
        do
        {
            position.x = rnd.Next(3, map.GetLength(0)-3);
            position.z = rnd.Next(3, map.GetLength(1)-3);
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