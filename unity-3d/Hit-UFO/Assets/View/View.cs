using UnityEngine;

public class View : MonoBehaviour
{
    private SSDirector ssdDirector;
    private GUIStyle bigStyle;
    private UFOController u;

    private const int LabelWidth = 100;
    private const int LabelHeight = 30;
    private const int BigFontSize = 50;
    private const int SmallFontSize = 30;

    public bool start = false;
    private int currentLevel = 1; // 当前关卡
    private float forceMultiplier = 1.0f; // 力度倍数

    public void Init()
    {
        start = true;
        Debug.Log("View initialized");
    }

    void Start()
    {
        u = FindObjectOfType<UFOController>();
        ssdDirector = SSDirector.getInstance();
        InitializeGUIStyle();
    }

    void OnGUI()
    {
        if (start)
        {
            bigStyle.normal.textColor = Color.red;
            bigStyle.fontSize = BigFontSize;

            // 显示关卡、得分和游戏状态
            GUI.Label(new Rect(0, 0, LabelWidth, LabelHeight), $"Round {currentLevel}", bigStyle);
            GUI.Label(new Rect(500, 0, LabelWidth, LabelHeight), "Total Credit: " + ScoreManager.Instance.GetScore(), bigStyle);

            if (ssdDirector.gameEnd)
            {
                // 游戏结束时显示 "重启" 和 "下一关" 按钮
                if (GUI.Button(new Rect(450, 280, 100, 50), "RESTART"))
                {
                    RestartGame();
                }

                if (GUI.Button(new Rect(450, 350, 100, 50), "NEXT LEVEL"))
                {
                    NextLevel();
                }

                GUI.Label(new Rect(120, 200, 300, 200), "Congratulations on getting " + ScoreManager.Instance.GetScore() + " Points", bigStyle);
            }
        }
    }

    private void InitializeGUIStyle()
    {
        bigStyle = new GUIStyle
        {
            normal = { textColor = Color.white },
            fontSize = SmallFontSize
        };

        ssdDirector.ReStartGame();
        ssdDirector.SetGameEnd(false); // 使用 SetGameEnd 方法重置 gameEnd 状态
    }

    private void RestartGame()
    {
        currentLevel = 1; // 重置关卡
        forceMultiplier = 1.0f; // 重置力度倍数
        ssdDirector.ReStartGame();
        ssdDirector.SetGameEnd(false);
        u.RestartAllUFOs();
        ScoreManager.Instance.ResetScore();
    }

    private void NextLevel()
    {
        currentLevel++; // 增加关卡数
        forceMultiplier += 0.5f; // 增加力度倍数
        ssdDirector.ReStartGame();
        ssdDirector.SetGameEnd(false);
        ScoreManager.Instance.ResetScore();

        // 更新飞碟的力度
        foreach (var ufo in u.ufos)
        {
            if (ufo != null)
            {
                ufo.baseForce *= forceMultiplier; // 加强飞碟力度
                ufo.RestartMotion();
            }
        }

        Debug.Log($"Starting Level {currentLevel} with force multiplier {forceMultiplier}");
    }
}
