using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // ����ʵ��
    private static ScoreManager _instance;

    // ��ǰ����
    private int _score;

    // ��ȡ����ʵ��
    public static ScoreManager Instance
    {
        get
        {
            // ���ʵ��Ϊ�գ�����һ���µ�ʵ��
            if (_instance == null)
            {
                // ���ҵ�ǰ�������Ƿ����� ScoreManager ʵ��
                _instance = FindObjectOfType<ScoreManager>();

                // ���û�У�����һ���µ� ScoreManager ��Ϸ����
                if (_instance == null)
                {
                    GameObject obj = new GameObject("ScoreManager");
                    _instance = obj.AddComponent<ScoreManager>();
                }
            }
            return _instance;
        }
    }

    // ��ֹ�ⲿͨ�����캯������ʵ��
    private ScoreManager() { }

    // ��ȡ��ǰ����
    public int GetScore()
    {
        return _score;
    }

    // ���ӷ���
    public void AddScore(int amount)
    {
        _score += amount;
        Debug.Log($"���������� {amount}����ǰ������{_score}");
    }

    // ���ٷ���
    public void SubtractScore(int amount)
    {
        _score -= amount;
        Debug.Log($"���������� {amount}����ǰ������{_score}");
    }

    // ����������
    public void ResetScore()
    {
        _score = 0;
        Debug.Log("�����ѹ��㣡");
    }
}
