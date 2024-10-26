using UnityEngine;

namespace DevilBoatGame
{
    public class RoleModel
    {
        GameObject role;
        int role_sign;
        bool on_boat;
        LandModel land = (SSDirector.GetInstance().CurrentScenceController as Controller).src_land;

        DevilBoatActionManager moveController;
        Clickable click;
        public float speed = 15;

        public RoleModel(int id, Vector3 pos)
        {
            if (id == 0)
            {
                role_sign = 0;
                role = Object.Instantiate(Resources.Load("Prefabs/Priest", typeof(GameObject)), pos, Quaternion.identity) as GameObject;
            }
            else
            {
                role_sign = 1;
                role = Object.Instantiate(Resources.Load("Prefabs/Devil", typeof(GameObject)), pos, Quaternion.identity) as GameObject;
            }
            click = role.AddComponent(typeof(Clickable)) as Clickable;
            click.SetRole(this);
            moveController = (SSDirector.GetInstance().CurrentScenceController as Controller).action_manager;
        }

        public int GetSign() => role_sign;
        public string GetName() => role.name;
        public LandModel GetLandModel() => land;
        public bool IsOnBoat() => on_boat;
        public void SetName(string name) => role.name = name;

        public void Move(Vector3 end)
        {
            Vector3 middle = new Vector3(role.transform.position.x, end.y, end.z);
            moveController.moveRole(role, middle, end, speed);
        }

        public void ToLand(LandModel land)
        {
            Vector3 pos = land.GetEmptyPosition();
            Vector3 middle = new Vector3(role.transform.position.x, pos.y, pos.z);
            moveController.moveRole(role, middle, pos, speed);
            this.land = land;
            on_boat = false;
        }

        public void ToBoat(BoatModel boat)
        {
            Vector3 pos = boat.GetEmptyPosition();
            Vector3 middle = new Vector3(pos.x, role.transform.position.y, pos.z);
            moveController.moveRole(role, middle, pos, speed);
            this.land = null;
            on_boat = true;
        }

        public void Reset()
        {
            LandModel land = (SSDirector.GetInstance().CurrentScenceController as Controller).src_land;
            ToLand(land);
            land.AddRole(this);
        }
    }
}
