using DevilBoatGame;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    private GameObject canvas;
    private GameObject coverImage;
    private Texture2D buttonTexture;
    private GUIStyle buttonStyle;

    // 用于引用 Controller 脚本
    public Controller controllerGameObject;

    private bool isGameStarted = false;

    void Start()
    {
        // 加载按钮纹理
        buttonTexture = Resources.Load<Texture2D>("Textures/ButtonImage");
        if (buttonTexture == null)
        {
            Debug.LogError("ButtonImage not found. Make sure the image is placed in 'Assets/Resources/Textures' and the path is correct.");
            return;
        }

        // 初始化按钮样式
        buttonStyle = new GUIStyle();
        buttonStyle.normal.background = buttonTexture; // 设置按钮的背景图片
        buttonStyle.fontSize = 24; // 设置字体大小
        buttonStyle.alignment = TextAnchor.MiddleCenter; // 设置文字对齐方式
        buttonStyle.normal.textColor = Color.black; // 设置文字颜色
        buttonStyle.hover.textColor = Color.yellow;  // 悬停时字体颜色为黄色

        CreateUI();
    }

    // 创建 UI 元素
    private void CreateUI()
    {
        // 创建 Canvas
        canvas = new GameObject("Canvas");
        Canvas c = canvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.AddComponent<CanvasScaler>();
        canvas.AddComponent<GraphicRaycaster>();

        // 创建封面图片
        coverImage = new GameObject("CoverImage");
        coverImage.transform.SetParent(canvas.transform);
        Image img = coverImage.AddComponent<Image>();

            // 设置封面图片路径
            Texture2D texture = Resources.Load<Texture2D>("Textures/CoverImage");
        if (texture == null)
        {
            Debug.LogError("CoverImage not found. Make sure the image is placed in 'Assets/Resources/Textures' and the path is correct.");
            return; // 提前返回，避免继续执行
        }
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // 调整 RectTransform 以覆盖整个屏幕
        RectTransform rectTransform = img.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero; // 左下角锚点 (0, 0)
        rectTransform.anchorMax = Vector2.one; // 右上角锚点 (1, 1)
        rectTransform.offsetMin = Vector2.zero; // 左下角偏移
        rectTransform.offsetMax = Vector2.zero; // 右上角偏移
        rectTransform.anchoredPosition = Vector2.zero; // 确保位置居中

        // 确保 Raycast Target 启用
        img.raycastTarget = false; // 防止封面图片覆盖按钮的点击


        Debug.Log("UI 创建完成");
    }

    void OnGUI()
    {
        // 如果游戏还未启动，显示开始按钮
        if (!isGameStarted)
        {
            // 创建一个带有自定义样式的按钮
            if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.65f, Screen.width * 0.2f, Screen.height * 0.1f), "开始游戏", buttonStyle))
            {
                Debug.Log("按钮被点击，启动 Controller 脚本");
                // 启动 Controller 脚本
                if (controllerGameObject == null)
                {
                    controllerGameObject = gameObject.AddComponent<Controller>();
                }

                // 标记游戏已启动，隐藏按钮
                isGameStarted = true;

                // 隐藏开始页面的 UI
                canvas.SetActive(false);
            }
        }
    }
}
