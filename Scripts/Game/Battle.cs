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
        public static List<Enemy> enemys;
        public static List<Ally> allys;
        public static List<int> expEarns;
        public static RandomWrap rw;
        public Transform[] enemyPos;
        public Transform[] allyPos;
        public static Tran2 prevTrans;
        internal static Enemy enemy { get { return enemys[0]; } }
        internal static Ally ally { get { return allys[0]; } }
        internal static int expEarn
        {
            get { return expEarns[0]; }
            set { expEarns[0] = value; }
        }
        [Button]
        void GetPos()
        {
            allyPos = transform.GetChild(0).GetChildsL1().ToArray();
            enemyPos = transform.GetChild(1).GetChildsL1().ToArray();
        }
        public override void _Awake()
        {
            base._Awake();
            rw = new RandomWrap();
        }
        public static void Adventure()
        {
            if (rw.Trigger())
            {
                Start();
            }
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

            CharCtrl.I.collider.SetTran2(prevTrans); // 还原玩家角色位置
            CharCtrl.I.P.SaveGame(); // 保存玩家角色数据（自动存档）

            Camera.main.Enable<FollowPosition>();
        }

        internal static void Remove(Enemy enemy)
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

        public static void Start()
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

            prevTrans = CharCtrl.I.collider.Tran2(); // 保存玩家角色位置

            Camera.main.Disable<FollowPosition>();
            Camera.main.SetPosXZ(18, 13.5f);
        }
        public void AddEnemy(Enemy enemy, int num)
        {
            enemys.Add(enemy);
            CopyTrans(enemy.transform, enemyPos[num]);
        }
        public void AddAlly(Ally ally, int num)
        {
            allys.Add(ally);
            CopyTrans(ally.transform, allyPos[num]);
        }
        void CopyTrans(Transform t, Transform t2)
        {
            t.SetPosXZ(t2.position);
            t.SetLocalEulerY(t2.localEulerAngles.y);
        }
    }
}