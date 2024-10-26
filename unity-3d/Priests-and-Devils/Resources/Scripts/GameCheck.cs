using UnityEngine;
using DevilBoatGame;

public class GameCheck : MonoBehaviour
{
    private Controller sceneController;

    private void Start()
    {
        sceneController = SSDirector.GetInstance().CurrentScenceController as Controller;
    }

    public int GameJudge()
    {
        int startLandPriests = sceneController.src_land.GetTotal(0);
        int startLandDevils = sceneController.src_land.GetTotal(1);
        int destinationLandPriests = sceneController.des_land.GetTotal(0);
        int destinationLandDevils = sceneController.des_land.GetTotal(1);

        // 如果所有角色都在终点，表示获胜
        if (destinationLandPriests + destinationLandDevils == 6)
        {
            return 1; // 游戏胜利
        }

        // 根据船的位置判断失败条件
        if (sceneController.boat.GetBoatMark() == 1) // 船在起始点
        {
            // 检查终点陆地上的角色数量，牧师少于魔鬼时失败
            if (destinationLandPriests < destinationLandDevils && destinationLandPriests > 0)
            {
                return -1; // 游戏失败
            }
        }
        else // 船在终点
        {
            // 检查起始陆地上的角色数量，牧师少于魔鬼时失败
            if (startLandPriests < startLandDevils && startLandPriests > 0)
            {
                return -1; // 游戏失败
            }
        }

        return 0; // 游戏继续进行
    }
}
