﻿using CommunityToolkit.Maui;
using E_Commercenaveen.Interfaces;
using E_Commercenaveen.Pages;
using E_Commercenaveen.Services;
using E_Commercenaveen.ViewModels;
using Microsoft.Extensions.Logging;

namespace E_Commercenaveen
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiCommunityToolkit();

            builder.Services.AddSingleton<IPlatformHttpMessageHandler>(sp =>
            {
#if ANDROID
                return new Platforms.Android.AndroidHttpMessageHandler();
#elif IOS
			return new Platforms.iOS.IosHttpMessageHandler();
#endif
                return null;
            });


            builder.Services.AddHttpClient(Constants.AppConstants.HttpClientName, httpClient =>
            {
                var baseAddress = DeviceInfo.Platform == DevicePlatform.Android
                                    ?"https://10.0.2.2:12345"
                                    : "https://localhost:12345";
                httpClient.BaseAddress = new Uri(baseAddress);
            })
            .ConfigureHttpMessageHandlerBuilder(configBuilder =>
             {
                 var platformHttpMessageHandler = configBuilder.Services.GetRequiredService<IPlatformHttpMessageHandler>();
                 configBuilder.PrimaryHandler = platformHttpMessageHandler.GetHttpMessageHandler();
             });

            builder.Services.AddSingleton<CategoryService>();
            builder.Services.AddSingleton<ProductsService>();
            builder.Services.AddTransient<OffersService>();
            builder.Services.AddSingleton<HomePageViewModel>();
            builder.Services.AddSingleton<HomePage>();


            builder.Services.AddSingleton<CartViewModel>();
            builder.Services.AddTransient<CartPage>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
