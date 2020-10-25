﻿// lindexi
// 20:54

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lindexi.Imageshack.Smms;
using smms.Model;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace smms
{
    public class ViewModel : NotifyProperty
    {
        public ViewModel()
        {
        }

        private BitmapImage _bitmap;

        public BitmapImage Bitmap
        {
            set
            {
                _bitmap = value;
                OnPropertyChanged();
            }
            get
            {
                return _bitmap;
            }
        }

        public async void FileDataPackageView(DataPackageView dataView)
        {
            if (dataView.Contains(StandardDataFormats.StorageItems))
            {
                var files = await dataView.GetStorageItemsAsync();
                StorageFile file = files.OfType<StorageFile>().First();
                if (file.FileType == ".png" || file.FileType == ".jpg"
                    || file.FileType == ".gif")
                {
                    // 拖放的是图片文件。
                    BitmapImage bitmap = new BitmapImage();
                    await bitmap.SetSourceAsync(await file.OpenAsync(FileAccessMode.Read));
                    Bitmap = bitmap;
                    File = file;
                }
            }
        }

        public string Reminder
        {
            set
            {
                _reminder = value;
                OnPropertyChanged();
            }
            get
            {
                return _reminder;
            }
        }

        public StorageFile File
        {
            set;
            get;
        }

        public async void UpLoad()
        {
            //判断文件不存在
            if (File == null)
            {
                FileOpenPicker picker = new FileOpenPicker()
                {
                    FileTypeFilter =
                    {
                        ".png",
                        ".jpg"
                    }
                };

                File = await picker.PickSingleFileAsync();
            }

            Imageshack imageshack = new Imageshack()
            {
                File = File,
            };
            //上传完成事件，其中str为sm.ms返回，一般为json
            //Reminder是例子，可以根据具体修改，注意要同步CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
            imageshack.OnUploadedEventHandler += (sender, str) => Reminder = str.Replace("\\/", "/");
            imageshack.UpLoad();

            //var smmsImageshack = new SmmsImageshack(File);
            //var smms =await smmsImageshack.UploadImage();
        }

        private string _reminder;

        /// <summary>
        ///     文件存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns>true 文件不存在 False文件存在</returns>
        private async Task<bool> exist(string name)
        {
            try
            {
                var file = await ApplicationData.Current.
                    LocalFolder.GetFileAsync(name);
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            return true;
        }

        private async Task<StorageFile> Write(string name, string str)
        {
            StorageFile file = await ApplicationData.Current.
                LocalFolder.CreateFileAsync(name, CreationCollisionOption.OpenIfExists);
            using (Stream stream = await file.OpenStreamForWriteAsync())
            {
                byte[] buffer = Encoding.ASCII.GetBytes(str);
                stream.Write(buffer, 0, buffer.Length);
            }
            return file;
        }

        /// <summary>
        ///     随机字符串
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private string RanStr(int n)
        {
            Random ran = new Random();
            return RanStr(n, ran);
        }

        private string RanStr(int n, Random ran)
        {
            StringBuilder str = new StringBuilder();
            int[] chinesecharacters = new int[2]
            {
                19968, 40895
            };
            for (int i = 0; i < n; i++)
            {
                str.Append(Convert.ToChar(ran.Next(chinesecharacters[0], chinesecharacters[1])));
            }
            return str.ToString();
        }
    }
}
