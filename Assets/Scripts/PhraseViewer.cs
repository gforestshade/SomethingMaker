using UnityEngine;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// 上の句や下の句を表示するやつ
    /// </summary>
    public class PhraseViewer : MonoBehaviour
    {

        /// <summary>
        /// 表示用
        /// </summary>
        [SerializeField]
        UnityEngine.UI.Text text = default;

        /// <summary>
        /// 表示
        /// </summary>
        /// <param name="phrase">表示する文字列</param>
        public void Show(string phrase)
        {
            gameObject.SetActive(true);
            text.text = phrase;
        }

    }
}