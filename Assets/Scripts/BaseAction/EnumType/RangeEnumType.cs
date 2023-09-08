using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BusinessAlgorithm.BaseAction {
    // 矩形范围枚举
    public enum RectRangType {
        Both,      // 左右两边
        Left,      // 左边
        Right,     // 右边
    }

    // 矩形中心点枚举
    public enum RectCenterRangType {
        Center,    // 矩形正中心
        Bottom,    // 矩形底边
    }
}