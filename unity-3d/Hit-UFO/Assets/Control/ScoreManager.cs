using UnityEngine;
public class ScoreManager
{
    private static ScoreManager instance; // ����ʵ��
    private int totalScore = 0; // �ܷ���

    // ˽�й��캯������ֹ�ⲿֱ��ʵ����
    private ScoreManager() { }

    /// <summary>
    /// ��ȡ����ʵ��
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
    /// ���ӵ÷�
    /// </summary>
    public void AddScore(int score)
    {
        totalScore += score;
        Debug.Log($"Score added: {score}, Total score: {totalScore}");
    }

    /// <summary>
    /// ��ȡ��ǰ�ܷ�
    /// </summary>
    public int GetScore()
    {
        return totalScore;
    }

    /// <summary>
    /// ���÷���
    /// </summary>
    public void ResetScore()
    {
        totalScore = 0;
        Debug.Log("Score reset to 0.");
    }
}
