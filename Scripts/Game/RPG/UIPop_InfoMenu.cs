using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Esa.UI_
{
    public class UIPop_InfoMenu : Singleton<UIPop_InfoMenu>
    {
        public RectTrans rt;
        public bool updateRT;
        public float cornerSize = 10f;
        public UIGrid grid;
        public float speed = 100f;
        public int order = 11;
        private void Awake()
        {
            grid = GetComponentInChildren<UIGrid>();
            grid.onClick = OnClick;
            grid.drawOrder = order;
        }
        void Start()
        {
            GetTrans();
            
            this.AddInput(Input, 10, false);
            this.DestroyImages();
        }
        private void OnClick(int i)
        {
            switch (i)
            {
                case 0:
                    GetComponent<ReturnButton>().OnClick();
                    break;
                case 1:
                    SceneManager.LoadScene("Title", LoadSceneMode.Single);
                    break;
                default:
                    break;
            }
        }
        private void Update()
        {
            //grid.drawOrder = order;
            if (updateRT) GetTrans();
            if (Events.Mouse1)
            {
                GetComponent<RectTransform>().offsetMin += Vector2.left * speed * Time.deltaTime;
            }
            else if (Events.Mouse2)
            {
                GetComponent<RectTransform>().offsetMin += Vector2.right * speed * Time.deltaTime;
            }
        }
        [Button]
        void GetTrans()
        {
            rt = new RectTrans(this);
        }
        void Input()
        {
            this.BeginOrtho();
            this.DrawBG(cornerSize);
            if (Events.Key(KeyCode.C)) OnClick(1);
        }
    }
}