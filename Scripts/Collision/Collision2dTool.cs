using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa.Physic
{
    public static class Collision2dTool
    {
        public static bool IsCollision(this BoxCollider2D box, CircleCollider2D circle)
        {
            return IsCollision(circle, box);
        }
        public static bool IsCollision(this CircleCollider2D circle, BoxCollider2D box)
        {
            Rect rt = new Rect(box.UIPos(), box.size);
            rt.MovePivot(-Vector2.one * 0.5f);
            return IsCollision(circle.UIPos(), circle.radius, rt);
        }
        public static bool IsCollision(Vector2 circlePos, float radius, Rect rt)
        {
            return IsCollision(circlePos.x, circlePos.y, radius, rt.pos.x, rt.pos.y, rt.size.x, rt.size.y);
        }
        // 矩形和圆形碰撞检测
        public static bool IsCollision(
            float circleXPos, float circleYPos, float radius,
            float rtX, float rtY,
            float w, float h)
        {
            float R = radius;
            float x = circleXPos;
            float y = circleYPos;

            var r = rtX + w;
            var b = rtY + h;

            // 四角的四个象限
            //分别判断矩形4个顶点与圆心的距离是否<=圆半径；如果<=，说明碰撞成功   
            //两个坐标的平方和比较，与Distance比较同效果，效率更高
            if (((rtX - x) * (rtX - x) + (rtY - y) * (rtY - y)) <= R * R) return true;
            if (((r - x) * (r - x) + (rtY - y) * (rtY - y)) <= R * R) return true;
            if (((rtX - x) * (rtX - x) + (b - y) * (b - y)) <= R * R) return true;
            if (((r - x) * (r - x) + (b - y) * (b - y)) <= R * R) return true;

            // 左右两个象限
            //判断当圆心的Y坐标进入矩形内时X的位置，如果X在(rtX-R)到(rtX+w+R)这个范围内，则碰撞成功   
            float minDisX = 0;
            if (y >= rtY && y <= b)
            {
                if (x < rtX)
                    minDisX = rtX - x;
                else if (x > r)
                    minDisX = x - rtX - w;
                else
                    return true;
                if (minDisX <= R)
                    return true;
            }

            // 上下两个象限
            //判断当圆心的X坐标进入矩形内时Y的位置，如果X在(rtY-R)到(rtY+h+R)这个范围内，则碰撞成功
            float minDisY = 0;
            if (x >= rtX && x <= r)
            {
                if (y < rtY)
                    minDisY = rtY - y;
                else if (y > b)
                    minDisY = y - rtY - h;
                else
                    return true;
                if (minDisY <= R)
                    return true;
            }

            return false;
        }
    }
}