using UnityEngine;
using UnityEngine.UI;

public class StartMenuView : MonoBehaviour
{
    public GameObject startMenuPanel;
    public Button startGameButton;

    private void Awake()
    {
        // 創建 Canvas
        Canvas canvas = CreateCanvas();

        // 創建 StartMenuPanel
        startMenuPanel = CreatePanel(canvas.transform);

        // 創建 StartGame 按鈕
        startGameButton = CreateButton(startMenuPanel.transform, "开始游戏");
    }

    // 創建 Canvas
    private Canvas CreateCanvas()
    {
        GameObject canvasObj = new GameObject("Canvas");
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080); // 參考分辨率，根據實際情況設置

        canvasObj.AddComponent<GraphicRaycaster>();  // 必須有這個才能處理 UI 按鈕事件
        return canvas;
    }

    // 創建面板
    private GameObject CreatePanel(Transform parent)
    {
        GameObject panelObj = new GameObject("StartMenuPanel");
        panelObj.transform.SetParent(parent);

        RectTransform rectTransform = panelObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(2160, 1600); // 設定面板大小

        // 創建 RawImage 來顯示圖片背景
        RawImage rawImage = panelObj.AddComponent<RawImage>();
        rawImage.texture = Resources.Load<Texture>("backgroundImage"); // 從資源中加載圖片
        rawImage.rectTransform.sizeDelta = new Vector2(1920, 1080);  // 調整背景圖像的大小，讓它覆蓋整個面板
        // 讓面板在屏幕中央顯示
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);  // 鎖定中心點
        rectTransform.anchoredPosition = Vector2.zero;

        return panelObj;
    }


    // 創建按鈕
    private Button CreateButton(Transform parent, string buttonText)
    {
        GameObject buttonObj = new GameObject(buttonText);
        buttonObj.transform.SetParent(parent);

        RectTransform rectTransform = buttonObj.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(400, 140); // 設定按鈕大小

        // 設定按鈕文本
        GameObject textObj = new GameObject();
        textObj.transform.SetParent(buttonObj.transform);
        Text text = textObj.AddComponent<Text>();
        text.text = buttonText;
        // 設定字體大小
        text.fontSize = 50;  // 設定字體大小，根據需要調整
        text.font = Resources.Load<Font>("Fonts/NotoSansSC-Black"); // 使用內建字體（或自定義字體）
        text.color = Color.black;
        text.alignment = TextAnchor.MiddleCenter;

        // 設定文本框大小
        RectTransform textRectTransform = textObj.GetComponent<RectTransform>();
        textRectTransform.sizeDelta = new Vector2(400, 140);  // 設定文本框範圍


        // 設定按鈕背景圖片
        RawImage buttonImage = buttonObj.AddComponent<RawImage>();
        buttonImage.texture = Resources.Load<Texture>("ButtonImage"); // 加載 ButtonImage.png 圖片，確保它位於 Resources 資料夾中

        // 確保圖片的顏色不會干擾圖片顯示
        buttonImage.color = Color.white;

        // 設定按鈕的交互
        Button button = buttonObj.AddComponent<Button>();
        button.targetGraphic = buttonImage;

        // 讓按鈕也位於屏幕中央
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;

        return button;
    }


}
