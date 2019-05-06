# SomethingMaker
～～ 大ヒット○○メーカー ～～

## これは何？
これは　すこやかメテオ作　大ヒットノベルメーカー　のソースコードです。  
このプロジェクト全体を、MITライセンスの元で自由にご利用いただけます。

## つかいかた
Unity2018.3.5のプロジェクトになっています。  
テストデータが入っていますので、GameScene.unityを開けばテストプレイできます。

## 素材の差し替え
### 文字列データ
Assets/Resources/novelData.json がノベルデータです。

- Id1: 上の句ID
- Id2: 下の句ID
- Title1: 上の句の文字列
- Title2: 下の句の文字列
- Rank: ランクを表す文字列 A,B,C,Dのいずれか
- Outline: リザルトに表示される説明文
- Writer: リザルトのWriter欄に表示される文字列
- Illust: リザルトのIllustrator欄に表示される文字列

### 制限
同じ上の句IDには同じ文字列が割り当てられている必要があります。  
下の句IDにはその制限はありません。

### 画像データ
Assets/Resources/Image/ が表紙画像用のフォルダになっています。  
novelData.jsonで設定した各作品について、  
"(上の句ID)-(下の句ID).png"というファイル名で画像を入れてください。

### BGM, SE
GameSceneにあるSoundControllerにAudioClipを設定することで音を設定できます。

- Se List
    + 0番目: ボタン押下音
    + 1番目: RankAを取ったとき
    + 2番目: RankBを取ったとき
    + 3番目: RankCを取ったとき
- Bgm List
    + 0番目: スロット画面のBGM
    + 1番目: ギャラリー画面のBGM

## Tweetボタン
デフォルトではオフにしてありますが、Tweet機能をつけることもできます。
TweetManagerSettingsにハッシュタグのリストとimgurのClient IDを指定して、
Scene中にあるTweetボタンをActiveにしてください。
