using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using TMPro;

public class PlayfabManager : MonoBehaviour
{
    //HelixJumpLeaderboard
    public GameObject UsernamePanel;
    public GameObject PlayerItemPrefab;
    public TMP_InputField UserNameInput;
    public static PlayfabManager LeaderBoardInstance;

    public GameObject ball;
    public GameObject helix;
    public GameObject loadingScrene;


    [Header("PlayFab Settings")]
    public string leaderboardName = "HelixHighScoreLeaderBoard";

    public event Action<bool> OnLoginComplete;
    public event Action<bool> OnScorePosted;
    public event Action<List<PlayerLeaderboardEntry>> OnLeaderboardReceived;
    public event Action<bool> OnDisplayNameSet;

    private void Awake()
    {
        if (LeaderBoardInstance == null)
        {
            LeaderBoardInstance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoginWithDeviceID();
    }

    public void LoginWithDeviceID()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "1A6208";
        }

        string customId;
        if (PlayerPrefs.HasKey("PlayFabCustomId"))
        {
            customId = PlayerPrefs.GetString("PlayFabCustomId");
        }
        else
        {
            customId = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("PlayFabCustomId", customId);
            PlayerPrefs.Save();
        }

        var request = new LoginWithCustomIDRequest
        {
            CustomId = customId,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
    }


    private void OnLoginSuccess(LoginResult result)
    {
        OnLoginComplete?.Invoke(true);

        if (!PlayerPrefs.HasKey("Username"))
        {
            UsernamePanel.SetActive(true);
        }
        else
        {
            StartCoroutine(DelayLoad());
        }
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogError($"PlayFab login failed: {error.GenerateErrorReport()}");
        OnLoginComplete?.Invoke(false);
    }

    public void PostHighScore(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = leaderboardName,
                    Value = score
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnScorePostSuccess, OnScorePostFailure);
    }

    private void OnScorePostSuccess(UpdatePlayerStatisticsResult result)
    {
        OnScorePosted?.Invoke(true);
        GetHighScores(100);
    }

    private void OnScorePostFailure(PlayFabError error)
    {
        Debug.LogError($"Failed to post score: {error.GenerateErrorReport()}");
        OnScorePosted?.Invoke(false);
    }

    public void GetHighScores(int maxResults = 100)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = leaderboardName,
            StartPosition = 0,
            MaxResultsCount = maxResults
        };

        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardFailure);
    }

    public Transform contentParent;
    private void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        OnLeaderboardReceived?.Invoke(result.Leaderboard);

        for (int i = contentParent.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.transform.GetChild(i).gameObject);
        }
        foreach (var item in result.Leaderboard)
        {
            GameObject playerItem = Instantiate(PlayerItemPrefab, contentParent, false);
            var leaderBoardContents = playerItem.GetComponent<PlayerItemUI>();
            leaderBoardContents.rankText.text = (item.Position + 1).ToString();
            leaderBoardContents.playerNameText.text = item.DisplayName;
            leaderBoardContents.scoreText.text = item.StatValue.ToString();
        }
    }

    private void OnGetLeaderboardFailure(PlayFabError error)
    {
        Debug.LogError($"Failed to get leaderboard: {error.GenerateErrorReport()}");
        OnLeaderboardReceived?.Invoke(null);
    }
    public void OnUserNameSubmit()
    {
        if (!string.IsNullOrEmpty(UserNameInput.text))
        {
            SetUserDisplayName(UserNameInput.text);
            UsernamePanel.SetActive(false);
            helix.SetActive(true);
            ball.SetActive(true);
        }
    }
    public void SetUserDisplayName(string displayName)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = displayName
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameSuccess, OnDisplayNameFailure);
    }

    private void OnDisplayNameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log($"Display name set successfully: {result.DisplayName}");
        OnDisplayNameSet?.Invoke(true);
        PlayerPrefs.SetString("Username", result.DisplayName);
    }

    private void OnDisplayNameFailure(PlayFabError error)
    {
        Debug.LogError($"Failed to set display name: {error.GenerateErrorReport()}");
        OnDisplayNameSet?.Invoke(false);
    }

    // Utility method to get player's current leaderboard position
    public void GetPlayerLeaderboardPosition()
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = leaderboardName,
            MaxResultsCount = 1
        };

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnGetPlayerPositionSuccess, OnGetPlayerPositionFailure);
    }

    private void OnGetPlayerPositionSuccess(GetLeaderboardAroundPlayerResult result)
    {
        if (result.Leaderboard.Count > 0)
        {
            var playerEntry = result.Leaderboard[0];
            Debug.Log($"Player position: {playerEntry.Position + 1}, Score: {playerEntry.StatValue}");
        }
    }

    private void OnGetPlayerPositionFailure(PlayFabError error)
    {
        Debug.LogError($"Failed to get player position: {error.GenerateErrorReport()}");
    }
    IEnumerator DelayLoad()
    {
        UsernamePanel.SetActive(false);
        loadingScrene.SetActive(true);
        yield return new WaitForSeconds(5f);
        loadingScrene.SetActive(false);
        ball.SetActive(true);
        helix.SetActive(true);
    }
}