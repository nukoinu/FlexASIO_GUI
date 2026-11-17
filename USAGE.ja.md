# FlexASIO GUI の使い方

FlexASIO GUI は、DAW が FlexASIO 経由で使うマイクやスピーカーを選び、設定を `FlexASIO.toml` に保存する Windows アプリです。以下の手順は .NET 11 対応の x64 版を対象とし、ボタン名は画面の英語表記に合わせています。

## インストールする

1. FlexASIO ドライバーをインストールします。すでに DAW の ASIO ドライバー一覧に **FlexASIO** が表示されていれば、この手順は省略できます。 [FlexASIO の配布ページ](https://github.com/dechamps/FlexASIO/releases)
2. FlexASIO GUI の x64 インストーラーを実行します。この版には .NET 11 Desktop Runtime が含まれています。
3. Windows のスタートメニューから **FlexASIO GUI** を開きます。インストーラーを使わず発行フォルダーから起動する場合は、同梱ファイルと同じ場所にある `FlexASIOGUI.exe` を開きます。 [発行手順](README.md)

既存の設定を編集する前に、`%USERPROFILE%\FlexASIO.toml` があればコピーを取ってください。GUI の保存処理はファイル全体を書き換えるため、手書きのコメントや GUI が扱わない設定項目は保存時に失われることがあります。

## 入出力を選んで DAW で確認する

1. **Backend** で `Windows WASAPI` を選びます。初回は入出力それぞれの **WASAPI** 欄で **Set:** を外し、既定の共有モードを使います。
2. **Input Device** で録音するマイクやオーディオインターフェースを選び、**Output Device** で再生するスピーカーやヘッドホンを選びます。再生だけなら入力側を `(None)` にします。`(None)` を選んだ側は無効になります。
3. 初回は **Set Buffer Size** と **Set Input Latency**、**Set Output Latency** のチェックを外し、**Channels** を `0` にします。各項目に FlexASIO の既定値が使われます。
4. **Output Preview** でバックエンドと入出力を確認し、**Save to Default FlexASIO.toml** を押します。保存先は `%USERPROFILE%\FlexASIO.toml` です。エクスプローラーのアドレスバーに `%USERPROFILE%` と入力すると、そのフォルダーを開けます。
5. DAW のオーディオ設定でドライバーの種類に **ASIO**、ドライバーに **FlexASIO** を選びます。DAW 側でも使用する入出力チャンネルを有効にし、短い音源を再生して確認します。録音する場合はトラックに入力を割り当て、入力メーターが反応するか確認します。

DAW がすでに FlexASIO を使用中で、保存後も設定が反映されない場合は、ドライバーを選び直すか DAW を再起動します。 [FlexASIO の設定ファイル仕様](https://github.com/dechamps/FlexASIO/blob/master/CONFIGURATION.md)

## 画面の項目

| 項目 | 使い方 |
| --- | --- |
| **Backend** | FlexASIO が Windows の音声デバイスへ接続する方式です。選択した方式に応じて入出力の候補が変わります。 |
| **Input Device** / **Output Device** | 録音・再生デバイスを個別に選択します。プレビューで `device` の指定が省略されている側は、そのバックエンドの既定デバイスが使われます。`device = ""` は無効です。 |
| **Set Buffer Size** | ASIO バッファーの推奨サイズをサンプル数で指定します。値を小さくすると遅延を減らせますが、音切れが起きやすくなります。DAW にバッファー設定がある場合は、まず DAW 側で調整します。 |
| **Channels** | 入出力それぞれのチャンネル数を指定します。`0` は設定ファイルでの指定を省略します。 |
| **Set Input Latency** / **Set Output Latency** | バックエンドに提示する遅延の目安を秒単位で指定します。実際の遅延はバックエンドやデバイスによって変わります。 |
| **WASAPI** | 入出力ごとに **Set:** をオンにすると、**Exclusive** と **AutoConvert** を明示的に設定します。**Exclusive** は排他モードを使います。**AutoConvert** は共有モードでサンプルレートとチャンネル数の自動変換を許可します。 |
| **Refresh Devices** | 選択中のバックエンドのデバイス一覧を表示し直します。接続した機器が出ない場合は GUI を開き直します。 |

## 設定を別名で保存・読み込む

- **Save as** は、現在のプレビューを指定したファイルに保存します。設定を試し分ける場合は `FlexASIO-playback.toml` のように名前を付けて保存できます。
- **Load from** は、指定した TOML ファイルを GUI に読み込みます。読み込んだ内容を使うには、プレビューを確認して **Save to Default FlexASIO.toml** を押します。
- **Copy to Clipboard** は、プレビューの TOML をクリップボードにコピーします。テキストエディターに貼り付けて確認や編集ができます。

## うまく動かないとき

- **DAW に FlexASIO が表示されない**場合は、FlexASIO ドライバーがインストール済みか確認して DAW を開き直します。
- **デバイスが表示されない**場合は、Windows のサウンド設定で機器が認識されているか確認し、GUI を開き直して **Backend** とデバイス一覧を確認します。FlexASIO 側の一覧は、ドライバーのインストール先にある `x64\PortAudioDevices.exe` でも確認できます。 [デバイス一覧ツール](https://github.com/dechamps/FlexASIO#device-list-program)
- **音が出ない、録音できない**場合は、プレビューと保存先を確認し、使う側が `(None)` になっていないか、DAW で対象チャンネルが有効かを確認します。WASAPI の排他モードを設定している場合は、共有モードに戻して試します。
- **音が途切れる**場合は、バッファーサイズを大きくして再生を試します。再生だけに使う場合は入力を `(None)` にします。
- **設定を戻したい**場合は、バックアップを `%USERPROFILE%\FlexASIO.toml` に戻します。設定前の既定動作に戻したい場合は、既存ファイルを別名にしてから DAW を再起動します。
- **原因を調べたい**場合は、`%USERPROFILE%` に空の `FlexASIO.log` を作り、DAW を再起動して問題を再現します。確認が終わったらログファイルを別のフォルダーに移すか削除します。 [FlexASIO のログの説明](https://github.com/dechamps/FlexASIO#logging)
