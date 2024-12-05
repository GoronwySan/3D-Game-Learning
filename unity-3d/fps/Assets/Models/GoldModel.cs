using UnityEngine;

public class GoldModel : MonoBehaviour
{
    // 單例實例
    private static GoldModel _instance;

    public static GoldModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GoldModel>();  // 查找現有的實例
                if (_instance == null)
                {
                    // 如果場景中沒有實例，創建一個新的
                    GameObject obj = new GameObject("GoldModel");
                    _instance = obj.AddComponent<GoldModel>();
                }
            }
            return _instance;
        }
    }

    // 玩家當前擁有的金幣數量
    public int GoldAmount { get; private set; }

    // 金幣的最大上限 (可以根據需求設置)
    public int MaxGoldAmount = 100000;

    // 初始化金幣數量
    private void Start()
    {
        // 假設初始金幣數量是0
        GoldAmount = 0;
    }

    // 增加金幣
    public void AddGold(int amount)
    {
        if (amount < 0) return;  // 防止負數添加
        GoldAmount = Mathf.Min(GoldAmount + amount, MaxGoldAmount);  // 確保金幣不超過上限
        Debug.Log($"Added {amount} gold. Current gold: {GoldAmount}");
    }

    // 減少金幣，並檢查是否足夠
    public bool SpendGold(int amount)
    {
        if (amount < 0) return false;  // 防止負數消耗
        if (GoldAmount >= amount)
        {
            GoldAmount -= amount;
            Debug.Log($"Spent {amount} gold. Remaining gold: {GoldAmount}");
            return true;
        }
        else
        {
            Debug.Log("Not enough gold to spend.");
            return false;
        }
    }

    // 檢查是否擁有足夠金幣
    public bool HasEnoughGold(int amount)
    {
        return GoldAmount >= amount;
    }

    // 顯示當前金幣數量
    public void DisplayGoldAmount()
    {
        Debug.Log($"Current gold: {GoldAmount}");
    }
}
