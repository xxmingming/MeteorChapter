namespace C0
{
    /*外传数组，ID:2000，name显示在插件面板, zip标识dll存放的压缩包路径，解压后为Dlc/C0.dll
     * 编译后把生成的C0.dll发给我，然后把这个剧本相关的模型和NPC定义也发给我
     * 就可以让客户端动态加载你写好的剧本以及使用你的模型了
     * reference标识引用，可以引用模型，模型定义在Plugins.json内，模型引用
     *"Dlc":[
        {
            "Idx":2000,
            "name": "外传",
            "zip":"Dlc/C0.zip",
            "desc":"外传",
            "comment":"外传关卡,需要鸣人模型.",
            "reference":[
                {
                    "Model":[2000]
                }
            ]
        }
    ]
     */
    //OnStart处应用了npc2000_1,这个脚本里引用了模型2000,model = 2000
    public class LevelScript_c001 : LevelScriptBase
    {
        int g_iStoneMaxHP = 500;
        const int g_iNumStones = 32;
        int[] g_iStoneHP;
        int[] g_bStoneAlive;
        //此函数决定场景起始的时候，场景内的物件的处理，这里让尖刺不要旋转，伤害为5点HP一次
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

        //处理箱子，酒坛，桌子，椅子等道具
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

        //石头遭到攻击时被客户端调用
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

        //石头在待机时被框架调用
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

        //关卡时间长度：分钟
        int RoundTime = 10;
        //主角出生点
        int PlayerSpawn = 9;
        //主角出生方向-不太重要，就是角色朝着哪个方向
        int PlayerSpawnDir = 90;
        //主手武器
        int PlayerWeapon = 5;
        //背包武器
        int PlayerWeapon2 = 1;
        //角色气血：150
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

        //关卡启动时，增加NPC初始化
        //可以在触发一些条件时，调用AddNPC即可添加NPC，NPC也需要提交给我
        public override void OnStart()
        {
            AddNPC("npc2000_1");
            base.OnStart();
        }

        //每一帧被客户端调用
        public override int OnUpdate()
        {
            return 0;
        }
    }
}
