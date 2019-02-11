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
        private static Vector3 centerE;
        private static Vector3 centerA;
        private static Vector3 os;

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
            centerE = (a.position + d.position) * 0.5f;
            // 因为pos对象是随手摆的，所以计算一下平均间隔
            var s1 = -a.position + b.position;
            var s2 = -b.position + c.position;
            var s3 = -c.position + d.position;
            os = (s1 + s2 + s3) / 3;

            a = allyPos[0];
            d = allyPos[3];
            centerA = (a.position + d.position) * 0.5f;
        }
        private static void RandomEnemy()
        {
            enemys = new List<Char>();
            enemys.Add(CharMgr.NewChar(1));
            enemys.Add(CharMgr.NewChar(1));
            enemys.Add(CharMgr.NewChar(1));
            enemys.Add(CharMgr.NewChar(1));

            var centerT = enemyPos[0].Tran2().SetZ(centerE.z);
            var poss = centerT.pos.Average(os, enemys.Count, Vectors.half);
            for (int i = 0; i < poss.Length; i++)
            {
                enemys[i].SetTran2(centerT.SetPos(poss[i]));
            }

            Vector3 posView;
            foreach (var enemy in enemys)
            {
                posView = Camera.main.WorldToViewportPoint(enemy.transform.position);// 设置显示UI条的位置
                enemy.P.SetPos(posView.MulRef());
            }

            allys = new List<Char>();
            allys.Add(CharCtrl.I.C);

            centerT = allyPos[0].Tran2().SetZ(centerA.z);
            poss = centerT.pos.Average(os, allys.Count, Vectors.half);
            for (int i = 0; i < poss.Length; i++)
            {
                allys[i].SetTran2(centerT.SetPos(poss[i]));
            }

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
            CharCtrl.I.SetTran2(prevTrans);
            CharCtrl.I.Enable<CopyPosition>();
            Camera.main.Enable<FollowPosition>();
            BattleMenu.I.Disactive();

            ClearEnemys();
        }
        void ClearEnemys()
        {
            foreach (var enemy in enemys)
            {
                enemy.DestroyGO();
            }
            enemys.Clear();
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
            prevTrans = CharCtrl.I.transform.Tran2();
            RandomEnemy(); // 随机生成敌人
        }
    }
}