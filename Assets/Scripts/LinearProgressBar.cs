using UnityEngine;
using UnityEngine.Events;

// 長いので別名をつける
using UImage = UnityEngine.UI.Image;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// プログレスバーみたいなアニメーションするやつ
    /// </summary>
    public class LinearProgressBar : MonoBehaviour
    {

        /// <summary>
        /// 表示用
        /// </summary>
        [SerializeField]
        private UImage _Image = default;

        /// <summary>
        /// かかる時間
        /// </summary>
        [SerializeField]
        float _MoveTime = 3f;
        public float MoveTime { get => _MoveTime; set => _MoveTime = value; }

        /// <summary>
        /// 表示時に自動で開始するか
        /// </summary>
        [SerializeField]
        bool _MoveOnEnable = true;

        /// <summary>
        /// 終わったときのイベント
        /// </summary>
        [SerializeField]
        UnityEvent _OnMoved = default;


        /// <summary>
        /// 動いているか
        /// </summary>
        public bool IsMove { get; set; }


        /// <summary>
        /// 現在までの累積時間
        /// </summary>
        private float elapsedTime;


        // 最初は動かさない
        private void Awake()
        {
            IsMove = false;
        }

        // 表示時フラグがtrueなら即動かす
        private void OnEnable()
        {
            if (_MoveOnEnable)
            {
                Move();
            }
        }

        /// <summary>
        /// 動かす
        /// </summary>
        public void Move()
        {
            IsMove = true;
            elapsedTime = 0f;
            _Image.fillAmount = 0f;
        }

        // 本体
        private void Update()
        {
            if (IsMove)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime <= MoveTime)
                {
                    _Image.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / MoveTime);
                }
                else
                {
                    IsMove = false;
                    _OnMoved.Invoke();
                }
            }
        }

    }
}
