using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;
    // 게임 내 시간
    // 실제 시간
    private int year;   // 년도
    private int day;    // 일
    public int hour = 6;    // 시
    public int minute = 0;      // 분
    private float currentTime = 0;

    public UnityAction<int, int> onUpdateTime;
    public AudioClip endDayClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void Init()
    {
        var info = InfoManager.instance.GetInfo();
        this.year = info.playerInfo.playYear;
        this.day = info.playerInfo.playDay;
        StopAllCoroutines();
        this.TimeStart();
    }
    public void StopTimeRoutine()
    {
        hour = 6;    // 시
        minute = 0;      // 분
        currentTime = 0;
        StopAllCoroutines();
    }
    private void TimeStart()
    {
        StartCoroutine(TimeRoutine());
    }

    private IEnumerator TimeRoutine()
    {
        while (true)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= 7f)
            {                
                minute += 1;
                currentTime = 0;

                if (minute == 6)
                {
                    minute = 0;
                    hour += 1;
                }
                this.onUpdateTime(hour, minute);
            }
            yield return null;
        }
    }

    public void EndDay()
    {
        hour = 6;
        minute = 0;
        SoundManager.instance.PlaySound(endDayClip);
        SoundManager.instance.StopBGMSound();
    }

    public void Pause()
    {
        Time.timeScale = 0F;
    }

    public void Resume()
    {
        Time.timeScale = 1.0F;
    }

}

//오전 6시부터 시작되어 새벽 2시에 하루가 끝난다.
//실제 시간 7초가 지나면 10분씩 증가한다.
//새벽 2시가 되면 게임 진행 상황을 저장하고 다음날 오전 6시가 된다. => 실제 시간을 0초로 변경함
//사용자가 하루를 일찍 종료할 수 있다. : 다음날 오전 6시가 된다. -> 실제 시간을 0초로 변경함