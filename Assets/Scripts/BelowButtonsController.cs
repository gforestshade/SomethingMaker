using UnityEngine;

using UButton = UnityEngine.UI.Button;
using UText = UnityEngine.UI.Text;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// スロット画面の下のほうにあるボタン類を制御するやつ
    /// </summary>
    public class BelowButtonsController : MonoBehaviour
    {

        /// <summary>
        /// ストップボタン
        /// </summary>
        [SerializeField]
        UButton _StopButton = default;

        /// <summary>
        /// 執筆中の画像
        /// </summary>
        [SerializeField]
        GameObject _Writing = default;

        /// <summary>
        /// 執筆中のタイトル
        /// </summary>
        [SerializeField]
        UText _WritingTitle = default;

        /// <summary>
        /// 執筆中のプログレスバー
        /// </summary>
        [SerializeField]
        LinearProgressBar _WritingProgressBar = default;

        /// <summary>
        /// つぎへボタン
        /// </summary>
        [SerializeField]
        UButton _NextButton = default;


        /// <summary>
        /// 初期化
        /// スロット画面初期化時に一緒に呼んでほしい
        /// ストップボタンを表示して他を非表示にする
        /// </summary>
        public void Init()
        {
            _StopButton.gameObject.SetActive(true);

            _NextButton.gameObject.SetActive(false);
            _NextButton.interactable = true;

            _Writing.SetActive(false);
        }

        /// <summary>
        /// 執筆する
        /// </summary>
        /// <param name="rankIndex">ランクID</param>
        public void Write(string title, string rank)
        {
            _WritingTitle.text = title;

            // 執筆時間
            float writeTime = 3f;

            // 執筆時間の抽選
            switch (rank)
            {
                case "A":
                    writeTime = Random.Range(3.5f, 5f);
                    break;
                case "B":
                    writeTime = Random.Range(3f, 4.5f);
                    break;
                case "C":
                    writeTime = Random.Range(2.5f, 3.5f);
                    break;
                case "D":
                    writeTime = 1.5f;
                    break;
                default:
                    break;
            }

            // 執筆バーを表示する
            // LinearProgressBarによって自動でアニメーションがはじまる
            _WritingProgressBar.MoveTime = writeTime;
            _Writing.SetActive(true);
        }

        /// <summary>
        /// 執筆が終わったとき
        /// つぎへボタンを入場させる
        /// </summary>
        public void OnWriteFinished()
        {
            // つぎへボタンを表示する
            // LinearEntererによって自動でアニメーションがはじまる
            _NextButton.gameObject.SetActive(true);
        }

    }
}
