using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esa
{
    using UI_;
    public class Enemy : Char
    {
        public CharCtrl charCtrl;
        void Start()
        {
            charCtrl = CharCtrl.I;
        }
        void Update()
        {
            if (Game.pause) return;
            if (BattleMenu.I.active) return;
            if (Battle.on)
            {
                //agilityStoring += Time.deltaTime;
                //if (agilityStoring >= agility)
                //{
                //    agilityStoring = 0;
                //    DoAction(0);
                //}
            }
        }
        public ParticleSystem attackEffect;
        public float attackValueMax = 20;
        public UnityEngine.UI.Text Damage;
        private void DoAction(int v)
        {
            AttackAnim(attackValueMax).StartCo();
        }
        public float attackAnimDura = 0.4f;
        public float attackMoveDura = 0.4f;
        IEnumerator AttackAnim(float damage)
        {
            Game.pause = true;
            float t = 0f;
            Vector3 oPos = transform.position;
            while (t < attackMoveDura)
            {
                var toPos = charCtrl.transform.position + charCtrl.transform.forward * 1.5f;
                transform.position = Vector3.Lerp(oPos, toPos, t / attackMoveDura);
                t += Time.deltaTime;
                yield return 0;
            }
            t = 0;
            while (t < attackAnimDura)
            {
                t += Time.deltaTime;
                yield return 0;
            }
            AudioMgr.I.PlaySound("Slash");
            Attack(damage);
            t = 0;
            while (t < attackAnimDura * 0.7f)
            {
                t += Time.deltaTime;
                yield return 0;
            }
            transform.position = oPos;
            Game.pause = false;
        }

        private void Attack(float damage)
        {
            attackEffect.Play();
            var d = Mathf.RoundToInt(Random.value * damage);
            charCtrl.P.hp -= d;

            Damage.text = d.ToString();
            Damage.GetComponent<UIJump>().Jump();
        }
    }
}