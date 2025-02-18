﻿using Windows.Storage;

namespace MoneyFox.Desktop.Infrastructure
{
    public static partial class Preferences
    {
        private static readonly object locker = new object();

        private static bool PlatformContainsKey(string key, string sharedName)
        {
            lock(locker)
            {
                ApplicationDataContainer appDataContainer = GetApplicationDataContainer(sharedName);
                return appDataContainer.Values.ContainsKey(key);
            }
        }

        private static void PlatformRemove(string key, string sharedName)
        {
            lock(locker)
            {
                ApplicationDataContainer appDataContainer = GetApplicationDataContainer(sharedName);
                if(appDataContainer.Values.ContainsKey(key))
                {
                    appDataContainer.Values.Remove(key);
                }
            }
        }

        private static void PlatformClear(string sharedName)
        {
            lock(locker)
            {
                ApplicationDataContainer appDataContainer = GetApplicationDataContainer(sharedName);
                appDataContainer.Values.Clear();
            }
        }

        private static void PlatformSet<T>(string key, T value, string sharedName)
        {
            lock(locker)
            {
                ApplicationDataContainer appDataContainer = GetApplicationDataContainer(sharedName);

                if(value == null)
                {
                    if(appDataContainer.Values.ContainsKey(key))
                    {
                        appDataContainer.Values.Remove(key);
                    }

                    return;
                }

                appDataContainer.Values[key] = value;
            }
        }

        private static T PlatformGet<T>(string key, T defaultValue, string sharedName)
        {
            lock(locker)
            {
                ApplicationDataContainer appDataContainer = GetApplicationDataContainer(sharedName);
                if(appDataContainer.Values.ContainsKey(key))
                {
                    object tempValue = appDataContainer.Values[key];
                    if(tempValue != null)
                    {
                        return (T)tempValue;
                    }
                }
            }

            return defaultValue;
        }

        private static ApplicationDataContainer GetApplicationDataContainer(string sharedName)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if(string.IsNullOrWhiteSpace(sharedName))
            {
                return localSettings;
            }

            if(!localSettings.Containers.ContainsKey(sharedName))
            {
                localSettings.CreateContainer(sharedName, ApplicationDataCreateDisposition.Always);
            }

            return localSettings.Containers[sharedName];
        }
    }
}