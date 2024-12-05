using UnityEngine;

public class GoldModel : MonoBehaviour
{
    // ��������
    private static GoldModel _instance;

    public static GoldModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GoldModel>();  // ���ҬF�еČ���
                if (_instance == null)
                {
                    // ��������Л]�Ќ���������һ���µ�
                    GameObject obj = new GameObject("GoldModel");
                    _instance = obj.AddComponent<GoldModel>();
                }
            }
            return _instance;
        }
    }

    // ��Ү�ǰ���еĽ��Ŕ���
    public int GoldAmount { get; private set; }

    // ���ŵ�������� (���Ը��������O��)
    public int MaxGoldAmount = 100000;

    // ��ʼ�����Ŕ���
    private void Start()
    {
        // ���O��ʼ���Ŕ�����0
        GoldAmount = 0;
    }

    // ���ӽ���
    public void AddGold(int amount)
    {
        if (amount < 0) return;  // ��ֹؓ�����
        GoldAmount = Mathf.Min(GoldAmount + amount, MaxGoldAmount);  // �_�����Ų����^����
        Debug.Log($"Added {amount} gold. Current gold: {GoldAmount}");
    }

    // �p�ٽ��ţ��K�z���Ƿ����
    public bool SpendGold(int amount)
    {
        if (amount < 0) return false;  // ��ֹؓ������
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

    // �z���Ƿ����������
    public bool HasEnoughGold(int amount)
    {
        return GoldAmount >= amount;
    }

    // �@ʾ��ǰ���Ŕ���
    public void DisplayGoldAmount()
    {
        Debug.Log($"Current gold: {GoldAmount}");
    }
}
