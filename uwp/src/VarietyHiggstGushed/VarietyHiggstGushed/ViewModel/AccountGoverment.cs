﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VarietyHiggstGushed.Model;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;

namespace VarietyHiggstGushed.ViewModel
{
    public class AccountGoverment
    {
        public AccountGoverment()
        {
            JwAccountGoverment = this;
        }

        private static AccountGoverment _accountGoverment;

        public Account Account { set; get; }

        public JwStorage JwStorage { get; set; } = new JwStorage();

        public static AccountGoverment JwAccountGoverment
        {
            set => _accountGoverment = value;
            get => _accountGoverment ?? (_accountGoverment = new AccountGoverment());
        }

        ///// <summary>
        ///// 第一次使用
        ///// </summary>
        //public bool TtjeqbikiFbr { get; set; } = true;

        public async Task Read()
        {
            //如果重新开始
            JwStorage = new JwStorage
            {
                TranStoragePrice = 100,
                TransitStorage = 100
            };

            var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(
                "ms-appx:///PropertyStorage.txt"));
            var str = (await FileIO.ReadTextAsync(file)).Split('\n');
            var propertyStorage = new List<Property>();
            for (var i = 0; i < str.Length; i++)
            {
                try
                {
                    propertyStorage.Add(new Property(str[i], str[i + 1]));
                    i++;
                }
                catch (FormatException e)
                {
                    e.Data.Add("1", str[i]);
                    e.Data.Add("num", str[i + 1]);
                    throw e;
                }
            }

            propertyStorage.Sort((a, b) => a.Value.CompareTo(b.Value));

            await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                () =>
                {
                    JwStorage.PropertyStorage.Clear();
                    foreach (var temp in propertyStorage)
                    {
                        JwStorage.PropertyStorage.Add(new WqmnygDcxwptivk(temp));
                    }
                });

            Account = new Account();
        }

        public async Task<bool> ReadJwStorage()
        {
            await RrhkpWjwyAccount();
            //读取存档
            var str = "JwStorage";

            var folder = ApplicationData.Current.RoamingFolder;
            try
            {
                var file = await folder.GetFileAsync(str);
                str = await FileIO.ReadTextAsync(file);
                JwStorage = JsonConvert.DeserializeObject<JwStorage>(str);
            }
            catch (FileNotFoundException)
            {
                return false;
            }

            return JwStorage != null;
        }

        public async Task HscurqtacabfgzAccount()
        {
            var str = nameof(Account);
            var folder = ApplicationData.Current.RoamingFolder;
            var file = await folder.CreateFileAsync(str, CreationCollisionOption.ReplaceExisting);
            str = JsonConvert.SerializeObject(Account);
            await FileIO.WriteTextAsync(file, str);
        }

        public async Task Storage()
        {
            await HscurqtacabfgzAccount();
            var str = "JwStorage";
            var folder = ApplicationData.Current.RoamingFolder;
            var file = await folder.CreateFileAsync(str, CreationCollisionOption.ReplaceExisting);
            str = JsonConvert.SerializeObject(JwStorage);
            await FileIO.WriteTextAsync(file, str);
        }

        private async Task RrhkpWjwyAccount()
        {
            var str = nameof(Account);
            var folder = ApplicationData.Current.RoamingFolder;
            try
            {
                var file = await folder.GetFileAsync(str);
                str = await FileIO.ReadTextAsync(file);
                Account = JsonConvert.DeserializeObject<Account>(str);
            }
            catch (FileNotFoundException)
            {
                Account = new Account();
            }

            if (Account == null)
            {
                Account = new Account();
            }
        }
    }
}
