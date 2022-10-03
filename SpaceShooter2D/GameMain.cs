using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameMain : MonoBehaviour
{
    public enum ePlayMode
    {
        Test, Release
    }

    public Image dim;
    public Button btnSpawnEnemyA;
    private Player player;
    public GameObject playerPrefab;
    public ePlayMode playMode;
    private EnemyGenerator enemyGenerator;
    private ItemSpawner itemSpawner;
    private BossSpawner bossSpawner;
    private VfxSpawner vfxSpawner;
    private UIGame uiGame;
    private BackGrounds backGrounds;
    private Camera mainCamera;

    private int score = 0;
    private float delta = 0;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        this.btnSpawnEnemyA.onClick.AddListener(() =>
        {
            this.enemyGenerator.CreateEnemy();
        });

        if(playMode == ePlayMode.Release)
        {
            this.StartCoroutine(this.WaitForSec(() =>
            {
                App.instance.Fade(this.dim, App.eFadeType.FadeIn, () =>
                {
                    this.StartGame();
                });
            }));
        }
        else
        {
            this.StartGame();
        }

        this.enemyGenerator.onEnemyDie = (enemyPosition, score) =>
        {            
            this.score += score;
            uiGame.score = this.score;
            uiGame.UpdateUi();
            this.vfxSpawner.SpawnExplosion(enemyPosition);
            vfxSpawner.onExplosionComplet = (explosionPosition) =>
            {
                itemSpawner.SpawnItem(explosionPosition);
            };
            
        };
        this.bossSpawner.onBossDie = (bossPosition, score, bossGo) =>
        {
            this.score += score;
            uiGame.score = this.score;
            uiGame.UpdateUi();

            vfxSpawner.SpawnExplosions(bossPosition);
            vfxSpawner.onExplosionsComplet = (explosionPosition) => 
            {
                Destroy(bossGo);
                itemSpawner.SpawnItem(explosionPosition);
                backGrounds.BossKillEffect();
                this.bossSpawner.ReservationBossAppear(20f);
            };
            
        };

        this.player.onEatItem = (score) =>
        {
            this.score += score;
            uiGame.playerBombCount = player.bombCount;
            uiGame.score = this.score;
            uiGame.UpdateUi();
        };

        this.player.onHit = (currentHp) =>
        {
            uiGame.playerCurrentHp = currentHp;
            uiGame.UpdateUi();
        };
        this.player.onUseBomb = (bombCount) =>
        {
            uiGame.playerBombCount = bombCount;
            uiGame.UpdateUi();
            this.StartCoroutine(CameraEffectRoutine());
        };

        this.player.onDie = () =>
        {
            InfoManager.instance.score = this.score;
            SceneManager.LoadScene("GameOver");
        };

        uiGame.playerCurrentHp = player.maxHp;
        uiGame.playerBombCount = player.bombCount;
        uiGame.score = this.score;
        uiGame.UpdateUi();      

    }

    private void StartGame()
    {
        Debug.Log("Start Game!");
        CreatePlayer();

        this.enemyGenerator.CreateEnemies();
        this.bossSpawner.ReservationBossAppear(20f);
    }



    private void CreatePlayer()
    {
        this.player = Instantiate<GameObject>(this.playerPrefab).GetComponent<Player>();
        player.transform.position = new Vector3(0, -4, 0);
    }

    IEnumerator WaitForSec(UnityAction callback)
    {
        yield return new WaitForSeconds(0.3f);
        callback();
    }

    private void Init()
    {
        this.enemyGenerator = GameObject.Find("EnemyGenerator").GetComponent<EnemyGenerator>();
        this.itemSpawner = GameObject.Find("ItemSpawner").GetComponent<ItemSpawner>();
        this.bossSpawner = GameObject.Find("BossSpawner").GetComponent<BossSpawner>();
        this.vfxSpawner = GameObject.Find("VfxSpawner").GetComponent<VfxSpawner>();
        this.uiGame = GameObject.Find("UIGame").GetComponent<UIGame>();
        this.backGrounds = GameObject.Find("BackGrounds").GetComponent<BackGrounds>();
        this.mainCamera = Camera.main;
    }

    private IEnumerator CameraEffectRoutine()
    {
        for(int i=0; i<2; i++)
        {
            mainCamera.transform.position = new Vector3(0.2f, 0, -10);
            yield return new WaitForSeconds(0.05f);
            mainCamera.transform.position = new Vector3(-0.2f, 0, -10);
            yield return new WaitForSeconds(0.05f);
        }
        mainCamera.transform.position = new Vector3(0, 0, -10);
    }



}
