using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossSpawner : MonoBehaviour
{
    private enum eSoundState
    {
        BossAppear
    }

    public GameObject[] bossPrefabs;

    public UnityAction<Vector3, int, GameObject> onBossDie;
    private AudioSource audioSource;
    public AudioClip audioBossAppear;

    private void Start()
    {
        this.audioSource = GetComponent<AudioSource>();
    }


    private void CreateBoss()
    {
        PlaySound(eSoundState.BossAppear);
        var bossGo = Instantiate<GameObject>(this.bossPrefabs[0]);

        var initPos = new Vector3(0, 6, 0);
        bossGo.transform.position = initPos;        

        var boss = bossGo.GetComponent<Boss01>();
        boss.Init();
        boss.Appear();
        boss.onDie = (dieStatus) =>
        {
            boss.Stop();
            this.onBossDie(boss.transform.position, boss.score, bossGo);
        };
        boss.onAppearComplete = () =>
        {
            
        };
    }

    public void ReservationBossAppear(float time)
    {
        StartCoroutine(ReservationBossAppearRoutine(time));
    }

    private IEnumerator ReservationBossAppearRoutine(float time)
    {
        float delta = 0;
        while(true)
        {
            delta += Time.deltaTime;
            if(delta >= time)
            {
                break;
            }
            yield return null;
        }
        CreateBoss();
    }

    private void PlaySound(eSoundState state)
    {
        if (state == eSoundState.BossAppear)
        {
            audioSource.clip = audioBossAppear;
        }

        audioSource.Play();
    }
}
