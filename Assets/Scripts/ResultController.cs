using System.Collections;
using UnityEngine;

// 長いので別名をつける
using UText = UnityEngine.UI.Text;
using UImage = UnityEngine.UI.Image;
using URawImage = UnityEngine.UI.RawImage;
using static TweetWithScreenShot.TweetManager;


namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// リザルト画面を制御するやつ
    /// </summary>
    public class ResultController : MonoBehaviour, ICanvas<NovelInfo, bool>
    {

        /// <summary>
        /// タイトル
        /// </summary>
        [SerializeField]
        UText _Title = default;

        /// <summary>
        /// あらすじ
        /// </summary>
        [SerializeField]
        UText _Outline = default;

        /// <summary>
        /// 作者
        /// </summary>
        [SerializeField]
        UText _Writer = default;

        /// <summary>
        /// イラストレーター
        /// </summary>
        [SerializeField]
        UText _Illustrator = default;

        /// <summary>
        /// ランク画像表示用
        /// </summary>
        [SerializeField]
        UImage _Rank = default;

        /// <summary>
        /// ランク用の画像
        /// </summary>
        [SerializeField]
        Sprite[] _RankImages = default;

        /// <summary>
        /// ランクが表示されるまでの時間
        /// </summary>
        [SerializeField]
        float _DelayTime1 = 1f;

        /// <summary>
        /// ランクが表示されたあと歓声が鳴るまでの時間
        /// </summary>
        [SerializeField]
        float _DelayTime2 = 0.5f;

        /// <summary>
        /// 歓声が再生開始されたあと遷移可能になるまでの時間
        /// </summary>
        [SerializeField]
        float _DelayTime3 = 2f;

        /// <summary>
        /// 表紙画像表示用
        /// </summary>
        [SerializeField]
        URawImage _Image = default;

        /// <summary>
        /// No Image のやつ
        /// </summary>
        [SerializeField]
        Texture2D _NoImageTexture = default;


        /// <summary>
        /// ゲーム全体の制御システム
        /// </summary>
        private GameController gc;

        /// <summary>
        /// ノベル情報
        /// </summary>
        private NovelInfo novel;

        /// <summary>
        /// 歓声を再生するか
        /// </summary>
        private bool isPlayCheer;

        /// <summary>
        /// 次に進める状態か
        /// </summary>
        private bool canNext;


        private WaitForSeconds delay1, delay2, delay3;


        private void Awake()
        {
            delay1 = new WaitForSeconds(_DelayTime1);
            delay2 = new WaitForSeconds(_DelayTime2);
            delay3 = new WaitForSeconds(_DelayTime3);
        }

        /// <summary>
        /// リザルト画面表示
        /// </summary>
        /// <param name="gameController">ゲーム全体の制御システム</param>
        /// <param name="novel">ノベル情報</param>
        public void Draw(GameController gameController, NovelInfo novel, bool isPlayCheer)
        {
            gc = gameController;
            this.novel = novel;
            this.isPlayCheer = isPlayCheer;
            canNext = false;
            gameObject.SetActive(true);

            // 文字列の類はそのままセット
            _Title.text = novel.Title1 + novel.Title2;
            _Outline.text = novel.Outline;
            _Writer.text = novel.Writer;
            _Illustrator.text = novel.Illust;

            // 表紙画像は動的にロードする
            Texture2D novelTexture = Resources.Load<Texture2D>($"Image/{novel.Id1}-{novel.Id2}") ?? _NoImageTexture;
            _Image.texture = novelTexture;

            // 一旦スタンプを非表示に
            _Rank.gameObject.SetActive(false);
        }

        /// <summary>
        /// リザルト画面を動かす
        /// </summary>
        public void Show()
        {
            // スタンプアニメ
            StartCoroutine(CoAnimation(novel.RankIndex));
        }

        /// <summary>
        /// リザルト画面アニメーション
        /// </summary>
        private IEnumerator CoAnimation(int rankIndex)
        {
            // n秒待つ
            yield return delay1;

            // すたんぷぽん
            // Unity側で設定されている表示時アニメーションが流れる
            _Rank.gameObject.SetActive(true);

            // [ランクID]番目の画像をセット
            // 画像の配列はあらかじめInspectorから設定しておく
            _Rank.sprite = _RankImages[rankIndex];

            // Animator
            Animator rankAnimator = _Rank.GetComponent<Animator>();

            // 表示時アニメーションが終わるまで待つ
            yield return new WaitWhile(() => rankAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f);

            // ぱーん
            gc.SoundController.PlaySe(0);

            // さらにm秒待つ
            yield return delay2;

            // わーわー
            if (isPlayCheer)
            {
                PlayRankSe();

                yield return delay3;
            }

            // 遷移可能にする
            canNext = true;
        }

        private void PlayRankSe()
        {
            switch (novel.Rank)
            {
                case "A":
                    gc.SoundController.PlaySe(1);
                    break;
                case "B":
                    gc.SoundController.PlaySe(2);
                    break;
                case "C":
                    gc.SoundController.PlaySe(3);
                    break;
                default:
                    break;
            }
        }

        public void OnTweetClick()
        {
            var postCoroutine = UploadAndTweet($"大ヒットラノベメーカーで　{novel.Title1 + novel.Title2}を書いたよ！");
            StartCoroutine(postCoroutine);
        }

        /// <summary>
        /// 画面がクリックされたとき
        /// 遷移可能ならフェード経由で元の画面に遷移
        /// </summary>
        public void OnClick()
        {
            if (canNext)
            {
                gc.SoundController.UnPauseBgm();
                gc.FadeToNextCanvas(gameObject, gc.PrevCanvas);
            }
        }

    }
}