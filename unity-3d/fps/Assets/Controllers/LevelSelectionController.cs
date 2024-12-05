using UnityEngine;
using UnityEngine.SceneManagement;  // 引入 SceneManager 命名空间

public class LevelSelectionController : MonoBehaviour
{
    public LevelSelectionView levelSelectionView;
    private LevelModel levelModel;

    private void Start()
    {
        levelModel = FindObjectOfType<LevelModel>();  // 确保场景中有 LevelModel
        levelSelectionView = gameObject.AddComponent<LevelSelectionView>();

        levelSelectionView.OnLevelSelectedEvent += OnLevelSelected;  // 订阅事件
    }

    // 显示关卡选择界面
    public void DisplayLevelSelection()
    {
        levelSelectionView.levelSelectionPanel.SetActive(true); // 显示关卡选择面板
    }

    // 选择关卡后执行的操作
    public void OnLevelSelected(int levelIndex)
    {
        Debug.Log($"Level {levelIndex + 1} selected.");
        Cursor.lockState = CursorLockMode.Locked;  // 锁定鼠标到屏幕中心
        Cursor.visible = false;  // 隐藏鼠标

        // 加载 "Pack Overview" 场景
        SceneManager.LoadScene("Pack Overview");  // 加载场景名为 "Pack Overview"

        levelSelectionView.levelSelectionPanel.SetActive(false); // 隐藏关卡选择面板
    }
}
