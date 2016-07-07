using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;

namespace Kant.Tools.EventAggregator
{
    /// <summary>
    /// 基于RX扩展的事件聚合类
    /// </summary>
    public class EventHub : IEventHub
    {
        #region Methods

        /// <summary>
        /// 获取事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <returns>要接收通知的事件</returns>
        public IObservable<TEvent> GetEvent<TEvent>()
        {
            var subject = (ISubject<TEvent>)subjects.GetOrAdd(typeof(TEvent), t => new Subject<TEvent>());

            return subject.AsObservable();
        }

        /// <summary>
        /// 通过标识获取事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="channel">事件标识</param>
        /// <returns>要接收通知的事件</returns>
        public EventWithSubscribe<TEvent> GetEventWithChannel<TEvent>(string channel)
        {
            return (EventWithSubscribe<TEvent>)subjectsWithChannel.GetOrAdd(
                   channel, t => new EventWithSubscribe<TEvent>(new Subject<TEvent>(), new BlockingCollection<SubscribeRecord<TEvent>>()));
        }

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="sampleEvent">要发布的事件对象</param>
        public void Publish<TEvent>(TEvent sampleEvent)
        {
            object subject;

            if (subjects.TryGetValue(typeof(TEvent), out subject))
            {
                ((ISubject<TEvent>)subject).OnNext(sampleEvent);
            }
        }

        /// <summary>
        /// 发布事件并赋以标识
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="channel">事件标识</param>
        /// <param name="sampleEvent">要发布的事件对象</param>
        public void PublishToChannel<TEvent>(string channel, TEvent sampleEvent)
        {
            object eventWithAction;

            if (subjectsWithChannel.TryGetValue(channel, out eventWithAction))
            {
                ((ISubject<TEvent>)((EventWithSubscribe<TEvent>)eventWithAction).ObservableEvent).OnNext(sampleEvent);
            }
        }

        #endregion

        #region Fields & Properties

        /// <summary>
        /// 线程安全的并发字典，以事件类型作为协议
        /// </summary>
        private readonly ConcurrentDictionary<Type, object> subjects = new ConcurrentDictionary<Type, object>();

        /// <summary>
        /// 以字符串标识作为协议
        /// </summary>
        private readonly ConcurrentDictionary<string, object> subjectsWithChannel = new ConcurrentDictionary<string, object>();

        #endregion
    }
}
