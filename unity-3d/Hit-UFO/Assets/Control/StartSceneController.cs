using UnityEngine;
using UnityEngine.UI;

public class StartSceneController : MonoBehaviour
{
    private GameObject canvas;
    private GameObject coverImage;
    private Texture2D buttonTexture;
    private GUIStyle buttonStyle;

    // 引用需要添加的脚本
    private FirstController firstController;
    private CCActionManager ccActionManager;
    private PickupObject pickupObject;
    private View view;
    private GroundController GR;

    private bool isGameStarted = false;

    void Start()
    {
        // 加载按钮纹理
        buttonTexture = Resources.Load<Texture2D>("Textures/ButtonImage");
        if (buttonTexture == null)
        {
            Debug.LogError("ButtonImage not found. Make sure the image is placed in 'Assets/Scenes/Textures' and the path is correct.");
            return;
        }

        // 初始化按钮样式
        buttonStyle = new GUIStyle
        {
            normal = { background = buttonTexture, textColor = Color.black },
            fontSize = 24,
            alignment = TextAnchor.MiddleCenter,
            hover = { textColor = Color.yellow }
        };

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
            Debug.LogError("CoverImage not found. Make sure the image is placed in 'Assets/Scenes/Textures' and the path is correct.");
            return;
        }
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        // 调整 RectTransform 以覆盖整个屏幕
        RectTransform rectTransform = img.GetComponent<RectTransform>();
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.anchoredPosition = Vector2.zero;

        img.raycastTarget = false;

        Debug.Log("UI 创建完成");
    }

    void OnGUI()
    {
        // 如果游戏还未启动，显示开始按钮
        if (!isGameStarted)
        {
            if (GUI.Button(new Rect(Screen.width * 0.4f, Screen.height * 0.65f, Screen.width * 0.2f, Screen.height * 0.1f), "开始游戏", buttonStyle))
            {
                Debug.Log("按钮被点击，启动脚本");

                // 添加所有需要的脚本
                AddControllerScripts();

                // 标记游戏已启动，隐藏按钮
                isGameStarted = true;

                // 隐藏开始页面的 UI
                canvas.SetActive(false);
            }
        }
    }

    private void AddControllerScripts()
    {
        //firstController = gameObject.AddComponent<FirstController>();

        //ccActionManager = gameObject.AddComponent<CCActionManager>();
        pickupObject = gameObject.AddComponent<PickupObject>();
        view = gameObject.AddComponent<View>();
        GR = gameObject.AddComponent<GroundController>();

        // 按顺序初始化，确保依赖脚本加载完成
        //firstController.Init(SSDirector.getInstance());
        //ccActionManager.Init();
        pickupObject.Init();
        view.Init();

        Debug.Log("所有控制脚本已成功挂载并初始化。");
    }

}
