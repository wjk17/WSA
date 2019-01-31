using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using System;
    using UI_;
    public class Battle : Singleton<Battle>
    {
        public static bool on; // 战斗中
        public List<Char> _enemys;
        public List<Char> _allys;
        public static List<int> expEarns;
        public static RandomWrap rw;
        public Transform[] _enemyPos;
        public Transform[] _allyPos;
        public static Tran2 prevTrans;
        private static Vector3 center;
        private static float space;

        public static List<Char> enemys { get { return I._enemys; } set { I._enemys = value; } }
        public static List<Char> allys { get { return I._allys; } set { I._allys = value; } }
        public static Transform[] enemyPos { get { return I._enemyPos; } set { I._enemyPos = value; } }
        public static Transform[] allyPos { get { return I._allyPos; } set { I._allyPos = value; } }
        internal static Char enemy { get { return enemys[0]; } }
        internal static Char ally { get { return allys[0]; } }
        internal static int expEarn
        {
            get { return expEarns[0]; }
            set { expEarns[0] = value; }
        }
        [Button]
        void GetPos()
        {
            _allyPos = transform.GetChild(0).GetChildsL1().ToArray();
            _enemyPos = transform.GetChild(1).GetChildsL1().ToArray();
        }
        public override void _Awake()
        {
            base._Awake();
            prevTrans = CharCtrl.I.collider.Tran2();
            rw = new RandomWrap();
            CalculatePos();
        }
        public static void Adventure()
        {
            if (rw.Trigger())
            {
                _Start();
            }
        }
        void CalculatePos()
        {
            var a = enemyPos[0];
            var b = enemyPos[1];
            var c = enemyPos[2];
            var d = enemyPos[3];
            center = (a.position + d.position) * 0.5f;
            // 因为pos对象是随手摆的，所以计算一下平均间隔
            var s1 = Mathf.Abs(a.position.z - b.position.z);
            var s2 = Mathf.Abs(b.position.z - c.position.z);
            var s3 = Mathf.Abs(c.position.z - d.position.z);
            space = (s1 + s2 + s3) / 3;

        }
        private static void RandomEnemy()
        {
            enemys = new List<Char>();
            enemys.Add(CharMgr.NewChar(1));
            //enemys.Add(CharMgr.NewChar(1));
            //enemys.Add(CharMgr.NewChar(1));
            //enemys.Add(CharMgr.NewChar(1));

            var centerT = enemyPos[0].Tran2().SetZ(center.z);
            if (enemys.Count == 1)
            {
                enemy.SetTran2(centerT);
            }
            else if (enemys.Count == 2)
            {
                enemys[0].SetTran2(centerT.AddZ(-space * 0.5f));
                enemys[1].SetTran2(centerT.AddZ(space * 0.5f));
            }
            else if (enemys.Count == 3)
            {
                enemys[0].SetTran2(centerT.AddZ(-space));
                enemys[1].SetTran2(centerT);
                enemys[2].SetTran2(centerT.AddZ(space));
            }
            else if (enemys.Count == 4)
            {
                enemys[0].SetTran2(centerT.AddZ(-space * 1.5f));
                enemys[1].SetTran2(centerT.AddZ(-space * 0.5f));
                enemys[2].SetTran2(centerT.AddZ(space * 0.5f));
                enemys[3].SetTran2(centerT.AddZ(space * 1.5f));
            }

            Vector3 posView;
            foreach (var enemy in enemys)
            {
                posView = Camera.main.WorldToViewportPoint(enemy.transform.position);// 设置显示UI条的位置
                enemy.P.SetPos(posView.MulRef());
            }



            allys = new List<Char>();
            allys.Add(CharCtrl.I.C);
            ally.SetTran2(allyPos[0].Tran2());

            posView = Camera.main.WorldToViewportPoint(CharCtrl.I.transform.position);
            CharCtrl.I.P.SetPos(posView.MulRef());
        }

        public static void End()
        {
            on = false;
            SceneTransition.I.Go(I.EndBattle);
        }
        void EndBattle()
        {
            AudioMgr.I.PlayBGM("Town");

            Game.I.tiles[0].Active();
            Game.I.tiles[1].Disactive();

            UICornerRT.I.Active();

            CharCtrl.I.P.SaveGame(); // 保存玩家角色数据（自动存档）

            CharCtrl.I.Enable<CopyPosition>();
            Camera.main.Enable<FollowPosition>();
        }

        internal static void Remove(Char enemy)
        {
            enemys.Remove(enemy);
            enemy.DestroyGO();
            if (enemys.Count == 0)
            {
                Win();
            }
        }

        private static void Win()
        {
            Inventory.I.Active();
            End();
        }

        public static void _Start()
        {
            on = true;
            SceneTransition.I.Go(I.StartBattle);
        }
        void StartBattle()
        {
            AudioMgr.I.PlayBGM("Battle");

            Game.I.tiles[0].Disactive();
            Game.I.tiles[1].Active();

            UICornerRT.I.Disactive();
            UIPop_InfoMenu.I.Disactive();

            Camera.main.Disable<FollowPosition>();
            Camera.main.SetPosXZ(18, 13.5f);

            CharCtrl.I.Disable<CopyPosition>();
            RandomEnemy(); // 随机生成敌人
        }
    }
}