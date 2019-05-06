using System.Collections;
using UnityEngine;

namespace SukoyakaMeteor.LightNovelMaker
{

    /// <summary>
    /// 音を鳴らすやつ
    /// </summary>
    public class SoundController : MonoBehaviour
    {

        /// <summary>
        /// SE素材のリスト
        /// </summary>
        [SerializeField]
        AudioClip[] _SeList = default;

        /// <summary>
        /// BGM素材のリスト
        /// </summary>
        [SerializeField]
        AudioClip[] _BgmList = default;

        /// <summary>
        /// SEを鳴らすやつ
        /// </summary>
        [SerializeField]
        AudioSource _SeSource = default;

        /// <summary>
        /// BGMを鳴らすやつ
        /// </summary>
        [SerializeField]
        AudioSource _BgmSource = default;

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
        /// 音量係数
        /// すべての音量設定にこの値を乗算する
        /// </summary>
        public float BgmVolume { get; set; } = 0.5f;


        /// <summary>
        /// SEを再生する
        /// </summary>
        /// <param name="index">再生したいSEのID</param>
        public void PlaySe(int index)
        {
            _SeSource.PlayOneShot(_SeList[index]);
        }

        /// <summary>
        /// BGMを再生する
        /// すでに何か鳴ってた場合はフェードで変更、
        /// 鳴ってなかったら普通に再生する
        /// </summary>
        /// <param name="index">再生したいBGMのID</param>
        public void PlayBgm(int index)
        {
            if (_BgmSource.isPlaying)
            {
                AudioClip newClip = _BgmList[index];
                StartCoroutine(CoChangeBgm(newClip));
            }
            else
            {
                _BgmSource.clip = _BgmList[index];
                _BgmSource.volume = BgmVolume;
                _BgmSource.Play();
            }
        }

        /// <summary>
        /// BGMを一時停止する
        /// </summary>
        public void PauseBgm()
        {
            StartCoroutine(CoPauseBgm());
        }

        /// <summary>
        /// BGMの一時停止を解除する
        /// </summary>
        public void UnPauseBgm()
        {
            StartCoroutine(CoUnPauseBgm());
        }

        /// <summary>
        /// BGMをフェードさせる
        /// </summary>
        /// <param name="fromVolume">最初の音量</param>
        /// <param name="toVolume">最後の音量</param>
        /// <param name="time">変化するのにかかる時間</param>
        private IEnumerator CoFadeBgm(float fromVolume, float toVolume, float time)
        {
            float elapsedTime = 0f;
            while (elapsedTime <= time)
            {
                elapsedTime += Time.deltaTime;
                _BgmSource.volume = Mathf.Lerp(fromVolume, toVolume, elapsedTime / time);
                yield return null;
            }
        }

        /// <summary>
        /// BGMをフェードで変更する・本体
        /// </summary>
        /// <param name="newIndex">次に鳴らすBGMのID</param>
        private IEnumerator CoChangeBgm(AudioClip newClip)
        {
            yield return CoFadeBgm(BgmVolume, 0f, _FadeTime);

            yield return new WaitForSeconds(_WaitTime);

            _BgmSource.clip = newClip;
            _BgmSource.Play();

            yield return CoFadeBgm(0f, BgmVolume, _FadeTime);
        }

        /// <summary>
        /// BGMを一時停止する・本体
        /// </summary>
        private IEnumerator CoPauseBgm()
        {
            yield return CoFadeBgm(BgmVolume, 0f, _FadeTime);

            _BgmSource.Pause();
        }

        /// <summary>
        /// BGMの一時停止を解除する・本体
        /// </summary>
        private IEnumerator CoUnPauseBgm()
        {
            _BgmSource.UnPause();

            yield return CoFadeBgm(0f, BgmVolume, _FadeTime);
        }
    }
}