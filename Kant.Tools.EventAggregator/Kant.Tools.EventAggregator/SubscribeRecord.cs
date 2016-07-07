using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kant.Tools.EventAggregator
{
    /// <summary>
    /// 订阅记录
    /// </summary>
    /// <typeparam name="TEvent">被订阅的事件类型</typeparam>
    public class SubscribeRecord<TEvent> : IUnsubscribe
    {
        #region Construtor

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="action">订阅的行为委托</param>
        /// <param name="subscribe">订阅值</param>
        /// <param name="suicideCommand">删除委托</param>
        public SubscribeRecord(Action<TEvent> action, IDisposable subscribe, Action<SubscribeRecord<TEvent>> suicideCommand)
        {
            Action = action;
            Subscribe = subscribe;
            suicide = suicideCommand;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 退订，删除自定记录
        /// </summary>
        public void Unsubscribe()
        {
            suicide(this);
        }

        #endregion

        #region Fields & Properties

        /// <summary>
        /// 订阅的行为委托
        /// </summary>
        public Action<TEvent> Action { get; private set; }

        /// <summary>
        /// 订阅值
        /// </summary>
        public IDisposable Subscribe { get; set; }

        /// <summary>
        /// 删除自身记录的行为委托
        /// </summary>
        private Action<SubscribeRecord<TEvent>> suicide;

        #endregion
    }
}
