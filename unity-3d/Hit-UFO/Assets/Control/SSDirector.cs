using UnityEngine;

public class SSDirector : System.Object
{
    // 单例实例
    private static SSDirector _instance;
    public static SSDirector getInstance()
    {
        if (_instance == null)
        {
            _instance = new SSDirector();
        }
        return _instance;
    }

    public FirstController firstcontroller { get; set; }
    public CCActionManager ccam { get; set; }

    private int creditCount;
    public bool gameEnd { get; private set; } = false;

    private SSDirector()
    {
        creditCount = 0;
    }

    public void AddCreditCount(int num)
    {
        creditCount += num;
    }

    public int GetCreditCount()
    {
        return creditCount;
    }

    public void ReStartGame()
    {
        firstcontroller?.ReStartGame();
        ccam?.ClearAllList();
        ResetState();
    }

    private void ResetState()
    {
        creditCount = 0;
        gameEnd = false;
    }

    public void SetGameEnd(bool end)
    {
        gameEnd = end;
    }

}
