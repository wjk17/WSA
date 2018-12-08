using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyMode
{
    Bezier,
    Constant, // 常数，硬转折，左端值优先，比如左右两点都是常数模式，使用左边点的值。
    Linear, // 一端线性一端贝塞尔时，线性端的控制点为线性方向t=0.3333333f
    None, // 一端none一端Bezier，就会使用二阶贝塞尔；两端none即是一阶贝塞尔，等同线性插值。
}
