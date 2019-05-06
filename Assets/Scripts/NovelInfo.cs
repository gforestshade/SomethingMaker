
namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// ノベルデータを管理する用
    /// </summary>
    [System.Serializable]
    public class NovelInfoList
    {

        /// <summary>
        /// ノベルデータ本体
        /// </summary>
        public NovelInfo[] list;


        /// <summary>
        /// ランダムなノベル情報を返す
        /// </summary>
        /// <returns>選ばれたノベル情報</returns>
        public NovelInfo GetRandomNovel()
        {
            return list[UnityEngine.Random.Range(0, list.Length)];
        }
    }

    /// <summary>
    /// ノベル1本の情報
    /// </summary>
    [System.Serializable]
    public class NovelInfo
    {
        public int Id1;
        public int Id2;
        public string Title1;
        public string Title2;
        public string Rank;
        public string Outline;
        public string Writer;
        public string Illust;

        // ランク文字列からランクIDを計算するやつをここにねじ込む
        private readonly string[] rankStrs = { "A", "B", "C", "D" };
        public int RankIndex => System.Array.IndexOf(rankStrs, Rank);
    }

}