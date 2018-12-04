using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Esa.UI
{
    public class ScreenRay : MonoBehaviour
    {
        public NavMeshAgent agent;
        void Update()
        {
            var cam = Camera.main;
            if (Events.MouseDown0)
            {
                var ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    agent.SetDestination(hit.point);
                }
            }
        }
    }
}
