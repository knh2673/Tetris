/////////////////////////////////////////////////////////////////////////////////////////////////////////
// TetrisMgr.cs
/////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisMgr : MonoBehaviour
{
    private static TetrisMgr instance = null;
    public static TetrisMgr Instance
    {
        get
        {
            return instance;
        }
    }

    private bool[,] tetrisPanel = new bool[10, 22];
    public bool[,] TetrisPanel
    {
        get
        {
            return tetrisPanel;
        }
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }


}
