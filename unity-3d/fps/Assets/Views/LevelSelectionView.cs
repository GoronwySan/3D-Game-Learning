using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionView : MonoBehaviour
{
    public event Action<int> OnLevelSelectedEvent;  // 定义一个事件

    public GameObject levelSelectionPanel;  // 面板容器
    private LevelModel levelModel;

    private void Awake()
    {
        // 使用單例模式直接獲取 LevelModel 實例
        levelModel = LevelModel.Instance;

        if (levelModel == null)
        {
            Debug.LogError("LevelModel not found! Ensure the LevelModel is properly initialized.");
            return;
        }

        Debug.Log("LevelModel successfully loaded.");

        levelSelectionPanel = CreatePanel();

        Debug.Log("Level Selection Panel created.");

        CreateLevelButtons(levelSelectionPanel.transform);

        levelSelectionPanel.SetActive(false);
    }

    // 創建面板容器
    private GameObject CreatePanel()
    {
        // 创建一个新的 Canvas
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080); // 參考分辨率，根據實際情況設置
        canvasObj.AddComponent<GraphicRaycaster>(); // 添加射线检测以处理 UI 事件

        // 创建面板
        GameObject panelObj = new GameObject("LevelSelectionPanel");
        panelObj.transform.SetParent(canvasObj.transform);  // 将面板挂载到 Canvas 下

        RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(1200, 800);  // 设置面板大小
        panelObj.AddComponent<Image>().color = new Color(0, 0, 0, 0.7f); // 设置背景色

        // 讓面板在屏幕中央顯示
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);  // 鎖定中心點
        rectTransform.anchoredPosition = Vector2.zero;

        return panelObj;
    }

    // 創建關卡按鈕
    private void CreateLevelButtons(Transform parent)
    {
        // 按鈕的大小
        Vector2 buttonSize = new Vector2(300, 140);
        int buttonsPerRow = 3;  // 每行四個按鈕
        float horizontalSpacing = 10f;  // 水平間距
        float verticalSpacing = 10f;    // 垂直間距

        // 計算總行數
        int totalRows = Mathf.CeilToInt((float)levelModel.TotalLevels / buttonsPerRow);

        // 創建每個關卡的按鈕
        for (int i = 0; i < levelModel.TotalLevels; i++)
        {
            Debug.Log($"Creating button for Level {i + 1}...");

            // 創建按鈕並設置其屬性
            Button levelButton = CreateButton(parent, $"关卡 {i + 1}");
            RectTransform buttonRect = levelButton.GetComponent<RectTransform>();
            buttonRect.sizeDelta = buttonSize;  // 設定按鈕大小

            // 計算每個按鈕的位置
            int row = i / buttonsPerRow; // 這是按鈕所在的行
            int col = i % buttonsPerRow; // 這是按鈕所在的列

            float xPos = col * (buttonSize.x + horizontalSpacing) - (buttonsPerRow * (buttonSize.x + horizontalSpacing) / 2) + 150; // 計算X坐標
            float yPos = -row * (buttonSize.y + verticalSpacing) + (totalRows * (buttonSize.y + verticalSpacing) / 2) - 150; // 計算Y坐標

            buttonRect.anchoredPosition = new Vector2(xPos, yPos);  // 設定按鈕的位置

            // 根據解鎖狀態設定按鈕
            Text buttonText = levelButton.GetComponentInChildren<Text>();
            if (levelModel.IsLevelUnlocked(i))
            {
                levelButton.interactable = true;
                buttonText.color = Color.white;  // 已解鎖顯示為白色
                levelButton.onClick.AddListener(() => OnLevelSelected(i));

                Debug.Log($"Level {i + 1} is unlocked. Button is interactable.");
            }
            else
            {
                levelButton.interactable = false;
                buttonText.color = Color.gray;  // 未解鎖顯示為灰色

                Debug.Log($"Level {i + 1} is locked. Button is non-interactive.");
            }
        }
    }

    // 創建自定義按鈕
    private Button CreateButton(Transform parent, string buttonText)
    {
        // 創建按鈕的 GameObject
        GameObject buttonObj = new GameObject(buttonText);
        buttonObj.transform.SetParent(parent);

        // 添加 RectTransform 並設置大小
        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(300, 140); // 設定按鈕大小

        // 創建文本
        GameObject textObj = new GameObject();
        textObj.transform.SetParent(buttonObj.transform);
        Text text = textObj.AddComponent<Text>();
        text.text = buttonText;
        text.fontSize = 50;  // 設定字體大小，根據需要調整
        text.font = Resources.Load<Font>("Fonts/NotoSansSC-Black"); // 使用內建字體（或自定義字體）
        text.color = Color.black;
        text.alignment = TextAnchor.MiddleCenter;

        // 設定文本框大小
        RectTransform textRectTransform = textObj.GetComponent<RectTransform>();
        textRectTransform.sizeDelta = new Vector2(300, 140);  // 設定文本框範圍

        // 設定按鈕背景圖片
        RawImage buttonImage = buttonObj.AddComponent<RawImage>();
        buttonImage.texture = Resources.Load<Texture>("ButtonImage"); // 加載 ButtonImage.png 圖片，確保它位於 Resources 資料夾中
        buttonImage.color = Color.white; // 確保圖片的顏色不會干擾圖片顯示

        // 添加 Button 組件並設定背景
        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;

        // 設定按鈕的交互
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;

        return button;
    }




    // 關卡選擇後的回調
    private void OnLevelSelected(int levelIndex)
    {
        levelIndex -= 9;
        Debug.Log($"Level {levelIndex + 1 - 10 } selected.");
        // 這裡可以觸發場景加載或其他操作
        OnLevelSelectedEvent?.Invoke(levelIndex);  // 触发事件，通知其他类
    }
}
