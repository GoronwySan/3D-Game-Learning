using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // 单例实例
    private static ScoreManager _instance;

    // 当前分数
    private int _score;

    // 获取单例实例
    public static ScoreManager Instance
    {
        get
        {
            // 如果实例为空，创建一个新的实例
            if (_instance == null)
            {
                // 查找当前场景中是否已有 ScoreManager 实例
                _instance = FindObjectOfType<ScoreManager>();

                // 如果没有，创建一个新的 ScoreManager 游戏对象
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ScoreManager");
                    _instance = obj.AddComponent<ScoreManager>();
                }
            }
            return _instance;
        }
    }

    // 禁止外部通过构造函数创建实例
    private ScoreManager() { }

    // 获取当前分数
    public int GetScore()
    {
        return _score;
    }

    // 增加分数
    public void AddScore(int amount)
    {
        _score += amount;
        Debug.Log($"分数增加了 {amount}，当前分数：{_score}");
    }

    // 减少分数
    public void SubtractScore(int amount)
    {
        _score -= amount;
        Debug.Log($"分数减少了 {amount}，当前分数：{_score}");
    }

    // 将分数归零
    public void ResetScore()
    {
        _score = 0;
        Debug.Log("分数已归零！");
    }
}
