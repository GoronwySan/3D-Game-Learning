using UnityEngine;

public class CharacterModel : MonoBehaviour
{
    // 角色屬性
    public int Health { get; private set; }  // 血量
    public int Attack { get; private set; }  // 攻擊力
    public int Agility { get; private set; } // 敏捷

    // 角色名稱
    public string CharacterName { get; private set; }

    // 角色的技能外殼
    public Skill[] Skills { get; private set; }

    // 角色購買狀態
    public bool IsPurchased { get; private set; }

    // 角色的價格（假設是金幣）
    public int PurchaseCost { get; private set; }

    // 角色屬性初始化
    public CharacterModel(string name, int health, int attack, int agility, int purchaseCost)
    {
        CharacterName = name;
        Health = health;
        Attack = attack;
        Agility = agility;
        PurchaseCost = purchaseCost;

        // 暫時不實現具體技能，設置為空的技能數組
        Skills = new Skill[0]; // 暫時不實現技能，可以根據需要在後續擴展
        IsPurchased = false; // 默認未購買
    }

    // 購買角色
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

    // 顯示角色屬性
    public void DisplayCharacterStats()
    {
        Debug.Log($"Character: {CharacterName}");
        Debug.Log($"Health: {Health}, Attack: {Attack}, Agility: {Agility}");
    }
}

// 角色技能外殼，暫時不具體實現技能邏輯
[System.Serializable]
public class Skill
{
    public string SkillName;
    public string SkillDescription;

    // 可以在這裡後續擴展具體技能邏輯
}
