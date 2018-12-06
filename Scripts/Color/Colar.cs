using UnityEngine;
namespace Esa
{
    public class Colar
    {
        public float h;
        public float s;
        public float v;
        public Colar(float h, float s, float v)
        {
            this.h = h;
            this.s = s;
            this.v = v;
        }
        public static Colar R
        {
            get { return (Colar)Color.red; }
        }
        public static Colar G
        {
            get { return (Colar)Color.green; }
        }
        public static Colar B
        {
            get { return (Colar)Color.blue; }
        }
        public static Colar H
        {
            get { return new Colar(1, 0, 0); }
        }
        public static Colar S
        {
            get { return new Colar(0, 1, 0); }
        }
        public static Colar V
        {
            get { return new Colar(0, 0, 1); }
        }
        public Color ToColor()
        {
            return Color.HSVToRGB(h, s, v);
        }
        public static explicit operator Color(Colar color)
        {
            return color.ToColor();
        }
        public static explicit operator Colar(Color color)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            return new Colar(h, s, v);
        }
        public static Colar operator *(float f, Colar asc)
        {
            return asc * f;
        }
        public static Colar operator *(Colar asc, float f)
        {
            asc.h *= f;
            asc.s *= f;
            asc.v *= f;
            return asc;
        }
        public static Color operator +(Color color, Colar asc)
        {
            return asc + color;
        }
        public static Color operator +(Colar asc, Color color)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            h += asc.h;
            s += asc.s;
            v += asc.v;
            return Color.HSVToRGB(h, s, v);
        }
        public static Color operator -(Color color, Colar asc)
        {
            return asc - color;
        }
        public static Color operator -(Colar asc, Color color)
        {
            float h, s, v;
            Color.RGBToHSV(color, out h, out s, out v);
            h -= asc.h;
            s -= asc.s;
            v -= asc.v;
            return Color.HSVToRGB(h, s, v);
        }
    }
}