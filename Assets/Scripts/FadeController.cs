using System.Collections;
using UnityEngine;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// フェード画面を制御するやつ
    /// </summary>
    public class FadeController : MonoBehaviour
    {

        /// <summary>
        /// Alphaを簡単にいじる用のCanvasGroup
        /// </summary>
        [SerializeField]
        CanvasGroup _CanvasGroup = default;

        /// <summary>
        /// フェード時間
        /// </summary>
        [SerializeField]
        float _FadeTime = 0.5f;

        /// <summary>
        /// 待ち時間
        /// </summary>
        [SerializeField]
        float _WaitTime = 0.3f;


        /// <summary>
        /// フェード画面
        /// </summary>
        /// <param name="switchToNext">次の画面への切り替え</param>
        /// <param name="showNext">次の画面の表示</param>
        public void Show(System.Action switchToNext, System.Action showNext)
        {
            gameObject.SetActive(true);
            StartCoroutine(CoShow(switchToNext, showNext));
        }

        /// <summary>
        /// フェード画面アニメーション
        /// </summary>
        private IEnumerator CoShow(System.Action switchToNext, System.Action showNext)
        {
            yield return CoFade(_CanvasGroup, 0f, 1f, _FadeTime);

            switchToNext();

            yield return new WaitForSeconds(_WaitTime);

            yield return CoFade(_CanvasGroup, 1f, 0f, _FadeTime);

            showNext();

            gameObject.SetActive(false);
        }

        /// <summary>
        /// フェードする
        /// </summary>
        /// <param name="cg">対象のCanvasGroup</param>
        /// <param name="fromAlpha">このアルファ値から</param>
        /// <param name="toAlpha">このアルファ値まで</param>
        /// <param name="time">フェードするのにかかる時間</param>
        private IEnumerator CoFade(CanvasGroup cg, float fromAlpha, float toAlpha, float time)
        {
            cg.alpha = fromAlpha;

            float elaspedTime = 0f;
            while (elaspedTime <= time)
            {
                elaspedTime += Time.deltaTime;
                cg.alpha = Mathf.Lerp(fromAlpha, toAlpha, elaspedTime / time);
                yield return null;
            }

            cg.alpha = toAlpha;
        }
    }
}