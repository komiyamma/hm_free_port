# HmFreePort

## 「空きポート」得る関数の提供

基本的には何らかの「空きポート番号」を指定して起動するサーバー系のものを実行する際に利用する。

- ANYCPU版 (Common Object Model)
- x86版 (Native呼び出し)
- x64版 (Native呼び出し)

の３つを用意。  
原則的には ANYCPU版を利用。  
どうしても利用できない場合のみ、x86 or x64を手持ちの秀丸のバージョンと合わせて利用する。

## 空きポートを使って起動する、身近にある簡易httpサーバーの例

- PHPのビルトインサーバー  
- node http-server  
- python http.server (簡易すぎて、素で使うには利用価値が低い)  

