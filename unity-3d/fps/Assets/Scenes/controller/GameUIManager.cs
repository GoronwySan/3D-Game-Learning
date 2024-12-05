using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class GameUIManager : MonoBehaviour
{
    // UI元素
    private Image healthBarImage;
    private TextMeshProUGUI healthText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI timeText;
    private TextMeshProUGUI arrowCountText;  // 用来显示箭的数量
    private Image arrowIcon;  // 用来显示图标（如果不能射箭）

    // 玩家脚本引用
    private Player player;

    // 分数管理器引用
    private ScoreManager scoreManager;

    private float gameTime = 0f;

    // UI元素的自定义位置变量
    private Vector2 healthBarPosition = new Vector2(-400, 20);
    private Vector2 healthTextPosition = new Vector2(200, 80);
    private Vector2 scoreTextPosition = new Vector2(-220, -5);
    private Vector2 timeTextPosition = new Vector2(30, -17);

    // 字体文件路径aa
    public string fontPath = "Fonts/NotoSansSC-Black SDF";  // 字体文件路径
    private CrossbowController crossbowController;
    // 字体资源
    private TMP_FontAsset customFont;

    void Start()
    {
        // 加载自定义字体
        customFont = Resources.Load<TMP_FontAsset>(fontPath);
        // 动态创建UI元素
        CreateUIElements();

        // 获取玩家和分数管理器的引用
        player = FindObjectOfType<Player>();
        scoreManager = FindObjectOfType<ScoreManager>();
        crossbowController = FindObjectOfType<CrossbowController>(); // 获取CrossbowController的引用


        // 初始化UI
        UpdateHealthUI();
        UpdateScoreUI();
        //UpdateTimeUI();
        UpdateArrowUI();  // 更新箭的UI
    }

    void Update()
    {
        // 更新游戏时间
        gameTime += Time.deltaTime;
        //UpdateTimeUI();

        // 更新血量UI
        UpdateHealthUI();

        // 更新分数UI
        UpdateScoreUI();

        UpdateArrowUI();  // 更新箭的UI
    }

    // 创建UI元素
    void CreateUIElements()
    {
        // 创建 Canvas
        GameObject canvasObj = new GameObject("Canvas");
        canvasObj.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        Canvas canvas = canvasObj.GetComponent<Canvas>();
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        // 创建血量条 (Image 模拟)
        GameObject healthBarObj = new GameObject("HealthBar");
        healthBarObj.transform.SetParent(canvasObj.transform);
        healthBarImage = healthBarObj.AddComponent<Image>();
        RectTransform healthBarRect = healthBarObj.GetComponent<RectTransform>();
        healthBarRect.sizeDelta = new Vector2(80, 30); // 设定血量条大小
        // 设置旋转为 (0, 0, 180)
        healthBarRect.rotation = Quaternion.Euler(0, 0, 180);
        // 设置 pivot 为 (1, 0.5)，表示在右边的中间位置
        healthBarRect.pivot = new Vector2(1f, 0.5f);
        // 设置 anchorMax 和 anchorMin，确保图像的锚点和大小不受影响
        healthBarRect.anchorMin = new Vector2(0, 0.0f); // 锚点从左边界开始
        healthBarRect.anchorMax = new Vector2(0.4f, 0f); // 锚点到右边界
        healthBarRect.anchoredPosition = healthBarPosition; // 使用自定义位置
        healthBarImage.color = Color.red; // 设置颜色为红色

        // 创建血量文字 (TextMeshPro)
        GameObject healthTextObj = new GameObject("HealthText");
        healthTextObj.transform.SetParent(canvasObj.transform);
        healthText = healthTextObj.AddComponent<TextMeshProUGUI>();
        healthText.fontSize = 36;
        healthText.font = customFont; // 设置为自定义字体
        healthText.alignment = TextAlignmentOptions.Left;
        RectTransform healthTextRect = healthTextObj.GetComponent<RectTransform>();
        healthTextRect.sizeDelta = new Vector2(400, 30);
        healthTextRect.anchorMin = new Vector2(0, 0);
        healthTextRect.anchorMax = new Vector2(0, 0);
        healthTextRect.anchoredPosition = healthTextPosition; // 使用自定义位置

        // 创建分数文字 (TextMeshPro)
        GameObject scoreTextObj = new GameObject("ScoreText");
        scoreTextObj.transform.SetParent(canvasObj.transform);
        scoreText = scoreTextObj.AddComponent<TextMeshProUGUI>();
        scoreText.fontSize = 36;
        scoreText.font = customFont; // 设置为自定义字体
        scoreText.alignment = TextAlignmentOptions.TopRight;
        RectTransform scoreTextRect = scoreTextObj.GetComponent<RectTransform>();
        scoreTextRect.sizeDelta = new Vector2(400, 30);
        scoreTextRect.anchorMin = new Vector2(1, 1);
        scoreTextRect.anchorMax = new Vector2(1, 1);
        scoreTextRect.anchoredPosition = scoreTextPosition; // 使用自定义位置

        // 创建时间文字 (TextMeshPro)
        //GameObject timeTextObj = new GameObject("TimeText");
        //timeTextObj.transform.SetParent(canvasObj.transform);
        //timeText = timeTextObj.AddComponent<TextMeshProUGUI>();
        //timeText.fontSize = 36;
        //timeText.font = customFont; // 设置为自定义字体
        //timeText.alignment = TextAlignmentOptions.Center;
        //RectTransform timeTextRect = timeTextObj.GetComponent<RectTransform>();
        //timeTextRect.sizeDelta = new Vector2(400, 30);
        //timeTextRect.anchorMin = new Vector2(0.5f, 1);
        //timeTextRect.anchorMax = new Vector2(0.5f, 1);
        //timeTextRect.anchoredPosition = timeTextPosition; // 使用自定义位置

        // 创建箭数量文字 (TextMeshPro)
        GameObject arrowCountObj = new GameObject("ArrowCountText");
        arrowCountObj.transform.SetParent(canvasObj.transform);
        arrowCountText = arrowCountObj.AddComponent<TextMeshProUGUI>();
        arrowCountText.fontSize = 36;
        arrowCountText.font = customFont; // 设置为自定义字体
        arrowCountText.alignment = TextAlignmentOptions.BottomRight;
        RectTransform arrowCountRect = arrowCountObj.GetComponent<RectTransform>();
        arrowCountRect.sizeDelta = new Vector2(200, 30);
        arrowCountRect.anchorMin = new Vector2(1, 0);
        arrowCountRect.anchorMax = new Vector2(1, 0);
        arrowCountRect.anchoredPosition = new Vector2(-100, 20);  // 放置在右下角偏上位置

        // 创建箭图标 (Image) - 用于显示不能射箭时的图标
        GameObject arrowIconObj = new GameObject("ArrowIcon");
        arrowIconObj.transform.SetParent(canvasObj.transform);
        arrowIcon = arrowIconObj.AddComponent<Image>();
        RectTransform arrowIconRect = arrowIconObj.GetComponent<RectTransform>();
        arrowIconRect.sizeDelta = new Vector2(50, 50); // 图标大小
        arrowIconRect.anchorMin = new Vector2(1, 0);  // 放在右下角
        arrowIconRect.anchorMax = new Vector2(1, 0);
        arrowIconRect.anchoredPosition = new Vector2(-100, 30);  // 放置在右下角偏上位置
        arrowIcon.enabled = false;  // 默认不显示图标
    }

    // 更新血量UI
    void UpdateHealthUI()
    {
        if (player == null) return;

        // 获取当前血量
        float currentHealth = player.GetCurrentHealth();
        float healthPercentage = currentHealth / player.maxHealth;  // 血量百分比

        // 更新血量条宽度
        healthBarImage.rectTransform.localScale = new Vector3(healthPercentage, 1f, 1f);  // 从左到右缩放

        // 根据血量百分比来动态改变颜色
        healthBarImage.color = Color.Lerp(Color.red, Color.green, healthPercentage);

        // 更新血量文字
        healthText.text = "血量: " + currentHealth.ToString("F0") + " / " + player.maxHealth.ToString("F0");
    }



    // 更新分数UI
    void UpdateScoreUI()
    {
        if (scoreManager == null) return;

        // 更新分数文字
        scoreText.text = "分数: " + scoreManager.GetScore().ToString();
    }

    // 更新箭的UI
    void UpdateArrowUI()
    {
        if (crossbowController == null)
        {
            Debug.Log("dafdsads");
            return;
        }

        int remainingArrows = crossbowController.GetRemainingArrows();  // 获取剩余箭数量
        Debug.Log("asdfasdk" + remainingArrows);

        // 判断是否可以射箭，若不能射箭则显示图标，隐藏箭的数量
        if (crossbowController.CanShootArrow())
        {
            arrowIcon.enabled = false;  // 隐藏图标
            arrowCountText.text = "剩余箭: " + remainingArrows.ToString();  // 显示箭的数量
            arrowCountText.enabled = true;  // 显示箭的数量
        }
        else
        {
            arrowIcon.enabled = true;   // 显示图标（不能射箭时）
            arrowCountText.enabled = false;  // 隐藏箭的数量显示
                                             // 设置图标（例如不可射箭的图标）
            arrowIcon.sprite = Resources.Load<Sprite>("stop");  // 加载图标（请确保图标路径正确）
        }
    }


    // 更新游戏时间UI
    void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60f);
        int seconds = Mathf.FloorToInt(gameTime % 60f);
        timeText.text = "时间: " + minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
