﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using lindexi.MVVM.Framework.Annotations;
using lindexi.uwp.Framework.ViewModel;

namespace lindexi.MVVM.Framework.ViewModel
{
    /// <summary>
    /// 处理消息
    /// </summary>
    public class Composite : IComposite
    {
        /// <inheritdoc />
        public Composite()
        {
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="message">处理的消息类型</param>
        public Composite([NotNull] Type message)
        {
            if (ReferenceEquals(message, null))
                throw new ArgumentNullException(nameof(message));
            PredicateMessage = new TypePredicateMessage(message);
        }

        /// <summary>
        /// 判断消息是否符合
        /// </summary>
        public IPredicateMessage PredicateMessage
        {
            get => _predicateMessage ?? ViewModel.PredicateMessage.Instance;
            set => _predicateMessage = value;
        }

        /// <inheritdoc />
        public virtual void Run([NotNull] IViewModel source, [NotNull] IMessage message)
        {
            if (ReferenceEquals(source, null))
                throw new ArgumentNullException(nameof(source));
            if (ReferenceEquals(message, null))
                throw new ArgumentNullException(nameof(message));
            var viewModel = source as ViewModelBase;
            if (viewModel != null)
            {
                Run(viewModel, message);
            }
        }

        /// <summary>
        /// 是否已经使用函数
        /// </summary>
        private bool _run;

        private IPredicateMessage _predicateMessage;

        /// <inheritdoc />
        public virtual void Run(ViewModelBase source, IMessage message)
        {
            if (_run)
            {
                return;
            }

            _run = true;
            Run((IViewModel) source, message);
            _run = false;
        }

        static Composite()
        {
            CompositeList = new List<Composite>()
            {
                new NavigateComposite()
            };
        }

        /// <summary>
        /// 获取现有的处理
        /// </summary>
        /// <returns></returns>
        public static List<Composite> GetCompositeList()
        {
            return CompositeList.ToList();
        }

        /// <summary>
        /// 查找可以运行的消息
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="message"></param>
        /// <param name="compositeList"></param>
        [PublicAPI]
        public static bool Run([NotNull] IViewModel viewModel, [NotNull] IMessage message,
            IEnumerable<Composite> compositeList = null)
        {
            if (ReferenceEquals(viewModel, null))
                throw new ArgumentNullException(nameof(viewModel));
            if (ReferenceEquals(message, null))
                throw new ArgumentNullException(nameof(message));
            if (!message.Predicate(viewModel))
            {
                // 如果消息不是发送到这个 ViewModel 就返回
                return false;
            }

            if (compositeList == null || !compositeList.Any())
            {
                compositeList = CompositeList;
            }

            var composite = false;
            var exceptionList = new List<Exception>();

            foreach (var temp in compositeList.Where(temp => temp.PredicateMessage.Predicate(message)))
            {
                try
                {
                    temp.Run(viewModel, message);
                }
                catch (Exception e)
                {
                    Trace.Write(e);
                    exceptionList.Add(e);
                }

                composite = true;
            }

            if (exceptionList.Any())
            {
                if (exceptionList.Count == 1)
                {
                    throw exceptionList[0];
                }

                throw new AggregateException(
                    "调用 static bool Run(IViewModel viewModel, IMessage message, IEnumerable<Composite> compositeList) 发现异常",
                    exceptionList);
            }

            return composite;
        }

        private static IReadOnlyCollection<Composite> CompositeList { get; }
    }


    /// <summary>
    /// 处理消息
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public class Composite<TMessage> : Composite where TMessage : IMessage
    {
        /// <inheritdoc />
        public Composite()
        {
            PredicateMessage = new TypePredicateMessage<TMessage>();
        }

        /// <inheritdoc />
        public sealed override void Run(IViewModel source, IMessage message)
        {
            Run(source, (TMessage) message);
        }

        /// <summary>
        /// 运行处理传入对应的消息
        /// </summary>
        /// <param name="source"></param>
        /// <param name="message"></param>
        protected virtual void Run(IViewModel source, TMessage message)
        {
        }
    }

    /// <summary>
    /// 表示一个类是处理
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CompositeAttribute : Attribute
    {
    }
}
