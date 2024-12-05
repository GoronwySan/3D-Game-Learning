using UnityEngine;
using UnityEngine.SceneManagement;  // ���� SceneManager �����ռ�

public class LevelSelectionController : MonoBehaviour
{
    public LevelSelectionView levelSelectionView;
    private LevelModel levelModel;

    private void Start()
    {
        levelModel = FindObjectOfType<LevelModel>();  // ȷ���������� LevelModel
        levelSelectionView = gameObject.AddComponent<LevelSelectionView>();

        levelSelectionView.OnLevelSelectedEvent += OnLevelSelected;  // �����¼�
    }

    // ��ʾ�ؿ�ѡ�����
    public void DisplayLevelSelection()
    {
        levelSelectionView.levelSelectionPanel.SetActive(true); // ��ʾ�ؿ�ѡ�����
    }

    // ѡ��ؿ���ִ�еĲ���
    public void OnLevelSelected(int levelIndex)
    {
        Debug.Log($"Level {levelIndex + 1} selected.");
        Cursor.lockState = CursorLockMode.Locked;  // ������굽��Ļ����
        Cursor.visible = false;  // �������

        // ���� "Pack Overview" ����
        SceneManager.LoadScene("Pack Overview");  // ���س�����Ϊ "Pack Overview"

        levelSelectionView.levelSelectionPanel.SetActive(false); // ���عؿ�ѡ�����
    }
}
