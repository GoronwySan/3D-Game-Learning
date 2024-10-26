using UnityEngine;
using DevilBoatGame;

public class Controller : MonoBehaviour, ISceneController, IUserAction
{
    public LandModel src_land; // 起点陆地
    public LandModel des_land; // 终点陆地
    public BoatModel boat; // 船
    public RoleModel[] roles; // 角色数组

    public UserGUI user_gui; // GUI界面，用于控制游戏状态
    public DevilBoatActionManager action_manager; // 动作控制器
    public GameCheck checker; // 裁判

    private const int RoleCount = 6; // 角色总数
    private const int HalfRoleCount = 3; // 每种角色的数量

    void Start()
    {
        InitializeSceneController();
        InitializeComponents();
        LoadResources();
    }

    private void InitializeSceneController()
    {
        SSDirector director = SSDirector.GetInstance();
        director.CurrentScenceController = this;
    }

    private void InitializeComponents()
    {
        user_gui = gameObject.AddComponent<UserGUI>();
        action_manager = gameObject.AddComponent<DevilBoatActionManager>();
        checker = gameObject.AddComponent<GameCheck>();
    }

    public void LoadResources()
    {
        CreateWater();
        InitializeLandAndBoat();
        InitializeRoles();
    }

    private void CreateWater()
    {
        GameObject water = Instantiate(Resources.Load("Prefabs/Water", typeof(GameObject)), Vector3.zero, Quaternion.identity) as GameObject;
    }

    private void InitializeLandAndBoat()
    {
        src_land = new LandModel(1);
        des_land = new LandModel(-1);
        boat = new BoatModel();
        roles = new RoleModel[RoleCount];
    }

    private void InitializeRoles()
    {
        for (int i = 0; i < HalfRoleCount; i++)
        {
            CreateRole(0, "priest" + i, i);
        }
        for (int i = 0; i < HalfRoleCount; i++)
        {
            CreateRole(1, "Devil" + i, i + HalfRoleCount);
        }
    }

    private void CreateRole(int roleType, string roleName, int index)
    {
        RoleModel role = new RoleModel(roleType, src_land.GetEmptyPosition());
        role.SetName(roleName);
        src_land.AddRole(role);
        roles[index] = role;
    }

    public void MoveBoat()
    {
        if (boat.Total() == 0 || user_gui.status != 0)
        {
            return; // 空船或游戏状态不符时不能移动
        }
        boat.Move();
        UpdateGameStatus();
    }

    public void MoveRole(RoleModel role)
    {
        if (user_gui.status != 0) return;

        if (role.IsOnBoat()) // 如果角色在船上，则下船
        {
            LandModel targetLand = boat.GetBoatMark() == -1 ? des_land : src_land;
            boat.RemoveRole(role.GetName());
            role.ToLand(targetLand);
            targetLand.AddRole(role);
        }
        else // 如果角色不在船上，则上船
        {
            LandModel currentLand = role.GetLandModel();
            if (boat.Total() >= 2 || currentLand.GetLandMark() != boat.GetBoatMark())
            {
                return; // 如果船已满或船不在此岸，则不能上船
            }
            currentLand.RemoveRole(role.GetName());
            role.ToBoat(boat);
            boat.AddRole(role);
        }

        UpdateGameStatus();
    }

    public void Restart()
    {
        src_land.Reset();
        des_land.Reset();
        boat.Reset();
        foreach (RoleModel role in roles)
        {
            role.Reset();
        }
        user_gui.status = 0;
    }

    private void UpdateGameStatus()
    {
        user_gui.status = checker.GameJudge();
    }
}
