using System.Collections.Generic;
using UnityEngine;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// 履歴を管理するやつ
    /// </summary>
    [System.Serializable]
    public class Histories
    {

        /// <summary>
        /// PlayerPrefsに渡すキー
        /// </summary>
        const string SAVE_KEY = "Histories";

        /// <summary>
        /// 履歴データ
        /// </summary>
        [SerializeField]
        private List<History> list;


        // コンストラクタ
        private Histories(List<History> list)
        {
            this.list = list;
        }

        /// <summary>
        /// 指定したノベルを引いた回数を取得する
        /// </summary>
        /// <param name="id1">上の句ID</param>
        /// <param name="id2">下の句ID</param>
        /// <returns>回数</returns>
        public int Get(int id1, int id2)
        {
            int index = list.FindIndex(i => i.Id1 == id1 && i.Id2 == id2);
            if (index >= 0)
            {
                return list[index].Count;
            }
            else
            {
                // そのノベルの履歴がなければ0
                return 0;
            }
        }

        /// <summary>
        /// 指定したノベルを引いた回数に1を足す
        /// </summary>
        /// <param name="id1">上の句ID</param>
        /// <param name="id2">下の句ID</param>
        public void Add(int id1, int id2)
        {
            int index = list.FindIndex(i => i.Id1 == id1 && i.Id2 == id2);
            if (index >= 0)
            {
                list[index].Count++;
            }
            else
            {
                // そのノベルの履歴がなければつくって+1
                list.Add(new History() { Id1 = id1, Id2 = id2, Count = 1 });
            }
        }

        /// <summary>
        /// セーブデータをロードする
        /// なければ作って返す
        /// </summary>
        /// <returns>ロードしたデータ</returns>
        public static Histories Load()
        {
            string json = PlayerPrefs.GetString(SAVE_KEY);

            Debug.Log($"Load Histories:\n{json}");

            if (string.IsNullOrEmpty(json))
            {
                // データがなければ初期状態にする
                var l = new List<History>();
                return new Histories(l);
            }
            else
            {
                return JsonUtility.FromJson<Histories>(json);
            }
        }

        /// <summary>
        /// セーブする
        /// </summary>
        public void Save()
        {
            string json = JsonUtility.ToJson(this);
            PlayerPrefs.SetString(SAVE_KEY, json);

            Debug.Log($"Save Histories:\n{json}");
        }


    }

    /// <summary>
    /// ノベル1本の履歴
    /// </summary>
    [System.Serializable]
    public class History
    {
        public int Id1;
        public int Id2;
        public int Count;
    }

}