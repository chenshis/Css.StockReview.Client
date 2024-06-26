﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Api.Dtos
{
    public class DragonTigerDto
    {
        public List<DragonTigerGetInfo> DragonTigerGetInfos { get; set; } = new List<DragonTigerGetInfo>();
        public List<DragonTigerGetInfo> DragonTigerGetInfosOne { get; set; } = new List<DragonTigerGetInfo>();
        public List<DragonTigerGetInfo> DragonTigerGetInfosTwo { get; set; } = new List<DragonTigerGetInfo>();
        public List<DragonTigerGetInfo> DragonTigerGetInfosThree { get; set; } = new List<DragonTigerGetInfo>();
        public List<DragonTigerGetInfo> DragonTigerGetInfosFous { get; set; } = new List<DragonTigerGetInfo>();
        public List<SpeculatvieGroupsInfo> SpeculatvieGroups { get; set; } = new List<SpeculatvieGroupsInfo>();
        public DateInfo DateInfo { get; set; } = new DateInfo();
    }

    public class DateInfo
    {
        public string DateOne { get; set; }
        public string DateTwo { get; set; }
        public string DateThree { get; set; }
        public string DateFour { get; set; }
    }

    public class SpeculatvieGroupsInfo 
    {
        private string _name;
        private bool _isChecked;

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                if (_isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class DragonTigerGetInfo
    {
        /// <summary>
        /// 股票名称
        /// </summary>
        public string DragonName { get; set; }
        /// <summary>
        /// 游资
        /// </summary>
        public string DragonSpeculative { get; set; }
        /// <summary>
        /// 涨幅
        /// </summary>
        public string DragonIncrease { get; set; }

        /// <summary>
        /// 涨幅 颜色
        /// </summary>
        public string DragonIncreaseColor { get; set; }
        /// <summary>
        /// 净买
        /// </summary>
        public string DragonPurchase { get; set; }
         /// <summary>
        /// 净买 颜色
        /// </summary>
        public string DragonPurchaseColor { get; set; }
        /// <summary>
        /// 板
        /// </summary>
        public string DragonPlate { get; set; }
    }
}
