using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Xml.Linq;
using System;


namespace TweetWithScreenShot
{

    public class TweetManager : MonoBehaviour
    {

        [SerializeField]
        private string[] hashTags = default;

        [SerializeField]
        private string clientID = default;

        public string ClientID
        {
            get
            {
                if (string.IsNullOrEmpty(clientID))
                {
                    throw new Exception("ClientIDをセットしてください");
                }
                return clientID;
            }
        }

        private static TweetManager instance;
        public static TweetManager Instance => instance ?? (instance = FindObjectOfType<TweetManager>());


        public static IEnumerator UploadAndTweet(string text)
        {
            yield return new WaitForEndOfFrame();
            var tex = ScreenCapture.CaptureScreenshotAsTexture();

            // imgurへアップロード
            UnityWebRequest www;

            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("image", Convert.ToBase64String(tex.EncodeToJPG()));
            wwwForm.AddField("type", "base64");

            www = UnityWebRequest.Post("https://api.imgur.com/3/image.xml", wwwForm);

            www.SetRequestHeader("AUTHORIZATION", "Client-ID " + Instance.ClientID);

            yield return www.SendWebRequest();

            string UploadedURL = "";
            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Data: " + www.downloadHandler.text);
                XDocument xDoc = XDocument.Parse(www.downloadHandler.text);
                string url = xDoc.Element("data").Element("link").Value;
                UploadedURL = url.Remove(url.Length - 4, 4);
            }

            text += " " + UploadedURL;
            string hashtags = "&hashtags=";
            if (Instance.hashTags.Length > 0)
            {
                hashtags += string.Join(",", Instance.hashTags);
            }

            // ツイッター投稿用URL
            string TweetURL = "http://twitter.com/intent/tweet?text=" + text + hashtags;

#if UNITY_WEBGL && !UNITY_EDITOR
            Application.ExternalEval(string.Format("window.open('{0}','_blank')", TweetURL));
#elif UNITY_EDITOR
            System.Diagnostics.Process.Start(TweetURL);
#else
            Application.OpenURL(TweetURL);
#endif
        }
    }
}
