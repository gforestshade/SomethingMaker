using UnityEngine;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// タイトル画面を制御するやつ
    /// </summary>
    public class TitleController : MonoBehaviour
    {

        [SerializeField]
        private GameObject _Debug = default;


        private GameController gc;

        /// <summary>
        /// タイトル画面を表示
        /// </summary>
        public void Show(GameController gameController)
        {
            gc = gameController;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 画面がクリックされたとき
        /// フェード経由でスロット画面に遷移
        /// </summary>
        public void OnClick()
        {
            gc.FadeToThrotte(gameObject);
        }


        private void Awake()
        {
            _Debug.SetActive(false);
        }

        private void Update()
        {
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.LeftAlt)) && Input.GetKeyDown(KeyCode.D))
            {
                _Debug.SetActive(true);
            }
        }

        public void Debug_AllAdd()
        {
            foreach (var novel in gc.NovelInfoList.list)
            {
                gc.Histories.Add(novel.Id1, novel.Id2);
            }
            gc.Histories.Save();
        }

        public void Debug_Delete()
        {
            PlayerPrefs.DeleteKey("Histories");
        }
    }
}