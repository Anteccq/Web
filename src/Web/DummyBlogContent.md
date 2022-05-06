これは試験用ダミーコンテンツ。

マイニングに興味があったもしくはやってみたことがある人は聞いたことがあるかもしれない。Difficultyとは、その名の通り困難性を意味している。ブロックに対して難しさとは何だと疑問に思うかもしれないが、このシステムがブロックチェーン技術への信頼性を生み出している。

Difficultyとは「先頭nビットが0である値になるまで計算しなさい」という条件を示していて、たとえば7だったら `0x01FFFFF...` 以下になるまで繰り返し計算しなさいということになる。この条件の厳しさは、新たなブロックがブロックチェーンに紡がれる速度を制御するためで、たとえばビットコインでは、全世界のコンピュータが計算し続けて10分に一回しか正解が見つからないような値になっている。

> 参考：もし手元にプール接続型ではないBitcoinマイナーがあるならば、採掘できるまで動かしてみると良いだろう。数か月後、電気代の明細をみてあなたはあきらめるだろう。

このDifficulty値は可変であり、たとえば10秒に1回だけ正解が見つかるように設定したとして、実際は6秒であった場合はBitsの数が増え、次回の計算量が増える。逆に18秒もかかっていた場合は、困難値を下げることで、10秒に近づくよう時間が調整される。

> 注意：簡略的に説明をするためビット数によるDifficulty定義を行っているが、ビットコインやイーサリアム等多くのブロックチェーンでは、さらに細かいDifficulty調整がされている。
> ここに過去に私が実験的実装を行ったリポジトリを[こちら](https://github.com/Anteccq/ArCanaRain/blob/main/ArCanaRain.Difficulty/Programs.cs)に示しておく。


[リンク](https://github.com/Anteccq)

![画像](https://github.com/Anteccq.png)

## BlogRepository Interface

```csharp
using System;
using System.Reactive.Concurrency;
using Microsoft.Extensions.DependencyInjection;
using Sample.Models.Notification;
using Sample.ViewModels;

namespace Sample.Views;

public class ViewModelResolver
{
    public static ViewModelResolver Resolver { get; } = new();

    private readonly IServiceProvider _serviceProvider;
    private ViewModelResolver()
    {
        _serviceProvider = Configure(services =>
        {
            services.AddSingleton<IScheduler>(_ => DefaultScheduler.Instance);
            services.AddSingleton<INotificator, ToastNotificator>();
            services.AddSingleton<MainWindowViewModel>();
        });
    }

    public T Get<T>() where T : ViewModelBase
        => _serviceProvider.GetRequiredService<T>();

    private static IServiceProvider Configure(Action<IServiceCollection> configure)
    {
        var serviceCollection = new ServiceCollection();
        configure(serviceCollection);
        return serviceCollection.BuildServiceProvider();
    }
}
```

## 書くことが思いつかない。
まぁそんなわけで。
ではさようなら。