﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VarietyHiggstGushed.Annotations;
using VarietyHiggstGushed.Model;

namespace VarietyHiggstGushed.ViewModel
{
    public class StorageModel : INotifyPropertyChanged
    {
        private int _pinkieDuchesneGeraldo = 1;
        private Property _carloPiperIsaacProperty;

        public StorageModel()
        {
            JwStorage = AccountGoverment.JwAccountGoverment.JwStorage;
            MerilynPinkieDuchesneGeraldo();
            PinkieDuchesneGeraldo--;
        }

        //public AmeriStorage AmeriStorage
        //{
        //    set;
        //    get;
        //}

        public JwStorage JwStorage { get; set; }

        //private async void Read()
        //{
        //    await AccountGoverment.JwAccountGoverment.Read();
        //}

        public ObservableCollection<Property> PropertyStorage
        {
            set;
            get;
        } = new ObservableCollection<Property>();

        public int LansheehyBrunaSharon
        {
            get { return _lansheehyBrunaSharon; }
            set { _lansheehyBrunaSharon = value; OnPropertyChanged(); }
        }

        public void NewLansheehyBrunaSharon()
        {
            //买入
            if (CarloPiperIsaacProperty == null)
            {
                LansheehyBrunaSharon = 0;
                return;
            }
            //判断最大可以买入
            //价钱
            var s = (int) Math.Floor(JwStorage.TranStoragePrice / CarloPiperIsaacProperty.Price);
            if (LansheehyBrunaSharon > s)
            {
                LansheehyBrunaSharon = s;
            }

            JwStorage.NewProperty(CarloPiperIsaacProperty, LansheehyBrunaSharon);
            LansheehyBrunaSharon = 0;
        }

        public void AimeeLansheehyBrunaSharon()
        {
            JwStorage.TisProperty(CarloPiperIsaacProperty, LansheehyBrunaSharon);
            LansheehyBrunaSharon = 0;
        }

        public void NewTransit()
        {
            if (JwStorage.TranStoragePrice > 50)
            {
                JwStorage.TranStoragePrice -= 50;
                JwStorage.TransitStorage++;
            }
        }

        /// <summary>
        /// 天数
        /// </summary>
        public int PinkieDuchesneGeraldo
        {
            get { return _pinkieDuchesneGeraldo; }
            set { _pinkieDuchesneGeraldo = value; OnPropertyChanged(); }
        }

        public Property CarloPiperIsaacProperty
        {
            get { return _carloPiperIsaacProperty; }
            set
            {
                _carloPiperIsaacProperty = value;
                //LansheehyBrunaSharon = _carloPiperIsaacProperty.Num;
                OnPropertyChanged();
            }
        }

        public void MerilynPinkieDuchesneGeraldo()
        {
            PropertyStorage.Clear();
            foreach (var temp in JwStorage.PropertyStorage)
            {
                temp.Price = _random.Next(80, 120) * temp.Value / 100;
                if (_random.Next(JwStorage.PropertyStorage.Count) < JwStorage.PropertyStorage.Count / 2 - PropertyStorage.Count)
                {
                    PropertyStorage.Add(temp);
                }
            }
            //下天
         

            PinkieDuchesneGeraldo++;
        }

        private Random _random = new Random();
        private int _lansheehyBrunaSharon;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}