using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Esa.UI
{
    public class BtnToggleActive : MonoBehaviour
    {
        public bool startActive = true;
        public GameObject[] targets;
        void Start()
        {
            foreach (var target in targets)
            {
                target.SetActive(startActive);
            }
            GetComponent<Button>().onClick.AddListener(OnClick);
        }
        void OnClick()
        {
            foreach (var target in targets)
            {
                target.SetActive(!target.activeSelf);
            }
        }
    }
}