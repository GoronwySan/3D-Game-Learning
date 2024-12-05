using UnityEngine;

public class LevelModel : MonoBehaviour
{
    // 單例實例
    public static LevelModel Instance { get; private set; }

    // 假設最大關卡數量
    public int TotalLevels = 10;

    // 關卡解鎖狀態，0 = 未解鎖, 1 = 已解鎖
    private bool[] levelUnlocked;

    // 當前進度（已完成的關卡數）
    public int CompletedLevels { get; private set; }

    private void Awake()
    {
        // 確保 LevelModel 只有一個實例
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);  // 使 LevelModel 不會在場景切換時銷毀
        }
        else
        {
            Destroy(gameObject);  // 防止重複創建實例
        }

        // 初始化所有關卡為未解鎖
        levelUnlocked = new bool[TotalLevels];
        levelUnlocked[0] = true;  // 第一關是開放的

        CompletedLevels = 0;
        UpdateCompletedLevels();
    }

    // 更新已完成關卡數
    private void UpdateCompletedLevels()
    {
        CompletedLevels = 0;
        for (int i = 0; i < TotalLevels; i++)
        {
            if (levelUnlocked[i])
            {
                CompletedLevels++;
            }
        }
    }

    // 嘗試解鎖某個關卡，確保是前一關卡完成後才能解鎖
    public bool TryUnlockLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= TotalLevels)
        {
            Debug.LogError("Invalid level index.");
            return false;
        }

        if (levelIndex == 0 || levelUnlocked[levelIndex - 1])  // 確保前一關卡已完成
        {
            levelUnlocked[levelIndex] = true;
            UpdateCompletedLevels();
            Debug.Log($"Level {levelIndex + 1} unlocked!");
            return true;
        }
        else
        {
            Debug.Log("Previous level is not completed yet.");
            return false;
        }
    }

    // 解鎖所有關卡 (不可逆操作)
    public void UnlockAllLevels()
    {
        for (int i = 0; i < TotalLevels; i++)
        {
            levelUnlocked[i] = true;
        }
        UpdateCompletedLevels();
        Debug.Log("All levels have been unlocked!");
    }

    // 檢查某個關卡是否已解鎖
    public bool IsLevelUnlocked(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= TotalLevels)
        {
            Debug.LogError("Invalid level index.");
            return false;
        }
        return levelUnlocked[levelIndex];
    }

    // 顯示當前已解鎖的關卡
    public void DisplayUnlockedLevels()
    {
        Debug.Log("Unlocked levels:");
        for (int i = 0; i < TotalLevels; i++)
        {
            if (levelUnlocked[i])
            {
                Debug.Log($"Level {i + 1}");
            }
        }
    }
}
