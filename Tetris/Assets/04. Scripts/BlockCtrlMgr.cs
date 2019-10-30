/////////////////////////////////////////////////////////////////////////////////////////////////////////
// BlockCtrlMgr.cs
/////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCtrlMgr : MonoBehaviour
{
    [SerializeField] private GameObject[] prefebs = new GameObject[7];

    [SerializeField] private WaitForSeconds waitSec;

    private GameObject block;
    private List<GameObject> blockList;
    private GameObject blockMgr;
    private bool[,] tetrisPanel;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();

        PullDownBlockEverySeconds();
    }

    private void InputKey()
    {
        if (TetrisMgr.Instance.IsGameOver) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Debug.Log("left");
            MoveBlockLeftRight(KeyCode.LeftArrow);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            //Debug.Log("right");
            MoveBlockLeftRight(KeyCode.RightArrow);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            //Debug.Log("Up");
            RotateBlock();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            //Debug.Log("Down");
            PullDownBlockOnce();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Space");
            AttachImmediately();
        }
    }

    private void Initialize()
    {
        waitSec = new WaitForSeconds(2.0f);
        blockMgr = GameObject.Find("BlockCtrlMgr");

        tetrisPanel = TetrisMgr.Instance.TetrisPanel;

        time = 0.0f;

        block = CreateBlock();

        block.transform.position = new Vector3(-0.01f, 19.0f, 5.0f);

        UpdateBlockToTetrisPanel(true);


        blockList = new List<GameObject>();

        for(int i = 0; i < 3; ++i)
        {
            blockList.Add(CreateBlock());

            blockList[i].transform.position += Vector3.back * 7.0f * (2 - i);
        }
    }

    private GameObject CreateBlock()
    {
        int _prefebNum = Random.Range(0, 7);
        GameObject _block = Instantiate<GameObject>(prefebs[_prefebNum]); // create block object on screen.

        _block.transform.position = new Vector3(-0.01f, 10.0f, 30.0f); // set block's position to spawn point.

        return _block;
    }

    private bool PullDownBlockOnce()
    {
        UpdateBlockToTetrisPanel(false); // update current block position to tetris panel to false.
        
        block.transform.position += Vector3.down;

        if (!CheckBlockCrash()) // check block crash.
        {
            block.transform.position += Vector3.up;
            UpdateBlockToTetrisPanel(true); // update current block position to tetris panel to true.

            return false;
        }

        UpdateBlockToTetrisPanel(true); // update current block position to tetris panel to true.
        
        return true;
    }

    private bool MoveBlockLeftRight(KeyCode key)
    {
        Vector3 _dir = Vector3.zero; // initialize.

        if(key == KeyCode.LeftArrow) // player push left arrow.
        {
            _dir = Vector3.back;
        }
        else if(key == KeyCode.RightArrow) // player push right arrow.
        {
            _dir = Vector3.forward;
        }

        UpdateBlockToTetrisPanel(false); // update current block position to tetris panel to false.

        block.transform.position += _dir;

        if (!CheckBlockCrash()) // check block crash.
        {
            block.transform.position += -_dir;
            UpdateBlockToTetrisPanel(true); // update current block position to tetris panel to true.

            return false;
        }

        UpdateBlockToTetrisPanel(true); // update current block position to tetris panel to true.

        return true;
    }
    
    private bool RotateBlock()
    {
        Vector3 _degree = new Vector3(90.0f , 0.0f, 0.0f);

        UpdateBlockToTetrisPanel(false); // update current block position to tetris panel to false.

        block.transform.Rotate(_degree, Space.World);

        if (!CheckBlockCrash()) // check block crash.
        {
            block.transform.Rotate(-_degree, Space.World);
            UpdateBlockToTetrisPanel(true); // update current block position to tetris panel to true.

            return false;
        }

        UpdateBlockToTetrisPanel(true); // update current block position to tetris panel to true.

        return true;
    }

    private bool AttachImmediately()
    {
        while (PullDownBlockOnce()) ;

        int _childCount = block.transform.childCount;

        for (int i = _childCount - 1; i >= 0; --i)
        {
            Transform _tr = block.transform.GetChild(i);

            _tr.parent = blockMgr.transform;

            Debug.Log(_tr.position);
        }

        TetrisMgr.Instance.CheckLineFull();

        UpdateToControlNewBlock();

        time = 0.0f;

        return true;
    }

    private void UpdateBlockToTetrisPanel(bool _setting)
    {
        for (int i = 0; i < block.transform.childCount; ++i)
        {
            Transform _tr = block.transform.GetChild(i); // child object's Transform component.
            
            //Debug.Log(Mathf.Round(_tr.position.z) + "," + Mathf.Round(_tr.position.y));
            tetrisPanel[(int)Mathf.Round(_tr.position.z), (int)Mathf.Round(_tr.position.y)] = _setting; // set tetris panel.
        }
    }

    private bool CheckBlockCrash()
    {
        for (int i = 0; i < block.transform.childCount; ++i)
        {
            Transform _tr = block.transform.GetChild(i); // child object's Transform component.

            if ((int)Mathf.Round(_tr.position.y) < 0) // crash into floor.
            {
                Debug.Log("Floor");
                return false;
            }
            if((int)Mathf.Round(_tr.position.z) < 0 || (int)Mathf.Round(_tr.position.z) > 9) // crash into wall.
            {
                Debug.Log("Wall");
                return false;
            }
            if (tetrisPanel[(int)Mathf.Round(_tr.position.z), (int)Mathf.Round(_tr.position.y)] == true) // crash into other block.
            {
                Debug.Log("Crash");
                Debug.Log(_tr.position.z + "," + _tr.position.y);
                return false;
            }
        }

        return true;
    }

    private void PullDownBlockEverySeconds()
    {
        if (TetrisMgr.Instance.IsGameOver) return;

        time += Time.deltaTime;
        if(time < 2.0f - TetrisMgr.Instance.Level * 0.09f)
            return;
        Debug.Log(TetrisMgr.Instance.Level);

        if (block != null)
        {
            if (!PullDownBlockOnce())
            {
                int _childCount = block.transform.childCount;

                for (int i = _childCount-1; i >= 0; --i)
                {
                    Transform _tr = block.transform.GetChild(i);

                    _tr.parent = blockMgr.transform;

                    Debug.Log(_tr.position);
                }

                TetrisMgr.Instance.CheckLineFull();

                UpdateToControlNewBlock();
            }
        }

        time = 0.0f;
    }

    private bool UpdateToControlNewBlock()
    {
        Destroy(block);

        block = blockList[0];
        block.transform.position = new Vector3(-0.01f, 19.0f, 5.0f);

        blockList.Remove(blockList[0]);
        blockList.Add(CreateBlock());
        
        for (int i = 0; i < 2; ++i)
        {
            blockList[i].transform.position += Vector3.back * 7.0f;
        }

        // Game Over
        if (!CheckBlockCrash())
        {
            TetrisMgr.Instance.IsGameOver = true;
            TetrisMgr.Instance.GameOver();
            Destroy(block);
            block = null;

            return false;
        }

        UpdateBlockToTetrisPanel(true);

        return true;
    }
}
