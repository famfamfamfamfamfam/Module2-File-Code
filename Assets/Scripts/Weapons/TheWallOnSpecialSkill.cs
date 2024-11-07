using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWallOnSpecialSkill : MonoBehaviour, IOnSpecialSkill
{
    float timeCount = 0;
    public void OnSpecialSkill()
    {
        if (Time.timeScale == 0)
        {
            if (timeCount < 0.9f)
                transform.position += Vector3.up * 0.45f * Time.unscaledDeltaTime;
            else if (timeCount < 1.5)
                transform.position += Vector3.down * 0.3f * Time.unscaledDeltaTime;
            else
            {
                timeCount = 0;
                Time.timeScale = 1;
                return;
            }
            timeCount += Time.unscaledDeltaTime;
        }
    }
}
