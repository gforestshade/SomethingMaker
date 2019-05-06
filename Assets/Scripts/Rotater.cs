using UnityEngine;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// 回すやつ
    /// </summary>
    public class Rotater : MonoBehaviour
    {
        /// <summary>
        /// 回転速度
        /// </summary>
        [SerializeField]
        float rotSpeed = 200f;

        /// <summary>
        /// 回すかどうかのフラグ
        /// publicなプロパティにして外から操作することを想定する
        /// </summary>
        public bool IsRotate { get; set; } = true;


        private void Awake()
        {
            transform.Rotate(Random.Range(0f, 180f), 0f, 0f);
        }

        private void Update()
        {
            if (IsRotate)
            {
                transform.Rotate(rotSpeed * Time.deltaTime, 0f, 0f);
            }
        }

    }
}
