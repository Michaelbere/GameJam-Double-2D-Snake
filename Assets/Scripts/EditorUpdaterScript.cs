using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EditorUpdaterScript : MonoBehaviour
{

    public BoardTileGenerator board;
    // Start is called before the first frame update

    void Awake() {
        board.initMapEditor();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
