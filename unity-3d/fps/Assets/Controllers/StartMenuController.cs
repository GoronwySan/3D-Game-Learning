using UnityEngine;

public class StartMenuController : MonoBehaviour
{
    public StartMenuView startMenuView;
    private LevelSelectionController levelSelectionController;
    private LevelModel levelModel;
    private CharacterModel characterModel;
    private GoldModel goldModel;

    private void Start()
    {
        Debug.Log("StartMenuController initialized.");

        // 創建並初始化視圖層
        startMenuView = gameObject.AddComponent<StartMenuView>();
        Debug.Log("StartMenuView created and attached to StartMenuController.");

        // 註冊按鈕點擊事件
        if (startMenuView.startGameButton != null)
        {
            startMenuView.startGameButton.onClick.AddListener(OnStartGameButtonClicked);
            Debug.Log("Start Game button click listener registered.");
        }
        else
        {
            Debug.LogError("Start Game button is not assigned in the StartMenuView.");
        }

        // 挂载 LevelModel 和 GoldModel，通过代码挂载
        levelModel = gameObject.AddComponent<LevelModel>();
        goldModel = gameObject.AddComponent<GoldModel>();

        Debug.Log("LevelModel and GoldModel created.");

        // 創建 LevelSelectionController
        levelSelectionController = gameObject.AddComponent<LevelSelectionController>();
        Debug.Log("LevelSelectionController created and attached.");
    }

    // 開始遊戲時的行為
    private void OnStartGameButtonClicked()
    {
        Debug.Log("Start Game button clicked.");

        // 當按鈕被點擊時，隱藏開始遊戲面板
        if (startMenuView.startMenuPanel != null)
        {
            startMenuView.startMenuPanel.SetActive(false);
            Debug.Log("Start menu panel hidden.");
        }
        else
        {
            Debug.LogError("Start menu panel is not assigned in the StartMenuView.");
        }

        // 可以在此處加載新的遊戲場景或選擇角色等
        // 例如：SceneManager.LoadScene("LevelSelectionScene");
        Debug.Log("Displaying level selection menu...");
        levelSelectionController.DisplayLevelSelection();
    }
}
