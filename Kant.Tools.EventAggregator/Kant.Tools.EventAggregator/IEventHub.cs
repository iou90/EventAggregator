using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kant.Tools.EventAggregator
{
    /// <summary>
    /// 事件聚合接口，事件发布与获取
    /// </summary>
    public interface IEventHub
    {
        /// <summary>
        /// 发布任意类型的事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <param name="sampleEvent">事件对象</param>
        void Publish<TEvent>(TEvent sampleEvent);

        /// <summary>
        /// 获取事件
        /// </summary>
        /// <typeparam name="TEvent">事件类型</typeparam>
        /// <returns>事件对象</returns>
        IObservable<TEvent> GetEvent<TEvent>();
    }
}
