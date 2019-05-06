using UnityEngine;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// ギャラリー画面の上半分をつくるやつ
    /// </summary>
    public class GallaryPanelCreater : MonoBehaviour
    {

        /// <summary>
        /// ノベルを表示する用
        /// </summary>
        [SerializeField]
        GallaryNovelCreater[] _Novels = default;

        /// <summary>
        /// 1パネル当たり何個ノベルを表示しているか
        /// </summary>
        public int ItemsCount => _Novels.Length;


        /// <summary>
        /// 上半分を生成する
        /// </summary>
        /// <param name="parent">親にあたるクラス　ここでは使わずボタンまで渡す</param>
        /// <param name="novelInfoList">ノベル情報リスト</param>
        /// <param name="pageIndex">何ページ目を生成するか</param>
        public void Create(GallaryController parent, NovelInfoList novelInfoList, Histories histories, int pageIndex)
        {
            for (int i = 0; i < ItemsCount; i++)
            {
                // {pageIndex}ページ目の{i}個目のノベル
                int novelId = pageIndex * ItemsCount + i;

                NovelInfo novel = null;
                if (novelId >= 0 && novelId < novelInfoList.list.Length)
                {
                    novel = novelInfoList.list[novelId];
                    if (histories.Get(novel.Id1, novel.Id2) <= 0)
                    {
                        novel = null;
                    }
                }

                _Novels[i].Draw(parent, novel);
            }
        }
    }
}
