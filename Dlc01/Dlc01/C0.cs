namespace C0
{
    //OnStart处应用了npc2000_1,这个脚本里引用了模型2000,model = 2000
    public class LevelScript_c001 : LevelScriptBase
    {
        int g_iStoneMaxHP = 500;
        const int g_iNumStones = 32;
        int[] g_iStoneHP;
        int[] g_bStoneAlive;

        public override void Scene_OnLoad()
        {

            int i;
            string name = "";

            for (i = 1; i <= g_iNumStones; i++)
            {
                MakeString(ref name, "D_St", i);
                SetSceneItem(name, "name", "machine");
                SetSceneItem(name, "attribute", "damagevalue", 50);
            }

            g_iStoneMaxHP = g_iLevel01StoneMaxHP;
        }

        public override void Scene_OnInit()
        {
            int i;
            string name = "";

            InitBoxes(g_iNumBoxes);
            InitBBoxes(g_iNumBBoxes);
            InitChairs(g_iNumChairs);
            InitDeskes(g_iNumDeskes);
            InitJugs(g_iNumJugs);
            g_iStoneHP = new int[g_iNumStones];
            g_bStoneAlive = new int[g_iNumStones];
            for (i = 1; i <= g_iNumStones; i++)
            {
                g_iStoneHP[i - 1] = g_iStoneMaxHP;
                g_bStoneAlive[i - 1] = 1;

                Output(i, g_iStoneHP[i - 1]);
                MakeString(ref name, "D_St", i);
                SetSceneItem(name, "attribute", "active", 1);
                SetSceneItem(name, "attribute", "damage", 0);
                SetSceneItem(name, "attribute", "collision", 1);
                SetSceneItem(name, "pose", 0, 0);
                SetSceneItem(name, StoneOnAttack, StoneOnIdle);

                MakeString(ref name, "D_itSt", i);
                SetSceneItem(name, "attribute", "active", 0);
                SetSceneItem(name, "attribute", "interactive", 0);
                MakeString(ref name, "D_wpSt", i);
                SetSceneItem(name, "attribute", "active", 0);
                SetSceneItem(name, "attribute", "interactive", 0);
            }
        }

        public int RemoveItem(int id)
        {
            int pose = GetSceneItem(id, "pose");
            if (pose == 0)
            {
                return 0;
            }
            int state = GetSceneItem(id, "state");
            if (state != 3)
            {
                return 0;
            }
            NetEvent(1);
            SetSceneItem(id, "attribute", "active", 0);
            NetEvent(0);
            return 1;
        }

        public int ActiveStoneItem(int index)
        {
            string stonename = "";
            string itemname = "";
            string weaponname = "";

            MakeString(ref stonename, "D_St", index);
            int pose = GetSceneItem(stonename, "pose");
            if (pose == 1)
            {
                return 0;
            }

            MakeString(ref itemname, "D_itSt", index);
            MakeString(ref weaponname, "D_wpSt", index);

            Output("Active Stone", index);
            NetEvent(1);
            CreateEffect(stonename, "StoneBRK");
            SetSceneItem(stonename, "pose", 1, 0);
            SetSceneItem(stonename, "attribute", "collision", 0);
            SetSceneItem(stonename, "attribute", "damage", 1);
            SetSceneItem(itemname, "attribute", "active", 1);
            SetSceneItem(itemname, "attribute", "interactive", 1);
            SetSceneItem(weaponname, "attribute", "active", 1);
            SetSceneItem(itemname, "attribute", "interactive", 1);
            NetEvent(0);
            return 1;
        }

        public int StoneOnAttack(int id, int index, int damage)
        {
            string name = "";
            g_iStoneHP[index - 1] = g_iStoneHP[index - 1] - damage;
            Output(g_iStoneHP[index - 1]);
            MakeString(ref name, "D_st", index);

            NetEvent(1);
            CreateEffect(name, "StoneHIT");
            NetEvent(0);

            if (g_iStoneHP[index - 1] <= 0)
            {
                ActiveStoneItem(index);
            }
            return 0;
        }

        public void StoneOnIdle(int id, int index)
        {
            if (g_bStoneAlive[index - 1] == 1)
            {
                if (RemoveItem(id) == 1)
                {
                    g_bStoneAlive[index - 1] = 0;
                }
            }
        }

        int RoundTime = 10;
        int PlayerSpawn = 9;
        int PlayerSpawnDir = 90;
        int PlayerWeapon = 5;
        int PlayerWeapon2 = 1;
        int PlayerHP = 1500;
        public override int GetRoundTime() { return RoundTime; }
        public override int GetPlayerSpawn() { return PlayerSpawn; }
        public override int GetPlayerSpawnDir() { return PlayerSpawnDir; }
        public override int GetPlayerWeapon() { return PlayerWeapon; }
        public override int GetPlayerWeapon2() { return PlayerWeapon2; }
        public override int GetPlayerMaxHp() { return PlayerHP; }
        int trg0 = 0;
        int trg1 = 0;
        int trg2 = 0;
        int trg3 = 0;
        int trg4 = 0;
        int trg5 = 0;
        int trg6 = 0;

        public override void OnStart()
        {
            AddNPC("npc2000_1");
            base.OnStart();
        }

        public override int OnUpdate()
        {
            return 0;
        }
    }
}
