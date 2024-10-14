using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

public class Game : MonoBehaviour
{
    // Model: �洢��Ϸ��״̬������
    public Texture2D backgroundTexture;  // ��������
    public GUIStyle textStyle;  // �Զ����ı���ʽ
    public GUIStyle buttonStyle;  // �Զ��尴ť��ʽ
    public AudioClip backgroundMusic;  // �������ֵ���ƵƬ��

    private AudioSource audioSource;   // ������ƵԴ
    private int[,] grid = new int[10, 20];  // ��Ϸ����0: �գ�1: ���
    private Vector2Int currentPosition = new Vector2Int(4, 19);  // ��ǰ����λ��
    private int[,] currentPiece;  // ��ǰ����
    private bool gameActive = false;  // ��Ϸ�Ƿ񼤻�״̬
    private float fallTimer = 0f;
    private float fallInterval = 2f;  // �����Զ�����ļ��ʱ��
    private bool settingsActive = false;
    private DateTime startTime;
    private int score = 0;
    private int selectedMenuIndex = 0;  // ���ڼ�¼��ǰѡ�еİ�ť
    private int selectedSettingsIndex = 0;  // ���ڼ�¼��ǰ���ò˵��е�ѡ��
    private float volume = 0.0f;  // ����
    private float[] difficultyLevels = { 1.5f, 1.0f, 0.5f };  // �����Ѷȼ������١�����������
    private int selectedDifficulty = 1;  // Ĭ��ѡ�������Ѷ�
    private bool gameOver = false;  // ��Ϸ������־
    private DateTime gameEndTime;   // ��¼��Ϸ������ʱ��
    private TimeSpan totalTime;     // ��Ϸ��ʱ��
    private Rect windowRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 - 100, 300, 200);  // ��Ϸ�������ڵ�����
    private int selectedButtonIndex = 0;  // ��¼��ǰѡ�еİ�ť����


    private List<int[,]> tetrominoes = new List<int[,]> // ������ 4 �����˹������״
    {
        new int[,] { { 1, 1, 1, 1 } },  // I ��
        new int[,] { { 1, 1 }, { 1, 1 } },  // O ��
        new int[,] { { 0, 1, 0 }, { 1, 1, 1 } },  // T ��
        new int[,] { { 1, 1, 0 }, { 0, 1, 1 } },  // Z ��
        new int[,] { { 0, 1, 1 }, { 1, 1, 0 } },  // S ��
        new int[,] { { 1, 1, 1 }, { 1, 0, 0 } },  // L ��
        new int[,] { { 1, 1, 1 }, { 0, 0, 1 } }   // J ��
    };

    void Start()
    {
        InitializeStyles();
        ShowMainMenu();
        GenerateInitialBlocks();

        // ��ʼ����ƵԴ
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = backgroundMusic;  // ʹ�ñ༭���з������ƵƬ��
        audioSource.loop = true;  // ѭ������
        audioSource.volume = volume;  // ���ó�ʼ����
        audioSource.Play();  // ���ű�������
    }

    // View: ���ƽ����UI
    void OnGUI()
    {
        // ��ȡ��ǰ��Ļ��ȣ���̬�������ű���
        float scaleFactor = Screen.width / 1000f;  // �������ʱʹ��1000���

        // ��̬������Ϸ�������ڵĿ�ߣ��������ݵĸ߶ȺͿ��
        float windowWidth = 400 * scaleFactor;  // ��̬���㴰�ڿ��
        float windowHeight = (150 + 100 * 2) * scaleFactor;  // ���ݱ�ǩ�Ͱ�ť��������̬���㴰�ڸ߶�

        // ���㴰�ڵ�λ�ã�ʹ�����
        windowRect = new Rect(Screen.width / 2 - windowWidth / 2, Screen.height / 2 - windowHeight / 2, windowWidth, windowHeight);

        // ���Ʊ���
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
            windowRect = GUI.Window(0, windowRect, DrawGameOverWindow, "��Ϸ����");
        }
        else
        {
            DrawGameUI();
        }
    }

    //��ʼ��������ʽ
    private void InitializeStyles()
    {
        textStyle = new GUIStyle();
        textStyle.fontSize = 48;  // ������
        textStyle.alignment = TextAnchor.MiddleCenter;
        textStyle.normal.textColor = Color.white;  // ʹ�ð�ɫ����

        buttonStyle = new GUIStyle();
        buttonStyle.fontSize = 36;  // ������
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle.normal.textColor = Color.white;  // ��ť������ɫ����Ϊ��ɫ
        buttonStyle.normal.background = MakeTex(2, 2, new Color(0.3f, 0.4f, 0.6f));  // ʹ����ɫ����
        buttonStyle.hover.background = MakeTex(2, 2, new Color(0.4f, 0.5f, 0.7f));   // ��ͣʱ�ı���ɫ
        buttonStyle.hover.textColor = Color.yellow;  // ��ͣʱ������ɫΪ��ɫ
    }


    //��������
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

    //���ݵ�ǰ���Ѷ����÷��ض�Ӧ�ı�ǩ
    private string GetDifficultyLabel()
    {
        switch (selectedDifficulty)
        {
            case 0: return "����";
            case 1: return "����";
            case 2: return "����";
            default: return "δ֪";
        }
    }

    // �������˵�
    private void DrawMainMenu()
    {
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 200, 200, 50), "����˹����", textStyle);

        GUIStyle highlightedButtonStyle = new GUIStyle(buttonStyle);
        highlightedButtonStyle.normal.textColor = Color.yellow;

        if (selectedMenuIndex == 0)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "��ʼ����Ϸ", highlightedButtonStyle))
            {
                StartNewGame();
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "��ʼ����Ϸ", buttonStyle))
            {
                StartNewGame();
            }
        }

        if (selectedMenuIndex == 1)
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 60, 200, 50), "����", highlightedButtonStyle))
            {
                ShowSettings();
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 + 60, 200, 50), "����", buttonStyle))
            {
                ShowSettings();
            }
        }
    }

    // �������ò˵�
    private void DrawSettingsMenu()
    {
        // ��ȡ��ǰ��Ļ��ȣ���̬�������ű���
        float scaleFactor = Screen.width / 1000f;  // �������ʱʹ��1000���

        // ��̬��������Ͱ�ť��С
        GUIStyle dynamicTextStyle = new GUIStyle(textStyle);
        dynamicTextStyle.fontSize = Mathf.RoundToInt(48 * scaleFactor);

        GUIStyle dynamicButtonStyle = new GUIStyle(buttonStyle);
        dynamicButtonStyle.fontSize = Mathf.RoundToInt(36 * scaleFactor);

        GUIStyle dynamicHighlightedButtonStyle = new GUIStyle(dynamicButtonStyle);
        dynamicHighlightedButtonStyle.normal.textColor = Color.yellow;

        // ʹ��GUILayout�Զ����֣���������Ļ��С����Ԫ��
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 150 * scaleFactor, Screen.height / 2 - 200 * scaleFactor, 300 * scaleFactor, 400 * scaleFactor));

        GUILayout.Label("����", dynamicTextStyle);

        GUILayout.Space(20 * scaleFactor);  // ��̬���

        // �л�ȫ����ť
        if (selectedSettingsIndex == 0)
        {
            if (GUILayout.Button("�л�ȫ��", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        }
        else
        {
            if (GUILayout.Button("�л�ȫ��", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                Screen.fullScreen = !Screen.fullScreen;
            }
        }

        GUILayout.Space(20 * scaleFactor);  // ��̬���

        // �������ڰ�ť
        if (selectedSettingsIndex == 1)
        {
            if (GUILayout.Button($"����: {Mathf.Round(volume * 100)}%", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
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
            if (GUILayout.Button($"����: {Mathf.Round(volume * 100)}%", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
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

        GUILayout.Space(20 * scaleFactor);  // ��̬���

        // �Ѷȵ��ڰ�ť
        if (selectedSettingsIndex == 2)
        {
            if (GUILayout.Button($"�Ѷ�: {GetDifficultyLabel()}", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                selectedDifficulty = (selectedDifficulty + 1) % 3;
                fallInterval = difficultyLevels[selectedDifficulty];
            }
        }
        else
        {
            if (GUILayout.Button($"�Ѷ�: {GetDifficultyLabel()}", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                selectedDifficulty = (selectedDifficulty + 1) % 3;
                fallInterval = difficultyLevels[selectedDifficulty];
            }
        }

        GUILayout.Space(20 * scaleFactor);  // ��̬���

        // ���ذ�ť
        if (selectedSettingsIndex == 3)
        {
            if (GUILayout.Button("����", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                ShowMainMenu();
            }
        }
        else
        {
            if (GUILayout.Button("����", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                ShowMainMenu();
            }
        }

        GUILayout.EndArea();
    }


    // ������Ϸ
    private void DrawGameUI()
    {
        // ��ȡ��ǰ��Ļ��ȣ���̬�������ű���
        float scaleFactor = Screen.width / 1000f;  // �������ʱʹ��1000���

        // ��̬��������Ͱ�ť��С
        GUIStyle dynamicTextStyle = new GUIStyle(textStyle);
        dynamicTextStyle.fontSize = Mathf.RoundToInt(36 * scaleFactor);

        GUIStyle dynamicButtonStyle = new GUIStyle(buttonStyle);
        dynamicButtonStyle.fontSize = Mathf.RoundToInt(28 * scaleFactor);

        // �������̵Ĵ�С��λ��
        int tileSize = Mathf.RoundToInt(25 * scaleFactor);  // ��̬����ÿ������Ĵ�С
        int startX = Screen.width / 2 - (grid.GetLength(0) * tileSize) / 2;
        int startY = Screen.height / 2 - (grid.GetLength(1) * tileSize) / 2 + Mathf.RoundToInt(100 * scaleFactor);  // ����100����

        // ȷ����ť�ͱ�ǩ��λ�ã����������̵��Ϸ�
        int uiTopY = startY - Mathf.RoundToInt(80 * scaleFactor);  // �趨һ�����룬ȷ����ť�ͱ�ǩ����������̫Զ

        // ���������水ť�����������Ϸ�
        if (GUI.Button(new Rect(startX - 140, uiTopY, 150 * scaleFactor, 50 * scaleFactor), "����������", dynamicButtonStyle))
        {
            ShowMainMenu();
        }

        // ������ʾ�����ڷ��ذ�ť���ұ�
        GUI.Label(new Rect(startX + 40 * scaleFactor, uiTopY, 200 * scaleFactor, 50 * scaleFactor), "����: " + score, dynamicTextStyle);

        // ʱ����ʾ�����ڷ������ұ�
        TimeSpan gameTime = DateTime.Now - startTime;
        GUI.Label(new Rect(startX + 240 * scaleFactor, uiTopY, 200 * scaleFactor, 50 * scaleFactor), "ʱ��: " + gameTime.ToString(@"mm\:ss"), dynamicTextStyle);

        // ��������
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

                // ��̬����ÿ�������λ�úʹ�С
                GUI.Button(new Rect(startX + x * tileSize, startY + (grid.GetLength(1) - 1 - y) * tileSize, tileSize, tileSize), label);
            }
        }
    }

    // ������Ϸ��������
    private void DrawGameOverWindow(int windowID)
    {
        // ��ȡ��ǰ��Ļ��ȣ���̬�������ű���
        float scaleFactor = Screen.width / 1000f;  // �������ʱʹ��1000���

        // ��̬��������Ͱ�ť��С
        GUIStyle dynamicTextStyle = new GUIStyle(textStyle);
        dynamicTextStyle.fontSize = Mathf.RoundToInt(48 * scaleFactor);

        GUIStyle dynamicButtonStyle = new GUIStyle(buttonStyle);
        dynamicButtonStyle.fontSize = Mathf.RoundToInt(36 * scaleFactor);

        GUIStyle dynamicHighlightedButtonStyle = new GUIStyle(dynamicButtonStyle);
        dynamicHighlightedButtonStyle.normal.textColor = Color.yellow;

        // ʹ��GUILayout�Զ����֣���������Ļ��С����Ԫ��
        GUILayout.BeginVertical("box", GUILayout.Width(380 * scaleFactor), GUILayout.Height(300 * scaleFactor));

        // ��ʾ��������Ϸʱ�䣬��̬��������
        GUILayout.Label("����: " + score, dynamicTextStyle);
        GUILayout.Label("ʱ��: " + totalTime.ToString(@"mm\:ss"), dynamicTextStyle);

        GUILayout.Space(10 * scaleFactor);  // ��̬���

        // "��ʼ����Ϸ" ��ť
        if (selectedButtonIndex == 0)
        {
            if (GUILayout.Button("��ʼ����Ϸ", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                StartNewGame();
            }
        }
        else
        {
            if (GUILayout.Button("��ʼ����Ϸ", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                StartNewGame();
            }
        }

        GUILayout.Space(10 * scaleFactor);  // ��̬���

        // "����������" ��ť
        if (selectedButtonIndex == 1)
        {
            if (GUILayout.Button("����������", dynamicHighlightedButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                ShowMainMenu();
            }
        }
        else
        {
            if (GUILayout.Button("����������", dynamicButtonStyle, GUILayout.Height(50 * scaleFactor)))
            {
                ShowMainMenu();
            }
        }

        GUILayout.EndVertical();
    }

    // Controller: �����û����벢����ģ��
    //������Ϸ����ѭ��
    void Update()
    {
        if (!gameActive && !settingsActive)
        {
            HandleMainMenuInput();  // �������˵�����
        }
        else if (settingsActive)
        {
            HandleSettingsInput();  // �������ò˵�����
        }
        // �����ǰ����Ϸ�������棬����ʹ�ü��̿��ư�ťѡ��
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

    // �������˵�����
    private void HandleMainMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedMenuIndex = (selectedMenuIndex - 1 + 2) % 2;  // ѭ��ѡ�񡰿�ʼ����Ϸ���͡����á���ť
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedMenuIndex = (selectedMenuIndex + 1) % 2;  // ����ѭ��ѡ��
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (selectedMenuIndex == 0)
            {
                StartNewGame();  // ѡ�񡰿�ʼ����Ϸ��
            }
            else if (selectedMenuIndex == 1)
            {
                ShowSettings();  // ѡ�����á�
            }
        }
    }

    // ������������
    private void HandleSettingsInput()
    {
        // ���̲���������֮ǰ���߼�
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedSettingsIndex = (selectedSettingsIndex - 1 + 4) % 4;  // ѭ��ѡ���ĸ�����ѡ��
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedSettingsIndex = (selectedSettingsIndex + 1) % 4;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectedSettingsIndex == 1)  // ��������
            {
                volume = Mathf.Max(0f, volume - 0.1f);  // ��������
                audioSource.volume = volume;  // ������ƵԴ������
            }
            else if (selectedSettingsIndex == 2)  // �Ѷȵ���
            {
                selectedDifficulty = Mathf.Max(0, selectedDifficulty - 1);  // �����Ѷ�
                fallInterval = difficultyLevels[selectedDifficulty];
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectedSettingsIndex == 1)  // ��������
            {
                volume = Mathf.Min(1f, volume + 0.1f);  // �������
                audioSource.volume = volume;  // ������ƵԴ������
            }
            else if (selectedSettingsIndex == 2)  // �Ѷȵ���
            {
                selectedDifficulty = Mathf.Min(2, selectedDifficulty + 1);  // ����Ѷ�
                fallInterval = difficultyLevels[selectedDifficulty];
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (selectedSettingsIndex == 0)
            {
                Screen.fullScreen = !Screen.fullScreen;  // �л�ȫ��/����ģʽ
            }
            else if (selectedSettingsIndex == 3)
            {
                ShowMainMenu();  // �������˵�
            }
        }

    }

    // ������Ϸ������������
    private void HandleKeyboardNavigation()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedButtonIndex = (selectedButtonIndex - 1 + 2) % 2;  // ѭ��ѡ��ť��0��1֮��ѭ����
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
                // ���Դ�����Ϸ�е��������
            }
        }
    }

    //�л������˵�
    private void ShowMainMenu()
    {
        gameActive = false;
        settingsActive = false;
        gameOver = false;  // ������Ϸ������־
    }

    //��ʼһ������Ϸ
    private void StartNewGame()
    {
        gameOver = false;  // ������Ϸ������־
        gameActive = true;
        currentPosition = new Vector2Int(4, 19);
        grid = new int[10, 20];
        fallTimer = 0f;
        score = 0;
        startTime = DateTime.Now;
        GenerateInitialBlocks();
        SpawnNewPiece();
    }

    //�л�������
    private void ShowSettings()
    {
        settingsActive = true;
    }

    //�����û��ļ������룬������Ӧ�Ĳ�����������Ϸ״̬
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

    //ִ�з������ת�߼�
    private void RotatePiece()
    {
        // ��ʱ������ת��ķ���
        int[,] rotatedPiece = new int[currentPiece.GetLength(1), currentPiece.GetLength(0)];

        for (int x = 0; x < currentPiece.GetLength(0); x++)
        {
            for (int y = 0; y < currentPiece.GetLength(1); y++)
            {
                rotatedPiece[y, currentPiece.GetLength(0) - 1 - x] = currentPiece[x, y];
            }
        }

        // �����ת���λ���Ƿ�Ϸ�
        if (CanMove(currentPosition, rotatedPiece))
        {
            currentPiece = rotatedPiece;  // ����Ϸ���ִ����ת
        }
    }

    //��鷽���Ƿ�������µ�λ�÷���
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

    //������Զ������߼�
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

    //�����û�����ķ������ƶ���ǰ�ķ���
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

    //����������������鵱ǰ�����ڸ���λ���Ƿ���ԺϷ�����
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

    //���ݵ�ǰ�����λ�ú���״������̶�������
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

    //�����������̣��ӵײ����ϼ��ÿһ���Ƿ���ȫ����
    private void ClearCompleteLines()
    {
        // �ӵײ���ʼ��飬����������ȷ������������������
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
                // �������󽫵�ǰ�����¼��һ��
                y++;  // ��Ϊ����������������л��½�����Ҫ���¼�鵱ǰ��
                score += 10;
            }
        }
    }

    //���ָ�����У������Ϸ�������������һ��
    private void ClearLine(int line)
    {
        // ��������е����и�������Ϊ0
        for (int x = 0; x < grid.GetLength(0); x++)
        {
            grid[x, line] = 0;
        }

        // ��������������������������ƶ�
        for (int y = line; y < grid.GetLength(0) - 1; y++)
        {
            for (int x = 0; x < grid.GetLength(0); x++)
            {
                grid[x, y] = grid[x, y + 1];
            }
        }
    }

    //�������һ���µķ���
    private void SpawnNewPiece()
    {
        currentPiece = tetrominoes[UnityEngine.Random.Range(0, tetrominoes.Count)];
        currentPosition = new Vector2Int(4, 19);

        // ������ɵķ����Ƿ���Է��ã������������Ϸ����
        if (!CanMove(currentPosition, currentPiece))
        {
            GameOver();
        }
    }

    //������Ϸ�������߼�
    private void GameOver()
    {
        gameActive = false;
        gameOver = true;
        gameEndTime = DateTime.Now;
        totalTime = gameEndTime - startTime;  // ֹͣ��ʱ
        Debug.Log("��Ϸ����");
        // ���������������Ϸ����ʱ�Ĵ����߼�������ʾ������������Ϸ
    }

    //��Ϸ��ʼʱ������������ϵĳ�ʼ����
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
    //    highlightedButtonStyle.normal.textColor = Color.yellow;  // ����ʱ����Ϊ��ɫ
    //    return highlightedButtonStyle;
    //}
}
