using DevilBoatGame;
using UnityEngine;
using UnityEngine.UI;

public class UserGUI : MonoBehaviour
{
    private GameObject canvas;
    private GameObject coverImage;
    private GameObject backgroundQuad; // 用于显示背景的Quad
    private IUserAction action;
    public int status = 0;
    private Texture2D buttonTexture;
    private GUIStyle buttonStyle;

    private GUIStyle white_style = new GUIStyle();
    private GUIStyle black_style = new GUIStyle();
    private GUIStyle title_style = new GUIStyle();

    private bool showRules = false; // 控制规则窗口显示的布尔变量
    private Rect windowRect = new Rect(Screen.width / 2 -300, Screen.height / 2 - 300, 600, 500); // 窗口位置和大小

    public Texture2D backgroundTexture; // 背景图片

    private void Start()
    {
        CreateUI();
        CreateBackground(); // 创建3D背景
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;

        // 字体样式初始化
        white_style.normal.textColor = Color.white;
        white_style.fontSize = 20;
        white_style.wordWrap = true; // 自动换行

        black_style.normal.textColor = Color.black;
        black_style.fontSize = 30;

        title_style.normal.textColor = Color.black;
        title_style.fontSize = 45;

        // 加载按钮纹理
        buttonTexture = Resources.Load<Texture2D>("Textures/ButtonImage");

        // 初始化按钮样式
        buttonStyle = new GUIStyle();
        buttonStyle.normal.background = buttonTexture; // 设置按钮的背景图片
        buttonStyle.fontSize = 24; // 设置字体大小
        buttonStyle.alignment = TextAnchor.MiddleCenter; // 设置文字对齐方式
        buttonStyle.normal.textColor = Color.black; // 设置文字颜色
        buttonStyle.hover.textColor = Color.yellow;  // 悬停时字体颜色为黄色
    }

    private void OnGUI()
    {
        // 如果 showRules 为 true，先将背景画面的透明度设置为 50%
        if (showRules)
        {
            GUI.color = new Color(1, 1, 1, 0.5f); // 设置透明度为 50%
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), ""); // 绘制一个全屏的半透明背景
            GUI.color = Color.white; // 还原透明度
        }

        // 创建一个按钮，点击后切换 showRules 状态，按钮位置在左上角
        if (GUI.Button(new Rect(30, 20, 120, 60), "显示规则", buttonStyle))
        {
            showRules = true; // 设置规则窗口显示状态为 true
        }

        // 如果 showRules 为 true，显示规则窗口
        if (showRules)
        {
            windowRect = GUI.Window(0, windowRect, ShowRulesWindow, "游戏规则");
        }

        if (status == -1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 20, 180, 100, 30), "坠落了", white_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 40, 240, 100, 30), "轮回"))
            {
                action.Restart();
                status = 0;
            }
        }
        else if (status == 1)
        {
            GUI.Label(new Rect(Screen.width / 2 - 20, 180, 100, 30), "净化", white_style);
            if (GUI.Button(new Rect(Screen.width / 2 - 40, 240, 100, 30), "轮回"))
            {
                action.Restart();
                status = 0;
            }
        }
    }

    // 显示规则的窗口内容
    void ShowRulesWindow(int windowID)
    {
        // 在窗口内部显示游戏规则的内容，自动换行
        GUI.Label(new Rect(20, 40, 560, 60), "牧师与魔鬼游戏的规则如下：\r\n\r\n" +
            "1.游戏背景：有3个牧师和3个魔鬼在河的一侧，他们需要利用一艘小船过河。" +
            "小船最多只能承载两个人（可以是牧师、魔鬼或空船），并且至少需要有一个人驾驶。\r\n\r\n" +
            "2.过河条件：牧师和魔鬼可以单独或组合坐船过河。每次行动后，船必须返回到对岸接人。\r\n\r\n" +
            "3.限制条件：\r\n在任意一侧，若魔鬼的数量多于牧师的数量，牧师将会被魔鬼吃掉（即游戏失败）。\r\n" +
            "必须通过合适的行动，使得所有的牧师和魔鬼都成功渡河。\r\n\r\n" +
            "4.胜利条件：所有的牧师和魔鬼都安全地从河的起始一侧到达对岸，并满足上述条件。\r\n", white_style);
        //GUI.Label(new Rect(20, 120, 260, 60), "在开船之后，每一边魔鬼数量都不能多于牧师数量。", white_style);

        // 创建一个关闭按钮
        if (GUI.Button(new Rect(250, 440, 100, 30), "关闭"))
        {
            showRules = false; // 设置规则窗口显示状态为 false，隐藏窗口
        }

        // 使窗口可拖动
        GUI.DragWindow();
    }

    private void CreateUI()
    {
        // 创建 Canvas
        canvas = new GameObject("Canvas");
        Canvas c = canvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();

        Debug.Log("UI 创建完成");
    }

    private void CreateBackground()
    {
        backgroundTexture = Resources.Load<Texture2D>("Textures/GameCoverImage");
        // 创建一个 Quad 作为背景
        backgroundQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
        backgroundQuad.name = "BackgroundQuad";

        // 设置背景材质
        Material backgroundMaterial = new Material(Shader.Find("Unlit/Texture"));
        backgroundMaterial.mainTexture = backgroundTexture;
        backgroundQuad.GetComponent<Renderer>().material = backgroundMaterial;

        // 设置背景 Quad 的位置和缩放
        backgroundQuad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 10f;
        backgroundQuad.transform.localScale = new Vector3(21f, 12f, 1f);

        // 确保背景 Quad 面向相机
        backgroundQuad.transform.rotation = Camera.main.transform.rotation;

        // 设置背景 Quad 的层级，避免与其他 3D 对象发生深度冲突
        backgroundQuad.GetComponent<Renderer>().sortingOrder = -10;
    }
}
