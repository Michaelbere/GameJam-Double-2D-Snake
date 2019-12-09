using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTileGenerator : MonoBehaviour
{

    public SnakeBodyScript snakeHead;

    private int[,] map = new int[15, 15];

    private System.Random rnd = new System.Random();
    // Start is called before the first frame update
    void Start()
    {
        // map init
        for (int row = 0; row < map.GetLength(0); row++)
        {
            for (int col = 0; col < map.GetLength(1); col++)
            {
                map[row, col] = 1; // a 30X30 full map
            }
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
                    newTile.transform.localPosition = new Vector3(col, 0, row);
                }
            }
        }
    }

    public Vector3 getFreePosition(int zCoordinate)
    {
        Vector3 position = new Vector3(0, zCoordinate, 0);
        do
        {
            position.x = rnd.Next(0, map.GetLength(1));
            position.z = rnd.Next(0, map.GetLength(0));
        } while (!availablePosition(position));
        return position;
    }

    private bool availablePosition(Vector3 position)
    {
        SnakeBodyScript cur = this.snakeHead;
        while (true)
        {
            Debug.Log(cur);
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
}
