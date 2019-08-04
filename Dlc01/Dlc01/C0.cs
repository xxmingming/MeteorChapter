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
            "comment":"外传关卡.记录模型依赖",
            "reference":[
                {
                    "Model":[2000]
                }
            ]
        }
    ]
     */
    //后传-伏虎山
    public class LevelScript_c001 : LevelScriptBase
    {
        public override void Scene_OnLoad()
        {
        }

        //处理箱子，酒坛，桌子，椅子等道具
        public override void Scene_OnInit()
        {
            SetSceneItem("D_floor01", "name", "machine");
            SetSceneItem("D_floor01", "attribute", "collision", 1);
            InitBoxes(g_iNumBoxes);
            InitBBoxes(g_iNumBBoxes);
            InitChairs(g_iNumChairs);
            InitDeskes(g_iNumDeskes);
            InitJugs(g_iNumJugs);
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
            //NetEvent(1);
            SetSceneItem(id, "attribute", "active", 0);
            //NetEvent(0);
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
            //NetEvent(1);
            CreateEffect(stonename, "StoneBRK");
            SetSceneItem(stonename, "pose", 1, 0);
            SetSceneItem(stonename, "attribute", "collision", 0);
            SetSceneItem(stonename, "attribute", "damage", 1);
            SetSceneItem(itemname, "attribute", "active", 1);
            SetSceneItem(itemname, "attribute", "interactive", 1);
            SetSceneItem(weaponname, "attribute", "active", 1);
            SetSceneItem(itemname, "attribute", "interactive", 1);
            //NetEvent(0);
            return 1;
        }

        //关卡时间长度：分钟
        int RoundTime = 15;
        //主角出生点
        int PlayerSpawn = 6;
        //主角出生方向-不太重要，就是角色朝着哪个方向
        int PlayerSpawnDir = 90;
        //主手武器
        int PlayerWeapon = 14;
        //背包武器
        int PlayerWeapon2 = 26;
        //角色气血：150
        int PlayerHP = 3500;
        public override int GetRoundTime() { return RoundTime; }
        public override int GetPlayerSpawn() { return PlayerSpawn; }
        public override int GetPlayerSpawnDir() { return PlayerSpawnDir; }
        public override int GetPlayerWeapon() { return PlayerWeapon; }
        public override int GetPlayerWeapon2() { return PlayerWeapon2; }
        public override int GetPlayerMaxHp() { return PlayerHP; }
        public override GameResult OnUnitDead(MeteorUnit deadUnit)
        {
            if (deadUnit.Attr.NpcTemplate == "c001_npc01_01")
                return GameResult.Fail;
            return base.OnUnitDead(deadUnit);
        }
        int g_counter = 0;
        int trg0 = 0;
        int trg1 = 0;
        int trg2 = 0;
        int trg3 = 0;
        int trg4 = 0;
        int trg5 = 0;
        int timer0 = 0;
        int survivor = -1;

        //关卡启动时，增加NPC初始化
        //可以在触发一些条件时，调用AddNPC即可添加NPC，NPC也需要提交给我
        public override void OnStart()
        {
            AddNPC("c001_npc01_01");
            AddNPC("c001_npc01_02");
            AddNPC("c001_npc01_03");
            AddNPC("c001_npc01_04");
            AddNPC("c001_npc01_05");
            AddNPC("c001_npc01_06");
            AddNPC("c001_npc01_07");
            AddNPC("c001_npc01_08");
            base.OnStart();
        }

        //每一帧被客户端调用
        public override int OnUpdate()
        {
            int player = GetChar("player");
            if (player < 0)
            {
                return 0;
            }

            int c;
            int c2 = 0;
            int c3;
            int c4;
            int c5;
            int t;

            g_counter = g_counter + 1;

            if (trg0 == 0)
            {
                c = GetChar("冷燕");
                c2 = GetChar("官差﹒甲");
                c3 = GetChar("官差﹒乙");
                c4 = GetChar("官差﹒丙");
                c5 = GetChar("蒙面人");
                if (c >= 0 && c2 >= 0 && c3 >= 0 && c4 >= 0 && c5 >= 0)
                {
                    ChangeBehavior(c, "follow", player);
                    Perform(c, "pause", 8);
                    Perform(c, "say", "没想到五爪峰能通这里，星魂哥这是哪里？");
                    Perform(c, "pause", 10);
                    Perform(c, "faceto", c3);

                    ChangeBehavior(c2, "follow", c);
                    Perform(c2, "pause", 15);
                    Perform(c2, "aggress");
                    Perform(c2, "say", "你们还是来了，我们老大等候多时了");
                    Perform(c2, "pause", 2);
                    Perform(c2, "faceto", c);

                    ChangeBehavior(c3, "follow", player); PlayerPerform("say", "这里应该是伏虎山吧，要小心周围的敌人");
                    Perform(c3, "pause", 1);
                    Perform(c3, "say", "");
                    Perform(c3, "pause", 10);
                    Perform(c3, "faceto", c);

                    ChangeBehavior(c4, "follow", player);
                    Perform(c4, "pause", 5);
                    Perform(c4, "say", "看我怎么收拾你");
                    Perform(c4, "pause", 20);
                    Perform(c4, "faceto", c);

                    ChangeBehavior(c5, "patrol", 172, 127, 133, 127);
                    Perform(c5, "pause", 10);
                    Perform(c5, "faceto", player);
                    Perform(c5, "pause", 20);
                    Perform(c5, "faceto", player);

                    PlayerPerform("block", 0);
                    PlayerPerform("pause", 20);
                    PlayerPerform("block", 0);

                    trg0 = 1;
                    timer0 = GetGameTime() + 25;
                }
            }

            if (trg0 == 1)
            {
                c = GetChar("冷燕");
                c2 = GetAnyChar("官差﹒甲");
                c3 = GetAnyChar("官差﹒乙");
                c4 = GetAnyChar("官差﹒丙");
                c5 = -1;

                if (GetHP(c2) > 0 && GetHP(c3) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = c2;
                }
                if (GetHP(c3) > 0 && GetHP(c2) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = c3;
                }
                if (GetHP(c4) > 0 && GetHP(c3) <= 0 && GetHP(c2) <= 0)
                {
                    c5 = c4;
                }
                if (GetHP(c2) <= 0 && GetHP(c3) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = 9999;
                }

                if (c5 >= 0)
                {
                    if (c5 != 9999)
                    {
                        ChangeBehavior(c5, "follow", "vip");
                        Perform(c5, "aggress");
                        Perform(c5, "say", "可恶，你给我等着，我去叫人收拾你");
                        Perform(c5, "faceto", player);

                        Perform(c, "aggress");
                        Perform(c, "say", "");
                        Perform(c, "pause", 2);
                        Perform(c, "faceto", c5);

                        PlayerPerform("say", "");
                        PlayerPerform("pause", 5);

                        survivor = c5;
                    }

                    trg0 = 2;
                }
            }

            if (trg0 == 2 && survivor >= 0)
            {
                c = GetChar("破空");
                if (c >= 0 && GetHP(survivor) > 0)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", survivor);
                    if (Distance(0, 1) < 150)
                    {
                        Perform(survivor, "say", "");
                        Perform(survivor, "pause", 5);
                        Perform(survivor, "say", "");
                        Perform(survivor, "pause", 5);
                        Perform(survivor, "say", "");
                        Perform(survivor, "faceto", c);

                        ChangeBehavior(c, "follow", player);
                        Perform(c, "say", "");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "");
                        Perform(c, "pause", 3);
                        Perform(c, "faceto", survivor);

                        c2 = GetChar("军枪官差﹒甲");
                        if (c2 >= 0)
                        {
                            Perform(c2, "say", "");
                            Perform(c2, "guard", 12);
                            Perform(c2, "faceto", c);
                        }
                        c2 = GetChar("军枪官差﹒乙");
                        if (c2 >= 0)
                        {
                            Perform(c2, "say", "");
                            Perform(c2, "guard", 12);
                            Perform(c2, "faceto", c);
                        }

                        trg0 = 3;
                    }
                }
            }

            if (trg1 == 0)
            {
                c = GetChar("破空");
                c2 = GetChar("冷燕");

                SetTarget(0, "char", c);
                SetTarget(1, "char", player);
                if (c >= 0 && c2 >= 0 && GetEnemy(c) == player && Distance(0, 1) < 200)
                {
                    ChangeBehavior(c, "follow", player);
                    if (trg0 == 3)
                    {
                        Perform(c, "say", "大家都给我上,杀了他们");
                        Perform(c, "pause", 8);
                        Perform(c, "say", "你跑不掉的");
                        Perform(c, "pause", 1);
                        Perform(c, "say", "没想到我会在这把");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "好小子我们又见面了");
                        Perform(c, "faceto", player);

                        Perform(c2, "pause", 5);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 10);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 2);
                        Perform(c2, "faceto", c);

                        PlayerPerform("block", 0);
                        PlayerPerform("pause", 5);
                        PlayerPerform("say", "");
                        PlayerPerform("pause", 6);
                        PlayerPerform("say", "");
                        PlayerPerform("pause", 4);
                        PlayerPerform("block", 1);

                        StopPerform(survivor);

                        t = 17;
                    }
                    else
                    {
                        Perform(c, "say", "来人呀给我杀了他们");
                        Perform(c, "pause", 3);
                        Perform(c, "say", "");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "终于在这等到你了，拿命来");
                        Perform(c, "faceto", player);

                        Perform(c2, "pause", 5);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 2);
                        Perform(c2, "faceto", c);

                        PlayerPerform("block", 0);
                        PlayerPerform("pause", 4);
                        PlayerPerform("say", "");
                        PlayerPerform("pause", 4);
                        PlayerPerform("block", 1);

                        survivor = -1;
                        t = 10;
                    }

                    c3 = GetChar("军枪官差﹒甲");
                    if (c3 >= 0)
                    {
                        StopPerform(c3);
                        Perform(c3, "say", "");
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("军枪官差﹒乙");
                    if (c3 >= 0)
                    {
                        StopPerform(c3);
                        Perform(c3, "say", "");
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("官差﹒甲");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("官差﹒乙");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("官差﹒丙");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("蒙面人");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }

                    trg1 = 1;
                    trg5 = 1;
                }
            }

            if (trg5 == 0)
            {
                c = GetChar("破空");
                if (c >= 0)
                {
                    if (g_counter % 45 == 0 && timer0 > 0 && GetGameTime() > timer0 && GetEnemy(c) < 0)
                    {
                        Perform(c, "say", "跑那去了？所有人把他们给我找出来");
                    }

                    c3 = GetChar("军枪官差﹒甲");
                    if (c3 >= 0 && (GetEnemy(c3) == player || GetEnemy(c3) == c2))
                    {
                        Perform(c3, "say", "发现了");
                        Perform(c3, "faceto", player);
                        ChangeBehavior(c, "follow", player);
                        trg1 = 1;
                    }
                    c3 = GetChar("军枪官差﹒乙");
                    if (c3 >= 0 && (GetEnemy(c3) == player || GetEnemy(c3) == c2))
                    {
                        Perform(c3, "say", "发现了");
                        Perform(c3, "faceto", player);
                        ChangeBehavior(c, "follow", player);
                        trg1 = 1;
                    }
                    trg5 = 1;
                }
            }

            if (trg2 == 0)
            {
                c = GetChar("冷燕");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "我快不行了，你快走吧，不要管我");
                    Perform(c, "faceto", player);
                    trg2 = 1;
                }
            }
            if (trg2 == 1)
            {
                c = GetAnyChar("冷燕");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg2 = 2;
                }
            }

            if (g_counter % 20 == 0 && timer0 > 0 && GetGameTime() > timer0)
            {
                c = GetChar("冷燕");
                if (c >= 0 && !IsPerforming(player))
                {
                    SetTarget(0, "char", player);
                    SetTarget(1, "char", c);
                    if (Distance(0, 1) > 500)
                    {
                        Say(c, "星魂哥等等我");
                    }

                    if (g_counter % 40 == 0 && trg2 == 1)
                    {
                        Perform(c, "guard", 2);
                        Perform(c, "faceto", player);
                        Perform(c, "pause", 2);
                        Perform(c, "faceto", player);
                        Perform(c, "say", "");
                    }
                }
            }

            if (trg3 == 0)
            {
                c = GetChar("蒙面人");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", player);
                    if (Distance(0, 1) < 200)
                    {
                        ChangeBehavior(c, "kill", player);
                        Perform(c, "aggress");
                        Perform(c, "say", "哼，竟然不理我，看招");
                        Perform(c, "say", "小子你是快活林的杀手？");
                        Perform(c, "faceto", player);
                        trg3 = 1;
                    }
                }
            }
            if (trg3 == 1)
            {
                c = GetChar("蒙面人");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    ChangeBehavior(c, "dodge", player);
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg3 = 2;
                }
            }
            if (trg3 == 2)
            {
                c = GetAnyChar("蒙面人");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg3 = 3;
                }
            }

            if (trg4 == 0)
            {
                c = GetChar("破空");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg4 = 1;
                }
            }
            if (trg4 == 1)
            {
                c = GetAnyChar("破空");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg4 = 2;
                }
            }
            return base.OnUpdate();
        }
    }

    //后传-圆满楼
    public class LevelScript_c002 : LevelScriptBase
    {
        public override void Scene_OnLoad()
        {
        }

        //处理箱子，酒坛，桌子，椅子等道具
        public override void Scene_OnInit()
        {
            InitBoxes(g_iNumBoxes);
            InitBBoxes(g_iNumBBoxes);
            InitChairs(g_iNumChairs);
            InitDeskes(g_iNumDeskes);
            InitJugs(g_iNumJugs);
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
            //NetEvent(1);
            SetSceneItem(id, "attribute", "active", 0);
            //NetEvent(0);
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
            //NetEvent(1);
            CreateEffect(stonename, "StoneBRK");
            SetSceneItem(stonename, "pose", 1, 0);
            SetSceneItem(stonename, "attribute", "collision", 0);
            SetSceneItem(stonename, "attribute", "damage", 1);
            SetSceneItem(itemname, "attribute", "active", 1);
            SetSceneItem(itemname, "attribute", "interactive", 1);
            SetSceneItem(weaponname, "attribute", "active", 1);
            SetSceneItem(itemname, "attribute", "interactive", 1);
            //NetEvent(0);
            return 1;
        }

        //关卡时间长度：分钟
        int RoundTime = 15;
        //主角出生点
        int PlayerSpawn = 1;
        //主角出生方向-不太重要，就是角色朝着哪个方向
        int PlayerSpawnDir = 0;
        //主手武器
        int PlayerWeapon = 14;
        //背包武器
        int PlayerWeapon2 = 25;
        //角色气血：150
        int PlayerHP = 3500;
        public override int GetRoundTime() { return RoundTime; }
        public override int GetPlayerSpawn() { return PlayerSpawn; }
        public override int GetPlayerSpawnDir() { return PlayerSpawnDir; }
        public override int GetPlayerWeapon() { return PlayerWeapon; }
        public override int GetPlayerWeapon2() { return PlayerWeapon2; }
        public override int GetPlayerMaxHp() { return PlayerHP; }
        int g_counter = 0;
        int trg0 = 0;
        int trg1 = 0;
        int trg2 = 0;
        int trg3 = 0;
        int trg4 = 0;
        int trg5 = 0;
        int timer0 = 0;
        int survivor = -1;

        //关卡启动时，增加NPC初始化
        //可以在触发一些条件时，调用AddNPC即可添加NPC，NPC也需要提交给我
        public override void OnStart()
        {
            AddNPC("c001_npc03_01");
            AddNPC("c001_npc03_02");
            AddNPC("c001_npc03_03");
            AddNPC("c001_npc03_04");
            AddNPC("c001_npc03_05");
            base.OnStart();
        }

        //每一帧被客户端调用
        public override int OnUpdate()
        {
            int player = GetChar("player");
            if (player < 0)
            {
                return 0;
            }

            int c;
            int c2 = 0;
            int c3;
            int c4;
            int c5;
            int t;

            g_counter = g_counter + 1;

            if (trg0 == 0)
            {
                c = GetChar("冷燕");
                c2 = GetChar("");
                c3 = GetChar("");
                c4 = GetChar("");
                c5 = GetChar("");
                if (c >= 0 && c2 >= 0 && c3 >= 0 && c4 >= 0 && c5 >= 0)
                {
                    ChangeBehavior(c, "follow", player);
                    Perform(c, "pause", 8);
                    Perform(c, "say", "");
                    Perform(c, "pause", 10);
                    Perform(c, "faceto", c3);

                    ChangeBehavior(c2, "follow", c);
                    Perform(c2, "pause", 15);
                    Perform(c2, "aggress");
                    Perform(c2, "say", "");
                    Perform(c2, "pause", 2);
                    Perform(c2, "faceto", c);

                    ChangeBehavior(c3, "follow", player); PlayerPerform("say", "");
                    Perform(c3, "pause", 1);
                    Perform(c3, "say", "");
                    Perform(c3, "pause", 10);
                    Perform(c3, "faceto", c);

                    ChangeBehavior(c4, "follow", player);
                    Perform(c4, "pause", 5);
                    Perform(c4, "say", "");
                    Perform(c4, "pause", 20);
                    Perform(c4, "faceto", c);

                    ChangeBehavior(c5, "patrol", 172, 127, 133, 127);
                    Perform(c5, "pause", 10);
                    Perform(c5, "faceto", player);
                    Perform(c5, "pause", 20);
                    Perform(c5, "faceto", player);

                    PlayerPerform("block", 0);
                    PlayerPerform("pause", 20);
                    PlayerPerform("block", 0);

                    trg0 = 1;
                    timer0 = GetGameTime() + 25;
                }
            }
            if (trg0 == 1)
            {
                c = GetChar("冷燕");
                c2 = GetAnyChar("");
                c3 = GetAnyChar("");
                c4 = GetAnyChar("");
                c5 = -1;

                if (GetHP(c2) > 0 && GetHP(c3) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = c2;
                }
                if (GetHP(c3) > 0 && GetHP(c2) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = c3;
                }
                if (GetHP(c4) > 0 && GetHP(c3) <= 0 && GetHP(c2) <= 0)
                {
                    c5 = c4;
                }
                if (GetHP(c2) <= 0 && GetHP(c3) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = 9999;
                }

                if (c5 >= 0)
                {
                    if (c5 != 9999)
                    {
                        ChangeBehavior(c5, "follow", "vip");
                        Perform(c5, "aggress");
                        Perform(c5, "say", "");
                        Perform(c5, "faceto", player);

                        Perform(c, "aggress");
                        Perform(c, "say", "");
                        Perform(c, "pause", 2);
                        Perform(c, "faceto", c5);

                        PlayerPerform("say", "");
                        PlayerPerform("pause", 5);

                        survivor = c5;
                    }

                    trg0 = 2;
                }
            }

            if (trg0 == 2 && survivor >= 0)
            {
                c = GetChar("蒙面人");
                if (c >= 0 && GetHP(survivor) > 0)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", survivor);
                    if (Distance(0, 1) < 150)
                    {
                        Perform(survivor, "say", "");
                        Perform(survivor, "pause", 5);
                        Perform(survivor, "say", "");
                        Perform(survivor, "pause", 5);
                        Perform(survivor, "say", "");
                        Perform(survivor, "faceto", c);

                        ChangeBehavior(c, "follow", player);
                        Perform(c, "say", "");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "");
                        Perform(c, "pause", 3);
                        Perform(c, "faceto", survivor);

                        c2 = GetChar("金刀门﹒甲");
                        if (c2 >= 0)
                        {
                            Perform(c2, "say", "");
                            Perform(c2, "guard", 12);
                            Perform(c2, "faceto", c);
                        }
                        c2 = GetChar("金刀门﹒乙");
                        if (c2 >= 0)
                        {
                            Perform(c2, "say", "");
                            Perform(c2, "guard", 12);
                            Perform(c2, "faceto", c);
                        }

                        trg0 = 3;
                    }
                }
            }

            if (trg1 == 0)
            {
                c = GetChar("蒙面人");
                c2 = GetChar("冷燕");

                SetTarget(0, "char", c);
                SetTarget(1, "char", player);
                if (c >= 0 && c2 >= 0 && GetEnemy(c) == player && Distance(0, 1) < 200)
                {
                    ChangeBehavior(c, "follow", player);
                    if (trg0 == 3)
                    {
                        Perform(c, "say", "");
                        Perform(c, "pause", 8);
                        Perform(c, "say", "");
                        Perform(c, "pause", 1);
                        Perform(c, "say", "小子看你往哪跑");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);

                        Perform(c2, "pause", 5);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 10);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 2);
                        Perform(c2, "faceto", c);

                        PlayerPerform("block", 0);
                        PlayerPerform("pause", 5);
                        PlayerPerform("say", "");
                        PlayerPerform("pause", 6);
                        PlayerPerform("say", "");
                        PlayerPerform("pause", 4);
                        PlayerPerform("block", 1);

                        StopPerform(survivor);

                        t = 17;
                    }
                    else
                    {
                        Perform(c, "say", "给我杀了他们");
                        Perform(c, "pause", 3);
                        Perform(c, "say", "不错");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "好小子，你跑不掉了，拿命来吧");
                        Perform(c, "faceto", player);

                        Perform(c2, "pause", 5);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 4);
                        Perform(c2, "faceto", c);

                        PlayerPerform("block", 0);
                        PlayerPerform("pause", 4);
                        PlayerPerform("say", "你们是范璇谁派来的？");
                        PlayerPerform("pause", 2);
                        PlayerPerform("block", 1);

                        survivor = -1;
                        t = 10;
                    }

                    c3 = GetChar("金刀门﹒甲");
                    if (c3 >= 0)
                    {
                        StopPerform(c3);
                        Perform(c3, "say", "");
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("金刀门﹒乙");
                    if (c3 >= 0)
                    {
                        StopPerform(c3);
                        Perform(c3, "say", "");
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("蒙面人.护卫");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }

                    trg1 = 1;
                    trg5 = 1;
                }
            }

            if (trg5 == 0)
            {
                c = GetChar("蒙面人");
                if (c >= 0)
                {
                    if (g_counter % 45 == 0 && timer0 > 0 && GetGameTime() > timer0 && GetEnemy(c) < 0)
                    {
                        Perform(c, "say", "跑那去了？所有人把他们给我找出来");
                    }

                    c3 = GetChar("金刀门﹒甲");
                    if (c3 >= 0 && (GetEnemy(c3) == player || GetEnemy(c3) == c2))
                    {
                        Perform(c3, "say", "发现了");
                        Perform(c3, "faceto", player);
                        ChangeBehavior(c, "follow", player);
                        trg1 = 1;
                    }
                    c3 = GetChar("金刀门﹒乙");
                    if (c3 >= 0 && (GetEnemy(c3) == player || GetEnemy(c3) == c2))
                    {
                        Perform(c3, "say", "发现了");
                        Perform(c3, "faceto", player);
                        ChangeBehavior(c, "follow", player);
                        trg1 = 1;
                    }
                    trg5 = 1;
                }
            }

            if (trg2 == 0)
            {
                c = GetChar("冷燕");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "我快不行了，你快走吧，不要管我");
                    Perform(c, "faceto", player);
                    trg2 = 1;
                }
            }
            if (trg2 == 1)
            {
                c = GetAnyChar("冷燕");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg2 = 2;
                }
            }

            if (g_counter % 20 == 0 && timer0 > 0 && GetGameTime() > timer0)
            {
                c = GetChar("冷燕");
                if (c >= 0 && !IsPerforming(player))
                {
                    SetTarget(0, "char", player);
                    SetTarget(1, "char", c);
                    if (Distance(0, 1) > 500)
                    {
                        Say(c, "星魂哥等等我");
                    }

                    if (g_counter % 40 == 0 && trg2 == 1)
                    {
                        Perform(c, "guard", 2);
                        Perform(c, "faceto", player);
                        Perform(c, "pause", 2);
                        Perform(c, "faceto", player);
                        Perform(c, "say", "");
                    }
                }
            }

            if (trg3 == 0)
            {
                c = GetChar("蒙面人.护卫");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", player);
                    if (Distance(0, 1) < 200)
                    {
                        ChangeBehavior(c, "kill", player);
                        Perform(c, "say", "这次你可跑不掉了");
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);
                        trg3 = 1;
                    }
                }
            }
            if (trg3 == 1)
            {
                c = GetChar("蒙面人.护卫");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    ChangeBehavior(c, "dodge", player);
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg3 = 2;
                }
            }
            if (trg3 == 2)
            {
                c = GetAnyChar("蒙面人.护卫");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg3 = 3;
                }
            }

            if (trg4 == 0)
            {
                c = GetChar("蒙面人");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg4 = 1;
                }
            }
            if (trg4 == 1)
            {
                c = GetAnyChar("蒙面人");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg4 = 2;
                }
            }
            return base.OnUpdate();
        }
    }

    //后传-秦皇陵
    public class LevelScript_c003 : LevelScriptBase
    {
        int RoundTime = 20;
        int PlayerSpawn = 1;
        int PlayerSpawnDir = 90;
        int PlayerWeapon = 32;
        int PlayerWeapon2 = 6;
        int PlayerHP = 3500;
        public override int GetRoundTime() { return RoundTime; }
        public override int GetPlayerSpawn() { return PlayerSpawn; }
        public override int GetPlayerSpawnDir() { return PlayerSpawnDir; }
        public override int GetPlayerWeapon() { return PlayerWeapon; }
        public override int GetPlayerWeapon2() { return PlayerWeapon2; }
        public override int GetPlayerMaxHp() { return PlayerHP; }
        public override void Scene_OnLoad()
        {
            int i;
            string name = "";

            // knife	
            for (i = 1; i <= 3; i++)
            {
                MakeString(ref name, "D_knife", i);
                SetSceneItem(name, "name", "machine");

                SetSceneItem(name, "attribute", "damage", 1);
                SetSceneItem(name, "attribute", "damagevalue", 300);
            }

            // stone step
            for (i = 1; i <= 3; i++)
            {
                MakeString(ref name, "D_sn02st", i);
                SetSceneItem(name, "name", "machine");
                SetSceneItem(name, "attribute", "collision", 1);
            }
        }

        public override void Scene_OnInit()
        {
            SetSceneItem("D_knife01", "pose", 1, 1);
            SetSceneItem("D_knife01", "frame", 0);

            SetSceneItem("D_knife02", "pose", 1, 1);
            SetSceneItem("D_knife02", "frame", 30);

            SetSceneItem("D_knife03", "pose", 1, 1);
            SetSceneItem("D_knife03", "frame", 60);

            InitBoxes(g_iNumBoxes);
            InitBBoxes(g_iNumBBoxes);
            InitChairs(g_iNumChairs);
            InitDeskes(g_iNumDeskes);
            InitJugs(g_iNumJugs);

            SetSceneItem("D_sn02st01", "attribute", "active", 0);
            SetSceneItem("D_sn02st02", "attribute", "active", 0);
            SetSceneItem("D_sn02st03", "attribute", "active", 0);

            SetSceneItem("D_IPItem01", "attribute", "active", 1);
        }

        public void D_IPItem01_OnPickUp()
        {
            //

            SetSceneItem("D_sn02st01", "attribute", "active", 1);
            SetSceneItem("D_sn02st02", "attribute", "active", 1);
            SetSceneItem("D_sn02st03", "attribute", "active", 1);

            CreateEffect("D_sn02st01", "FuTi-UP", true);
            SetSceneItem("D_sn02st01", "pose", 1, 1);
            SetSceneItem("D_sn02st01", "frame", 0);

            SetSceneItem("D_sn02st02", "pose", 1, 1);
            SetSceneItem("D_sn02st02", "frame", 60);

            SetSceneItem("D_sn02st03", "pose", 1, 1);
            SetSceneItem("D_sn02st03", "frame", 120);

            //
        }

        int trg0 = 0;
        int trg1 = 0;
        int trg2 = 0;
        int trg3 = 0;
        int trg4 = 0;
        int trg5 = 0;
        int trg6 = 0;
        int trg7 = 0;
        int trg8 = 0;
        int trg9 = 0;
        int timer0 = 0;
        int timer1 = 0;
        int gameover = 0;
        int now = 0;
        public override void OnStart()
        {
            AddNPC("c001_npc04_01");
            base.OnStart();
        }

        int FindEnemy(int c, int p)
        {
            int c2;

            if (c < 0)
            {
                return 0;
            }

            if (GetEnemy(c) == p)
            {
                c2 = GetChar("");
                if (c2 >= 0)
                {
                    Perform(c, "say", "");

                    ChangeBehavior(c2, "follow", p);
                    Perform(c2, "say", "");
                    Perform(c2, "pause", 3);
                }
                return 1;
            }

            return 0;
        }

        public override int OnUpdate()
        {
            int player = GetAnyChar("player");
            if (player < 0)
            {
                return 0;
            }

            int c;
            int c2;
            int flag;

            if (trg9 == 0)
            {
                PlayerPerform("say", "终于来到了秦皇陵,不知宝贝在哪，我要找找");
                trg9 = 1;
            }

            if (trg0 == 0)
            {
                c = GetChar("");
                if (c >= 0)
                {
                    SetTarget(0, "waypoint", 119);      // near flag position
                    SetTarget(1, "char", player);
                    if (Distance(0, 1) < 400)
                    {
                        ChangeBehavior(c, "follow", "flag");
                        Perform(c, "say", "");
                        SetTarget(0, "waypoint", 83);   // stone position
                        ChangeBehavior(c, "attacktarget", 0, 3);
                        trg0 = 1;
                    }
                }
            }

            if (trg1 == 0)
            {
                c = GetChar("盗墓者");
                if (c >= 0)
                {
                    if (GetChar("flag") == c)
                    {
                        Perform(c, "say", "这地方的出口在哪，我要想想");
                        Perform(c, "pause", 8);
                        Perform(c, "say", "哈，拿到宝贝了，一定很值钱啊,我发财了,哈哈");
                        trg1 = 1;
                    }
                }
            }
            if (trg1 == 1)
            {
                c = GetChar("盗墓者");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    ChangeBehavior(c, "patrol", 59, 92, 83); PlayerPerform("say", "识相的交出宝贝，不然");
                    Perform(c, "guard", 5);
                    Perform(c, "say", "让我交出宝贝，你小子找死，看招");
                    Perform(c, "faceto", player);
                    trg1 = 2;
                }
            }
            if (trg1 == 2)
            {
                c = GetChar("盗墓者");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2 && GetEnemy(c) == player)
                {



                    Perform(c, "say", "看来我要来硬的了，看招"); Perform(c, "use", 34); Perform(c, "use", 33);
                    Perform(c, "faceto", player);
                    trg1 = 3;
                    timer0 = GetGameTime() + 60;
                }
            }
            if (trg1 == 3)
            {
                c = GetChar("盗墓者");
                if (c >= 0 && GetGameTime() > timer0 && GetEnemy(c) == player)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", player);
                    if (Distance(0, 1) < 100)
                    {
                        ChangeBehavior(c, "follow", player);
                        Perform(c, "guard", 3);
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);
                        trg1 = 4;
                    }
                }
            }
            if (trg1 > 0 && trg1 != 5)
            {
                c = GetAnyChar("盗墓者");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "我﹒﹒我的宝贝啊﹒﹒﹒﹒");
                    trg1 = 5;
                }
            }

            if (trg2 == 0)
            {
                c = GetChar("");
                c2 = GetChar("盗墓者");
                if (c >= 0 && c2 >= 0 && GetEnemy(c) == c2 && GetEnemy(c2) == c)
                {
                    flag = GetChar("flag");
                    if (flag == c)
                    {
                        Perform(c2, "aggress");
                        Perform(c2, "say", "");
                        Perform(c2, "faceto", c);
                        trg2 = 1;
                    }

                    if (flag == c2)
                    {
                        Perform(c2, "pause", 4);
                        Perform(c, "aggress");
                        Perform(c, "say", "");
                        Perform(c, "faceto", c2);

                        Perform(c2, "aggress");
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 4);
                        Perform(c2, "faceto", c);
                        trg2 = 1;
                    }
                }
            }

            if (trg3 == 0)
            {
                c = GetChar("");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    Perform(c, "aggress");
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);

                    PlayerPerform("say", "");
                    PlayerPerform("pause", 4);

                    trg3 = 1;
                }
            }
            if (trg3 == 1)
            {
                c = GetChar("");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    if (GetChar("flag") == c)
                    {
                        ChangeBehavior(c, "run");
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);

                        PlayerPerform("say", "");
                        PlayerPerform("pause", 5);

                        trg3 = 2;
                    }
                }
            }

            if (trg4 == 0)
            {
                c = GetChar("flag");
                if (c == player)
                {
                    c2 = GetChar("");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "follow", "flag");
                    }

                    PlayerPerform("say", "此地不宜久留，我要找到出口出去才行");
                    PlayerPerform("pause", 3);
                    PlayerPerform("say", "终于拿到手了，这里这么多机关");
                    trg4 = 1;
                }
            }
            if (trg4 == 1)
            {
                c = GetChar("盗墓者");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    Perform(c, "attack");
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg4 = 2;
                }
            }

            if (trg5 == 0)
            {
                c = GetChar("");
                if (FindEnemy(c, player) != 0)
                {
                    trg5 = 1;
                }
            }

            if (trg6 == 0)
            {
                c = GetChar("");
                if (FindEnemy(c, player) != 0)
                {
                    trg6 = 1;
                }
            }

            if (trg7 == 0)
            {
                c = GetChar("");
                if (FindEnemy(c, player) != 0)
                {
                    trg7 = 1;
                }
            }

            if (trg5 == 0 && trg6 == 0 && trg7 == 0 && trg8 == 0)
            {
                c = GetChar("");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    ChangeBehavior(c, "follow", player);
                    Perform(c, "say", "");
                    Perform(c, "pause", 3);
                    Perform(c, "faceto", player);
                    trg8 = 1;
                }
            }


            now = GetGameTime();
            if (gameover == 0 && GetHP(player) <= 0)
            {
                gameover = -1;
                timer1 = now + 2;
            }
            if ((gameover == 1 || gameover == -1) && now > timer1)
            {
                GameOver(gameover);
                gameover = 2;
            }
            return 0;
        }
    }

    //后传-四方阵
    public class LevelScript_c004 : LevelScriptBase
    {
        int RoundTime = 30;
        int PlayerSpawn = 7;
        int PlayerSpawnDir = 0;
        int PlayerWeapon = 6;
        int PlayerWeapon2 = 0;
        int PlayerHP = 3500;
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
            AddNPC("c001_npc05_01");
            AddNPC("c001_npc05_02");
            AddNPC("c001_npc05_03");
            base.OnStart();
        }

        int GotoLeader(int c)
        {
            int c2 = GetChar("江湖杀手.1");
            if (c2 >= 0)
            {
                ChangeBehavior(c, "follow", c2);
                SetTarget(0, "char", c2);
                ChangeBehavior(c, "attacktarget", 0);
                return 1;
            }

            return 0;
        }

        int Report(int c1, int c2, int c3)
        {
            if (c1 >= 0 && c3 >= 0)
            {
                SetTarget(0, "char", c1);
                SetTarget(1, "char", c3);

                if (Distance(0, 1) < 100)
                {
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "follow", c3);
                    }

                    ChangeBehavior(c1, "follow", c3); Perform(c2, "use", 33); Perform(c3, "use", 33);
                    Perform(c1, "pause", 4);
                    Perform(c1, "say", "");
                    Perform(c1, "faceto", c3);

                    int player = GetChar("player");
                    ChangeBehavior(c3, "follow", player);
                    Perform(c3, "say", "");
                    Perform(c3, "pause", 3);
                    Perform(c3, "faceto", c1);

                    return 1;
                }
            }

            return 0;
        }

        public override int OnUpdate()
        {
            int player = GetChar("player");
            if (player < 0)
            {
                return 0;
            }

            int c;
            int c2 = 0;
            int c3 = 0;

            if (trg0 == 0)
            {
                c = GetChar("江湖杀手.2");
                c2 = GetChar("江湖杀手.1");
                c3 = GetChar("江湖杀手.3");

                if (c >= 0 && c2 >= 0 && c3 >= 0)
                {
                    Perform(c, "say", "是");
                    Perform(c, "faceto", c2);
                    Perform(c, "use", 33);
                    ChangeBehavior(c2, "patrol", 0, 1, 4, 2, 3); Perform(c2, "use", 33);
                    Perform(c2, "say", "好小子，你跑不了了，给我上，杀了他范老大有赏");
                    Perform(c2, "faceto", c);

                    ChangeBehavior(c3, "patrol", 18, 19, 75, 55, 79, 38, 77, 78, 14, 20); Perform(c3, "use", 33);
                    Perform(c3, "say", "是");
                    Perform(c3, "pause", 3);
                    Perform(c3, "faceto", c);

                    trg0 = 4;
                }
            }
            if (trg0 == 1 && trg3 == 0)
            {
                c = GetChar("江湖杀手.1");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    c2 = GetChar("江湖杀手.2");
                    c3 = GetChar("江湖杀手.3");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "follow", c); Perform(c2, "use", 33);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 2);
                        Perform(c2, "faceto", c);
                    }
                    if (c3 >= 0)
                    {
                        ChangeBehavior(c3, "follow", c); Perform(c3, "use", 33);
                        Perform(c3, "say", "");
                        Perform(c3, "pause", 2);
                        Perform(c2, "faceto", c);
                    }

                    if (c2 < 0 && c3 < 0)
                    {
                        Perform(c, "say", "");
                        Perform(c, "pause", 5);
                    }

                    Perform(c, "say", "");
                    Perform(c, "faceto", player);

                    trg0 = 2;
                }
            }

            if (trg1 == 0 && trg0 == 1 && trg3 == 0)
            {
                c = GetChar("江湖杀手.2");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    Perform(c, "guard", 3); Perform(c2, "use", 33);
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg1 = 1;
                }
            }
            if (trg1 == 1)
            {
                c = GetChar("江湖杀手.2");
                if (c >= 0 && GetEnemy(c) != player)
                {
                    Perform(c, "say", "");
                    trg1 = 2;
                }
            }
            if (trg1 == 2 && trg3 == 0)
            {
                c = GetChar("江湖杀手.2");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    GotoLeader(c);

                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg1 = 3;
                    trg3 = 1;
                }
            }

            if (trg2 == 0 && trg0 == 1 && trg3 == 0)
            {
                c = GetChar("江湖杀手.3");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    Perform(c, "aggress"); Perform(c3, "use", 33);
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg2 = 1;
                }
            }
            if (trg2 == 1)
            {
                c = GetChar("江湖杀手.3");
                if (c >= 0 && GetEnemy(c) != player)
                {
                    Perform(c, "say", "");
                    trg2 = 2;
                }
            }
            if (trg2 == 2 && trg3 == 0)
            {
                c = GetChar("江湖杀手.3");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    GotoLeader(c);

                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg2 = 3;
                    trg3 = 1;
                }
            }

            if (trg4 == 0 && trg3 == 0)
            {
                c = GetChar("江湖杀手.2");
                if (c >= 0 && GetHP(c) <= GetMaxHP(c) / 4)
                {
                    GotoLeader(c);
                    Perform(c, "say", "");
                    trg4 = 1;
                    trg3 = 1;
                    trg1 = 3;
                }
            }
            if (trg5 == 0 && trg3 == 0)
            {
                c = GetChar("江湖杀手.3");
                if (c >= 0 && GetHP(c) <= GetMaxHP(c) / 4)
                {
                    GotoLeader(c);
                    Perform(c, "say", "");
                    trg5 = 1;
                    trg3 = 1;
                    trg2 = 3;
                }
            }

            if (trg3 == 1)
            {
                c = GetChar("江湖杀手.1");
                c2 = GetChar("江湖杀手.2");
                c3 = GetChar("江湖杀手.3");
                if (Report(c2, c3, c) != 0 || Report(c3, c2, c) != 0)
                {
                    trg3 = 2;
                }
            }

            if (trg6 == 0)
            {
                c = GetChar("江湖杀手.1");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "guard", 4);
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg6 = 1;
                }
            }
            if (trg6 == 1)
            {
                c = GetAnyChar("江湖杀手.1");
                if (GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg6 = 2;
                }
            }
            return base.OnUpdate();
        }
    }

    //洛阳城
    public class LevelScript_c005 : LevelScriptBase
    {
        int RoundTime = 30;
        int PlayerSpawn = 73;
        int PlayerSpawnDir = 330;
        int PlayerWeapon = 9;
        int PlayerWeapon2 = 16;
        int PlayerHP = 5000;
        public override int GetRoundTime() { return RoundTime; }
        public override int GetPlayerSpawn() { return PlayerSpawn; }
        public override int GetPlayerSpawnDir() { return PlayerSpawnDir; }
        public override int GetPlayerWeapon() { return PlayerWeapon; }
        public override int GetPlayerWeapon2() { return PlayerWeapon2; }
        public override int GetPlayerMaxHp() { return PlayerHP; }
        public override GameResult OnUnitDead(MeteorUnit deadUnit)
        {
            if (deadUnit.Attr.NpcTemplate == "c001_npc09_01")
                return GameResult.Fail;
            return base.OnUnitDead(deadUnit);
        }
        int g_counter = 0;
        int trg0 = 0;
        int trg1 = 0;
        int trg2 = 0;
        int trg3 = 0;
        int trg4 = 0;
        int trg5 = 0;
        int timer0 = 0;
        int survivor = -1;
        public override void OnStart()
        {
            AddNPC("c001_npc09_01");
            AddNPC("c001_npc09_02");
            AddNPC("c001_npc09_03");
            AddNPC("c001_npc09_04");
            AddNPC("c001_npc09_05");
            AddNPC("c001_npc09_06");
            AddNPC("c001_npc09_07");
            AddNPC("c001_npc09_08");
            base.OnStart();
        }

        public override int OnUpdate()
        {
            int player = GetChar("player");
            if (player < 0)
            {
                return 0;
            }

            int c;
            int c2 = 0;
            int c3;
            int c4;
            int c5;
            int t;

            g_counter = g_counter + 1;

            if (trg0 == 0)
            {
                c = GetChar("冷燕");
                c2 = GetChar("孙玉伯");
                c3 = GetChar("律香川");
                c4 = GetChar("孙剑");
                c5 = GetChar("护法");
                if (c >= 0 && c2 >= 0 && c3 >= 0 && c4 >= 0 && c5 >= 0)
                {
                    ChangeBehavior(c, "follow", player);
                    Perform(c, "pause", 8);
                    Perform(c, "say", "孙伯伯多谢了，但范璇不知道在什么地方");
                    Perform(c, "pause", 10);
                    Perform(c, "faceto", c3);

                    ChangeBehavior(c2, "follow", c);
                    Perform(c2, "pause", 15);

                    Perform(c2, "say", "冷燕的父母和高寄萍都是我多年的好友, 范璇好大的胆,星少侠和冷姑娘带路吧,老夫祝你们一臂之力"); PlayerPerform("say", "对了，这里有范璇的人我们去问问，走吧");
                    Perform(c2, "pause", 4);
                    Perform(c2, "faceto", c);

                    ChangeBehavior(c3, "follow", player);
                    Perform(c3, "pause", 5);
                    Perform(c3, "say", "早就听说范璇这群人，做尽坏事");
                    Perform(c3, "pause", 15);
                    Perform(c3, "faceto", c);

                    ChangeBehavior(c4, "follow", player);
                    Perform(c4, "pause", 5);
                    Perform(c4, "say", "这等坏人，该杀");
                    Perform(c4, "pause", 20);
                    Perform(c4, "faceto", c);

                    ChangeBehavior(c5, "patrol", 172, 127, 133, 127);
                    Perform(c5, "pause", 10);
                    Perform(c5, "faceto", player);
                    Perform(c5, "pause", 20);
                    Perform(c5, "faceto", player);

                    PlayerPerform("block", 0);
                    PlayerPerform("pause", 20);
                    PlayerPerform("block", 0);

                    trg0 = 1;
                    timer0 = GetGameTime() + 25;
                }
            }

            if (trg0 == 1)
            {
                c = GetChar("冷燕");
                c2 = GetAnyChar("孙玉伯");
                c3 = GetAnyChar("律香川");
                c4 = GetAnyChar("孙剑");
                c5 = -1;

                if (GetHP(c2) > 0 && GetHP(c3) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = c2;
                }
                if (GetHP(c3) > 0 && GetHP(c2) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = c3;
                }
                if (GetHP(c4) > 0 && GetHP(c3) <= 0 && GetHP(c2) <= 0)
                {
                    c5 = c4;
                }
                if (GetHP(c2) <= 0 && GetHP(c3) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = 9999;
                }

                if (c5 >= 0)
                {
                    if (c5 != 9999)
                    {
                        ChangeBehavior(c5, "follow", "vip");
                        Perform(c5, "aggress");
                        Perform(c5, "say", "");
                        Perform(c5, "faceto", player);

                        Perform(c, "aggress");
                        Perform(c, "say", "");
                        Perform(c, "pause", 2);
                        Perform(c, "faceto", c5);

                        PlayerPerform("say", "");
                        PlayerPerform("pause", 5);

                        survivor = c5;
                    }

                    trg0 = 2;
                }
            }

            if (trg0 == 2 && survivor >= 0)
            {
                c = GetChar("蒙面人");
                if (c >= 0 && GetHP(survivor) > 0)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", survivor);
                    if (Distance(0, 1) < 150)
                    {
                        Perform(survivor, "say", "");
                        Perform(survivor, "pause", 5);
                        Perform(survivor, "say", "");
                        Perform(survivor, "pause", 5);
                        Perform(survivor, "say", "");
                        Perform(survivor, "faceto", c);

                        ChangeBehavior(c, "follow", player);
                        Perform(c, "say", "");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "");
                        Perform(c, "pause", 3);
                        Perform(c, "faceto", survivor);

                        c2 = GetChar("地头蛇.黑龙");
                        if (c2 >= 0)
                        {
                            Perform(c2, "say", "");
                            Perform(c2, "guard", 12);
                            Perform(c2, "faceto", c);
                        }
                        c2 = GetChar("地头蛇.煞虎");
                        if (c2 >= 0)
                        {
                            Perform(c2, "say", "");
                            Perform(c2, "guard", 12);
                            Perform(c2, "faceto", c);
                        }

                        trg0 = 3;
                    }
                }
            }

            if (trg1 == 0)
            {
                c = GetChar("蒙面人");
                c2 = GetChar("冷燕");

                SetTarget(0, "char", c);
                SetTarget(1, "char", player);
                if (c >= 0 && c2 >= 0 && GetEnemy(c) == player && Distance(0, 1) < 200)
                {
                    ChangeBehavior(c, "follow", player);
                    if (trg0 == 3)
                    {
                        Perform(c, "say", "");
                        Perform(c, "pause", 8);
                        Perform(c, "say", "");
                        Perform(c, "pause", 1);
                        Perform(c, "say", "");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);

                        Perform(c2, "pause", 5);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 10);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 2);
                        Perform(c2, "faceto", c);

                        PlayerPerform("block", 0);
                        PlayerPerform("pause", 5);
                        PlayerPerform("say", "");
                        PlayerPerform("pause", 6);
                        PlayerPerform("say", "");
                        PlayerPerform("pause", 4);
                        PlayerPerform("block", 1);

                        StopPerform(survivor);

                        t = 17;
                    }
                    else
                    {
                        Perform(c, "say", "大家一起上,杀了他们");
                        Perform(c, "pause", 3);
                        Perform(c, "say", "我不知道，你们休想活着离开这里");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "小子，你终于来了，果然不出我们范老大所料");
                        Perform(c, "faceto", player);

                        Perform(c2, "pause", 5);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 2);
                        Perform(c2, "faceto", c);

                        PlayerPerform("block", 0);
                        PlayerPerform("pause", 4);
                        PlayerPerform("say", "又是你们这群人，范璇在哪？");
                        PlayerPerform("pause", 4);
                        PlayerPerform("block", 1);

                        survivor = -1;
                        t = 10;
                    }

                    c3 = GetChar("地头蛇.黑龙");
                    if (c3 >= 0)
                    {
                        StopPerform(c3);
                        Perform(c3, "say", "");
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("地头蛇.煞虎");
                    if (c3 >= 0)
                    {
                        StopPerform(c3);
                        Perform(c3, "say", "");
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("孙玉伯");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("律香川");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("孙剑");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("护法");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }

                    trg1 = 1;
                    trg5 = 1;
                }
            }

            if (trg5 == 0)
            {
                c = GetChar("蒙面人");
                if (c >= 0)
                {
                    if (g_counter % 45 == 0 && timer0 > 0 && GetGameTime() > timer0 && GetEnemy(c) < 0)
                    {
                        Perform(c, "say", "跑那去了？所有人就算翻了洛阳城也要给我找出来");
                    }

                    c3 = GetChar("地头蛇.黑龙");
                    if (c3 >= 0 && (GetEnemy(c3) == player || GetEnemy(c3) == c2))
                    {
                        Perform(c3, "say", "发现了");
                        Perform(c3, "faceto", player);
                        ChangeBehavior(c, "follow", player);
                        trg1 = 1;
                    }
                    c3 = GetChar("地头蛇.煞虎");
                    if (c3 >= 0 && (GetEnemy(c3) == player || GetEnemy(c3) == c2))
                    {
                        Perform(c3, "say", "发现了");
                        Perform(c3, "faceto", player);
                        ChangeBehavior(c, "follow", player);
                        trg1 = 1;
                    }
                    trg5 = 1;
                }
            }

            if (trg2 == 0)
            {
                c = GetChar("冷燕");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "我快不行了，你快走吧，不要管我");
                    Perform(c, "faceto", player);
                    trg2 = 1;
                }
            }
            if (trg2 == 1)
            {
                c = GetAnyChar("冷燕");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg2 = 2;
                }
            }

            if (g_counter % 20 == 0 && timer0 > 0 && GetGameTime() > timer0)
            {
                c = GetChar("冷燕");
                if (c >= 0 && !IsPerforming(player))
                {
                    SetTarget(0, "char", player);
                    SetTarget(1, "char", c);
                    if (Distance(0, 1) > 500)
                    {
                        Say(c, "星魂哥等等我");
                    }

                    if (g_counter % 40 == 0 && trg2 == 1)
                    {
                        Perform(c, "guard", 2);
                        Perform(c, "faceto", player);
                        Perform(c, "pause", 2);
                        Perform(c, "faceto", player);
                        Perform(c, "say", "");
                    }
                }
            }

            if (trg3 == 0)
            {
                c = GetChar("护法");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", player);
                    if (Distance(0, 1) < 200)
                    {
                        ChangeBehavior(c, "kill", player);
                        Perform(c, "aggress");
                        Perform(c, "say", "");
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);
                        trg3 = 1;
                    }
                }
            }
            if (trg3 == 1)
            {
                c = GetChar("护法");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    ChangeBehavior(c, "dodge", player);
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg3 = 2;
                }
            }
            if (trg3 == 2)
            {
                c = GetAnyChar("护法");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg3 = 3;
                }
            }

            if (trg4 == 0)
            {
                c = GetChar("蒙面人");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg4 = 1;
                }
            }
            if (trg4 == 1)
            {
                c = GetAnyChar("蒙面人");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg4 = 2;
                }
            }

            return base.OnUpdate();
        }
    }

    //卧龙窟
    public class LevelScript_c006 : LevelScriptBase
    {
        int RoundTime = 35;
        int PlayerSpawn = 31;
        int PlayerSpawnDir = 130;
        int PlayerWeapon = 14;
        int PlayerWeapon2 = 16;
        int PlayerHP = 5000;
        public override int GetRoundTime() { return RoundTime; }
        public override int GetPlayerSpawn() { return PlayerSpawn; }
        public override int GetPlayerSpawnDir() { return PlayerSpawnDir; }
        public override int GetPlayerWeapon() { return PlayerWeapon; }
        public override int GetPlayerWeapon2() { return PlayerWeapon2; }
        public override int GetPlayerMaxHp() { return PlayerHP; }
        public override GameResult OnUnitDead(MeteorUnit deadUnit)
        {
            if (deadUnit.Attr.NpcTemplate == "c001_npc10_01")
                return GameResult.Fail;
            return base.OnUnitDead(deadUnit);
        }
        int g_counter = 0;
        int trg0 = 0;
        int trg1 = 0;
        int trg2 = 0;
        int trg3 = 0;
        int trg4 = 0;
        int trg5 = 0;
        int timer0 = 0;
        int survivor = -1;
        public override void OnStart()
        {
            AddNPC("c001_npc10_01");
            AddNPC("c001_npc10_02");
            AddNPC("c001_npc10_03");
            AddNPC("c001_npc10_04");
            AddNPC("c001_npc10_05");
            AddNPC("c001_npc10_06");
            AddNPC("c001_npc10_07");
            AddNPC("c001_npc10_08");
            base.OnStart();
        }

        public override int OnUpdate()
        {
            int player = GetChar("player");
            if (player < 0)
            {
                return 0;
            }

            int c;
            int c2 = 0;
            int c3;
            int c4;
            int c5;
            int t;

            g_counter = g_counter + 1;

            if (trg0 == 0)
            {
                c = GetChar("冷燕");
                c2 = GetChar("孙玉伯");
                c3 = GetChar("律香川");
                c4 = GetChar("孙剑");
                c5 = GetChar("终极杀手");
                if (c >= 0 && c2 >= 0 && c3 >= 0 && c4 >= 0 && c5 >= 0)
                {
                    ChangeBehavior(c, "follow", player);
                    Perform(c, "pause", 8);
                    Perform(c, "say", "是啊哥哥，范璇会藏在这吗？");
                    Perform(c, "pause", 10);
                    Perform(c, "faceto", c3);

                    ChangeBehavior(c2, "follow", c);
                    Perform(c2, "pause", 15);

                    Perform(c2, "say", "星少侠，范璇会在这里吗？"); PlayerPerform("say", "我也不知道，我们只有找找看了，走");
                    Perform(c2, "pause", 4);
                    Perform(c2, "faceto", c);

                    ChangeBehavior(c3, "follow", player);
                    Perform(c3, "pause", 5);
                    Perform(c3, "say", "");
                    Perform(c3, "pause", 15);
                    Perform(c3, "faceto", c);

                    ChangeBehavior(c4, "follow", player);
                    Perform(c4, "pause", 5);
                    Perform(c4, "say", "");
                    Perform(c4, "pause", 20);
                    Perform(c4, "faceto", c);

                    ChangeBehavior(c5, "patrol", 172, 127, 133, 127);
                    Perform(c5, "pause", 10);
                    Perform(c5, "faceto", player);
                    Perform(c5, "pause", 20);
                    Perform(c5, "faceto", player);

                    PlayerPerform("block", 0);
                    PlayerPerform("pause", 20);
                    PlayerPerform("block", 0);

                    trg0 = 1;
                    timer0 = GetGameTime() + 25;
                }
            }

            if (trg0 == 1)
            {
                c = GetChar("冷燕");
                c2 = GetAnyChar("孙玉伯");
                c3 = GetAnyChar("律香川");
                c4 = GetAnyChar("孙剑");
                c5 = -1;

                if (GetHP(c2) > 0 && GetHP(c3) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = c2;
                }
                if (GetHP(c3) > 0 && GetHP(c2) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = c3;
                }
                if (GetHP(c4) > 0 && GetHP(c3) <= 0 && GetHP(c2) <= 0)
                {
                    c5 = c4;
                }
                if (GetHP(c2) <= 0 && GetHP(c3) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = 9999;
                }

                if (c5 >= 0)
                {
                    if (c5 != 9999)
                    {
                        ChangeBehavior(c5, "follow", "vip");
                        Perform(c5, "aggress");
                        Perform(c5, "say", "");
                        Perform(c5, "faceto", player);

                        Perform(c, "aggress");
                        Perform(c, "say", "");
                        Perform(c, "pause", 2);
                        Perform(c, "faceto", c5);

                        PlayerPerform("say", "");
                        PlayerPerform("pause", 5);

                        survivor = c5;
                    }

                    trg0 = 2;
                }
            }

            if (trg0 == 2 && survivor >= 0)
            {
                c = GetChar("范璇");
                if (c >= 0 && GetHP(survivor) > 0)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", survivor);
                    if (Distance(0, 1) < 150)
                    {
                        Perform(survivor, "say", "");
                        Perform(survivor, "pause", 5);
                        Perform(survivor, "say", "");
                        Perform(survivor, "pause", 5);
                        Perform(survivor, "say", "");
                        Perform(survivor, "faceto", c);

                        ChangeBehavior(c, "follow", player);
                        Perform(c, "say", "");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "");
                        Perform(c, "pause", 3);
                        Perform(c, "faceto", survivor);

                        c2 = GetChar("左护法");
                        if (c2 >= 0)
                        {
                            Perform(c2, "say", "");
                            Perform(c2, "guard", 12);
                            Perform(c2, "faceto", c);
                        }
                        c2 = GetChar("右护法");
                        if (c2 >= 0)
                        {
                            Perform(c2, "say", "");
                            Perform(c2, "guard", 12);
                            Perform(c2, "faceto", c);
                        }

                        trg0 = 3;
                    }
                }
            }

            if (trg1 == 0)
            {
                c = GetChar("范璇");
                c2 = GetChar("冷燕");

                SetTarget(0, "char", c);
                SetTarget(1, "char", player);
                if (c >= 0 && c2 >= 0 && GetEnemy(c) == player && Distance(0, 1) < 200)
                {
                    ChangeBehavior(c, "follow", player);
                    if (trg0 == 3)
                    {
                        Perform(c, "say", "");
                        Perform(c, "pause", 8);
                        Perform(c, "say", "");
                        Perform(c, "pause", 1);
                        Perform(c, "say", "");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);

                        Perform(c2, "pause", 5);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 10);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 2);
                        Perform(c2, "faceto", c);

                        PlayerPerform("block", 0);
                        PlayerPerform("pause", 5);
                        PlayerPerform("say", "");
                        PlayerPerform("pause", 6);
                        PlayerPerform("say", "");
                        PlayerPerform("pause", 4);
                        PlayerPerform("block", 1);

                        StopPerform(survivor);

                        t = 17;
                    }
                    else
                    {
                        Perform(c, "say", "都给我上,杀了他们");
                        Perform(c, "pause", 3);
                        Perform(c, "say", "不错你父母是我杀的，今天就让你们死在这里");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "好小子，没想到你能找到这来");
                        Perform(c, "faceto", player);

                        Perform(c2, "pause", 5);
                        Perform(c2, "say", "范璇你当年害死我父母，抢走我家轩龙宝剑，今日非杀了你替我爹娘报仇");
                        Perform(c2, "pause", 2);
                        Perform(c2, "faceto", c);

                        PlayerPerform("block", 0);
                        PlayerPerform("pause", 4);
                        PlayerPerform("say", "范璇你果然在这");
                        PlayerPerform("pause", 4);
                        PlayerPerform("block", 1);

                        survivor = -1;
                        t = 10;
                    }

                    c3 = GetChar("左护法");
                    if (c3 >= 0)
                    {
                        StopPerform(c3);
                        Perform(c3, "say", "");
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("右护法");
                    if (c3 >= 0)
                    {
                        StopPerform(c3);
                        Perform(c3, "say", "");
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("孙玉伯");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("律香川");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("孙剑");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("终极杀手");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }

                    trg1 = 1;
                    trg5 = 1;
                }
            }

            if (trg5 == 0)
            {
                c = GetChar("范璇");
                if (c >= 0)
                {
                    if (g_counter % 45 == 0 && timer0 > 0 && GetGameTime() > timer0 && GetEnemy(c) < 0)
                    {
                        Perform(c, "say", "跑那去了？所有人就算翻了洛阳城也要给我找出来");
                    }

                    c3 = GetChar("左护法");
                    if (c3 >= 0 && (GetEnemy(c3) == player || GetEnemy(c3) == c2))
                    {
                        Perform(c3, "say", "发现了");
                        Perform(c3, "faceto", player);
                        ChangeBehavior(c, "follow", player);
                        trg1 = 1;
                    }
                    c3 = GetChar("右护法");
                    if (c3 >= 0 && (GetEnemy(c3) == player || GetEnemy(c3) == c2))
                    {
                        Perform(c3, "say", "发现了");
                        Perform(c3, "faceto", player);
                        ChangeBehavior(c, "follow", player);
                        trg1 = 1;
                    }
                    trg5 = 1;
                }
            }

            if (trg2 == 0)
            {
                c = GetChar("冷燕");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "我快不行了，你快走吧，不要管我");
                    Perform(c, "faceto", player);
                    trg2 = 1;
                }
            }
            if (trg2 == 1)
            {
                c = GetAnyChar("冷燕");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg2 = 2;
                }
            }

            if (g_counter % 20 == 0 && timer0 > 0 && GetGameTime() > timer0)
            {
                c = GetChar("冷燕");
                if (c >= 0 && !IsPerforming(player))
                {
                    SetTarget(0, "char", player);
                    SetTarget(1, "char", c);
                    if (Distance(0, 1) > 500)
                    {
                        Say(c, "星魂哥等等我");
                    }

                    if (g_counter % 40 == 0 && trg2 == 1)
                    {
                        Perform(c, "guard", 2);
                        Perform(c, "faceto", player);
                        Perform(c, "pause", 2);
                        Perform(c, "faceto", player);
                        Perform(c, "say", "");
                    }
                }
            }

            if (trg3 == 0)
            {
                c = GetChar("终极杀手");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", player);
                    if (Distance(0, 1) < 200)
                    {
                        ChangeBehavior(c, "kill", player);
                        Perform(c, "aggress");
                        Perform(c, "say", "");
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);
                        trg3 = 1;
                    }
                }
            }
            if (trg3 == 1)
            {
                c = GetChar("终极杀手");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    ChangeBehavior(c, "dodge", player);
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg3 = 2;
                }
            }
            if (trg3 == 2)
            {
                c = GetAnyChar("终极杀手");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg3 = 3;
                }
            }

            if (trg4 == 0)
            {
                c = GetChar("范璇");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg4 = 1;
                }
            }
            if (trg4 == 1)
            {
                c = GetAnyChar("范璇");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg4 = 2;
                }
            }
            return base.OnUpdate();
        }
    }

    //烽火雷
    public class LevelScript_c007 : LevelScriptBase
    {
        int RoundTime = 20;
        int PlayerSpawn = 17;
        int PlayerSpawnDir = 90;
        int PlayerWeapon = 23;
        int PlayerWeapon2 = 18;
        int PlayerHP = 3500;
        public override int GetRoundTime() { return RoundTime; }
        public override int GetPlayerSpawn() { return PlayerSpawn; }
        public override int GetPlayerSpawnDir() { return PlayerSpawnDir; }
        public override int GetPlayerWeapon() { return PlayerWeapon; }
        public override int GetPlayerWeapon2() { return PlayerWeapon2; }
        public override int GetPlayerMaxHp() { return PlayerHP; }
        int g_iMaxStoves = 6;
        int g_iMaxStoveHP = 200;
        int[] g_bStoveAlive;
        int[] g_iStoveHP;

        int g_iTeamAStoves;
        int g_iTeamBStoves;

        public override void Scene_OnLoad()
        {
            int i;
            string name = "";

            g_iMaxStoveHP = g_iLevel12StoveHP;

            for (i = 1; i <= g_iMaxStoves; i++)
            {
                MakeString(ref name, "D_AStove", i);
                SetSceneItem(name, "name", "machine");
                MakeString(ref name, "D_BStove", i);
                SetSceneItem(name, "name", "machine");
            }
        }

        public override void Scene_OnInit()
        {
            int i;
            string name = "";

            g_iTeamAStoves = 3;
            g_iTeamBStoves = 3;
            g_bStoveAlive = new int[g_iMaxStoves];
            g_iStoveHP = new int[g_iMaxStoves];
            for (i = 1; i <= g_iMaxStoves; i++)
            {
                g_bStoveAlive[i - 1] = 1;
                g_iStoveHP[i - 1] = g_iMaxStoveHP;
                MakeString(ref name, "D_AStove", i);
                SetSceneItem(name, StoveOnAttack, null);
                SetSceneItem(name, "pose", 0, 0);
                SetSceneItem(name, "attribute", "active", 1);
                SetSceneItem(name, "attribute", "collision", 1);

                MakeString(ref name, "D_BStove", i);
                SetSceneItem(name, StoveOnAttack, null);
                SetSceneItem(name, "pose", 0, 0);
                SetSceneItem(name, "attribute", "active", 1);
                SetSceneItem(name, "attribute", "collision", 1);
            }

            InitBoxes(g_iNumBoxes);
            InitBBoxes(g_iNumBBoxes);
            InitChairs(g_iNumChairs);
            InitDeskes(g_iNumDeskes);
            InitJugs(g_iNumJugs);
        }

        public int StoveOnAttack(int index, int characterid, int damage)
        {
            if (g_bStoveAlive[index - 1] == 0)
            {
                return 0;
            }
            Output("OnAttack", index, g_iStoveHP[index - 1]);
            string stovename = "";
            if (index <= 3)
            {
                if (GetTeam(characterid) == 1)
                {
                    return 0;
                }
                MakeString(ref stovename, "D_AStove", index);
            }
            else
            {
                if (GetTeam(characterid) == 2)
                {
                    return 0;
                }
                MakeString(ref stovename, "D_BStove", index - 3);
            }

            int id = GetSceneItem(stovename, "index");
            g_iStoveHP[index - 1] = g_iStoveHP[index - 1] - damage;
            if (g_iStoveHP[index - 1] > 0)
            {

                CreateEffect(id, "FireHIT");
                int state = GetSceneItem(id, "state");
                Output("state", state);
                if (state == 3)
                {
                    Output("Shake", id);
                    SetSceneItem(id, "pose", 1, 0);
                }

                return 0;
            }
            g_bStoveAlive[index - 1] = 0;
            if (index >= 1 && index <= 3)
            {
                g_iTeamAStoves = g_iTeamAStoves - 1;
            }
            else
            {
                g_iTeamBStoves = g_iTeamBStoves - 1;
            }


            CreateEffect(id, "FireBRK");
            SetSceneItem(id, "pose", 2, 0);
            SetSceneItem(id, "attribute", "interactive", 0);
            SetSceneItem(id, "attribute", "collision", 0);


            if (g_iTeamAStoves == 0)
            {
                GameCallBack("end", 2);
            }
            if (g_iTeamBStoves == 0)
            {
                GameCallBack("end", 1);
            }

            return 1;
        }

        int trg0 = 0;
        int trg1 = 0;
        int trg2 = 0;
        int trg3 = 0;
        int trg4 = 0;
        int trg5 = 0;
        int trg6 = 0;
        int trg7 = 0;
        int trg8 = 0;
        int trg9 = 0;
        int timer0 = 0;
        int timer1 = 0;
        int timer2 = 0;

        int hp0 = 0;
        int hp1 = 0;
        int hp2 = 0;
        int hp3 = 0;
        int hp4 = 0;

        public override void OnStart()
        {
            AddNPC("c001_npc13_01");
            AddNPC("c001_npc13_02");
            AddNPC("c001_npc13_05");
            AddNPC("c001_npc13_06");
            AddNPC("c001_npc13_07");
            AddNPC("c001_npc13_08");
            AddNPC("c001_npc13_09");
            AddNPC("c001_npc13_10");
            AddNPC("c001_npc13_11");
            AddNPC("c001_npc13_12");
            AddNPC("c001_npc13_13");
            AddNPC("c001_npc13_14");
            base.OnStart();
        }

        public override int OnUpdate()
        {
            int player = GetChar("player");
            if (player < 0)
            {
                return 0;
            }

            int c;
            int c2;
            int c3;
            int c4;
            int c5;

            if (trg0 == 0)
            {
                PlayerPerform("block", 0);
                PlayerPerform("crouch", 0);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "可恶");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "还有叶翔，石群，小何！");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "不好，这里发生了什么事.....没想到高大姐已经!");
                PlayerPerform("block", 0);
                trg0 = 1;
            }

            if (trg1 == 0)
            {
                c = GetChar("金枪侍卫");
                c2 = GetChar("大刀侍卫");

                if (CallFriend(c, c2, player) == 1)
                {
                    trg1 = 1;
                }
                if (trg1 == 0 && CallFriend(c2, c, player) == 1)
                {
                    trg1 = 1;
                }
            }

            if (trg2 == 0)
            {
                c = GetChar("野和尚﹒甲");
                c2 = GetChar("野和尚﹒乙");
                if (CallFriend2(c, c2, player, 1) == 1)
                {
                    trg2 = 1;
                }
                if (trg2 == 0 && 1 == CallFriend2(c2, c, player, 0))
                {
                    trg2 = 1;
                }
            }
            if (trg2 == 1)
            {
                c = GetChar("野和尚﹒甲");
                c2 = GetChar("野和尚﹒乙");
                if (c >= 0 && c2 >= 0 && GetEnemy(c) == player && GetEnemy(c2) == player)
                {
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    Perform(c2, "say", "");
                    Perform(c2, "faceto", player);
                    trg2 = 2;
                }
            }
            if (trg2 == 2)
            {
                c = GetChar("野和尚﹒甲");
                c2 = GetChar("野和尚﹒乙");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "aggress");
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg2 = 3;
                }
                if (c2 >= 0 && GetHP(c2) < GetMaxHP(c2) / 2)
                {
                    Perform(c2, "aggress");
                    Perform(c2, "say", "");
                    Perform(c2, "faceto", player);
                    trg2 = 3;
                }
            }

            if (trg3 == 0)
            {
                c = GetChar("无名杀手");
                SetTarget(0, "char", player);
                SetTarget(1, "char", c);
                if (c >= 0 && GetEnemy(c) == player && Distance(0, 1) < 100)
                {
                    ChangeBehavior(c, "kill", player);

                    c2 = GetChar("金枪侍卫");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "follow", player);
                    }
                    c2 = GetChar("大刀侍卫");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "follow", player);
                    }

                    c2 = GetAnyChar("野和尚﹒甲");
                    c3 = GetAnyChar("野和尚﹒乙");
                    if (c2 >= 0 && GetHP(c2) <= 0 && c3 >= 0 && GetHP(c3) <= 0)
                    {
                        Perform(c, "aggress");
                        Perform(c, "say", "");
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);

                        PlayerPerform("say", "");
                        PlayerPerform("pause", 5);
                    }
                    else
                    {
                        Perform(c, "say", "");
                        c2 = GetChar("野和尚﹒甲");
                        if (1 == BackGuard(c2, player, 1))
                        {
                            hp1 = GetHP(c2);
                        }
                        c2 = GetChar("野和尚﹒乙");
                        if (1 == BackGuard(c2, player, 1))
                        {
                            hp2 = GetHP(c2);
                        }
                        c2 = GetChar("蒙面人﹒甲");
                        if (1 == BackGuard(c2, player, 2))
                        {
                            hp3 = GetHP(c2);
                        }
                        c2 = GetChar("蒙面人﹒乙");
                        if (1 == BackGuard(c2, player, 2))
                        {
                            hp4 = GetHP(c2);
                        }
                        c2 = GetChar("金枪侍卫");
                        BackGuard(c2, player, 0);
                        c2 = GetChar("大刀侍卫");
                        BackGuard(c2, player, 0);
                    }
                    trg3 = 1;
                }
            }

            if (trg3 == 1 || trg3 == 2)
            {
                c5 = -1;
                c = GetChar("野和尚﹒甲");
                c2 = GetChar("野和尚﹒乙");
                c3 = GetChar("蒙面人﹒甲");
                c4 = GetChar("蒙面人﹒乙");

                if (hp1 > 0)
                {
                    if (c >= 0 && GetHP(c) < hp1)
                    {
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);
                        c5 = c;
                    }
                }
                if (hp2 > 0)
                {
                    if (c2 >= 0 && GetHP(c2) < hp2)
                    {
                        Perform(c2, "say", "");
                        Perform(c2, "faceto", player);
                        c5 = c2;
                    }
                }
                if (hp3 > 0)
                {
                    if (c3 >= 0 && GetHP(c3) < hp3)
                    {
                        Perform(c3, "say", "");
                        Perform(c3, "faceto", player);
                        c5 = c3;
                    }
                }
                if (hp4 > 0)
                {
                    if (c4 >= 0 && GetHP(c4) < hp4)
                    {
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);
                        c5 = c4;
                    }
                }
                if (c5 >= 0)
                {
                    ChangeBehavior(c, "kill", player);
                    ChangeBehavior(c2, "kill", player);
                    ChangeBehavior(c3, "kill", player);
                    ChangeBehavior(c4, "kill", player);
                    trg3 = 3;
                }
            }

            if (trg3 == 1)
            {
                c = GetChar("无名杀手");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) * 2 / 3)
                {
                    Perform(c, "guard", 4);
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);

                    trg3 = 2;
                }
            }
            if (trg3 == 2)
            {
                c = GetChar("无名杀手");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    c2 = GetChar("野和尚﹒甲");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "follow", c);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 6);
                        Perform(c2, "say", "");
                        Perform(c2, "faceto", c);
                        Perform(c2, "pause", 8);
                    }
                    c2 = GetChar("野和尚﹒乙");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "follow", c);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 6);
                        Perform(c2, "say", "");
                        Perform(c2, "faceto", c);
                        Perform(c2, "pause", 8);
                    }

                    trg3 = 3;
                }
            }
            if (trg3 == 3)
            {
                c = GetChar("无名杀手");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 3)
                {
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg3 = 4;
                }
            }
            if (trg3 == 4)
            {
                c = GetAnyChar("无名杀手");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");

                    c2 = GetChar("野和尚﹒甲");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "kill", player);
                    }
                    c2 = GetChar("野和尚﹒乙");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "kill", player);
                    }

                    trg3 = 5;
                }
            }

            if (trg4 == 0 && trg6 == 0)
            {
                c = GetChar("金刀门.护法");
                SetTarget(0, "char", player);
                SetTarget(1, "char", c);
                if (c >= 0 && GetEnemy(c) == player && Distance(0, 1) < 80)
                {
                    c2 = GetChar("范璇");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c, "follow", c2);
                        SetTarget(0, "char", c2);
                        ChangeBehavior(c, "attacktarget", 0);
                    }
                    Perform(c, "say", "");
                    Perform(c, "pause", 5);
                    Perform(c, "say", "老大那个杀手来了");
                    Perform(c, "say", "我们等候多时了");
                    Perform(c, "faceto", player);

                    PlayerPerform("block", 0);
                    PlayerPerform("say", "你们怎么会在这里，高大姐他们是不是被你们害的？");
                    PlayerPerform("pause", 3);
                    PlayerPerform("block", 1);

                    c3 = GetChar("无名杀手");
                    if (c3 >= 0)
                    {
                        ChangeBehavior(c3, "follow", player);
                    }
                    c3 = GetChar("蒙面人﹒甲");
                    if (c3 >= 0)
                    {
                        ChangeBehavior(c3, "follow", player);
                    }
                    c3 = GetChar("蒙面人﹒乙");
                    if (c3 >= 0)
                    {
                        ChangeBehavior(c3, "follow", player);
                    }
                    c3 = GetChar("金枪侍卫");
                    if (c3 >= 0)
                    {
                        ChangeBehavior(c2, "kill", player);
                    }
                    c3 = GetChar("大刀侍卫");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "kill", player);
                    }

                    trg4 = 1;
                }
            }
            if (trg4 == 1)
            {
                c = GetChar("金刀门.护法");
                c2 = GetChar("范璇");
                if (c >= 0 && c2 >= 0)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", c2);
                    if (Distance(0, 1) < 100)
                    {
                        ChangeBehavior(c, "patrol", 56, 66, 65, 61, 67);
                        Perform(c, "say", "不错，是我们害的，今天你也别想活着出去，我们上");
                        Perform(c, "pause", 5);
                        Perform(c, "faceto", player);
                        Perform(c, "say", "是");
                        Perform(c, "pause", 4);
                        Perform(c, "say", "");
                        Perform(c, "faceto", c2);

                        Perform(c2, "say", "");
                        Perform(c2, "say", "我有点事先离开这，替我杀了这小子");
                        Perform(c2, "pause", 3);
                        Perform(c2, "faceto", c);
                        trg4 = 2;
                        timer0 = GetGameTime() + 10;
                    }
                }
            }
            if (trg4 == 2 && GetGameTime() > timer0)
            {
                c = GetAnyChar("范璇");
                if (c >= 0)
                {
                    RemoveNPC(c);
                }
                trg4 = 3;
            }

            if (trg5 == 0)
            {
                c = GetChar("金刀门.护法");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "");
                    Perform(c, "pause", 3);
                    Perform(c, "say", "今天你别想活着出去");
                    Perform(c, "say", "好样的﹒﹒﹒竟然能伤的了我");
                    Perform(c, "faceto", player);

                    trg5 = 1;
                }
            }

            if (trg5 == 1)
            {
                c = GetAnyChar("金刀门.护法");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg5 = 2;
                }
            }

            if (trg6 == 0 && trg4 == 0)
            {
                c = GetChar("范璇");
                SetTarget(0, "char", player);
                SetTarget(1, "char", c);
                if (c >= 0 && GetEnemy(c) == player && Distance(0, 1) < 100)
                {
                    c2 = GetChar("金刀门.护法");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c, "follow", c2);
                        SetTarget(0, "char", c2);
                        ChangeBehavior(c, "attacktarget", 0);
                    }
                    Perform(c, "say", "不错是我们害的，敢跟我们作对，这就是你们的下场");
                    Perform(c, "pause", 6);
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);

                    PlayerPerform("block", 0);
                    PlayerPerform("say", "");
                    PlayerPerform("pause", 3);
                    PlayerPerform("say", "你们怎么会在这里，高大姐他们是不是你们害的？");
                    PlayerPerform("pause", 1);
                    PlayerPerform("block", 1);

                    PauseAll(10, player);

                    trg6 = 1;
                }
            }

            if (trg6 == 1)
            {
                c = GetChar("范璇");
                c2 = GetChar("金刀门.护法");
                if (c >= 0 && c2 >= 0)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", c2);
                    if (Distance(0, 1) < 120)
                    {
                        Perform(c2, "say", "都给我上");
                        Perform(c2, "aggress");
                        Perform(c2, "faceto", player);
                        Perform(c2, "say", "是");
                        Perform(c2, "pause", 8);
                        Perform(c2, "say", ""); PlayerPerform("say", "");
                        Perform(c2, "faceto", c);

                        SetTarget(0, "waypoint", 73);
                        ChangeBehavior(c, "attacktarget", 0);
                        Perform(c, "say", "我有点事先离开这，替我杀了这小子");
                        Perform(c, "say", "");
                        Perform(c, "pause", 4);
                        Perform(c, "faceto", c2);

                        PlayerPerform("say", "");
                        PlayerPerform("pause", 3);

                        c3 = GetChar("无名杀手");
                        if (c3 >= 0)
                        {
                            ChangeBehavior(c3, "follow", player);
                        }
                        c3 = GetChar("蒙面人﹒甲");
                        if (c3 >= 0)
                        {
                            ChangeBehavior(c3, "follow", player);
                        }
                        c3 = GetChar("蒙面人﹒乙");
                        if (c3 >= 0)
                        {
                            ChangeBehavior(c3, "follow", player);
                        }

                        trg6 = 2;
                        timer0 = GetGameTime() + 6;
                    }
                }
            }
            if (trg6 == 2 && GetGameTime() > timer0)
            {
                c = GetAnyChar("范璇");
                if (c >= 0)
                {
                    RemoveNPC(c);
                }
                trg6 = 3;
            }

            if (trg7 == 0)
            {
                c = GetAnyChar("金枪侍卫");
                c2 = GetAnyChar("大刀侍卫");
                c3 = GetAnyChar("野和尚﹒甲");
                c4 = GetAnyChar("野和尚﹒乙");
                if (c >= 0 && GetHP(c) <= 0 && c2 >= 0 && GetHP(c2) <= 0 && c3 >= 0 && GetHP(c3) <= 0 && c4 >= 0 && GetHP(c4) <= 0)
                {
                    RemoveNPC(c);
                    RemoveNPC(c2);
                    RemoveNPC(c3);
                    RemoveNPC(c4);
                    trg7 = 1;
                    timer1 = GetGameTime() + 5;
                }
            }
            if (trg7 == 1)
            {
                if (GetGameTime() > timer1)
                {
                    AddNPC("npc13_03");
                    AddNPC("npc13_04");
                    trg7 = 2;
                }
            }
            return base.OnUpdate();
        }

        int CallFriend(int c, int c2, int p)
        {
            if (c >= 0 && GetEnemy(c) == p)
            {
                ChangeBehavior(c, "follow", p);
                Perform(c, "guard", 3);
                Perform(c, "say", "");
                Perform(c, "say", "");
                Perform(c, "faceto", p);

                if (c2 >= 0)
                {
                    ChangeBehavior(c2, "follow", c);
                }
                return 1;
            }
            return 0;
        }

        int CallFriend2(int c, int c2, int p, int t)
        {
            if (c >= 0 && GetEnemy(c) == p)
            {
                ChangeBehavior(c, "follow", p);
                if (t == 1)
                {
                    Perform(c, "say", "");
                }
                else
                {
                    Perform(c, "say", "");
                }
                Perform(c, "faceto", p);

                if (c2 >= 0)
                {
                    ChangeBehavior(c2, "follow", c);
                }
                return 1;
            }
            return 0;
        }

        int BackGuard(int c, int p, int say)
        {
            if (c >= 0)
            {
                Perform(c, "guard", 100);
                Perform(c, "faceto", p);
                ChangeBehavior(c, "dodge", p);

                if (say == 1)
                {
                    Perform(c, "say", "");
                }
                if (say == 2)
                {
                    Perform(c, "say", "");
                }

                Perform(c, "pause", 4);
                Perform(c, "faceto", p);

                return 1;
            }
            return 0;
        }

        int PauseAll(int t, int p)
        {
            int c;
            c = GetChar("金枪侍卫");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("大刀侍卫");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("野和尚﹒甲");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("野和尚﹒乙");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("无名杀手");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("蒙面人﹒甲");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("蒙面人﹒乙");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }

            return 1;
        }
    }

    //五爪峰
    public class LevelScript_c008 : LevelScriptBase
    {
        int RoundTime = 15;
        int PlayerSpawn = 40;
        int PlayerSpawnDir = 0;
        int PlayerWeapon = 14;
        int PlayerWeapon2 = 24;
        int PlayerHP = 3000;
        public override int GetRoundTime() { return RoundTime; }
        public override int GetPlayerSpawn() { return PlayerSpawn; }
        public override int GetPlayerSpawnDir() { return PlayerSpawnDir; }
        public override int GetPlayerWeapon() { return PlayerWeapon; }
        public override int GetPlayerWeapon2() { return PlayerWeapon2; }
        public override int GetPlayerMaxHp() { return PlayerHP; }
        public override GameResult OnUnitDead(MeteorUnit deadUnit)
        {
            if (deadUnit.Attr.NpcTemplate == "c001_npc14_01")
                return GameResult.Fail;
            return base.OnUnitDead(deadUnit);
        }
        int g_iDoorMaxHP;
        int DoorState1HP;
        int DoorState2HP;
        int DoorState3HP;

        int ADoorHP;
        int BDoorHP;
        int ADoorAlive;
        int BDoorAlive;

        public override void Scene_OnLoad()
        {
            int i;
            string name = "";

            g_iDoorMaxHP = g_iLevel11DoorMaxHP;
            DoorState1HP = (g_iDoorMaxHP * 3) / 4;
            DoorState2HP = (g_iDoorMaxHP * 2) / 4;
            DoorState3HP = (g_iDoorMaxHP * 1) / 4;

            for (i = 1; i <= 5; i++)
            {
                MakeString(ref name, "D_APdoor", i);
                SetSceneItem(name, "name", "machine");
                SetSceneItem(name, "attribute", "collision", 0);
                SetSceneItem(name, "pose", 0, 0);

                MakeString(ref name, "D_BPdoor", i);
                SetSceneItem(name, "name", "machine");
                SetSceneItem(name, "attribute", "collision", 0);
                SetSceneItem(name, "pose", 0, 0);
            }

            SetSceneItem("D_APdoor06", "name", "machine");
            SetSceneItem("D_APdoor06", "attribute", "visible", 0);
            SetSceneItem("D_APdoor06", "attribute", "collision", 1);
            SetSceneItem("D_APdoor06", "pose", 0, 0);

            SetSceneItem("D_BPdoor06", "name", "machine");
            SetSceneItem("D_BPdoor06", "attribute", "visible", 0);
            SetSceneItem("D_BPdoor06", "attribute", "collision", 1);
            SetSceneItem("D_BPdoor06", "pose", 0, 0);
        }

        public override void Scene_OnInit()
        {
            InitBoxes(g_iNumBoxes);
            InitBBoxes(g_iNumBBoxes);
            InitChairs(g_iNumChairs);
            InitDeskes(g_iNumDeskes);
            InitJugs(g_iNumJugs);

            int i;
            string name = "";

            ADoorHP = g_iDoorMaxHP;
            ADoorAlive = 1;
            BDoorHP = g_iDoorMaxHP;
            BDoorAlive = 1;

            SetSceneItem("D_APdoor01", "attribute", "active", 1);
            SetSceneItem("D_APdoor01", "pose", 0, 0);

            SetSceneItem("D_BPdoor01", "attribute", "active", 1);
            SetSceneItem("D_BPdoor01", "pose", 0, 0);

            for (i = 2; i <= 5; i++)
            {
                MakeString(ref name, "D_APdoor", i);
                SetSceneItem(name, "attribute", "active", 0);
                SetSceneItem(name, "pose", 0, 0);

                MakeString(ref name, "D_BPdoor", i);
                SetSceneItem(name, "attribute", "active", 0);
                SetSceneItem(name, "pose", 0, 0);
            }
        }

        public int D_APdoor01_OnAttack(int id, int CharacterId, int damage)
        {
            if (GetTeam(CharacterId) == 1)
            {
                return 0;
            }

            int state = GetSceneItem(id, "state");
            if (state == 3)
            {

                CreateEffect(id, "GiMaHIT");
                SetSceneItem(id, "pose", 1, 0);

            }

            ADoorHP = ADoorHP - damage;
            Output("ADoor 1", ADoorHP);
            if (ADoorHP < DoorState1HP)
            {

                SetSceneItem(id, "attribute", "active", 0);
                SetSceneItem("D_APdoor02", "attribute", "active", 1);

            }
            return 1;
        }

        public int D_APdoor02_OnAttack(int id, int CharacterId, int damage)
        {
            if (GetTeam(CharacterId) == 1)
            {
                return 0;
            }

            int state = GetSceneItem(id, "state");
            if (state == 3)
            {

                CreateEffect(id, "GiMaHIT");
                SetSceneItem(id, "pose", 1, 0);

            }

            ADoorHP = ADoorHP - damage;
            Output("ADoor 2", ADoorHP);
            if (ADoorHP < DoorState2HP)
            {

                SetSceneItem(id, "attribute", "active", 0);
                SetSceneItem("D_APdoor03", "attribute", "active", 1);

            }
            return 1;
        }

        public int D_APdoor03_OnAttack(int id, int CharacterId, int damage)
        {
            if (GetTeam(CharacterId) == 1)
            {
                return 0;
            }

            int state = GetSceneItem(id, "state");
            if (state == 3)
            {

                CreateEffect(id, "GiMaHIT");
                SetSceneItem(id, "pose", 1, 0);

            }

            ADoorHP = ADoorHP - damage;
            Output("ADoor 3", ADoorHP);
            if (ADoorHP < DoorState3HP)
            {

                SetSceneItem(id, "attribute", "active", 0);
                SetSceneItem("D_APdoor04", "attribute", "active", 1);

            }
            return 1;
        }

        public int D_APdoor04_OnAttack(int id, int CharacterId, int damage)
        {
            if (GetTeam(CharacterId) == 1)
            {
                return 0;
            }

            int state = GetSceneItem(id, "state");
            if (state == 3)
            {

                CreateEffect(id, "GiMaHIT");
                SetSceneItem(id, "pose", 1, 0);

            }

            ADoorHP = ADoorHP - damage;
            Output("ADoor 4", ADoorHP);
            if (ADoorHP <= 0)
            {

                CreateEffect(id, "GiMaBRK");
                SetSceneItem(id, "attribute", "active", 0);
                SetSceneItem("D_APdoor05", "attribute", "active", 1);
                SetSceneItem("D_APdoor05", "attribute", "collision", 0);
                SetSceneItem("D_APdoor05", "pose", 1, 0);

                GameCallBack("end", 2);
            }
            return 1;
        }

        /* team 2 */
        public int D_BPdoor01_OnAttack(int id, int CharacterId, int damage)
        {
            if (GetTeam(CharacterId) == 2)
            {
                return 0;
            }

            int state = GetSceneItem(id, "state");
            if (state == 3)
            {

                CreateEffect(id, "GiMaHIT");
                SetSceneItem(id, "pose", 1, 0);

            }

            BDoorHP = BDoorHP - damage;
            Output("BDoor 1", BDoorHP);
            if (BDoorHP < DoorState1HP)
            {

                SetSceneItem(id, "attribute", "active", 0);
                SetSceneItem("D_BPdoor02", "attribute", "active", 1);

            }
            return 1;
        }

        public int D_BPdoor02_OnAttack(int id, int CharacterId, int damage)
        {
            if (GetTeam(CharacterId) == 2)
            {
                return 0;
            }

            int state = GetSceneItem(id, "state");
            if (state == 3)
            {

                CreateEffect(id, "GiMaHIT");
                SetSceneItem(id, "pose", 1, 0);

            }

            BDoorHP = BDoorHP - damage;
            Output("BDoor 2", BDoorHP);
            if (BDoorHP < DoorState2HP)
            {

                SetSceneItem(id, "attribute", "active", 0);
                SetSceneItem("D_BPdoor03", "attribute", "active", 1);

            }
            return 1;
        }

        public int D_BPdoor03_OnAttack(int id, int CharacterId, int damage)
        {
            if (GetTeam(CharacterId) == 2)
            {
                return 0;
            }

            int state = GetSceneItem(id, "state");
            if (state == 3)
            {

                CreateEffect(id, "GiMaHIT");
                SetSceneItem(id, "pose", 1, 0);

            }

            BDoorHP = BDoorHP - damage;
            Output("BDoor 3", BDoorHP);
            if (BDoorHP < DoorState3HP)
            {

                SetSceneItem(id, "attribute", "active", 0);
                SetSceneItem("D_BPdoor04", "attribute", "active", 1);

            }
            return 1;
        }

        public int D_BPdoor04_OnAttack(int id, int CharacterId, int damage)
        {
            if (GetTeam(CharacterId) == 2)
            {
                return 0;
            }

            int state = GetSceneItem(id, "state");
            if (state == 3)
            {

                CreateEffect(id, "GiMaHIT");
                SetSceneItem(id, "pose", 1, 0);

            }

            BDoorHP = BDoorHP - damage;
            Output("BDoor 4", BDoorHP);
            if (BDoorHP <= 0)
            {

                CreateEffect(id, "GiMaBRK");
                SetSceneItem(id, "attribute", "active", 0);
                SetSceneItem("D_BPdoor05", "attribute", "active", 1);
                SetSceneItem("D_BPdoor05", "pose", 1, 0);

                GameCallBack("end", 1);
            }
            return 1;
        }


        int g_counter = 0;
        int trg0 = 0;
        int trg1 = 0;
        int trg2 = 0;
        int trg3 = 0;
        int trg4 = 0;
        int trg5 = 0;
        int timer0 = 0;
        int survivor = -1;
        public override void OnStart()
        {
            AddNPC("c001_npc14_01");
            AddNPC("c001_npc14_02");
            AddNPC("c001_npc14_03");
            AddNPC("c001_npc14_04");
            AddNPC("c001_npc14_05");
            AddNPC("c001_npc14_06");
            AddNPC("c001_npc14_07");
            AddNPC("c001_npc14_08");
            base.OnStart();
        }

        public override int OnUpdate()
        {
            int player = GetChar("player");
            if (player < 0)
            {
                return 0;
            }

            int c;
            int c2 = 0;
            int c3;
            int c4;
            int c5;
            int t;

            g_counter = g_counter + 1;

            if (trg0 == 0)
            {
                c = GetChar("冷燕");
                c2 = GetChar("官差﹒甲");
                c3 = GetChar("官差﹒乙");
                c4 = GetChar("官差﹒丙");
                c5 = GetChar("破空");
                if (c >= 0 && c2 >= 0 && c3 >= 0 && c4 >= 0 && c5 >= 0)
                {
                    ChangeBehavior(c, "follow", player);
                    Perform(c, "pause", 8);
                    Perform(c, "say", "我怎么还没死，难道是刚才的蝴蝶救了我");
                    Perform(c, "pause", 10);
                    Perform(c, "faceto", c3);

                    ChangeBehavior(c2, "follow", c);
                    Perform(c2, "pause", 15);
                    Perform(c2, "aggress");
                    Perform(c2, "say", "终于追上你了");
                    Perform(c2, "pause", 2);
                    Perform(c2, "faceto", c);

                    ChangeBehavior(c3, "follow", player);
                    Perform(c3, "pause", 5);
                    Perform(c3, "say", "");
                    Perform(c3, "pause", 15);
                    Perform(c3, "faceto", c);

                    ChangeBehavior(c4, "follow", player);
                    Perform(c4, "pause", 5);
                    Perform(c4, "say", "我们奉范璇老大之命取你等人性命,我们上");
                    Perform(c4, "pause", 20);
                    Perform(c4, "faceto", c);

                    ChangeBehavior(c5, "patrol", 172, 127, 133, 127);
                    Perform(c5, "pause", 10);
                    Perform(c5, "faceto", player);
                    Perform(c5, "pause", 20);
                    Perform(c5, "faceto", player);

                    PlayerPerform("block", 0);
                    PlayerPerform("pause", 20);
                    PlayerPerform("block", 0);

                    trg0 = 1;
                    timer0 = GetGameTime() + 25;
                }
            }

            if (trg0 == 1)
            {
                c = GetChar("冷燕");
                c2 = GetAnyChar("官差﹒甲");
                c3 = GetAnyChar("官差﹒乙");
                c4 = GetAnyChar("官差﹒丙");
                c5 = -1;

                if (GetHP(c2) > 0 && GetHP(c3) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = c2;
                }
                if (GetHP(c3) > 0 && GetHP(c2) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = c3;
                }
                if (GetHP(c4) > 0 && GetHP(c3) <= 0 && GetHP(c2) <= 0)
                {
                    c5 = c4;
                }
                if (GetHP(c2) <= 0 && GetHP(c3) <= 0 && GetHP(c4) <= 0)
                {
                    c5 = 9999;
                }

                if (c5 >= 0)
                {
                    if (c5 != 9999)
                    {
                        ChangeBehavior(c5, "follow", "vip");
                        Perform(c5, "aggress");
                        Perform(c5, "say", "可恶，你给我等着");
                        Perform(c5, "faceto", player);

                        Perform(c, "aggress");
                        Perform(c, "say", "");
                        Perform(c, "pause", 2);
                        Perform(c, "faceto", c5);

                        PlayerPerform("say", "");
                        PlayerPerform("pause", 5);

                        survivor = c5;
                    }

                    trg0 = 2;
                }
            }

            if (trg0 == 2 && survivor >= 0)
            {
                c = GetChar("冯浩");
                if (c >= 0 && GetHP(survivor) > 0)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", survivor);
                    if (Distance(0, 1) < 150)
                    {
                        Perform(survivor, "say", "");
                        Perform(survivor, "pause", 5);
                        Perform(survivor, "say", "");
                        Perform(survivor, "pause", 5);
                        Perform(survivor, "say", "");
                        Perform(survivor, "faceto", c);

                        ChangeBehavior(c, "follow", player);
                        Perform(c, "say", "");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "");
                        Perform(c, "pause", 3);
                        Perform(c, "faceto", survivor);

                        c2 = GetChar("军枪官差﹒甲");
                        if (c2 >= 0)
                        {
                            Perform(c2, "say", "");
                            Perform(c2, "guard", 12);
                            Perform(c2, "faceto", c);
                        }
                        c2 = GetChar("军枪官差﹒乙");
                        if (c2 >= 0)
                        {
                            Perform(c2, "say", "");
                            Perform(c2, "guard", 12);
                            Perform(c2, "faceto", c);
                        }

                        trg0 = 3;
                    }
                }
            }

            if (trg1 == 0)
            {
                c = GetChar("冯浩");
                c2 = GetChar("冷燕");

                SetTarget(0, "char", c);
                SetTarget(1, "char", player);
                if (c >= 0 && c2 >= 0 && GetEnemy(c) == player && Distance(0, 1) < 200)
                {
                    ChangeBehavior(c, "follow", player);
                    if (trg0 == 3)
                    {
                        Perform(c, "say", "杀死他们向老大交代");
                        Perform(c, "pause", 8);
                        Perform(c, "say", "大家给我上");
                        Perform(c, "pause", 1);
                        Perform(c, "say", "简直活腻了");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "好小子敢杀我手下");
                        Perform(c, "faceto", player);

                        Perform(c2, "pause", 5);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 10);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 2);
                        Perform(c2, "faceto", c);

                        PlayerPerform("block", 0);
                        PlayerPerform("pause", 5);
                        PlayerPerform("say", "");
                        PlayerPerform("pause", 6);
                        PlayerPerform("say", "");
                        PlayerPerform("pause", 4);
                        PlayerPerform("block", 1);

                        StopPerform(survivor);

                        t = 17;
                    }
                    else
                    {
                        Perform(c, "say", "来人呀给我杀了他们");
                        Perform(c, "pause", 3);
                        Perform(c, "say", "敢杀屠大鹏，今天让你们偿命");
                        Perform(c, "pause", 5);
                        Perform(c, "say", "你这次可跑不了了");
                        Perform(c, "faceto", player);

                        Perform(c2, "pause", 5);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 2);
                        Perform(c2, "faceto", c);

                        PlayerPerform("block", 0);
                        PlayerPerform("pause", 4);
                        PlayerPerform("say", "你们竟然追杀到这，还不放过我们，休怪我无情");
                        PlayerPerform("pause", 4);
                        PlayerPerform("block", 1);

                        survivor = -1;
                        t = 10;
                    }

                    c3 = GetChar("军枪官差﹒甲");
                    if (c3 >= 0)
                    {
                        StopPerform(c3);
                        Perform(c3, "say", "");
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("军枪官差﹒乙");
                    if (c3 >= 0)
                    {
                        StopPerform(c3);
                        Perform(c3, "say", "");
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("官差﹒甲");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("官差﹒乙");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("官差﹒丙");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }
                    c3 = GetChar("破空");
                    if (c3 >= 0)
                    {
                        Perform(c3, "guard", t);
                        Perform(c3, "faceto", player);
                    }

                    trg1 = 1;
                    trg5 = 1;
                }
            }

            if (trg5 == 0)
            {
                c = GetChar("冯浩");
                if (c >= 0)
                {
                    if (g_counter % 45 == 0 && timer0 > 0 && GetGameTime() > timer0 && GetEnemy(c) < 0)
                    {
                        Perform(c, "say", "跑那去了？所有人就算翻了五爪峰也要给我找出来");
                    }

                    c3 = GetChar("军枪官差﹒甲");
                    if (c3 >= 0 && (GetEnemy(c3) == player || GetEnemy(c3) == c2))
                    {
                        Perform(c3, "say", "发现了");
                        Perform(c3, "faceto", player);
                        ChangeBehavior(c, "follow", player);
                        trg1 = 1;
                    }
                    c3 = GetChar("军枪官差﹒乙");
                    if (c3 >= 0 && (GetEnemy(c3) == player || GetEnemy(c3) == c2))
                    {
                        Perform(c3, "say", "发现了");
                        Perform(c3, "faceto", player);
                        ChangeBehavior(c, "follow", player);
                        trg1 = 1;
                    }
                    trg5 = 1;
                }
            }

            if (trg2 == 0)
            {
                c = GetChar("冷燕");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "我快不行了，你快走吧，不要管我");
                    Perform(c, "faceto", player);
                    trg2 = 1;
                }
            }
            if (trg2 == 1)
            {
                c = GetAnyChar("冷燕");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg2 = 2;
                }
            }

            if (g_counter % 20 == 0 && timer0 > 0 && GetGameTime() > timer0)
            {
                c = GetChar("冷燕");
                if (c >= 0 && !IsPerforming(player))
                {
                    SetTarget(0, "char", player);
                    SetTarget(1, "char", c);
                    if (Distance(0, 1) > 500)
                    {
                        Say(c, "等等我");
                    }

                    if (g_counter % 40 == 0 && trg2 == 1)
                    {
                        Perform(c, "guard", 2);
                        Perform(c, "faceto", player);
                        Perform(c, "pause", 2);
                        Perform(c, "faceto", player);
                        Perform(c, "say", "");
                    }
                }
            }

            if (trg3 == 0)
            {
                c = GetChar("破空");
                if (c >= 0 && GetEnemy(c) == player)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", player);
                    if (Distance(0, 1) < 200)
                    {
                        ChangeBehavior(c, "kill", player);
                        Perform(c, "aggress");
                        Perform(c, "say", "这次你可跑不掉了,看招");
                        Perform(c, "say", "小子敢杀屠大鹏");
                        Perform(c, "faceto", player);
                        trg3 = 1;
                    }
                }
            }
            if (trg3 == 1)
            {
                c = GetChar("破空");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    ChangeBehavior(c, "dodge", player);
                    Perform(c, "say", "还真有两下子，佩服");
                    Perform(c, "faceto", player);
                    trg3 = 2;
                }
            }
            if (trg3 == 2)
            {
                c = GetAnyChar("破空");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg3 = 3;
                }
            }

            if (trg4 == 0)
            {
                c = GetChar("冯浩");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg4 = 1;
                }
            }
            if (trg4 == 1)
            {
                c = GetAnyChar("冯浩");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg4 = 2;
                }
            }
            return base.OnUpdate();
        }
    }
    //五雷塔
    public class LevelScript_c009 : LevelScriptBase
    {
        public override int GetRoundTime() { return RoundTime; }
        public override int GetPlayerSpawn() { return PlayerSpawn; }
        public override int GetPlayerSpawnDir() { return PlayerSpawnDir; }
        public override int GetPlayerWeapon() { return PlayerWeapon; }
        public override int GetPlayerWeapon2() { return PlayerWeapon2; }
        public override int GetPlayerMaxHp() { return PlayerHP; }
        int RoundTime = 30;
        int PlayerSpawn = 115;
        int PlayerSpawnDir = 250;
        int PlayerWeapon = 5;
        int PlayerWeapon2 = 0;
        int PlayerHP = 5000;
        int trg0 = 0;
        int trg1 = 0;
        int trg2 = 0;
        int trg3 = 0;
        int trg4 = 0;
        int trg5 = 0;
        int trg6 = 0;
        int trg7 = 0;
        int trg8 = 0;
        int trg9 = 0;
        int timer0 = 0;
        int timer1 = 0;
        int timer2 = 0;

        int hp0 = 0;
        int hp1 = 0;
        int hp2 = 0;
        int hp3 = 0;
        int hp4 = 0;
        public override void OnStart()
        {
            AddNPC("c001_npc15_01");
            AddNPC("c001_npc15_02");
            AddNPC("c001_npc15_05");
            AddNPC("c001_npc15_06");
            AddNPC("c001_npc15_07");
            AddNPC("c001_npc15_08");
            AddNPC("c001_npc15_09");
            base.OnStart();
        }

        int CallFriend(int c, int c2, int p)
        {
            if (c >= 0 && GetEnemy(c) == p)
            {
                ChangeBehavior(c, "follow", p);
                Perform(c, "guard", 3);
                Perform(c, "say", "");
                Perform(c, "say", "");
                Perform(c, "faceto", p);

                if (c2 >= 0)
                {
                    ChangeBehavior(c2, "follow", c);
                }
                return 1;
            }
            return 0;
        }

        int CallFriend2(int c, int c2, int p, int t)
        {
            if (c >= 0 && GetEnemy(c) == p)
            {
                ChangeBehavior(c, "follow", p);
                if (t == 1)
                {
                    Perform(c, "say", "");
                }
                else
                {
                    Perform(c, "say", "");
                }
                Perform(c, "faceto", p);

                if (c2 >= 0)
                {
                    ChangeBehavior(c2, "follow", c);
                }
                return 1;
            }
            return 0;
        }

        int BackGuard(int c, int p, int say)
        {
            if (c >= 0)
            {
                Perform(c, "guard", 100);
                Perform(c, "faceto", p);
                ChangeBehavior(c, "dodge", p);

                if (say == 1)
                {
                    Perform(c, "say", "");
                }
                if (say == 2)
                {
                    Perform(c, "say", "");
                }

                Perform(c, "pause", 4);
                Perform(c, "faceto", p);

                return 1;
            }
            return 0;
        }

        int PauseAll(int t, int p)
        {
            int c;
            c = GetChar("金枪侍卫");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("大刀侍卫");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("野和尚﹒甲");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("野和尚﹒乙");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("无名杀手");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("蒙面人﹒甲");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }
            c = GetChar("蒙面人﹒乙");
            if (c >= 0)
            {
                Perform(c, "guard", t);
                Perform(c, "faceto", p);
            }

            return 1;
        }

        public override int OnUpdate()
        {
            int player = GetChar("player");
            if (player < 0)
            {
                return 0;
            }

            int c;
            int c2;
            int c3;
            int c4;
            int c5;

            if (trg0 == 0)
            {
                PlayerPerform("block", 0);
                PlayerPerform("crouch", 0);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "范璇，拿命来");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "不管这么多了");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("say", "不好我被范璇认出来了"); PlayerPerform("say", "");
                PlayerPerform("pause", 2);
                PlayerPerform("block", 0);
                trg0 = 1;
            }

            if (trg1 == 0)
            {
                c = GetChar("金枪侍卫");
                c2 = GetChar("大刀侍卫");

                if (CallFriend(c, c2, player) == 1)
                {
                    trg1 = 1;
                }
                if (trg1 == 0 && CallFriend(c2, c, player) == 1)
                {
                    trg1 = 1;
                }
            }

            if (trg2 == 0)
            {
                c = GetChar("野和尚﹒甲");
                c2 = GetChar("野和尚﹒乙");
                if (CallFriend2(c, c2, player, 1) == 1)
                {
                    trg2 = 1;
                }
                if (trg2 == 0 && CallFriend2(c2, c, player, 0) == 1)
                {
                    trg2 = 1;
                }
            }
            if (trg2 == 1)
            {
                c = GetChar("野和尚﹒甲");
                c2 = GetChar("野和尚﹒乙");
                if (c >= 0 && c2 >= 0 && GetEnemy(c) == player && GetEnemy(c2) == player)
                {
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    Perform(c2, "say", "");
                    Perform(c2, "faceto", player);
                    trg2 = 2;
                }
            }
            if (trg2 == 2)
            {
                c = GetChar("野和尚﹒甲");
                c2 = GetChar("野和尚﹒乙");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "aggress");
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg2 = 3;
                }
                if (c2 >= 0 && GetHP(c2) < GetMaxHP(c2) / 2)
                {
                    Perform(c2, "aggress");
                    Perform(c2, "say", "");
                    Perform(c2, "faceto", player);
                    trg2 = 3;
                }
            }

            if (trg3 == 0)
            {
                c = GetChar("无名杀手");
                SetTarget(0, "char", player);
                SetTarget(1, "char", c);
                if (c >= 0 && GetEnemy(c) == player && Distance(0, 1) < 100)
                {
                    ChangeBehavior(c, "kill", player);

                    c2 = GetChar("金枪侍卫");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "follow", player);
                    }
                    c2 = GetChar("大刀侍卫");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "follow", player);
                    }

                    c2 = GetAnyChar("野和尚﹒甲");
                    c3 = GetAnyChar("野和尚﹒乙");
                    if (c2 >= 0 && GetHP(c2) <= 0 && c3 >= 0 && GetHP(c3) <= 0)
                    {
                        Perform(c, "aggress");
                        Perform(c, "say", "");
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);

                        PlayerPerform("say", "");
                        PlayerPerform("pause", 5);
                    }
                    else
                    {
                        Perform(c, "say", "");
                        c2 = GetChar("野和尚﹒甲");
                        if (BackGuard(c2, player, 1) == 1)
                        {
                            hp1 = GetHP(c2);
                        }
                        c2 = GetChar("野和尚﹒乙");
                        if (BackGuard(c2, player, 1) == 1)
                        {
                            hp2 = GetHP(c2);
                        }
                        c2 = GetChar("蒙面人﹒甲");
                        if (BackGuard(c2, player, 2) == 1)
                        {
                            hp3 = GetHP(c2);
                        }
                        c2 = GetChar("蒙面人﹒乙");
                        if (BackGuard(c2, player, 2) == 1)
                        {
                            hp4 = GetHP(c2);
                        }
                        c2 = GetChar("金枪侍卫");
                        BackGuard(c2, player, 0);
                        c2 = GetChar("大刀侍卫");
                        BackGuard(c2, player, 0);
                    }
                    trg3 = 1;
                }
            }

            if (trg3 == 1 || trg3 == 2)
            {
                c5 = -1;
                c = GetChar("野和尚﹒甲");
                c2 = GetChar("野和尚﹒乙");
                c3 = GetChar("蒙面人﹒甲");
                c4 = GetChar("蒙面人﹒乙");

                if (hp1 > 0)
                {
                    if (c >= 0 && GetHP(c) < hp1)
                    {
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);
                        c5 = c;
                    }
                }
                if (hp2 > 0)
                {
                    if (c2 >= 0 && GetHP(c2) < hp2)
                    {
                        Perform(c2, "say", "");
                        Perform(c2, "faceto", player);
                        c5 = c2;
                    }
                }
                if (hp3 > 0)
                {
                    if (c3 >= 0 && GetHP(c3) < hp3)
                    {
                        Perform(c3, "say", "");
                        Perform(c3, "faceto", player);
                        c5 = c3;
                    }
                }
                if (hp4 > 0)
                {
                    if (c4 >= 0 && GetHP(c4) < hp4)
                    {
                        Perform(c, "say", "");
                        Perform(c, "faceto", player);
                        c5 = c4;
                    }
                }
                if (c5 >= 0)
                {
                    ChangeBehavior(c, "kill", player);
                    ChangeBehavior(c2, "kill", player);
                    ChangeBehavior(c3, "kill", player);
                    ChangeBehavior(c4, "kill", player);
                    trg3 = 3;
                }
            }

            if (trg3 == 1)
            {
                c = GetChar("无名杀手");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) * 2 / 3)
                {
                    Perform(c, "guard", 4);
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);

                    trg3 = 2;
                }
            }
            if (trg3 == 2)
            {
                c = GetChar("无名杀手");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    c2 = GetChar("野和尚﹒甲");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "follow", c);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 6);
                        Perform(c2, "say", "");
                        Perform(c2, "faceto", c);
                        Perform(c2, "pause", 8);
                    }
                    c2 = GetChar("野和尚﹒乙");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "follow", c);
                        Perform(c2, "say", "");
                        Perform(c2, "pause", 6);
                        Perform(c2, "say", "");
                        Perform(c2, "faceto", c);
                        Perform(c2, "pause", 8);
                    }

                    trg3 = 3;
                }
            }
            if (trg3 == 3)
            {
                c = GetChar("无名杀手");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 3)
                {
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);
                    trg3 = 4;
                }
            }
            if (trg3 == 4)
            {
                c = GetAnyChar("无名杀手");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");

                    c2 = GetChar("野和尚﹒甲");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "kill", player);
                    }
                    c2 = GetChar("野和尚﹒乙");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "kill", player);
                    }

                    trg3 = 5;
                }
            }

            if (trg4 == 0 && trg6 == 0)
            {
                c = GetChar("五雷塔.护法");
                SetTarget(0, "char", player);
                SetTarget(1, "char", c);
                if (c >= 0 && GetEnemy(c) == player && Distance(0, 1) < 80)
                {
                    c2 = GetChar("范璇");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c, "follow", c2);
                        SetTarget(0, "char", c2);
                        ChangeBehavior(c, "attacktarget", 0);
                    }
                    Perform(c, "say", "");
                    Perform(c, "pause", 5);
                    Perform(c, "say", "范老大，有刺客");
                    Perform(c, "say", "好小子，是你，竟然敢骗我们");
                    Perform(c, "faceto", player);

                    PlayerPerform("block", 0);
                    PlayerPerform("say", "范璇看来你果然在这，今天休想活着离开这里");
                    PlayerPerform("pause", 3);
                    PlayerPerform("block", 1);

                    c3 = GetChar("无名杀手");
                    if (c3 >= 0)
                    {
                        ChangeBehavior(c3, "follow", player);
                    }
                    c3 = GetChar("蒙面人﹒甲");
                    if (c3 >= 0)
                    {
                        ChangeBehavior(c3, "follow", player);
                    }
                    c3 = GetChar("蒙面人﹒乙");
                    if (c3 >= 0)
                    {
                        ChangeBehavior(c3, "follow", player);
                    }
                    c3 = GetChar("金枪侍卫");
                    if (c3 >= 0)
                    {
                        ChangeBehavior(c2, "kill", player);
                    }
                    c3 = GetChar("大刀侍卫");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c2, "kill", player);
                    }

                    trg4 = 1;
                }
            }
            if (trg4 == 1)
            {
                c = GetChar("五雷塔.护法");
                c2 = GetChar("范璇");
                if (c >= 0 && c2 >= 0)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", c2);
                    if (Distance(0, 1) < 100)
                    {
                        ChangeBehavior(c, "patrol", 56, 66, 65, 61, 67);
                        Perform(c, "say", "");
                        Perform(c, "pause", 5);
                        Perform(c, "faceto", player);
                        Perform(c, "say", "遵命");
                        Perform(c, "pause", 6);
                        Perform(c, "say", "");
                        Perform(c, "faceto", c2);

                        Perform(c2, "say", "");
                        Perform(c2, "say", "好小子，没想到你还没死，我出去避一避，这里就交给你们了，杀了他");
                        Perform(c2, "pause", 3);
                        Perform(c2, "faceto", c);
                        trg4 = 2;
                        timer0 = GetGameTime() + 10;
                    }
                }
            }
            if (trg4 == 2 && GetGameTime() > timer0)
            {
                c = GetAnyChar("范璇");
                if (c >= 0)
                {
                    RemoveNPC(c);
                }
                trg4 = 3;
            }

            if (trg5 == 0)
            {
                c = GetChar("五雷塔.护法");
                if (c >= 0 && GetHP(c) < GetMaxHP(c) / 2)
                {
                    Perform(c, "say", "");
                    Perform(c, "pause", 3);
                    Perform(c, "say", "");
                    Perform(c, "say", "果然有两下子");
                    Perform(c, "faceto", player);

                    trg5 = 1;
                }
            }

            if (trg5 == 1)
            {
                c = GetAnyChar("五雷塔.护法");
                if (c >= 0 && GetHP(c) <= 0)
                {
                    Say(c, "");
                    trg5 = 2;
                }
            }

            if (trg6 == 0 && trg4 == 0)
            {
                c = GetChar("范璇");
                SetTarget(0, "char", player);
                SetTarget(1, "char", c);
                if (c >= 0 && GetEnemy(c) == player && Distance(0, 1) < 100)
                {
                    c2 = GetChar("五雷塔.护法");
                    if (c2 >= 0)
                    {
                        ChangeBehavior(c, "follow", c2);
                        SetTarget(0, "char", c2);
                        ChangeBehavior(c, "attacktarget", 0);
                    }
                    Perform(c, "say", "好小子，竟然敢骗我，今天非杀了你不可");
                    Perform(c, "pause", 6);
                    Perform(c, "say", "");
                    Perform(c, "faceto", player);

                    PlayerPerform("block", 0);
                    PlayerPerform("say", "休要逃");
                    PlayerPerform("pause", 6);
                    PlayerPerform("say", "范璇终于找到你了，拿命来");
                    PlayerPerform("pause", 3);
                    PlayerPerform("block", 1);

                    PauseAll(10, player);

                    trg6 = 1;
                }
            }

            if (trg6 == 1)
            {
                c = GetChar("范璇");
                c2 = GetChar("五雷塔.护法");
                if (c >= 0 && c2 >= 0)
                {
                    SetTarget(0, "char", c);
                    SetTarget(1, "char", c2);
                    if (Distance(0, 1) < 120)
                    {
                        Perform(c2, "say", "给我杀了这小子，都给我上");
                        Perform(c2, "aggress");
                        Perform(c2, "faceto", player);
                        Perform(c2, "say", "是");
                        Perform(c2, "pause", 8);
                        Perform(c2, "say", "");
                        Perform(c2, "say", "");
                        Perform(c2, "faceto", c);

                        SetTarget(0, "waypoint", 73);
                        ChangeBehavior(c, "attacktarget", 0);
                        Perform(c, "say", "我先离开这，出去避一避，你们替我杀了这小子");
                        Perform(c, "say", "");
                        Perform(c, "pause", 4);
                        Perform(c, "faceto", c2);

                        PlayerPerform("say", "休要逃");
                        PlayerPerform("pause", 3);

                        c3 = GetChar("无名杀手");
                        if (c3 >= 0)
                        {
                            ChangeBehavior(c3, "follow", player);
                        }
                        c3 = GetChar("蒙面人﹒甲");
                        if (c3 >= 0)
                        {
                            ChangeBehavior(c3, "follow", player);
                        }
                        c3 = GetChar("蒙面人﹒乙");
                        if (c3 >= 0)
                        {
                            ChangeBehavior(c3, "follow", player);
                        }

                        trg6 = 2;
                        timer0 = GetGameTime() + 6;
                    }
                }
            }
            if (trg6 == 2 && GetGameTime() > timer0)
            {
                c = GetAnyChar("范璇");
                if (c >= 0)
                {
                    RemoveNPC(c);
                }
                trg6 = 3;
            }

            if (trg7 == 0)
            {
                c = GetAnyChar("金枪侍卫");
                c2 = GetAnyChar("大刀侍卫");
                c3 = GetAnyChar("野和尚﹒甲");
                c4 = GetAnyChar("野和尚﹒乙");
                if (c >= 0 && GetHP(c) <= 0 && c2 >= 0 && GetHP(c2) <= 0 && c3 >= 0 && GetHP(c3) <= 0 && c4 >= 0 && GetHP(c4) <= 0)
                {
                    RemoveNPC(c);
                    RemoveNPC(c2);
                    RemoveNPC(c3);
                    RemoveNPC(c4);
                    trg7 = 1;
                    timer1 = GetGameTime() + 5;
                }
            }
            if (trg7 == 1)
            {
                if (GetGameTime() > timer1)
                {
                    AddNPC("c001_npc15_03");
                    AddNPC("c001_npc15_04");
                    AddNPC("c001_npc15_10");
                    AddNPC("c001_npc15_11");
                    AddNPC("c001_npc15_12");
                    trg7 = 2;
                }
            }
            return base.OnUpdate();
        }
    }
}
