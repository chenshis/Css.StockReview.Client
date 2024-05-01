using Microsoft.Extensions.Caching.Memory;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using StockReview.Domain.Entities;
using StockReview.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StockReview.Client.ViewModels
{
    /// <summary>
    /// 菜单树
    /// </summary>
    public class TreeMenuViewModel
    {
        public List<MenuItemViewModel> Menus { get; set; } = new List<MenuItemViewModel>();

        private readonly IRegionManager _regionManager;
        private readonly IMemoryCache _memoryCache;

        public TreeMenuViewModel(IRegionManager regionManager, IMemoryCache memoryCache)
        {
            _regionManager = regionManager;
            _memoryCache = memoryCache;
            Init();
        }

        private void Init()
        {
            var menuEntities = _memoryCache.Get<List<MenuEntity>>(SystemConstant.TreeMenuView);
            if (menuEntities != null && menuEntities.Count > 0)
            {
                FillMenus(menuEntities, 0);
            }
        }


        /// <summary>
        /// 递归
        /// </summary>
        /// <param name="origMenus"></param>
        /// <param name="parentId"></param>
        private void FillMenus(List<MenuEntity> origMenus, int parentId)
        {
            var sub = origMenus.Where(m => m.ParentId == parentId).OrderBy(o => o.Index);
            if (sub.Count() > 0)
            {
                foreach (var item in sub)
                {
                    var menuItem = new MenuItemViewModel(_regionManager)
                    {
                        MenuHeader = item.MenuHeader,
                        MenuIcon = item.MenuIcon,
                        TargetView = item.TargetView
                    };
                    Menus.Add(menuItem);
                    menuItem.Children = new List<MenuItemViewModel>();
                    FillMenus(origMenus, item.MenuId);
                }
            }
        }
    }

    /// <summary>
    /// 菜单项模型
    /// </summary>
    public class MenuItemViewModel : BindableBase
    {
        /// <summary>
        /// 区域管理
        /// </summary>
        private readonly IRegionManager _regionManager;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="regionManager"></param>
        public MenuItemViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string MenuIcon { get; set; }
        /// <summary>
        /// 菜单头部信息
        /// </summary>
        public string MenuHeader { get; set; }

        /// <summary>
        /// 目标视图
        /// </summary>
        public string TargetView { get; set; }

        private bool _isExpanded;
        /// <summary>
        /// 是否伸展
        /// </summary>
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetProperty(ref _isExpanded, value); }
        }

        private bool _isSelected;
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected
        {
            get { return _isSelected; }
            set { SetProperty(ref _isSelected, value); }
        }

        /// <summary>
        /// 子项集合
        /// </summary>
        public List<MenuItemViewModel> Children { get; set; }

        /// <summary>
        /// 打开视图命令
        /// </summary>
        public ICommand OpenViewCommand
        {
            get => new DelegateCommand(() =>
            {
                if ((this.Children == null || this.Children.Count == 0)
                    && !string.IsNullOrEmpty(this.TargetView))
                {
                    this.IsSelected = true;
                    // 页面跳转
                    _regionManager.RequestNavigate(SystemConstant.MainContentRegion, TargetView);
                }
                else
                {
                    this.IsExpanded = !this.IsExpanded;
                    this.IsSelected = false;
                }
            });
        }
    }
}
