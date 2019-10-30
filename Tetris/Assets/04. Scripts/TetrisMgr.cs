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

    private Transform blockCtrlMgr;

    private bool[,] tetrisPanel = new bool[10, 22];
    public bool[,] TetrisPanel
    {
        get
        {
            return tetrisPanel;
        }
    }

    private int level = 1;
    public int Level
    {
        get
        {
            return level;
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
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Initialize()
    {
        blockCtrlMgr = GameObject.Find("BlockCtrlMgr").GetComponent<Transform>();
    }

    public void CheckLineFull()
    {
        bool _isFull = true;

        for(int j = 0; j < 20; ++j)
        {
            for(int i = 0; i < 10; ++i)
            {
                if (!tetrisPanel[i, j])
                {
                    _isFull = false;
                    break;
                }
            }

            if (_isFull)
            {
                ClearLine(j--);
            }
            else
                _isFull = true;
        }
    }

    private void ClearLine(int _line)
    {
        for(int i = blockCtrlMgr.childCount - 1; i >= 0; --i)
        {
            Transform _tr = blockCtrlMgr.GetChild(i);

            if (_tr.position.y > _line - 0.5f && _tr.position.y < _line + 0.5f)
            {
                Destroy(_tr.gameObject);
            }
            else if(_tr.position.y >= _line + 0.5f)
            {
                _tr.position += Vector3.down;
            }
        }

        for (int j = _line; j < 20; ++j)
        {
            for(int i = 0; i < 10; ++i)
            {
                tetrisPanel[i, j] = tetrisPanel[i, j + 1];
            }
        }

    }
}
