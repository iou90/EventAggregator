using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;

namespace Kant.Tools.EventAggregator
{
    /// <summary>
    /// 带有订阅集合的事件
    /// </summary>
    /// <typeparam name="TEvent">事件类型</typeparam>
    public class EventWithSubscribe<TEvent>
    {
        #region Constructor

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="e">要接收通知的事件</param>
        /// <param name="subscribes">订阅集合</param>
        public EventWithSubscribe(IObservable<TEvent> e, BlockingCollection<SubscribeRecord<TEvent>> subscribes)
        {
            ObservableEvent = e;
            subscribeCollection = subscribes;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 限定一个行为仅能订阅一次相同的事件
        /// </summary>
        /// <param name="action">要订阅在事件中的行为</param>
        /// <returns>订阅记录</returns>
        public SubscribeRecord<TEvent> SingleSubscribe(Action<TEvent> action)
        {
            // 先检查行为是否已经被订阅
            var subscribeRecordValue = subscribeCollection.Where<SubscribeRecord<TEvent>>(a => a.Action.Target.ToString() == action.Target.ToString() && a.Action.Method.ToString() == action.Method.ToString()).FirstOrDefault();

            if (subscribeRecordValue == null)
            {
                var subscribe = ObservableEvent.Subscribe(action);
                subscribeRecordValue = new SubscribeRecord<TEvent>(action, subscribe, RemoveSubcribeRecord);
                subscribeCollection.Add(subscribeRecordValue);

                return subscribeRecordValue;
            }
            else
            {
                return subscribeRecordValue;
            } // 若行为已订阅在事件内，则直接返回之前的订阅，避免再次订阅
        }

        /// <summary>
        /// 订阅，一个行为可多次订阅相同事件
        /// </summary>
        /// <param name="action">要订阅在事件中的行为</param>
        /// <returns>订阅记录</returns>
        public SubscribeRecord<TEvent> Subscribe(Action<TEvent> action)
        {
            var subscribe = ObservableEvent.Subscribe(action);
            var subscribeRecordValue = new SubscribeRecord<TEvent>(action, subscribe, RemoveSubcribeRecord);
            subscribeCollection.Add(subscribeRecordValue);

            return subscribeRecordValue;
        }

        /// <summary>
        /// 一次性订阅，事件触发时就自动退订
        /// </summary>
        /// <param name="action">要订阅在事件中的行为</param>
        public void SubscribeOnce(Action<TEvent> action)
        {
            SubscribeRecord<TEvent> subscribeRecordValue = null;

            IDisposable subscribe = ObservableEvent.Subscribe(t =>
            {
                action(t);
                RemoveSubcribeRecord(subscribeRecordValue);
            });

            subscribeRecordValue = new SubscribeRecord<TEvent>(action, subscribe, RemoveSubcribeRecord);
            subscribeCollection.Add(subscribeRecordValue);
        }

        /// <summary>
        /// 删除订阅记录
        /// </summary>
        /// <param name="subscribeRecord">要删除的订阅记录</param>
        private void RemoveSubcribeRecord(SubscribeRecord<TEvent> subscribeRecord)
        {
            if (subscribeRecord != null)
            {
                subscribeRecord.Subscribe.Dispose();
                subscribeCollection.TryTake(out subscribeRecord);
            }
        }

        #endregion

        #region Fields & Properties

        /// <summary>
        /// 要接收通知的事件
        /// </summary>
        public IObservable<TEvent> ObservableEvent { get; set; }

        /// <summary>
        /// 订阅集合
        /// </summary>
        private BlockingCollection<SubscribeRecord<TEvent>> subscribeCollection;

        #endregion
    }
}
