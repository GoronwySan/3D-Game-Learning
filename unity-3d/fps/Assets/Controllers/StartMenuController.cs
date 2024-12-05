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

        // �����K��ʼ��ҕ�D��
        startMenuView = gameObject.AddComponent<StartMenuView>();
        Debug.Log("StartMenuView created and attached to StartMenuController.");

        // �]�԰��o�c���¼�
        if (startMenuView.startGameButton != null)
        {
            startMenuView.startGameButton.onClick.AddListener(OnStartGameButtonClicked);
            Debug.Log("Start Game button click listener registered.");
        }
        else
        {
            Debug.LogError("Start Game button is not assigned in the StartMenuView.");
        }

        // ���� LevelModel �� GoldModel��ͨ���������
        levelModel = gameObject.AddComponent<LevelModel>();
        goldModel = gameObject.AddComponent<GoldModel>();

        Debug.Log("LevelModel and GoldModel created.");

        // ���� LevelSelectionController
        levelSelectionController = gameObject.AddComponent<LevelSelectionController>();
        Debug.Log("LevelSelectionController created and attached.");
    }

    // �_ʼ�[��r���О�
    private void OnStartGameButtonClicked()
    {
        Debug.Log("Start Game button clicked.");

        // �����o���c���r���[���_ʼ�[�����
        if (startMenuView.startMenuPanel != null)
        {
            startMenuView.startMenuPanel.SetActive(false);
            Debug.Log("Start menu panel hidden.");
        }
        else
        {
            Debug.LogError("Start menu panel is not assigned in the StartMenuView.");
        }

        // �����ڴ�̎���d�µ��[��������x���ɫ��
        // ���磺SceneManager.LoadScene("LevelSelectionScene");
        Debug.Log("Displaying level selection menu...");
        levelSelectionController.DisplayLevelSelection();
    }
}
