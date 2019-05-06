using UnityEngine;

// 長いので別名をつける
using URawImage = UnityEngine.UI.RawImage;
using UText = UnityEngine.UI.Text;
using UButton = UnityEngine.UI.Button;


namespace SukoyakaMeteor.LightNovelMaker
{

    public class GallaryNovelCreater : MonoBehaviour
    {

        /// <summary>
        /// 表紙画像表示用
        /// </summary>
        [SerializeField]
        URawImage _Image = default;

        /// <summary>
        /// タイトル
        /// </summary>
        [SerializeField]
        UText _Title = default;

        /// <summary>
        /// ボタン本体
        /// </summary>
        [SerializeField]
        UButton _Button = default;

        /// <summary>
        /// No Image のやつ
        /// </summary>
        [SerializeField]
        Texture2D _NoImageTexture = default;

        /// <summary>
        /// ノベル情報をもとに見た目を構築する
        /// </summary>
        /// <param name="parent">親になるクラス　ボタンのクリックイベントを親に向けて送るのに使う</param>
        /// <param name="novel">ノベル情報</param>
        public void Draw(GallaryController parent, NovelInfo novel)
        {
            if (novel != null)
            {
                _Title.text = novel.Title1 + novel.Title2;

                Texture2D novelTexture = Resources.Load<Texture2D>($"Image/{novel.Id1}-{novel.Id2}") ?? _NoImageTexture;
                _Image.texture = novelTexture;

                // ノベル情報を添えて親のメソッドを呼ぶ
                _Button.onClick.AddListener(() => parent.OnNovelClick(novel));
            }
            else
            {
                _Title.text = "???";
                _Image.texture = null;
                _Image.color = Color.clear;
            }

            // 文字数によってフォントサイズを変える
            int textLength = _Title.text.Length;
            if (textLength <= 25)
            {
                _Title.fontSize = 14;
            }
            else
            {
                _Title.fontSize = 10;
            }
        }
    }
}
