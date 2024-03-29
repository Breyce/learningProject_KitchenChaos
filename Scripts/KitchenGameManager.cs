using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class KitchenGameManager : NetworkBehaviour
{


    public static KitchenGameManager Instance { get; private set; }

    public event EventHandler OnStateChange;
    public event EventHandler OnLocalGamePause;
    public event EventHandler OnLocalGameUnpause;
    public event EventHandler OnMultiplayerGamePaused;
    public event EventHandler OnMultiplayerGameUnpaused;
    public event EventHandler OnLocalPlayerReadyChanged;

    private enum State
    {
        WaitingToStart,
        CountDownToStart,
        GamePlaying,
        GameOver
    }

    [SerializeField] private Transform playerPrefab;

    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    private bool isLocalPlayerReady;
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(0f);
    private float gamePlayingTimerMax = 300f;
    private bool isLocalGamePause = false;
    private NetworkVariable<bool> isGamePaused = new NetworkVariable<bool>(false);
    private Dictionary<ulong, bool> playerReadyDictionary;
    private Dictionary<ulong, bool> playerPauseDictionary;
    private bool autoTestGamePausedState; 

    private void Awake()
    {
        Instance = this;

        playerReadyDictionary = new Dictionary<ulong, bool>();
        playerPauseDictionary = new Dictionary<ulong, bool>();
    }

    private void Start()
    {
        GameInput.Instance.OnPauseAnction += GameInput_OnPauseAnction;
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        isGamePaused.OnValueChanged += IsGamePaused_OnValueChanged;

        if (IsServer)
        {
            NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += NetworkManager_OnLoadEventCompleted;
        }
    }

    private void NetworkManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        autoTestGamePausedState = true;
    }

    private void IsGamePaused_OnValueChanged(bool previousValue, bool newValue)
    {
        if(isGamePaused.Value)
        {
            Time.timeScale = 0f;
            OnMultiplayerGamePaused?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1f;
            OnMultiplayerGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChange?.Invoke(this, EventArgs.Empty);
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(state.Value == State.WaitingToStart)
        {
            isLocalPlayerReady = true;

            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);

            SetPlayerReadyServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsIsReady = true;
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                // This player is not ready
                allClientsIsReady = false;
                break;
            }
        }

        Debug.Log("allClietsIsReady?  " + allClientsIsReady);

        if (allClientsIsReady)
        {
            state.Value = State.CountDownToStart;
        }
    }

    private void GameInput_OnPauseAnction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }


    private void Update()
    {
        if(!IsServer)
        {
            return;
        }
        switch(state.Value)
        {
            case State.WaitingToStart:
                break; 
            case State.CountDownToStart:
                countdownToStartTimer.Value -= Time.deltaTime;
                if (countdownToStartTimer.Value < 0f)
                {
                    state.Value = State.GamePlaying;
                    gamePlayingTimer.Value = gamePlayingTimerMax;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value < 0f)
                {
                    state.Value = State.GameOver;
                }
                break;
            case State.GameOver:
                break;
        }
    }

    private void LateUpdate()
    {
        if (autoTestGamePausedState)
        {
            autoTestGamePausedState = false;
            TestGamePausedState();
        }
    }

    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }

    public bool IsCountdownToStartActive()
    {
        return state.Value == State.CountDownToStart;
    }

    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer.Value;
    }

    public bool IsWaitingToStart()
    {
        return state.Value == State.WaitingToStart;
    }

    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }

    public bool IsLocalPlayerReady() 
    {
        return isLocalPlayerReady;
    }

    public float GetPlayingTimerNormalized()
    {
        return 1 - gamePlayingTimer.Value / gamePlayingTimerMax;
    }

    public void TogglePauseGame()
    {
        isLocalGamePause = !isLocalGamePause;
        if(isLocalGamePause)
        {
            PauseGameServerRpc();
            OnLocalGamePause?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            UnpauseGameServerRpc();
            OnLocalGameUnpause?.Invoke(this, EventArgs.Empty);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void PauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPauseDictionary[serverRpcParams.Receive.SenderClientId] = true;
        TestGamePausedState();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UnpauseGameServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerPauseDictionary[serverRpcParams.Receive.SenderClientId] = false;
        TestGamePausedState();
    }

    private void TestGamePausedState()
    {
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(playerPauseDictionary.ContainsKey(clientId) && playerPauseDictionary[clientId])
            {
                // this player pause
                isGamePaused.Value = true;
                return;
            }

            // all player are Unpaused
            isGamePaused.Value = false;
        }
    }
}
