using UnityEngine;
using UnityEngine.SceneManagement;

public class App : MonoBehaviour
{
    /*
    오솔길 : Alley
    산맥 : MountainRange
    농장 : Farm
    버스정류장 : BusStop
    판다마을 : PandaVillage
    잉걸불수액숲 : CindersapForest
     */
    public enum eSceneType
    {
        App, LogoScene, LoadingScene, Title, Alley, MountainRange, Farm, BusStop, 
        PandaVillage, CindersapForest, House, VarietyStore, WoodworkingStore, ManiRanch, LoadGame, NewGame, FarmEdit, CoopScene
    }
    public enum eMapType
    {
        Alley = 1, MountainRange, Farm, BusStop, PandaVillage, CindersapForest,
        SecretForest, VarietyStore, WoodworkingStore, ManiRanch, FarmEdit, CoopScene
    }

    public static App instance;

    private eMapType beforeMap;
    private WoodworkingStoreMain woodworkingStoreMain;
    private ManiRanchMain maniRanch;
    private FarmMain farmMain;
    

    private UIApp uiApp;

    private void Awake()
    {
        App.instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        this.uiApp = GameObject.FindObjectOfType<UIApp>();
        this.uiApp.Init();

        this.LoadScene<LogoMain>(eSceneType.LogoScene);
    }

    SceneParams param = new SceneParams();

    public void LoadScene<T>(eSceneType sceneType) where T : SceneMain
    {
        var idx = (int)sceneType;
        SceneManager.LoadSceneAsync(idx).completed += (obj) =>
        {
            var main = GameObject.FindObjectOfType<T>();

            main.AddListener("EndDay", (data) =>
            {
                this.uiApp.FadeOut(0.5f, () =>
                {
                    param.SpawnPos = new Vector3(39, 32, 0);
                    this.LoadScene<HouseMain>(eSceneType.House);
                });
                return;
            });
            main.AddListener("onClickGotoTitle", (data) =>
            {
                this.uiApp.FadeOut(0.5f, () =>
                {
                    this.LoadScene<TitleMain>(eSceneType.Title);
                });
                return;
            });

            main.onDestroy.AddListener(() =>
            {
                uiApp.FadeOut();
            });

            switch (sceneType)
            {
                case eSceneType.LogoScene:
                    {
                        this.uiApp.FadeOutImmediately();
                        var logoMain = main as LogoMain;
                        logoMain.AddListener("onShowLogoComplete", (param) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<LoadingMain>(eSceneType.LoadingScene);
                            });
                        });

                        this.uiApp.FadeIn(0.555f, () =>
                        {
                            logoMain.Init();
                        });
                        break;
                    }
                case eSceneType.LoadingScene:
                    {
                        this.uiApp.FadeIn(0.5f, () => 
                        {
                            main.AddListener("onLoadComplete", (data) =>
                            {
                                this.uiApp.FadeOut(0.5f, () =>
                                {
                                    this.LoadScene<TitleMain>(eSceneType.Title);
                                });
                            });
                            main.Init();
                        });

                        break;
                    }
                case eSceneType.Title:
                    {
                        this.uiApp.FadeIn();
                                               
                        main.AddListener("onClickLoadGame", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<LoadGameMain>(eSceneType.LoadGame);
                            });
                        });
                        main.AddListener("onClickNewGame", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<NewGameMain>(eSceneType.NewGame);
                            });
                        });
                        main.Init();
                        break;
                    }
                case eSceneType.Farm:
                    {
                        this.uiApp.FadeIn();
                        this.farmMain = GameObject.FindObjectOfType<FarmMain>();
                        main.AddListener("onArrivalAlleyPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<AlleyMain>(eSceneType.Alley);
                                param.SpawnPos = new Vector3(14, 1.7f, 0);
                            });
                        });
                        main.AddListener("onArrivalBusStopPortal0", (data) =>
                        {
                            this.uiApp.FadeIn();

                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<BusStopMain>(eSceneType.BusStop);
                                param.SpawnPos = new Vector3(1, 6, 0);
                            });
                        });
                        main.AddListener("onArrivalCindersapForestPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<CindersapForestMain>(eSceneType.CindersapForest);
                                param.SpawnPos = new Vector3(68, 116.5f, 0);
                            });
                        });
                        main.AddListener("onArrivalHousePortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<HouseMain>(eSceneType.House);
                                param.SpawnPos = new Vector3(33, 31.5f, 0);
                            });
                        });
                        main.AddListener("onArrivalCoopScenePortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<CoopSceneMain>(eSceneType.CoopScene);
                                param.SpawnPos = new Vector3(32, 30, 0);
                            });
                        });
                        main.Init(param);
                        break;
                    }
                case eSceneType.Alley:
                    {
                        this.uiApp.FadeIn();

                        main.AddListener("onArrivalFarmPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<FarmMain>(eSceneType.Farm);
                                param.SpawnPos = new Vector3(40.5f, 62.25f, 0);
                            });
                        });
                        main.AddListener("onArrivalMountainRangePortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<MountainRangeMain>(eSceneType.MountainRange);
                                param.SpawnPos = new Vector3(1, 27, 0);
                            });
                        });

                        main.Init(param);
                        break;
                    }
                case eSceneType.MountainRange:
                    {
                        this.uiApp.FadeIn();

                        main.AddListener("onArrivalAlleyPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<AlleyMain>(eSceneType.Alley);
                                param.SpawnPos = new Vector3(48.25f, 25, 0);
                            });
                        });
                        main.AddListener("onArrivalPandaVillagePortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<PandaVillageMain>(eSceneType.PandaVillage);
                                param.SpawnPos = new Vector3(81f, 106.5f, 0);
                            });
                        });
                        main.AddListener("onArrivalWoodworkingStorePortal1", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<WoodworkingStoreMain>(eSceneType.WoodworkingStore);
                                param.SpawnPos = new Vector3(6, 1, 0);
                            });
                        });
                        main.AddListener("onArrivalWoodworkingStorePortal2", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<WoodworkingStoreMain>(eSceneType.WoodworkingStore);
                                param.SpawnPos = new Vector3(3, 15, 0);
                            });
                        });
                        main.Init(param);
                        break;
                    }
                case eSceneType.BusStop:
                    {
                        this.uiApp.FadeIn();

                        main.AddListener("onArrivalFarmPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<FarmMain>(eSceneType.Farm);
                                param.SpawnPos = new Vector3(78.5f, 47, 0);
                            });
                        });
                        main.AddListener("onArrivalPandaVillagePortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<PandaVillageMain>(eSceneType.PandaVillage);
                                param.SpawnPos = new Vector3(1, 54.75f, 0);
                            });
                        });
                        main.Init(param);
                        break;
                    }
                case eSceneType.PandaVillage:
                    {
                        this.uiApp.FadeIn();

                        main.AddListener("onArrivalMountainRangePortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<MountainRangeMain>(eSceneType.MountainRange);
                                param.SpawnPos = new Vector3(15.5f, 0.7f, 0);
                            });
                        });
                        main.AddListener("onArrivalBusStopPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<BusStopMain>(eSceneType.BusStop);
                                param.SpawnPos = new Vector3(33, 6, 0);
                            });
                        });
                        main.AddListener("onArrivalCindersapForestPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<CindersapForestMain>(eSceneType.CindersapForest);
                                param.SpawnPos = new Vector3(117.5f, 94, 0);
                            });
                        });
                        main.AddListener("onArrivalVarietyStorePortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<VarietyStoreMain>(eSceneType.VarietyStore);
                                param.SpawnPos = new Vector3(6, 4, 0);
                            });
                        });
                        main.Init(param);
                        break;
                    }
                case eSceneType.CindersapForest:
                    {
                        this.uiApp.FadeIn();

                        main.AddListener("onArrivalFarmPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<FarmMain>(eSceneType.Farm);
                                param.SpawnPos = new Vector3(40.5f, 0.5f, 0);
                            });
                        });
                        main.AddListener("onArrivalPandaVillagePortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<PandaVillageMain>(eSceneType.PandaVillage);
                                param.SpawnPos = new Vector3(1, 18, 0);
                            });
                        });
                        main.AddListener("onArrivalManiRanchPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<ManiRanchMain>(eSceneType.ManiRanch);
                                param.SpawnPos = new Vector3(13, 0.3f, 0);
                            });
                        });
                        main.Init(param);
                        break;
                    }
                case eSceneType.House:
                    {
                        this.uiApp.FadeIn();

                        main.AddListener("onArrivalFarmPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<FarmMain>(eSceneType.Farm);
                                param.SpawnPos = new Vector3(64f, 48.5f, 0);
                            });
                        });
                        main.Init(param);
                        break;
                    }
                case eSceneType.VarietyStore:
                    {
                        this.uiApp.FadeIn();

                        main.AddListener("onArrivalPandaVillagePortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<PandaVillageMain>(eSceneType.PandaVillage);
                                param.SpawnPos = new Vector3(44, 52, 0);
                            });
                        });

                        main.Init(param);
                        break;
                    }
                case eSceneType.WoodworkingStore:
                    {
                        this.uiApp.FadeIn();

                        this.woodworkingStoreMain = GameObject.FindObjectOfType<WoodworkingStoreMain>();


                        woodworkingStoreMain.AddListener("onArrivalMountainRangePortal1", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<MountainRangeMain>(eSceneType.MountainRange);
                                param.SpawnPos = new Vector3(12, 12.5f, 0);
                            });
                        });
                        woodworkingStoreMain.AddListener("onArrivalMountainRangePortal2", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<MountainRangeMain>(eSceneType.MountainRange);
                                param.SpawnPos = new Vector3(8, 18.5f, 0);
                            });
                        });
                        woodworkingStoreMain.AddListener("onClickFarmEdit", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<FarmEditMain>(eSceneType.FarmEdit);
                                //param.SpawnPos = new Vector3(8, 18.5f, 0);
                            });
                        });
                        beforeMap = eMapType.WoodworkingStore;
                        main.Init(param);
                        break;
                    }
                case eSceneType.ManiRanch:
                    {
                        this.uiApp.FadeIn();

                        this.maniRanch = GameObject.FindObjectOfType<ManiRanchMain>();

                        maniRanch.AddListener("onArrivalCindersapForestPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<CindersapForestMain>(eSceneType.CindersapForest);
                                param.SpawnPos = new Vector3(89.5f, 101, 0);
                            });
                        });
                        maniRanch.AddListener("onClickFarmEdit", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<FarmEditMain>(eSceneType.FarmEdit);
                            });
                        });
                        beforeMap = eMapType.ManiRanch;
                        maniRanch.Init(param);
                        break;
                    }
                case eSceneType.LoadGame:
                    {
                        this.uiApp.FadeIn();
                        main.AddListener("onLoadGame", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                param.SpawnPos = new Vector3(39, 32, 0);
                                this.LoadScene<HouseMain>(eSceneType.House);
                            });
                        });
                        main.AddListener("onExitBtnClick", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<TitleMain>(eSceneType.Title);
                            });
                        });
                        main.Init();
                        break;
                    }
                case eSceneType.NewGame:
                    {
                        this.uiApp.FadeIn();
                        main.AddListener("onCreateUser", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                param.SpawnPos = new Vector3(39, 32, 0);
                                this.LoadScene<HouseMain>(eSceneType.House);
                            });
                        });
                        main.AddListener("onExitBtnClick", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                this.LoadScene<TitleMain>(eSceneType.Title);
                            });
                        });
                        main.Init(param);
                        break;
                    }
                case eSceneType.FarmEdit:
                    {
                        this.uiApp.FadeIn();
                        var farmEditParam = new FarmEditParam();
                        if(beforeMap == eMapType.WoodworkingStore)
                        {
                            main.AddListener("onEditComplete", (data) =>
                            {
                                this.uiApp.FadeOut(0.5f, () =>
                                {
                                    param.SpawnPos = new Vector3(7, 2.5f, 0);
                                    this.LoadScene<WoodworkingStoreMain>(eSceneType.WoodworkingStore);
                                });
                            });

                            farmEditParam.objectId = this.woodworkingStoreMain.objectId;
                            farmEditParam.editType = this.woodworkingStoreMain.editType;
                        }
                        else if (beforeMap == eMapType.ManiRanch)
                        {
                            main.AddListener("onEditComplete", (data) =>
                            {
                                this.uiApp.FadeOut(0.5f, () =>
                                {
                                    param.SpawnPos = new Vector3(13, 1f, 0);
                                    this.LoadScene<ManiRanchMain>(eSceneType.ManiRanch);
                                });
                            });

                            farmEditParam.objectId = this.maniRanch.objectId;
                            farmEditParam.editType = this.maniRanch.editType;
                        }
                        main.Init(farmEditParam);
                        break;
                    }
                case eSceneType.CoopScene: 
                    {                        
                        this.uiApp.FadeIn();
                        var coopParam = new CoopParam();
                        coopParam.coopInfo = farmMain.enterCoopInfo;                        
                        main.AddListener("onArrivalFarmPortal0", (data) =>
                        {
                            this.uiApp.FadeOut(0.5f, () =>
                            {
                                param.SpawnPos = new Vector3(coopParam.coopInfo.posX +1, coopParam.coopInfo.posY -1, 0);
                                this.LoadScene<FarmMain>(eSceneType.Farm);
                            });
                        });
                        main.Init(coopParam);
                        break;
                    }
            }
        };
    }
}
