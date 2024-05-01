using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StockReview.Client.ContentModule.ViewModels
{
    public class UserManagementViewModel : NavigationAwareViewModelBase
    {
        public UserManagementViewModel(IUnityContainer unityContainer,
                                       IRegionManager regionManager) : base(unityContainer, regionManager)
        {
            this.PageTitle = "系统用户管理";
        }
    }
}
