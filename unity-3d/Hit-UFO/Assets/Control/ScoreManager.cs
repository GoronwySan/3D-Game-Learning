using UnityEngine;
public class ScoreManager
{
    private static ScoreManager instance; // 单例实例
    private int totalScore = 0; // 总分数

    // 私有构造函数，防止外部直接实例化
    private ScoreManager() { }

    /// <summary>
    /// 获取单例实例
    /// </summary>
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ScoreManager();
            }
            return instance;
        }
    }

    /// <summary>
    /// 增加得分
    /// </summary>
    public void AddScore(int score)
    {
        totalScore += score;
        Debug.Log($"Score added: {score}, Total score: {totalScore}");
    }

    /// <summary>
    /// 获取当前总分
    /// </summary>
    public int GetScore()
    {
        return totalScore;
    }

    /// <summary>
    /// 重置分数
    /// </summary>
    public void ResetScore()
    {
        totalScore = 0;
        Debug.Log("Score reset to 0.");
    }
}
