using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class Game : MonoBehaviour
{
    // Model: 存储游戏的状态和数据
    public Texture2D backgroundTexture;  // 背景纹理
    public GUIStyle textStyle;  // 自定义文本样式
    public GUIStyle buttonStyle;  // 自定义按钮样式
    public AudioClip backgroundMusic;  // 背景音乐的音频片段

    private AudioSource audioSource;   // 声明音频源
    private int[,] grid = new int[10, 20];  // 游戏网格，0: 空，1: 填充
    private Vector2Int currentPosition = new Vector2Int(4, 19);  // 当前方块位置
    private int[,] currentPiece;  // 当前方块
    private bool gameActive = false;  // 游戏是否激活状态
    private float fallTimer = 0f;
    private float fallInterval = 2f;  // 方块自动下落的间隔时间
    private bool settingsActive = false;
    private DateTime startTime;
    private int score = 0;
    private int selectedMenuIndex = 0;  // 用于记录当前选中的按钮
    private int selectedSettingsIndex = 0;  // 用于记录当前设置菜单中的选项
    private float volume = 0.0f;  // 音量
    private float[] difficultyLevels = { 1.5f, 1.0f, 0.5f };  // 三个难度级别：慢速、正常、快速
    private int selectedDifficulty = 1;  // 默认选择正常难度
    private bool gameOver = false;  // 游戏结束标志
    private DateTime gameEndTime;   // 记录游戏结束的时间
    private TimeSpan totalTime;     // 游戏总时间
    private Rect windowRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200);  // 游戏结束窗口的区域
    private int selectedButtonIndex = 0;  // 记录当前选中的按钮索引


    private List<int[,]> tetrominoes = new List<int[,]> // 常见的 4 格俄罗斯方块形状
    {
        new int[,] { { 1, 1, 1, 1 } },  // I 形
        new int[,] { { 1, 1 }, { 1, 1 } },  // O 形
        new int[,] { { 0, 1, 0 }, { 1, 1, 1 } },  // T 形
        new int[,] { { 1, 1, 0 }, { 0, 1, 1 } },  // Z 形
        new int[,] { { 0, 1, 1 }, { 1, 1, 0 } },  // S 形
        new int[,] { { 1, 1, 1 }, { 1, 0, 0 } },  // L 形
        new int[,] { { 1, 1, 1 }, { 0, 0, 1 } }   // J 形
    };

    void Start()
    {
        InitializeStyles();
        ShowMainMenu();
        GenerateInitialBlocks();

        // 初始化音频源
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;  // 使用编辑器中分配的音频片段
        audioSource.loop = true;  // 循环播放
        audioSource.volume = volume;  // 设置初始音量
        audioSource.Play();  // 播放背景音乐
    }

    // View: 绘制界面和UI
    void OnGUI()
    {
        // 获取当前屏幕宽度，动态计算缩放比例
        float scaleFactor = Screen.width / 1000f;  // 假设设计时使用1000宽度

        // 动态设置游戏结束窗口的宽高，计算内容的高度和宽度
        float windowWidth = 400 * scaleFactor;  // 动态计算窗口宽度
        float windowHeight = (150 + 100 * 2) * scaleFactor;  // 根据标签和按钮的数量动态计算窗口高度

        // 计算窗口的位置，使其居中
        windowRect = new Rect(Screen.width / 2 - windowWidth / 2, Screen.height / 2 - windowHeight / 2, windowWidth, windowHeight);

        // 绘制背景
        if (backgroundTexture != null)
        {
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture, ScaleMode.StretchToFill);
        }

        if (!gameActive && !settingsActive && !gameOver)
        {
            DrawMainMenu();
        }
        else if (settingsActive)
        {
            DrawSettingsMenu();
        }
        else if (gameOver)
        {
            windowRect = GUI.Window(0, windowRect, DrawGameOverWindow, "游戏结束");
        }
        else
        {
            DrawGameUI();
        }
    }

    //初始化界面样式
    private void InitializeStyles()
    {
        textStyle = new GUIStyle();
        textStyle.fontSize = 48;  // 字体变大
        textStyle.alignment = TextAnchor.MiddleCenter;
        textStyle.normal.textColor = Color.white;  // 使用白色字体

        buttonStyle = new GUIStyle();
        buttonStyle.fontSize = 36;  // 字体变大
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle.normal.textColor = Color.white;  // 按钮字体颜色设置为白色
        buttonStyle.normal.background = MakeTex(2, 2, new Color(0.3f, 0.4f, 0.6f));  // 使用亮色背景
        buttonStyle.hover.background = MakeTex(2, 2, new Color(0.4f, 0.5f, 0.7f));   // 悬停时的背景色
        buttonStyle.hover.textColor = Color.yellow;  // 悬停时字体颜色为黄色
    }


    //生成纹理
    private Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }

    //根据当前的难度设置返回对应的标签
    private string GetDifficultyLabel()
    {
        switch (selectedDifficulty)
        {
            case 0: return "慢速";
            case 1: return "正常";
            case 2: return "快速";
            default: return "未知";
        }
    }

    // 绘制主菜单
    private void DrawMainMenu()
    {
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 200, 200, 50), "俄罗斯方块", textStyle);

        GUIStyle highlightedButtonStyle = new GUIStyle(buttonStyle);
        highlightedButtonStyle.normal.textColor = Color.yellow;

        if (selectedMenuIndex == 0)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "开始新游戏", highlightedButtonStyle))
            {
                StartNewGame();
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "开始新游戏", buttonStyle))
            {
                StartNewGame();
            }
        }

        if (selectedMenuIndex == 1)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 60, 200, 50), "设置", highlightedButtonStyle))
            {
                ShowSettings();
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 60, 200, 50), "设置", buttonStyle))
            {
                ShowSettings();
            }
        }
    }

    // 绘制设置菜单
    private void DrawSettingsMenu()
    {
        // 获取当前屏幕宽度，动态计算缩放比例
        float scaleFactor = Screen.width / 1000f;  // 假设设计时使用1000宽度

        // 动态调整字体和按钮大小
        GUIStyle dynamicTextStyle = new GUIStyle(textStyle);
        dynamicTextStyle.fontSize = Mathf.RoundToInt(48 * scaleFactor);

        GUIStyle dynamicButtonStyle = new GUIStyle(buttonStyle);
        dynamicButtonStyle.fontSize = Mathf.RoundToInt(36 * scaleFactor);

        GUIStyle dynamicHighlightedButtonStyle = new GUIStyle(dynamicButtonStyle);
        dynamicHighlightedButtonStyle.normal.textColor = Color.yellow;

        // 使用GUILayout自动布局，并根据屏幕大小调整元素
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 150 * scaleFactor, Screen.height / 2 - 200 * scaleFactor, 300 * scaleFactor, 400 * scaleFactor));

        GUILayout.Label("设置", dynamicTextStyle);

        GUILayout.Space(20 * scaleFactor);  // 动态间距

        // 切换全屏按钮
        if (selectedSettingsIndex == 0)
        {
            if (GUILayout.Button("切换全屏", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        }
        else
        {
            if (GUILayout.Button("切换全屏", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        }

        GUILayout.Space(20 * scaleFactor);  // 动态间距

        // 音量调节按钮
        if (selectedSettingsIndex == 1)
        {
            if (GUILayout.Button($"音量: {Mathf.Round(volume * 100)}%", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                if (volume >= 1.0f)
                {
                    volume = 0.0f;
                }
                else
                {
                    volume = Mathf.Min(1f, volume + 0.1f);
                }
                audioSource.volume = volume;
            }
        }
        else
        {
            if (GUILayout.Button($"音量: {Mathf.Round(volume * 100)}%", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                if (volume >= 1.0f)
                {
                    volume = 0.0f;
                }
                else
                {
                    volume = Mathf.Min(1f, volume + 0.1f);
                }
                audioSource.volume = volume;
            }
        }

        GUILayout.Space(20 * scaleFactor);  // 动态间距

        // 难度调节按钮
        if (selectedSettingsIndex == 2)
        {
            if (GUILayout.Button($"难度: {GetDifficultyLabel()}", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                selectedDifficulty = (selectedDifficulty + 1) % 3;
                fallInterval = difficultyLevels[selectedDifficulty];
            }
        }
        else
        {
            if (GUILayout.Button($"难度: {GetDifficultyLabel()}", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                selectedDifficulty = (selectedDifficulty + 1) % 3;
                fallInterval = difficultyLevels[selectedDifficulty];
            }
        }

        GUILayout.Space(20 * scaleFactor);  // 动态间距

        // 返回按钮
        if (selectedSettingsIndex == 3)
        {
            if (GUILayout.Button("返回", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                ShowMainMenu();
            }
        }
        else
        {
            if (GUILayout.Button("返回", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                ShowMainMenu();
            }
        }

        GUILayout.EndArea();
    }


    // 绘制游戏
    private void DrawGameUI()
    {
        // 获取当前屏幕宽度，动态计算缩放比例
        float scaleFactor = Screen.width / 1000f;  // 假设设计时使用1000宽度

        // 动态调整字体和按钮大小
        GUIStyle dynamicTextStyle = new GUIStyle(textStyle);
        dynamicTextStyle.fontSize = Mathf.RoundToInt(36 * scaleFactor);

        GUIStyle dynamicButtonStyle = new GUIStyle(buttonStyle);
        dynamicButtonStyle.fontSize = Mathf.RoundToInt(28 * scaleFactor);

        // 计算棋盘的大小和位置
        int tileSize = Mathf.RoundToInt(25 * scaleFactor);  // 动态调整每个方块的大小
        int startX = Screen.width / 2 - (grid.GetLength(0) * tileSize) / 2;
        int startY = Screen.height / 2 - (grid.GetLength(1) * tileSize) / 2 + Mathf.RoundToInt(100 * scaleFactor);  // 下移100像素

        // 确定按钮和标签的位置，放置在棋盘的上方
        int uiTopY = startY - Mathf.RoundToInt(80 * scaleFactor);  // 设定一个距离，确保按钮和标签不会离棋盘太远

        // 返回主界面按钮放置在棋盘上方
        if (GUI.Button(new Rect(startX - 140, uiTopY, 150 * scaleFactor, 50 * scaleFactor), "返回主界面", dynamicButtonStyle))
        {
            ShowMainMenu();
        }

        // 分数显示放置在返回按钮的右边
        GUI.Label(new Rect(startX + 40 * scaleFactor, uiTopY, 200 * scaleFactor, 50 * scaleFactor), "分数: " + score, dynamicTextStyle);

        // 时间显示放置在分数的右边
        TimeSpan gameTime = DateTime.Now - startTime;
        GUI.Label(new Rect(startX + 240 * scaleFactor, uiTopY, 200 * scaleFactor, 50 * scaleFactor), "时间: " + gameTime.ToString(@"mm\:ss"), dynamicTextStyle);

        // 绘制棋盘
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                string label = grid[x, y] == 1 ? "X" : "";
                if (x >= currentPosition.x && x < currentPosition.x + currentPiece.GetLength(0) &&
                    y <= currentPosition.y && y > currentPosition.y - currentPiece.GetLength(1) &&
                    currentPiece[x - currentPosition.x, currentPosition.y - y] == 1)
                {
                    label = "X";
                }

                // 动态调整每个方块的位置和大小
                GUI.Button(new Rect(startX + x * tileSize, startY + (grid.GetLength(1) - 1 - y) * tileSize, tileSize, tileSize), label);
            }
        }
    }

    // 绘制游戏结束窗口
    private void DrawGameOverWindow(int windowID)
    {
        // 获取当前屏幕宽度，动态计算缩放比例
        float scaleFactor = Screen.width / 1000f;  // 假设设计时使用1000宽度

        // 动态调整字体和按钮大小
        GUIStyle dynamicTextStyle = new GUIStyle(textStyle);
        dynamicTextStyle.fontSize = Mathf.RoundToInt(48 * scaleFactor);

        GUIStyle dynamicButtonStyle = new GUIStyle(buttonStyle);
        dynamicButtonStyle.fontSize = Mathf.RoundToInt(36 * scaleFactor);

        GUIStyle dynamicHighlightedButtonStyle = new GUIStyle(dynamicButtonStyle);
        dynamicHighlightedButtonStyle.normal.textColor = Color.yellow;

        // 使用GUILayout自动布局，并根据屏幕大小调整元素
        GUILayout.BeginVertical("box", GUILayout.Width(380 * scaleFactor), GUILayout.Height(300 * scaleFactor));

        // 显示分数和游戏时间，动态调整字体
        GUILayout.Label("分数: " + score, dynamicTextStyle);
        GUILayout.Label("时间: " + totalTime.ToString(@"mm\:ss"), dynamicTextStyle);

        GUILayout.Space(10 * scaleFactor);  // 动态间距

        // "开始新游戏" 按钮
        if (selectedButtonIndex == 0)
        {
            if (GUILayout.Button("开始新游戏", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                StartNewGame();
            }
        }
        else
        {
            if (GUILayout.Button("开始新游戏", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                StartNewGame();
            }
        }

        GUILayout.Space(10 * scaleFactor);  // 动态间距

        // "返回主界面" 按钮
        if (selectedButtonIndex == 1)
        {
            if (GUILayout.Button("返回主界面", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                ShowMainMenu();
            }
        }
        else
        {
            if (GUILayout.Button("返回主界面", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                ShowMainMenu();
            }
        }

        GUILayout.EndVertical();
    }

    // Controller: 处理用户输入并更新模型
    //管理游戏的主循环
    void Update()
    {
        if (!gameActive && !settingsActive)
        {
            HandleMainMenuInput();  // 处理主菜单输入
        }
        else if (settingsActive)
        {
            HandleSettingsInput();  // 处理设置菜单输入
        }
        // 如果当前在游戏结束界面，允许使用键盘控制按钮选择
        if (gameOver || !gameActive)
        {
            HandleKeyboardNavigation();
        }


        if (gameActive)
        {
            HandleInput();
            HandleAutoFall();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMainMenu();
        }
    }

    // 处理主菜单输入
    private void HandleMainMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedMenuIndex = (selectedMenuIndex - 1 + 2) % 2;  // 循环选择“开始新游戏”和“设置”按钮
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedMenuIndex = (selectedMenuIndex + 1) % 2;  // 向下循环选择
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (selectedMenuIndex == 0)
            {
                StartNewGame();  // 选择“开始新游戏”
            }
            else if (selectedMenuIndex == 1)
            {
                ShowSettings();  // 选择“设置”
            }
        }
    }

    // 处理设置输入
    private void HandleSettingsInput()
    {
        // 键盘操作，保留之前的逻辑
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedSettingsIndex = (selectedSettingsIndex - 1 + 4) % 4;  // 循环选择四个设置选项
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedSettingsIndex = (selectedSettingsIndex + 1) % 4;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedSettingsIndex == 1)  // 音量调节
            {
                volume = Mathf.Max(0f, volume - 0.1f);  // 降低音量
                audioSource.volume = volume;  // 更新音频源的音量
            }
            else if (selectedSettingsIndex == 2)  // 难度调节
            {
                selectedDifficulty = Mathf.Max(0, selectedDifficulty - 1);  // 降低难度
                fallInterval = difficultyLevels[selectedDifficulty];
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedSettingsIndex == 1)  // 音量调节
            {
                volume = Mathf.Min(1f, volume + 0.1f);  // 提高音量
                audioSource.volume = volume;  // 更新音频源的音量
            }
            else if (selectedSettingsIndex == 2)  // 难度调节
            {
                selectedDifficulty = Mathf.Min(2, selectedDifficulty + 1);  // 提高难度
                fallInterval = difficultyLevels[selectedDifficulty];
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (selectedSettingsIndex == 0)
            {
                Screen.fullScreen = !Screen.fullScreen;  // 切换全屏/窗口模式
            }
            else if (selectedSettingsIndex == 3)
            {
                ShowMainMenu();  // 返回主菜单
            }
        }

    }

    // 处理游戏结束界面输入
    private void HandleKeyboardNavigation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + 2) % 2;  // 循环选择按钮（0和1之间循环）
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedButtonIndex = (selectedButtonIndex + 1) % 2;
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (gameOver)
            {
                if (selectedButtonIndex == 0) StartNewGame();
                else if (selectedButtonIndex == 1) ShowMainMenu();
            }
            else
            {
                // 可以处理游戏中的其他情况
            }
        }
    }

    //切换到主菜单
    private void ShowMainMenu()
    {
        gameActive = false;
        settingsActive = false;
        gameOver = false;  // 重置游戏结束标志
    }

    //开始一个新游戏
    private void StartNewGame()
    {
        gameOver = false;  // 重置游戏结束标志
        gameActive = true;
        currentPosition = new Vector2Int(4, 19);
        grid = new int[10, 20];
        fallTimer = 0f;
        score = 0;
        startTime = DateTime.Now;
        GenerateInitialBlocks();
        SpawnNewPiece();
    }

    //切换到设置
    private void ShowSettings()
    {
        settingsActive = true;
    }

    //监听用户的键盘输入，调用相应的操作来更新游戏状态
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MovePiece(Vector2Int.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MovePiece(Vector2Int.right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MovePiece(Vector2Int.down);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotatePiece();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowMainMenu();
        }
    }

    //执行方块的旋转逻辑
    private void RotatePiece()
    {
        // 临时保存旋转后的方块
        int[,] rotatedPiece = new int[currentPiece.GetLength(1), currentPiece.GetLength(0)];

        for (int x = 0; x < currentPiece.GetLength(0); x++)
        {
            for (int y = 0; y < currentPiece.GetLength(1); y++)
            {
                rotatedPiece[y, currentPiece.GetLength(0) - 1 - x] = currentPiece[x, y];
            }
        }

        // 检查旋转后的位置是否合法
        if (CanMove(currentPosition, rotatedPiece))
        {
            currentPiece = rotatedPiece;  // 如果合法，执行旋转
        }
    }

    //检查方块是否可以在新的位置放置
    private bool CanMove(Vector2Int newPosition, int[,] piece = null)
    {
        if (piece == null)
        {
            piece = currentPiece;
        }

        for (int x = 0; x < piece.GetLength(0); x++)
        {
            for (int y = 0; y < piece.GetLength(1); y++)
            {
                if (piece[x, y] == 1)
                {
                    int newX = newPosition.x + x;
                    int newY = newPosition.y - y;

                    if (newX < 0 || newX >= grid.GetLength(0) || newY < 0 || newY >= grid.GetLength(1) || grid[newX, newY] == 1)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    //方块的自动下落逻辑
    private void HandleAutoFall()
    {
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallInterval)
        {
            fallTimer = 0f;
            if (!MovePiece(Vector2Int.down))
            {
                PlacePiece();
                ClearCompleteLines();
                SpawnNewPiece();
            }
        }
    }

    //根据用户输入的方向来移动当前的方块
    private bool MovePiece(Vector2Int direction)
    {
        Vector2Int newPosition = currentPosition + direction;
        if (CanMove(newPosition))
        {
            currentPosition = newPosition;
            return true;
        }
        return false;
    }

    //辅助函数，用来检查当前方块在给定位置是否可以合法放置
    private bool CanMove(Vector2Int newPosition)
    {
        for (int x = 0; x < currentPiece.GetLength(0); x++)
        {
            for (int y = 0; y < currentPiece.GetLength(1); y++)
            {
                if (currentPiece[x, y] == 1)
                {
                    int newX = newPosition.x + x;
                    int newY = newPosition.y - y;

                    if (newX < 0 || newX >= grid.GetLength(0) || newY < 0 || newY >= grid.GetLength(1) || grid[newX, newY] == 1)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    //根据当前方块的位置和形状，将其固定在棋盘
    private void PlacePiece()
    {
        for (int x = 0; x < currentPiece.GetLength(0); x++)
        {
            for (int y = 0; y < currentPiece.GetLength(1); y++)
            {
                if (currentPiece[x, y] == 1)
                {
                    int newX = currentPosition.x + x;
                    int newY = currentPosition.y - y;

                    if (newX >= 0 && newX < grid.GetLength(0) && newY >= 0 && newY < grid.GetLength(1))
                    {
                        grid[newX, newY] = 1;
                    }
                }
            }
        }
    }

    //遍历整个棋盘，从底部向上检查每一行是否被完全填满
    private void ClearCompleteLines()
    {
        // 从底部开始检查，这样可以正确处理多个连续的完整行
        for (int y = grid.GetLength(1) - 1; y >= 0; y--)
        {
            bool lineComplete = true;
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (grid[x, y] == 0)
                {
                    lineComplete = false;
                    break;
                }
            }
            if (lineComplete)
            {
                ClearLine(y);
                // 行消除后将当前行重新检查一遍
                y++;  // 因为消除后，所有上面的行会下降，需要重新检查当前行
                score += 10;
            }
        }
    }

    //清除指定的行，将其上方的所有行下移一格
    private void ClearLine(int line)
    {
        // 将被清除行的所有格子设置为0
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            grid[x, line] = 0;
        }

        // 将消除的行上面的所有行向下移动
        for (int y = line; y < grid.GetLength(0) - 1; y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                grid[x, y] = grid[x, y + 1];
            }
        }
    }

    //随机生成一个新的方块
    private void SpawnNewPiece()
    {
        currentPiece = tetrominoes[UnityEngine.Random.Range(0, tetrominoes.Count)];
        currentPosition = new Vector2Int(4, 19);

        // 检查生成的方块是否可以放置，如果不行则游戏结束
        if (!CanMove(currentPosition, currentPiece))
        {
            GameOver();
        }
    }

    //处理游戏结束的逻辑
    private void GameOver()
    {
        gameActive = false;
        gameOver = true;
        gameEndTime = DateTime.Now;
        totalTime = gameEndTime - startTime;  // 停止计时
        Debug.Log("游戏结束");
        // 在这里添加其他游戏结束时的处理逻辑，如显示分数或重置游戏
    }

    //游戏开始时随机生成棋盘上的初始方块
    private void GenerateInitialBlocks()
    {
        for (int y = 0; y < 3; y++)
        {
            int filledCells = 0;
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                if (UnityEngine.Random.value > 0.3f && filledCells < grid.GetLength(0) - 1)
                {
                    grid[x, y] = 1;
                    filledCells++;
                }
                else
                {
                    grid[x, y] = 0;
                }
            }
        }
    }

    //private GUIStyle HighlightedButtonStyle()
    //{
    //    var highlightedButtonStyle = new GUIStyle(buttonStyle);
    //    highlightedButtonStyle.normal.textColor = Color.yellow;  // 高亮时字体为黄色
    //    return highlightedButtonStyle;
    //}
}
