using UnityEngine;

public class LevelModel : MonoBehaviour
{
    // ��������
    public static LevelModel Instance { get; private set; }

    // ���O����P������
    public int TotalLevels = 10;

    // �P�����i��B��0 = δ���i, 1 = �ѽ��i
    private bool[] levelUnlocked;

    // ��ǰ�M�ȣ�����ɵ��P������
    public int CompletedLevels { get; private set; }

    private void Awake()
    {
        // �_�� LevelModel ֻ��һ������
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);  // ʹ LevelModel �����ڈ����ГQ�r�N��
        }
        else
        {
            Destroy(gameObject);  // ��ֹ���}��������
        }

        // ��ʼ�������P����δ���i
        levelUnlocked = new bool[TotalLevels];
        levelUnlocked[0] = true;  // ��һ�P���_�ŵ�

        CompletedLevels = 0;
        UpdateCompletedLevels();
    }

    // ����������P����
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

    // �Lԇ���iĳ���P�����_����ǰһ�P���������ܽ��i
    public bool TryUnlockLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= TotalLevels)
        {
            Debug.LogError("Invalid level index.");
            return false;
        }

        if (levelIndex == 0 || levelUnlocked[levelIndex - 1])  // �_��ǰһ�P�������
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

    // ���i�����P�� (���������)
    public void UnlockAllLevels()
    {
        for (int i = 0; i < TotalLevels; i++)
        {
            levelUnlocked[i] = true;
        }
        UpdateCompletedLevels();
        Debug.Log("All levels have been unlocked!");
    }

    // �z��ĳ���P���Ƿ��ѽ��i
    public bool IsLevelUnlocked(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= TotalLevels)
        {
            Debug.LogError("Invalid level index.");
            return false;
        }
        return levelUnlocked[levelIndex];
    }

    // �@ʾ��ǰ�ѽ��i���P��
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
