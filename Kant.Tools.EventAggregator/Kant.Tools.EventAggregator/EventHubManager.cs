using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kant.Tools.EventAggregator
{
    /// <summary>
    /// 事件管理
    /// </summary>
    public class EventHubManager
    {
        #region Constructor 

        /// <summary>
        /// 返回唯一实例
        /// </summary>
        private static readonly EventHub instance;
        public static EventHub Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// 私有构造方法
        /// </summary>
        private EventHubManager()
        {
        }

        /// <summary>
        /// 静态构造方法
        /// </summary>
        static EventHubManager()
        {
            instance = new EventHub();
        }

        #endregion
    }
}
