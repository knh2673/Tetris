/////////////////////////////////////////////////////////////////////////////////////////////////////////
// TetrisMgr.cs
/////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]private int level = 1;
    public int Level
    {
        get
        {
            return level;
        }
    }

    private bool isGameOver;
    public bool IsGameOver
    {
        get
        {
            return isGameOver;
        }
        set
        {
            isGameOver = value;
        }
    }

    private int combo;

    [SerializeField] private Text scorePanel;
    [SerializeField] private Text levelPanel;
    [SerializeField] private Text comboPanel;
    [SerializeField] private GameObject gameOverPanel;
    private int score;

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

    private void Initialize()
    {
        blockCtrlMgr = GameObject.Find("BlockCtrlMgr").GetComponent<Transform>();
        if (blockCtrlMgr == null)
        {
            Debug.Log("TetrisMgr.cs");
            Debug.Log("void Initialize()");
            Debug.Log("Can't find BlockCtrlMgr game object.");
        }

        score = 0;
        combo = 0;

        isGameOver = false;
        gameOverPanel.SetActive(false);

        DisplayScore();
        DisplayLevel();
        DisplayCombo();
    }

    public void CheckLineFull()
    {
        bool _isFull = true;
        bool _isComboSuccess = false;
        int _multiplier = 0;

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
                _isComboSuccess = true;
                ++_multiplier;
            }
            else
                _isFull = true;
        }

        if (_isComboSuccess)
        {
            ++combo;
        }
        else
        {
            combo = 0;
        }

        CalculateScore(_multiplier);
        DisplayCombo();
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

    private void CalculateScore(int _multiplier)
    {
        score += 10 * _multiplier * (combo + 1);

        CalculateLevel();
        
        DisplayScore();
        DisplayLevel();
    }

    private void CalculateLevel()
    {
        while(score > 500 * level * level)
        {
            if (level > 20) break;

            ++level;
        }
    }

    private void DisplayScore()
    {
        scorePanel.text = "Score : " + score;
    }

    private void DisplayLevel()
    {
        levelPanel.text = "Level : " + level;
    }

    private void DisplayCombo()
    {
        comboPanel.text = "Combo : " + combo;
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
    }
}
