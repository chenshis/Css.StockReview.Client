using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockReview.Client.ContentModule.ViewModels
{
    /// <summary>
    /// 修改用户窗体视图模型
    /// </summary>
    public class ModifyUserDialogViewModel : DialogAwareViewModelBase
    {
        public override string Title { get; set; } = "用户信息编辑";
    }
}
