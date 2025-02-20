# getFreePort() 的な関数を提供しては？ という案。

「空きポート」得る、という関数を提供しては？ という案。

> createHttpServer や createSocketServer の作成ですでに実装しておるのでは？

# どのようなもの？

- C#　https://github.com/komiyamma/hm_free_port/blob/main/HmFreePort/HmFreePort/Program.cs

- C++
https://github.com/komiyamma/hm_free_port/blob/main/HmFreePort/CPP.ver/Program.cpp

# どのような目的で使うのだろう？

多くの場合は、「個別ブラウザ枠」や「レンダリング枠」の「urlプロパティ」に、  
http://localhost:port の形で開く目的で利用する。  
開くためには何らかのサーバーが必要で「その簡易サーバーを立てる」ために利用するかと思います。  

## なぜ http://localhost が必要になるの？ file:// ではダメなの？

- 秀丸の個別ブラウザ枠やレンダリング枠をマクロから利用する際は、その制御は「URLが移ってしまう」ことに対しては弱い。  
よって基本的には「SPA(Single Page Application)」に寄せた作りになりやすい。  
(URLを変えずにDOMの一部を変えるような作り)  

- ブラウザ用途の「素のJavaScript」を記述する人は相当に減少。  
これはCMSやSNS登場により、個人が「自身の力でサイトファイルを構築」する機会がほぼ消滅したことが大きな要因。  
 
- Reactの登場もあり(VueやAngularも同様でしょうが)、SPAに寄せたフレームワークは、  
基本的には file:// 上では動作せず、http:// が想定されている。  
その動作は localhost が最も安定していると断言出来るほど  
(なぜならSPA系フレームワークでは、ほぼ全ての人がlocalhost表示させながら作るから)  

- html/javascript部分は(素だろうがReactだろうが) Visual Studio Codeで作ってindex.htmlをビルド。  
秀丸マクロからは、そのindex.html を localhost:port の形で表示して、  
SPA的な該当のindex.html相手に情報やりとり。  
みたいになっていくんじゃないかなと。  

## 身近にある簡易サーバーの例

- PHPのビルトインサーバー  
- node http-server  
- python http.server (簡易すぎて、素で使うには利用価値が低い)  

# 使用した際のマクロの雰囲気

https://github.com/komiyamma/hm_free_port/blob/main/HmFreePort/HmFreePort/bin/Release/HmFreePort.mac

このマクロの
```js
// 空いているポート番号を探す。(49152～65535)
function getFreePort() {
    const com = createobject(String.raw`${currentMacroDirectory}\HmFreePort.dll`, "HmFreePort.HmFreePort");
    return com ? com.Port : 0;
}
```

の関数をユーザーが独自にこしらえなくてよいようにする。

# hidemaru.runProcess と相性が良き

- 「runProcess("stdio")」+「IP」+「documentRoot」を使い簡易サーバーを立てる→「プロセスＤ」誕生
- 秀丸で開いてたファイルを閉じることでrunProcessが「プロセスＤ」を閉じる  
(普通なら実行しっぱなしになりやすいデーモンが終了してくれる)

になるので、比較的「jsmode・個別ブラウザ」の現在の枠組みやインスタンスのライフサイクルに乗りやすいかと。  

> 完璧を期すなら、秀丸が強制終了などで落ちても「プロセスＤ」が解放されるよう、  
 マクロ作成者は「間にウィンドウハンドルとプロセスIDを紐づけ管理する.exe」を挟む必要があります。  
 が、通常の利用用途時ではそこまでしないでしょう。  


# 動作確認

https://github.com/komiyamma/hm_free_port/releases

-  HmFreePort.zip (ブロック解除必須)

- 「C:\あいうえお\index.html」に適当にHTMLを配置。

- 「php.exe」or「node.exe」or「python.exe」にパスが通っていること

-  マクロ内で ```const serverType = "php";```  
   っていうところがあるので、適当に切り替えてください。
