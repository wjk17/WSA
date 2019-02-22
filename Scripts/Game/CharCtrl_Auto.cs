using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Esa
{
    using UI_;
    public class CharCtrl_Auto : MonoBehaviour
    {
        public Char C { get { return GetComponent<Char>(); } }
        public UnitProp P { get { return C.P; } }
        public void Init()
        {
            this.AddInput(Input, 0, false);
            C.P = new UnitProp();
            P.charName = "knight";
        }
        void Input()
        {
            this.BeginOrtho();
            if (Battle.on)
            {
                P.Update();
                if (Game.pause || BattleMenu.I.active) { }
                else if (P.AgilityCheck())
                {
                    // DoAction
                }
            }
        }
        //public ParticleSystem attackEffect;
        //public float attackValueMax = 20;
        //public UnityEngine.UI.Text Damage;
        //private void DoAction(int v)
        //{
        //    AttackAnim(attackValueMax).StartCo();
        //}
        //public float attackAnimDura = 0.4f;
        //public float attackMoveDura = 0.4f;
        //IEnumerator AttackAnim(float damage)
        //{
        //    Game.pause = true;
        //    float t = 0f;
        //    Vector3 oPos = transform.position;
        //    while (t < attackMoveDura)
        //    {
        //        var toPos = charCtrl.transform.position + charCtrl.transform.forward * 1.5f;
        //        transform.position = Vector3.Lerp(oPos, toPos, t / attackMoveDura);
        //        t += Time.deltaTime;
        //        yield return 0;
        //    }
        //    t = 0;
        //    while (t < attackAnimDura)
        //    {
        //        t += Time.deltaTime;
        //        yield return 0;
        //    }
        //    AudioMgr.I.PlaySound("Slash");
        //    Attack(damage);
        //    t = 0;
        //    while (t < attackAnimDura * 0.7f)
        //    {
        //        t += Time.deltaTime;
        //        yield return 0;
        //    }
        //    transform.position = oPos;
        //    Game.pause = false;
        //}

        //private void Attack(float damage)
        //{
        //    attackEffect.Play();
        //    var d = Mathf.RoundToInt(Random.value * damage);
        //    charCtrl.P.hp -= d;

        //    Damage.text = d.ToString();
        //    Damage.GetComponent<UIJump>().Jump();
        //}
    }
}