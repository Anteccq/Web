# Anteccq Blog
行間のテストをしているわけです。
これは上とみっちりつながってます。

これは一行空いているわけです。



これはめっちゃ空いているわけです。
これはみっちりつながっているわけです。

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