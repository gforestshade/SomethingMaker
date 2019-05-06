using UnityEngine;
using UnityEngine.Events;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// 入場するやつ
    /// </summary>
    public class LinearEnterer : MonoBehaviour
    {

        /// <summary>
        /// 距離
        /// </summary>
        [SerializeField]
        float _Distance = 700f;

        /// <summary>
        /// かかる時間
        /// </summary>
        [SerializeField]
        float _MoveTime = 0.2f;
        public float MoveTime { get => _MoveTime; set => _MoveTime = value; }

        /// <summary>
        /// 表示時に自動で入場するか
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


        private Vector3 originalPos;
        private Vector3 fromPos;
        private Vector3 toPos;

        /// <summary>
        /// 現在までの累積時間
        /// </summary>
        private float elapsedTime;


        // 最初は動かさない
        private void Awake()
        {
            originalPos = transform.localPosition;
            IsMove = false;
        }

        // 表示時フラグがtrueなら即動かす
        private void OnEnable()
        {
            // はじめの位置と終わりの位置を計算
            toPos = originalPos;
            fromPos = toPos - transform.right * _Distance;

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
            transform.localPosition = fromPos;
        }

        // 本体
        private void Update()
        {
            if (IsMove)
            {
                elapsedTime += Time.deltaTime;

                if (elapsedTime <= MoveTime)
                {
                    transform.localPosition = Vector3.Lerp(fromPos, toPos, elapsedTime / MoveTime);
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
