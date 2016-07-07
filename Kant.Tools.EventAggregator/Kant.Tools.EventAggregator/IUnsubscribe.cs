using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kant.Tools.EventAggregator
{
    /// <summary>
    /// 退订接口
    /// </summary>
    public interface IUnsubscribe
    {
        /// <summary>
        /// 事件退订一个行为
        /// </summary>
        void Unsubscribe();
    }
}
