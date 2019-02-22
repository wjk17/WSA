using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI_
{
    public partial class CharCtrl : Singleton<CharCtrl>
    {
        public float eulerStart;
        public float speed = 0.1f;

        internal Animator animator;

        public new Transform collider;


        public ParticleSystem attackEffect;

        public GameObject charCamBtn;

        public int doubleChopMP = 35;
        public float doubleChopDelay = 0.3f;
        public float doubleChopFactor = 0.9f;
        public int backStabMP = 30;
        public float backStabFactor = 1.8f;
        public float backStabPosOs = -1.5f;

        public float attackMoveDura = 0.5f;
        public float attackAnimDura = 0.5f;
        public float attackPosOs = 0.5f;

        public float soundTime = 0.35f;

        public List<Skill> skills;
        public Char C { get { return GetComponent<Char>(); } }
        public UnitProp P { get { return C.P; } }

        private void Awake()
        {
            this.AddInput(Input, 0, false);
            C.P = new UnitProp();
            P.LoadGame();

            Camera.main.SetLocalPosXZ(0, 0);
        }
        void Start()
        {
            animator = GetComponent<Animator>();
            skills = SkillMgr.I.skills.Clone();
        }
        void Input()
        {
            Game.totalTime += Time.deltaTime;

            if (Game.pause) return;

            animator.SetClip((Battle.on || KeyMgr.Idle) ? "Idle" : "Walk");

            this.BeginOrtho();
            if (Battle.on)
            {
                P.Update(); // 更新属性和显示

                if (P.hp <= 0)
                {
                    P.hp = 1;
                    Battle.End();
                }
            }
            if (BattleMenu.I.active) return;

            // battle logic
            if (Battle.on)
            {
                if (P.AgilityCheck())
                {
                    BattleMenu.I.Active();
                    // DoAction
                }
                return;
            }

            if (KeyMgr.Idle) return;

            collider.SetLocalEulerY(eulerStart + KeyMgr.Dir * 45f);
            transform.SetLocalEulerY(collider.localEulerAngles.y);

            #region     // 使用collider的射线击中点来放置模型
            RaycastHit rh;
            if (RaycastTool.SVRaycast3D(collider.transform.position, Vector3.down, out rh, -1))
            {
                transform.SetPosY(rh.point.y);
            }
            collider.GetComponent<Rigidbody>().MovePosition(collider.position + collider.forward * speed);
            #endregion

            Battle.Adventure(); // 遇怪（根据几率）
        }

        private void FixedUpdate()
        {
            // 消除惯性
            collider.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        //// 防御：回血回蓝
        //public void Healing()
        //{
        //    //P.AddHP(Random.IntValue(v));
        //    //UI.I.StartCoro(HealMPDelay(v));
        //}
        //IEnumerator HealMPDelay(float v)
        //{
        //    yield return new WaitForSeconds(0.3f);
        //    P.AddMP(Random.IntValue(v));
        //}
        //void AttackTwice(float f, float os)
        //{
        //    _AttackTwice(f, os).StartCo();
        //}
        //IEnumerator _AttackTwice(float f, float os, Transform enemy)
        //{
        //    Game.pause = true;
        //    float t = 0f;
        //    Vector3 oPos = collider.position;
        //    animator.SetClip("Walk");
        //    while (t < attackMoveDura)
        //    {
        //        var toPos = enemy.transform.position + enemy.transform.forward * os;
        //        collider.position = Vector3.Lerp(oPos, toPos, t / attackMoveDura);
        //        t += Time.deltaTime;
        //        yield return 0;
        //    }
        //    // once
        //    animator.SetClip("Slash");
        //    while (!animator.finished)
        //    {
        //        yield return 0;
        //    }
        //    AudioMgr.I.PlaySound("Slash");
        //    Attack(f);
        //    // twice
        //    animator.playTime = 0;
        //    while (animator.nTime < 0.9f)
        //    {
        //        yield return 0;
        //    }
        //    AudioMgr.I.PlaySound("Slash");
        //    Attack(f);
        //    // wait
        //    yield return new WaitForSeconds(attackAnimDura * 0.7f);

        //    collider.position = oPos;
        //    Game.pause = false;
        //}
        IEnumerator _AttackAnim(float damage, float os, Char enemy)
        {
            Game.pause = true; // 这行是即刻运行的
            float t = 0f;
            Vector3 oPos = collider.position;
            animator.SetClip("Walk");
            while (t < attackMoveDura)
            {
                var toPos = enemy.transform.position + enemy.transform.forward * os;
                collider.position = Vector3.Lerp(oPos, toPos, t / attackMoveDura);
                t += Time.deltaTime;
                yield return 0;
            }
            animator.SetClip("Slash");
            var oFw = transform.forward;
            if (os == backStabPosOs) transform.forward = enemy.transform.forward;

            while (!animator.finished)
            {
                yield return 0;
            }
            AudioMgr.I.PlaySound("Slash");
            //Attack(damage, enemy);

            yield return new WaitForSeconds(attackAnimDura * 0.7f); // wait time

            if (os == backStabPosOs) transform.forward = oFw;
            collider.position = oPos;
            Game.pause = false;
        }
    }
}