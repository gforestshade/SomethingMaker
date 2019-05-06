using System.Collections;
using UnityEngine;

using UButton = UnityEngine.UI.Button;


namespace SukoyakaMeteor.LightNovelMaker
{

    public class GallaryController : MonoBehaviour, ICanvas
    {

        /// <summary>
        /// パネル部(ギャラリー画面の上半分)のプレハブ。
        /// ページャーを実装しているので、ここだけ動的にプレハブから生成。
        /// </summary>
        [SerializeField]
        GallaryPanelCreater _GallaryPanelPrefab = default;

        /// <summary>
        /// パネル部がスライドするのにかかる時間
        /// </summary>
        [SerializeField]
        float _SlideTime = 1f;

        /// <summary>
        /// 前のページに戻るボタン
        /// </summary>
        [SerializeField]
        UButton _PreviousPageButton = default;

        /// <summary>
        /// 次のページに進むボタン
        /// </summary>
        [SerializeField]
        UButton _NextPageButton = default;


        /// <summary>
        /// 現在のページ数
        /// </summary>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// ゲーム全体の制御システム
        /// </summary>
        private GameController gc;

        /// <summary>
        /// パネル部の横幅
        /// </summary>
        private float panelWidth;

        /// <summary>
        /// パネル部のy座標
        /// </summary>
        private float panelY;

        /// <summary>
        /// 最小のページ数
        /// 通常は0
        /// </summary>
        private int minPageIndex;

        /// <summary>
        /// 最大のページ数
        /// ノベル総数 / 1画面に表示できる数 てきなもの
        /// </summary>
        private int maxPageIndex;

        /// <summary>
        /// 今表示されているパネル部
        /// </summary>
        private GallaryPanelCreater currentPanel;


        /// <summary>
        /// 初期化。
        /// ゲーム開始時に1度だけ呼ばれるはず。
        /// </summary>
        /// <param name="gameController">ゲーム全体の制御システム</param>
        public void Init(GameController gameController)
        {
            // ページ数の最小と最大を計算
            minPageIndex = 0;
            maxPageIndex = gameController.NovelInfoList.list.Length / _GallaryPanelPrefab.ItemsCount;

            if (gameController.NovelInfoList.list.Length % _GallaryPanelPrefab.ItemsCount == 0)
            {
                maxPageIndex--;
            }

            // パネル部の幅と座標をあらかじめ計算しておく
            RectTransform rt = _GallaryPanelPrefab.GetComponent<RectTransform>();
            panelWidth = rt.sizeDelta.x;
            panelY = rt.localPosition.y;
        }

        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="gameController">ゲーム全体の制御システム</param>
        public void Draw(GameController gameController)
        {
            gc = gameController;
            gameObject.SetActive(true);

            // ページを生成
            if (currentPanel != null)
            {
                Destroy(currentPanel.gameObject);
            }
            currentPanel = Instantiate(_GallaryPanelPrefab, transform);
            currentPanel.Create(this, gameController.NovelInfoList, gameController.Histories, PageIndex);

            InvalidateButtons(PageIndex);
        }

        /// <summary>
        /// ギャラリー画面は表示しただけでは動かさないので
        /// 空のメソッド
        /// </summary>
        public void Show()
        {
        }

        /// <summary>
        /// ページを戻る/進むボタンの有効/無効を切り替える
        /// 例えばページ数が左端に達していたら戻るボタンが無効になる
        /// </summary>
        /// <param name="pageIndex">ページ数</param>
        public void InvalidateButtons(int pageIndex)
        {
            _PreviousPageButton.interactable = (pageIndex > minPageIndex);
            _NextPageButton.interactable = (pageIndex < maxPageIndex);
        }

        /// <summary>
        /// 次のページへ
        /// </summary>
        public void NextPage()
        {
            ChangePage(+1);
        }

        /// <summary>
        /// 前のページへ
        /// </summary>
        public void PrevPage()
        {
            ChangePage(-1);
        }

        /// <summary>
        /// ページを変える
        /// </summary>
        /// <param name="iChange">増えるページ数</param>
        private void ChangePage(int iChange)
        {
            // 最小と最大の中に収まるようにする
            PageIndex = Mathf.Clamp(PageIndex + iChange, minPageIndex, maxPageIndex);
            InvalidateButtons(PageIndex);

            // プラスなら1, マイナスなら-1
            int dir = 0;
            if (iChange > 0)
            {
                dir = 1;
            }
            else if (iChange < 0)
            {
                dir = -1;
            }
            StartCoroutine(CoSlidePage(dir));
        }

        /// <summary>
        /// ページをスライドさせるアニメーション
        /// </summary>
        /// <param name="direction">
        /// 方向　1か-1を指定する
        /// 1なら1ページ進み、-1なら1ページ戻る
        /// </param>
        private IEnumerator CoSlidePage(int direction)
        {
            // 新しいページのパネルを生成する
            GallaryPanelCreater newPanel = Instantiate(_GallaryPanelPrefab, transform);
            newPanel.transform.localPosition = new Vector3(panelWidth * direction, panelY);
            newPanel.Create(this, gc.NovelInfoList, gc.Histories, PageIndex);

            // 古いパネルと新しいパネルの両方を指定された方向に動かす
            float elaspedTime = 0f;
            while (elaspedTime < _SlideTime)
            {
                elaspedTime += Time.deltaTime;

                Vector3 pos = currentPanel.transform.localPosition;
                pos.x = Mathf.Lerp(0f, -panelWidth * direction, elaspedTime / _SlideTime);
                Vector3 dx = new Vector3(panelWidth * direction, 0f);

                currentPanel.transform.localPosition = pos;
                newPanel.transform.localPosition = pos + dx;

                yield return null;
            }

            // 新しいパネルをきっちり所定の位置に
            {
                Vector3 pos = newPanel.transform.localPosition;
                pos.x = 0f;
                newPanel.transform.localPosition = pos;
            }

            // 古いパネルを消して新しいパネルを参照するようにする
            Destroy(currentPanel.gameObject);
            currentPanel = newPanel;
        }

        /// <summary>
        /// ノベルをクリックしたとき
        /// フェード経由でリザルト画面に遷移する
        /// </summary>
        /// <param name="novel">ノベル情報</param>
        public void OnNovelClick(NovelInfo novel)
        {
            gc.FadeToResult(this, gameObject, novel, false);
        }

        /// <summary>
        /// モドルボタンをクリックしたとき
        /// フェード経由でスロット画面に遷移する
        /// </summary>
        public void OnBackToTitleClick()
        {
            gc.FadeToThrotte(gameObject);
        }
    }
}