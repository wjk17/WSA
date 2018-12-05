﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    public static class RaycastTool
    {
        public static LayerMask mask;
        public static float skinThickness;
        public static bool skin;
        public static Vector3 SVToRayHit(Vector3 screenPos)
        {
            Vector3 hitPoint;
            SVRaycast(screenPos, out hitPoint);
            return hitPoint;
        }
        public static bool SVRaycast(out Transform hitT, int layerMask)
        {
            return SVRaycast(Input.mousePosition, out hitT, layerMask);
        }
        public static bool SVRaycast(this MonoBehaviour m, Vector3 mousePos, out Transform hitT, int layerMask)
        {
            return SVRaycast(mousePos, out hitT, layerMask);
        }
        public static bool SVRaycast(Vector3 mousePos, out Transform hitT, int layerMask)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                var p = hit.transform;
                hitT = p;
                return true;
            }
            else
            {
                hitT = null;
                return false;
            }
        }
        public static RaycastHit[] SVRaycastAll(this MonoBehaviour m, int layerMask)
        {
            return SVRaycastAll(Input.mousePosition, layerMask);
        }
        public static RaycastHit[] SVRaycastAll(this MonoBehaviour m, int layerMask, Vector3 v)
        {
            return SVRaycastAll(v, layerMask);
        }
        public static RaycastHit[] SVRaycastAll(Vector3 mousePos, int layerMask)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(mousePos);
            return Physics.RaycastAll(ray, Mathf.Infinity, layerMask);
        }
        public static bool SVRaycast(out RaycastHit hit, int layerMask)
        {
            return SVRaycast(Input.mousePosition, out hit, layerMask);
        }
        public static bool SVRaycast(Vector3 mousePos, out RaycastHit hit, int layerMask)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool SVRaycast(Vector3 mousePos, out Vector3 hitPoint)
        {
            Camera cam = Camera.main;
            Ray ray = cam.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value))
            {
                var p = hit.point;

                if (skin) p += cam.transform.forward * -1 * skinThickness;
                hitPoint = p;
                return true;
            }
            else
            {
                hitPoint = Vector3.zero;
                return false;
            }
        }
    }
}