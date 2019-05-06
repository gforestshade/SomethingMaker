using UnityEngine;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// ゲーム全体の制御システム
    /// </summary>
    public class GameController : MonoBehaviour
    {

        /// <summary>
        /// タイトル画面
        /// </summary>
        [SerializeField]
        TitleController _TitleController = default;

        /// <summary>
        /// スロット画面
        /// </summary>
        [SerializeField]
        ThrotteController _ThrotteController = default;

        /// <summary>
        /// リザルト画面
        /// </summary>
        [SerializeField]
        ResultController _ResultController = default;

        /// <summary>
        /// ギャラリー画面
        /// </summary>
        [SerializeField]
        GallaryController _GallaryController = default;

        /// <summary>
        /// フェード画面
        /// </summary>
        [SerializeField]
        FadeController _FadeController = default;

        /// <summary>
        /// サウンド
        /// </summary>
        [SerializeField]
        SoundController _SoundController = default;


        public ICanvas ThrotteCanvas => _ThrotteController;
        public ICanvas GallaryCanvas => _GallaryController;
        public ICanvas<NovelInfo, bool> ResultCanvas => _ResultController;

        public ICanvas PrevCanvas { get; private set; }

        public SoundController SoundController => _SoundController; 


        /// <summary>
        /// ノベルデータ全体
        /// </summary>
        public NovelInfoList NovelInfoList { get; private set; }

        /// <summary>
        /// 履歴データ
        /// </summary>
        public Histories Histories { get; private set; }


        /// <summary>
        /// ゲーム開始
        /// </summary>
        private void Start()
        {
            // ノベルデータをJSONから読み込む
            TextAsset ta = Resources.Load<TextAsset>("noveldata");
            NovelInfoList = JsonUtility.FromJson<NovelInfoList>(ta.text);

            // 履歴を読み込む
            Histories = Histories.Load();

            // 一度全部非表示にする
            _FadeController.gameObject.SetActive(false);
            _ResultController.gameObject.SetActive(false);
            _ThrotteController.gameObject.SetActive(false);

            // ギャラリーは初期化処理をしておく
            _GallaryController.Init(this);
            _GallaryController.gameObject.SetActive(false);

            // タイトル画面へ
            _TitleController.Show(this);
        }

        /// <summary>
        /// フェード付きで次の画面を表示する　引数なし版
        /// </summary>
        public void FadeToNextCanvas(GameObject prevCanvasObj, ICanvas nextCanvas)
        {
            // インターフェース・名前付き引数・ラムダ式などを使って
            // 処理の一部分を外から与える
            _FadeController.Show(
                switchToNext: () =>
                {
                    prevCanvasObj?.SetActive(false);
                    nextCanvas.Draw(this);
                },
                showNext: nextCanvas.Show);
        }

        /// <summary>
        /// フェード付きで次の画面を表示する　1引数版
        /// </summary>
        public void FadeToNextCanvas<T>(GameObject prevCanvasObj, ICanvas<T> nextCanvas, T t)
        {
            // インターフェース・名前付き引数・ラムダ式などを使って
            // 処理の一部分を外から与える
            _FadeController.Show(
                switchToNext: () =>
                {
                    prevCanvasObj?.SetActive(false);
                    nextCanvas.Draw(this, t);
                },
                showNext: nextCanvas.Show);
        }

        /// <summary>
        /// フェード付きで次の画面を表示する　2引数版
        /// </summary>
        public void FadeToNextCanvas<T1, T2>(GameObject prevCanvasObj, ICanvas<T1, T2> nextCanvas, T1 t1, T2 t2)
        {
            // インターフェース・名前付き引数・ラムダ式などを使って
            // 処理の一部分を外から与える
            _FadeController.Show(
                switchToNext: () =>
                {
                    prevCanvasObj?.SetActive(false);
                    nextCanvas.Draw(this, t1, t2);
                },
                showNext: nextCanvas.Show);
        }

        /// <summary>
        /// フェード付きでスロット画面を表示する
        /// </summary>
        public void FadeToThrotte(GameObject prevObj)
        {
            SoundController.PlayBgm(0);

            FadeToNextCanvas(prevObj, _ThrotteController);
        }

        /// <summary>
        /// フェード付きでギャラリー画面を表示する
        /// </summary>
        public void FadeToGallary(GameObject prevObj)
        {
            SoundController.PlayBgm(1);

            FadeToNextCanvas(prevObj, _GallaryController);
        }

        /// <summary>
        /// フェード付きでリザルト画面を表示する
        /// </summary>
        public void FadeToResult(ICanvas prevCanvas, GameObject prevObj, NovelInfo novel, bool isPlayCheer)
        {
            PrevCanvas = prevCanvas;

            SoundController.PauseBgm();

            FadeToNextCanvas(prevObj, ResultCanvas, novel, isPlayCheer);
        }

    }
}
