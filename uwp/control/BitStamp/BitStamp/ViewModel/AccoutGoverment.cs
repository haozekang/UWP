﻿// lindexi
// 10:47

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Xaml;

namespace BitStamp.ViewModel
{
    public class AccoutGoverment
    {
        public AccoutGoverment()
        {
            Account = new Account();
            //Read();
            //Application.Current.Suspending += async (s, e) =>
            //{
            //    var deferral = e.SuspendingOperation.GetDeferral();
            //    await Storage();
            //    deferral.Complete();
            //};
        }

        public EventHandler OnReadEventHandler { set; get; }


        public Account Account { set; get; }

        public static AccoutGoverment AccountModel
        {
            set { _accountModel = value; }
            get { return _accountModel ?? (_accountModel = new AccoutGoverment()); }
        }

        public async Task Storage()
        {
            string folderStr = "account";
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            try
            {
                folder = await folder.GetFolderAsync(folderStr);
            }
            catch (FileNotFoundException)
            {
                folder = await folder.CreateFolderAsync(folderStr);
            }

            StorageFile file = await folder.CreateFileAsync(
                folderStr + ".json", CreationCollisionOption.ReplaceExisting);
            var json = JsonSerializer.Create();

            //json.Serialize(new JsonTextWriter(
            //    new StreamWriter(await file.OpenStreamForWriteAsync())), Account);

            StringBuilder str = new StringBuilder();
            StringWriter stream = new StringWriter(str);
            json.Serialize(new JsonTextWriter(
                stream), Account);
            using (StorageStreamTransaction transaction = await file.OpenTransactedWriteAsync())
            {
                using (DataWriter dataWriter = new DataWriter(transaction.Stream))
                {
                    dataWriter.WriteString(str.ToString());
                    transaction.Stream.Size = await dataWriter.StoreAsync();
                    await transaction.CommitAsync();
                }
            }
        }

        public async Task Read()
        {
            await Task.Delay(100);
            string folderStr = "account";
            StorageFolder folder = ApplicationData.Current.LocalFolder;
            bool first = false;
            try
            {
                folder = await folder.GetFolderAsync(folderStr);
            }
            catch (FileNotFoundException)
            {
                first = true;
            }

            if (!first)
            {
                try
                {
                    StorageFile file = await folder.GetFileAsync(folderStr + ".json");
                    var dwrotSvwm = await FileIO.ReadTextAsync(file);
                    Account = JsonConvert.DeserializeObject<Account>(dwrotSvwm);
                    if (Account != null)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(Account.Token))
                            {
                                Account.Folder = await
                                    StorageApplicationPermissions.FutureAccessList.
                                        GetFolderAsync(
                                            Account.Token);
                                Account.Address = Account.Folder.Path;
                            }
                            else
                            {
                                Account.Folder = KnownFolders.PicturesLibrary;
                                Account.Address = Account.Folder.Path;
                            }
                        }
                        catch (UnauthorizedAccessException)
                        {
                            Account.Folder = ApplicationData.Current.TemporaryFolder;
                        }
                    }
                    else
                    {
                        first = true;
                    }
                }
                catch (FileNotFoundException)
                {
                    first = true;
                }
            }
            //else
            //{
            //    folder = await ApplicationData.Current.LocalFolder.
            //        CreateFolderAsync(folderStr);
            //}

            if (first)
            {
                Account = new Account()
                {
                    Folder = KnownFolders.PicturesLibrary,
                    Address = KnownFolders.PicturesLibrary.Path,
                    Theme = ElementTheme.Light,
                    ThemeDay = true,
                    Str = "",
                    ImageShack = ImageShackEnum.Smms,
                    JiuYouImageShack = false,
                    QinImageShack = true,
                    SmmsImageShack = false,
                };

                Account.Token = StorageApplicationPermissions.FutureAccessList.Add(Account.Folder);

                await Storage();
            }

            OnReadEventHandler?.Invoke(this, null);
        }



        private static AccoutGoverment _accountModel;
    }
}
