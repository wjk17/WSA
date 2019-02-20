using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Esa
{
    using System;
    public static class PitchTool
    {
        public static int Pitch12To8_5(int i)
        {
            switch (i)
            {
                case 0: return 1;
                case 1: return -1;
                case 2: return 2;
                case 3: return -2;
                case 4: return 3;
                case 5: return 4;
                case 6: return -3; // difference
                case 7: return 5;
                case 8: return -4; //
                case 9: return 6;
                case 10: return -5; //
                case 11: return 7;
                case 12: return 8;
                default: throw new Exception("pitch: " + i.ToString());
            }
        }
        public static int Pitch12To8_5_Align(int i)
        {
            switch (i)
            {
                case 0: return 1;
                case 1: return -1;
                case 2: return 2;
                case 3: return -2;
                case 4: return 3;
                case 5: return 4;
                case 6: return -4;
                case 7: return 5;
                case 8: return -5;
                case 9: return 6;
                case 10: return -6;
                case 11: return 7;
                case 12: return 8;
                default: throw new Exception("pitch: " + i.ToString());
            }
        }
        public static int Pitch8_to_12(int i)
        {
            switch (i)
            {
                case 0:
                case 1:
                case 2:
                    return (i * 2);
                case 3:
                case 4:
                case 5:
                case 6:
                    return (i * 2 - 1);
                case -1: // 休止符
                    return 12;
                default:
                    if (i > 0)
                    {
                        var scale = i / 7;
                        return scale * 12 + Pitch8_to_12(i % 7);
                    }
                    else throw new Exception();
            }
        }
        public static int Pitch5_to_12(int i)
        {
            i++;
            switch (i)
            {
                case 1:
                case 2:
                    return (i * 2 - 1);
                case 4:
                case 5:
                case 6:
                    return ((i - 1) * 2);
                default: throw new Exception();
            }
        }
    }
    public enum Pitch // 音高 (12平均律 0~11)
    {
        C,      //0 1
        CSharp, //1 #1 (b2)
        D,      //2 2
        DSharp, //3 #2 (b3)
        E,      //4 3
        F,      //5 4
        FSharp, //6 #4 (b5)
        G,      //7 5
        GSharp, //8 #5 (b6)
        A,      //9 6
        ASharp, //10 #6 (b7)
        B,      //11 7
        Rest,   //12 0 （休止符）
    }
}