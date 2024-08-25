using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Block类继承自MonoBehaviour，用于挂载到Unity中的GameObject上
public class Block : MonoBehaviour
{
    // Update方法在每帧调用一次
    void Update()
    {
        // FIXME: 这个注释表示将来需要修复这里的代码
        // 在每帧中调用CheckPosition方法检查Block的位置
        CheckPosition();
    }

    // 这个方法用于检查对象的位置与摄像机位置的关系
    private void CheckPosition()
    {
        // 如果Block的Y轴位置比摄像机的Y轴位置低25个单位以上，
        // 销毁这个游戏对象以释放资源，避免不必要的计算
        if (Camera.main.transform.position.y - transform.position.y > 25)
        {
            Destroy(this.gameObject);
        }
    }
}