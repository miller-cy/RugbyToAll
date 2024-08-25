using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 事件处理类，用于管理游戏中的事件
public class EventHandler
{
    // 定义一个静态事件GetPointEvent，事件参数为int类型
    public static event Action<int> GetPointEvent;

    // 调用GetPointEvent事件的方法
    public static void CallGetPointEvent(int point) 
    {  // 使用简化的语法调用事件，只有在事件不为空时才调用
        GetPointEvent?.Invoke(point);

    }
    public static event Action GameOverEvent;
    public static void CallGameOverEvent() 
    {
       GameOverEvent?.Invoke();
    }
}