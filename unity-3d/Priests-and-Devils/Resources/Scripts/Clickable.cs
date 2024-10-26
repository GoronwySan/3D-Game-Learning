using UnityEngine;
using DevilBoatGame;

public class Clickable : MonoBehaviour
{
    private IUserAction action;
    private RoleModel role;

    public void SetRole(RoleModel role)
    {
        this.role = role;
    }

    private void Start()
    {
        action = SSDirector.GetInstance().CurrentScenceController as IUserAction;
    }

    private void OnMouseDown()
    {
        if (gameObject.name == "boat")
        {
            action.MoveBoat();
        }
        else if (role != null)
        {
            action.MoveRole(role);
        }
    }
}
