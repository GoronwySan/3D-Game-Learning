using UnityEngine;

namespace DevilBoatGame
{
    public class BoatModel
    {
        private GameObject boat;
        private Vector3[] src_empty_pos;
        private Vector3[] des_empty_pos;
        public float speed = 15;

        private DevilBoatActionManager moveController;
        private Clickable click;
        private int boat_mark = 1;
        private RoleModel[] roles = new RoleModel[2];

        public BoatModel()
        {
            boat = Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), new Vector3(2.5F, 0.5F, 0), Quaternion.identity) as GameObject;
            boat.name = "boat";
            moveController = (SSDirector.GetInstance().CurrentScenceController as Controller).action_manager;
            click = boat.AddComponent(typeof(Clickable)) as Clickable;
            src_empty_pos = new Vector3[] { new Vector3(2F, 1.1F, 0), new Vector3(3.3F, 1.1F, 0) };
            des_empty_pos = new Vector3[] { new Vector3(-3.3F, 1.1F, 0), new Vector3(-2F, 1.1F, 0) };
        }

        public int Total()
        {
            int count = 0;
            foreach (var role in roles) if (role != null) count++;
            return count;
        }

        public void Move()
        {
            Vector3 end = boat_mark == 1 ? new Vector3(-2.5F, 0.5F, 0) : new Vector3(2.5F, 0.5F, 0);
            Vector3[] positions = boat_mark == 1 ? des_empty_pos : src_empty_pos;
            moveController.moveBoat(boat, end, speed);
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null) roles[i].Move(positions[i]);
            }
            boat_mark *= -1;
        }

        public int GetBoatMark() => boat_mark;

        public Vector3 GetEmptyPosition()
        {
            Vector3[] positions = boat_mark == 1 ? src_empty_pos : des_empty_pos;
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null) return positions[i];
            }
            return Vector3.zero;
        }

        public void AddRole(RoleModel role)
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] == null)
                {
                    roles[i] = role;
                    break;
                }
            }
        }

        public RoleModel RemoveRole(string name)
        {
            for (int i = 0; i < roles.Length; i++)
            {
                if (roles[i] != null && roles[i].GetName() == name)
                {
                    roles[i] = null;
                    return roles[i];
                }
            }
            return null;
        }

        public void Reset()
        {
            if (boat_mark == -1)
            {
                moveController.moveBoat(boat, new Vector3(2.5F, 0.5F, 0), speed);
                boat_mark = 1;
            }
            roles = new RoleModel[2];
        }
    }
}
