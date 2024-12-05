using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    // ��ɫ����
    public int Health { get; private set; }  // Ѫ��
    public int Attack { get; private set; }  // ������
    public int Agility { get; private set; } // ����

    // ��ɫ���Q
    public string CharacterName { get; private set; }

    // ��ɫ�ļ����⚤
    public Skill[] Skills { get; private set; }

    // ��ɫُ�I��B
    public bool IsPurchased { get; private set; }

    // ��ɫ�ăr�񣨼��O�ǽ��ţ�
    public int PurchaseCost { get; private set; }

    // ��ɫ���Գ�ʼ��
    public CharacterModel(string name, int health, int attack, int agility, int purchaseCost)
    {
        CharacterName = name;
        Health = health;
        Attack = attack;
        Agility = agility;
        PurchaseCost = purchaseCost;

        // ���r�����F���w���ܣ��O�Þ�յļ��ܔ��M
        Skills = new Skill[0]; // ���r�����F���ܣ����Ը�����Ҫ�����m�Uչ
        IsPurchased = false; // Ĭ�Jδُ�I
    }

    // ُ�I��ɫ
    public void Purchase()
    {
        if (!IsPurchased)
        {
            IsPurchased = true;
            Debug.Log($"{CharacterName} has been purchased!");
        }
        else
        {
            Debug.Log($"{CharacterName} has already been purchased.");
        }
    }

    // �@ʾ��ɫ����
    public void DisplayCharacterStats()
    {
        Debug.Log($"Character: {CharacterName}");
        Debug.Log($"Health: {Health}, Attack: {Attack}, Agility: {Agility}");
    }
}

// ��ɫ�����⚤�����r�����w���F����߉݋
[System.Serializable]
public class Skill
{
    public string SkillName;
    public string SkillDescription;

    // �������@�e���m�Uչ���w����߉݋
}
