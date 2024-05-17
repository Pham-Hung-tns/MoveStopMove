using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>, IInitializeVariables
{
    public enum GameState
    { mainMenu, gameStarted, gameOver, gameWin }

    public GameState gameState;
    public List<Character> CharacterList = new List<Character>();
    public Transform[] Obstacle;
    public Camera mainCamera;
    public Camera shopCamera;
    public int TotalCharacterAmount, IsAliveAmount, KilledAmount, TotalCharAlive, SpawnAmount;
    private int LevelID;
    private GameObject currentLevel;
    private PlayerController currentPlayer;

    public PlayerController CurrentPlayer { get => currentPlayer; set => currentPlayer = value; }

    private void Awake()
    {
        CreateGame();
    }

    public void CreateGame()
    {
        SpawnAmount = 0;
        IsAliveAmount = 0;
        LevelID = PlayerPrefs.GetInt("LevelID", 1);
        AudioManager.Instance.OpenSound = true;
        InitVariables();
        InitMap();
        InitPlayer();
    }

    public void InitPlayer()
    {
        PlayerController player = Resources.Load<PlayerController>("Prefabs/Player");
        currentPlayer = Instantiate(player, currentLevel.transform.position, Quaternion.identity);
        ResetStats();
    }

    public void ResetStats()
    {
        currentPlayer.OnInit();
        CameraController.Instance.PlayerPosition = currentPlayer.transform;
    }

    private void InitMap()
    {
        GameObject level = Resources.Load<GameObject>($"Prefabs/Maps/Map_{LevelID}");
        currentLevel = Instantiate(level, transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    private void Update()
    {
        IsAliveAmount = AmountOfAliveEnemies();
        TotalCharAlive = SpawnAmount + IsAliveAmount;
    }

    public void InitVariables()
    {
        gameState = GameState.mainMenu;
        TotalCharacterAmount = 50; // so luong character mac dinh = 50
        KilledAmount = 0; // so luong character bi giet = 0.
    }

    public int AmountOfAliveEnemies() //Số enemy đang có trên map
    {
        int IsAliveAmount = 0;

        for (int i = 0; i < CharacterList.Count; i++)
        {
            if (CharacterList[i].gameObject.activeSelf)
            {
                if (CharacterList[i].IsDeath == false) IsAliveAmount++;
            }
        }
        return IsAliveAmount;
    }

    public void LoadNewLevel()
    {
        LevelID++;
        if (LevelID > 2) LevelID = 1;
        PlayerPrefs.SetInt("LevelID", LevelID);
        PlayerPrefs.Save();
        Destroy(currentLevel);
        //Destroy(currentPlayer);
        //TODO: chỗ này cần xem xét nên hiện UI nào (gameplay hay gameUI).
        InitVariables();
        InitMap();
        InitPlayer();
    }
}