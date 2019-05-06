using UnityEngine;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// スロット画面を制御するやつ
    /// </summary>
    public class ThrotteController : MonoBehaviour, ICanvas
    {

        /// <summary>
        /// 左のくるくる
        /// </summary>
        [SerializeField]
        Rotater _RotateLeft = default;

        /// <summary>
        /// 右のくるくる
        /// </summary>
        [SerializeField]
        Rotater _RotateRight = default;

        /// <summary>
        /// 左の結果表示
        /// </summary>
        [SerializeField]
        PhraseViewer _PhraseLeft = default;

        /// <summary>
        /// 右の結果表示
        /// </summary>
        [SerializeField]
        PhraseViewer _PhraseRight = default;

        /// <summary>
        /// 下のボタン類
        /// </summary>
        [SerializeField]
        BelowButtonsController _BelowButtons = default;


        /// <summary>
        /// 内部進行管理用の数
        /// </summary>
        private enum Progress
        {
            ROTATING_KAMINOKU, // 上の句回し中
            ROTATING_SHIMONOKU, // 下の句回し中
            STOPPED, // 止まった
        }

        /// <summary>
        /// 内部進行管理
        /// </summary>
        private Progress progress = Progress.ROTATING_KAMINOKU;

        /// <summary>
        /// 選択されたノベル
        /// </summary>
        private NovelInfo selectedNovel;

        /// <summary>
        /// ゲーム全体の制御システム
        /// </summary>
        private GameController gc;


        /// <summary>
        /// 表示
        /// </summary>
        public void Draw(GameController gameController)
        {
            gc = gameController;

            gameObject.SetActive(true);
            progress = Progress.ROTATING_KAMINOKU;

            // ここで抽選
            selectedNovel = gc.NovelInfoList.GetRandomNovel();
            Debug.Log(selectedNovel.Title1 + "/" + selectedNovel.Title2);

            // 両方のくるくるを表示
            _RotateLeft.gameObject.SetActive(true);
            _RotateRight.gameObject.SetActive(true);

            // 両方の結果表示を非表示に
            _PhraseLeft.gameObject.SetActive(false);
            _PhraseRight.gameObject.SetActive(false);

            // 下のボタン類を初期化
            _BelowButtons.Init();
        }

        /// <summary>
        /// 動かす
        /// </summary>
        public void Show()
        {
            // 両方のくるくるを回す
            _RotateLeft.IsRotate = true;
            _RotateRight.IsRotate = true;
        }

        /// <summary>
        /// 上の句を止めようとしているとき
        /// </summary>
        private void OnLeftStopping()
        {
            progress = Progress.ROTATING_SHIMONOKU;

            // 左のくるくるを非表示
            _RotateLeft.gameObject.SetActive(false);

            // 左の結果表示にセット
            _PhraseLeft.Show(selectedNovel.Title1);

            // ぱーん
            gc.SoundController.PlaySe(0);
        }

        /// <summary>
        /// 下の句を止めようとしているとき
        /// </summary>
        private void OnRightStopping()
        {
            progress = Progress.STOPPED;

            // 右のくるくるを非表示
            _RotateRight.gameObject.SetActive(false);

            // 右の結果表示にセット
            _PhraseRight.Show(selectedNovel.Title2);

            // ぱーん
            gc.SoundController.PlaySe(0);

            // 下のボタンを執筆中にする
            _BelowButtons.Write(selectedNovel.Title1 + selectedNovel.Title2, selectedNovel.Rank);
        }

        /// <summary>
        /// ストップボタンがクリックされたとき
        /// </summary>
        public void OnStopButtonClick()
        {
            // 進行度によってメソッドを呼び分ける
            if (progress == Progress.ROTATING_KAMINOKU)
            {
                OnLeftStopping();
            }
            else if (progress == Progress.ROTATING_SHIMONOKU)
            {
                OnRightStopping();
            }
        }

        /// <summary>
        /// つぎへボタンがクリックされたとき
        /// 一定時間後にリザルト画面へ遷移する
        /// </summary>
        public void OnNextButtonClick(UnityEngine.UI.Selectable sender)
        {
            // 連打できないようにボタンを無効化する
            sender.interactable = false;

            // 一定時間後にメソッドを呼ぶ
            Invoke("ShowResult", 0.3f);
        }

        /// <summary>
        /// フェード経由でリザルト画面へ遷移する
        /// </summary>
        private void ShowResult()
        {
            // 履歴更新
            gc.Histories.Add(selectedNovel.Id1, selectedNovel.Id2);
            gc.Histories.Save();

            // ゲーム全体の制御システムにお願いしてリザルト画面に遷移してもらう
            gc.FadeToResult(this, gameObject, selectedNovel, true);
        }

        /// <summary>
        /// フェード経由でギャラリー画面へ遷移する
        /// </summary>
        public void OnGallaryButtonClick()
        {
            gc.FadeToGallary(gameObject);
        }

    }
}