//===============================================================================
// TinyIoC - TinyMessenger
//
// A simple messenger/event aggregator.
// 
// http://hg.grumpydev.com/tinyioc
//===============================================================================
// Copyright © Steven Robbins.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;


namespace Harness.TinyMessenger
{
    #region Message Types / Interfaces
    /// <summary>
    /// A TinyMessage to be published/delivered by TinyMessenger
    /// </summary>
    

    /// <summary>
    /// Base class for messages that provides weak refrence storage of the sender
    /// </summary>
    public abstract class TinyMessageBase : IMessage
    {
        /// <summary>
        /// Store a WeakReference to the sender just in case anyone is daft enough to
        /// keep the message around and prevent the sender from being collected.
        /// </summary>
        private WeakReference _Sender;
        public object Sender
        {
            get
            {
                return (_Sender == null) ? null : _Sender.Target;
            }
        }

        /// <summary>
        /// Initializes a new instance of the MessageBase class.
        /// </summary>
        /// <param name="sender">Message sender (usually "this")</param>
        public TinyMessageBase(object sender)
        {
            if (sender == null)
                throw new ArgumentNullException("sender");

            _Sender = new WeakReference(sender);
        }
    }

    /// <summary>
    /// Generic message with user specified content
    /// </summary>
    /// <typeparam name="TContent">Content type to store</typeparam>
    public class GenericTinyMessage<TContent> : TinyMessageBase
    {
        /// <summary>
        /// Contents of the message
        /// </summary>
        public TContent Content { get; protected set; }

        /// <summary>
        /// Create a new instance of the GenericTinyMessage class.
        /// </summary>
        /// <param name="sender">Message sender (usually "this")</param>
        /// <param name="content">Contents of the message</param>
        public GenericTinyMessage(object sender, TContent content)
            : base(sender)
        {
            Content = content;
        }
    }

    /// <summary>
    /// Basic "cancellable" generic message
    /// </summary>
    /// <typeparam name="TContent">Content type to store</typeparam>
    public class CancellableGenericTinyMessage<TContent> : TinyMessageBase
    {
        /// <summary>
        /// Cancel action
        /// </summary>
        public Action Cancel { get; protected set; }

        /// <summary>
        /// Contents of the message
        /// </summary>
        public TContent Content { get; protected set; }

        /// <summary>
        /// Create a new instance of the CancellableGenericTinyMessage class.
        /// </summary>
        /// <param name="sender">Message sender (usually "this")</param>
        /// <param name="content">Contents of the message</param>
        /// <param name="cancelAction">Action to call for cancellation</param>
        public CancellableGenericTinyMessage(object sender, TContent content, Action cancelAction)
            : base(sender)
        {
            if (cancelAction == null)
                throw new ArgumentNullException("cancelAction");

            Content = content;
            Cancel = cancelAction;
        }
    }

    /// <summary>
    /// Represents an active subscription to a message
    /// </summary>
    public sealed class TinyMessageSubscriptionToken : IMessageSubscriptionToken
    {
        private WeakReference _Hub;
        private Type _MessageType;

        /// <summary>
        /// Initializes a new instance of the TinyMessageSubscriptionToken class.
        /// </summary>
        public TinyMessageSubscriptionToken(IMessengerHub hub, Type messageType)
        {
            if (hub == null)
                throw new ArgumentNullException("hub");

            if (!typeof(IMessage).IsAssignableFrom(messageType))
                throw new ArgumentOutOfRangeException("messageType");

            _Hub = new WeakReference(hub);
            _MessageType = messageType;
        }

        public void Dispose()
        {
            if (_Hub.IsAlive)
            {
                var hub = _Hub.Target as IMessengerHub;

                if (hub != null)
                {
                    var unsubscribeMethod = typeof(IMessengerHub).GetMethod("Unsubscribe", new Type[] {typeof(IMessageSubscriptionToken)});
                    unsubscribeMethod = unsubscribeMethod.MakeGenericMethod(_MessageType);
                    unsubscribeMethod.Invoke(hub, new object[] {this});
                }
            }

            GC.SuppressFinalize(this);
        }
    }

    
   

    /// <summary>
    /// Default "pass through" proxy.
    /// 
    /// Does nothing other than deliver the message.
    /// </summary>
    public sealed class DefaultTinyMessageProxy : IMessageProxy
    {
        private static readonly DefaultTinyMessageProxy _Instance = new DefaultTinyMessageProxy();

        static DefaultTinyMessageProxy()
        {
        }

        /// <summary>
        /// Singleton instance of the proxy.
        /// </summary>
        public static DefaultTinyMessageProxy Instance
        {
            get
            {
                return _Instance;
            }
        }

        private DefaultTinyMessageProxy()
        {
        }

        public void Deliver(IMessage message, IMessageSubscription subscription)
        {
            subscription.Deliver(message);
        }
    }
    #endregion

    #region Exceptions
    /// <summary>
    /// Thrown when an exceptions occurs while subscribing to a message type
    /// </summary>
    public class TinyMessengerSubscriptionException : Exception
    {
        private const string ERROR_TEXT = "Unable to add subscription for {0} : {1}";

        public TinyMessengerSubscriptionException(Type messageType, string reason)
            : base(String.Format(ERROR_TEXT, messageType, reason))
        {

        }

        public TinyMessengerSubscriptionException(Type messageType, string reason, Exception innerException)
            : base(String.Format(ERROR_TEXT, messageType, reason), innerException)
        {

        }
    }
    #endregion

    #region Hub Interface
    /// <summary>
    /// Messenger hub responsible for taking subscriptions/publications and delivering of messages.
    /// </summary>
    
    #endregion

    #region Hub Implementation
    /// <summary>
    /// Messenger hub responsible for taking subscriptions/publications and delivering of messages.
    /// </summary>
    public sealed class TinyMessengerHub : IMessengerHub
    {
        #region Private Types and Interfaces
        private class WeakTinyMessageSubscription<TMessage> : IMessageSubscription
            where TMessage : class, IMessage
        {
            protected IMessageSubscriptionToken _SubscriptionToken;
            protected WeakReference _DeliveryAction;
            protected WeakReference _MessageFilter;

            public IMessageSubscriptionToken SubscriptionToken
            {
                get { return _SubscriptionToken; }
            }

            public bool ShouldAttemptDelivery(IMessage message)
            {
                if (!(message is TMessage))
                    return false;

                if (!_DeliveryAction.IsAlive)
                    return false;

                if (!_MessageFilter.IsAlive)
                    return false;

                return ((Func<TMessage, bool>)_MessageFilter.Target).Invoke(message as TMessage);
            }

            public void Deliver(IMessage message)
            {
                if (!(message is TMessage))
                    throw new ArgumentException("Message is not the correct type");

                if (!_DeliveryAction.IsAlive)
                    return;

                ((Action<TMessage>)_DeliveryAction.Target).Invoke(message as TMessage);
            }

            /// <summary>
            /// Initializes a new instance of the WeakTinyMessageSubscription class.
            /// </summary>
            /// <param name="destination">Destination object</param>
            /// <param name="deliveryAction">Delivery action</param>
            /// <param name="messageFilter">Filter function</param>
            public WeakTinyMessageSubscription(TinyMessageSubscriptionToken subscriptionToken, Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter)
            {
                if (subscriptionToken == null)
                    throw new ArgumentNullException("subscriptionToken");

                if (deliveryAction == null)
                    throw new ArgumentNullException("deliveryAction");

                if (messageFilter == null)
                    throw new ArgumentNullException("messageFilter");

                _SubscriptionToken = subscriptionToken;
                _DeliveryAction = new WeakReference(deliveryAction);
                _MessageFilter = new WeakReference(messageFilter);
            }
        }

        private class StrongTinyMessageSubscription<TMessage> : IMessageSubscription
            where TMessage : class, IMessage
        {
            protected IMessageSubscriptionToken _SubscriptionToken;
            protected Action<TMessage> _DeliveryAction;
            protected Func<TMessage, bool> _MessageFilter;

            public IMessageSubscriptionToken SubscriptionToken
            {
                get { return _SubscriptionToken; }
            }

            public bool ShouldAttemptDelivery(IMessage message)
            {
                if (!(message is TMessage))
                    return false;

                return _MessageFilter.Invoke(message as TMessage);
            }

            public void Deliver(IMessage message)
            {
                if (!(message is TMessage))
                    throw new ArgumentException("Message is not the correct type");

                _DeliveryAction.Invoke(message as TMessage);
            }

            /// <summary>
            /// Initializes a new instance of the TinyMessageSubscription class.
            /// </summary>
            /// <param name="destination">Destination object</param>
            /// <param name="deliveryAction">Delivery action</param>
            /// <param name="messageFilter">Filter function</param>
            public StrongTinyMessageSubscription(IMessageSubscriptionToken subscriptionToken, Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter)
            {
                if (subscriptionToken == null)
                    throw new ArgumentNullException("subscriptionToken");

                if (deliveryAction == null)
                    throw new ArgumentNullException("deliveryAction");

                if (messageFilter == null)
                    throw new ArgumentNullException("messageFilter");

                _SubscriptionToken = subscriptionToken;
                _DeliveryAction = deliveryAction;
                _MessageFilter = messageFilter;
            }
        }
        #endregion

        #region Subscription dictionary
        private class SubscriptionItem
        {
            public IMessageProxy Proxy { get; private set; }
            public IMessageSubscription Subscription { get; private set; }

            public SubscriptionItem(IMessageProxy proxy, IMessageSubscription subscription)
            {
                Proxy = proxy;
                Subscription = subscription;
            }
        }

        private readonly object _SubscriptionsPadlock = new object();
        private readonly Dictionary<Type, List<SubscriptionItem>> _Subscriptions = new Dictionary<Type, List<SubscriptionItem>>();
        #endregion

        #region Public API
        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action.
        /// All references are held with strong references
        /// 
        /// All messages of this type will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <returns>TinyMessageSubscription used to unsubscribing</returns>
        public IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction) where TMessage : class, IMessage
        {
            return AddSubscriptionInternal<TMessage>(deliveryAction, (m) => true, true, DefaultTinyMessageProxy.Instance);
        }

        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action.
        /// Messages will be delivered via the specified proxy.
        /// All references (apart from the proxy) are held with strong references
        /// 
        /// All messages of this type will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="proxy">Proxy to use when delivering the messages</param>
        /// <returns>TinyMessageSubscription used to unsubscribing</returns>
        public IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, IMessageProxy proxy) where TMessage : class, IMessage
        {
            return AddSubscriptionInternal<TMessage>(deliveryAction, (m) => true, true, proxy);
        }

        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action.
        /// 
        /// All messages of this type will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
        /// <returns>TinyMessageSubscription used to unsubscribing</returns>
        public IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, bool useStrongReferences) where TMessage : class, IMessage
        {
            return AddSubscriptionInternal<TMessage>(deliveryAction, (m) => true, useStrongReferences, DefaultTinyMessageProxy.Instance);
        }

        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action.
        /// Messages will be delivered via the specified proxy.
        /// 
        /// All messages of this type will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
        /// <param name="proxy">Proxy to use when delivering the messages</param>
        /// <returns>TinyMessageSubscription used to unsubscribing</returns>
        public IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, bool useStrongReferences, IMessageProxy proxy) where TMessage : class, IMessage
        {
            return AddSubscriptionInternal<TMessage>(deliveryAction, (m) => true, useStrongReferences, proxy);
        }

        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action with the given filter.
        /// All references are held with WeakReferences
        /// 
        /// Only messages that "pass" the filter will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <returns>TinyMessageSubscription used to unsubscribing</returns>
        public IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter) where TMessage : class, IMessage
        {
            return AddSubscriptionInternal<TMessage>(deliveryAction, messageFilter, true, DefaultTinyMessageProxy.Instance);
        }

        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action with the given filter.
        /// Messages will be delivered via the specified proxy.
        /// All references (apart from the proxy) are held with WeakReferences
        /// 
        /// Only messages that "pass" the filter will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="proxy">Proxy to use when delivering the messages</param>
        /// <returns>TinyMessageSubscription used to unsubscribing</returns>
        public IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter, IMessageProxy proxy) where TMessage : class, IMessage
        {
            return AddSubscriptionInternal<TMessage>(deliveryAction, messageFilter, true, proxy);
        }

        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action with the given filter.
        /// All references are held with WeakReferences
        /// 
        /// Only messages that "pass" the filter will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
        /// <returns>TinyMessageSubscription used to unsubscribing</returns>
        public IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter, bool useStrongReferences) where TMessage : class, IMessage
        {
            return AddSubscriptionInternal<TMessage>(deliveryAction, messageFilter, useStrongReferences, DefaultTinyMessageProxy.Instance);
        }

        /// <summary>
        /// Subscribe to a message type with the given destination and delivery action with the given filter.
        /// Messages will be delivered via the specified proxy.
        /// All references are held with WeakReferences
        /// 
        /// Only messages that "pass" the filter will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
        /// <param name="proxy">Proxy to use when delivering the messages</param>
        /// <returns>TinyMessageSubscription used to unsubscribing</returns>
        public IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter, bool useStrongReferences, IMessageProxy proxy) where TMessage : class, IMessage
        {
            return AddSubscriptionInternal<TMessage>(deliveryAction, messageFilter, useStrongReferences, proxy);
        }

        /// <summary>
        /// Unsubscribe from a particular message type.
        /// 
        /// Does not throw an exception if the subscription is not found.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="subscriptionToken">Subscription token received from Subscribe</param>
        public void Unsubscribe<TMessage>(IMessageSubscriptionToken subscriptionToken) where TMessage : class, IMessage
        {
            RemoveSubscriptionInternal<TMessage>(subscriptionToken);
        }

        /// <summary>
        /// Publish a message to any subscribers
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="message">Message to deliver</param>
        public void Publish<TMessage>(TMessage message) where TMessage : class, IMessage
        {
            PublishInternal<TMessage>(message);
        }

        /// <summary>
        /// Publish a message to any subscribers asynchronously
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="message">Message to deliver</param>
        public void PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage
        {
            PublishAsyncInternal<TMessage>(message, null);
        }

        /// <summary>
        /// Publish a message to any subscribers asynchronously
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="message">Message to deliver</param>
        /// <param name="callback">AsyncCallback called on completion</param>
        public void PublishAsync<TMessage>(TMessage message, AsyncCallback callback) where TMessage : class, IMessage
        {
            PublishAsyncInternal<TMessage>(message, callback);
        }
        #endregion

        #region Internal Methods
        private IMessageSubscriptionToken AddSubscriptionInternal<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter, bool strongReference, IMessageProxy proxy)
                where TMessage : class, IMessage
        {
            if (deliveryAction == null)
                throw new ArgumentNullException("deliveryAction");

            if (messageFilter == null)
                throw new ArgumentNullException("messageFilter");

            if (proxy == null)
                throw new ArgumentNullException("proxy");

            lock (_SubscriptionsPadlock)
            {
                List<SubscriptionItem> currentSubscriptions;

                if (!_Subscriptions.TryGetValue(typeof(TMessage), out currentSubscriptions))
                {
                    currentSubscriptions = new List<SubscriptionItem>();
                    _Subscriptions[typeof(TMessage)] = currentSubscriptions;
                }

                var subscriptionToken = new TinyMessageSubscriptionToken(this, typeof(TMessage));

                IMessageSubscription subscription;
                if (strongReference)
                    subscription = new StrongTinyMessageSubscription<TMessage>(subscriptionToken, deliveryAction, messageFilter);
                else
                    subscription = new WeakTinyMessageSubscription<TMessage>(subscriptionToken, deliveryAction, messageFilter);

                currentSubscriptions.Add(new SubscriptionItem(proxy, subscription));

                return subscriptionToken;
            }
        }

        private void RemoveSubscriptionInternal<TMessage>(IMessageSubscriptionToken subscriptionToken)
                where TMessage : class, IMessage
        {
            if (subscriptionToken == null)
                throw new ArgumentNullException("subscriptionToken");

            lock (_SubscriptionsPadlock)
            {
                List<SubscriptionItem> currentSubscriptions;
                if (!_Subscriptions.TryGetValue(typeof(TMessage), out currentSubscriptions))
                    return;

                var currentlySubscribed = (from sub in currentSubscriptions
                                           where object.ReferenceEquals(sub.Subscription.SubscriptionToken, subscriptionToken)
                                           select sub).ToList();

                currentlySubscribed.Each(sub => currentSubscriptions.Remove(sub));
            }
        }

        private void PublishInternal<TMessage>(TMessage message)
                where TMessage : class, IMessage
        {
            if (message == null)
                throw new ArgumentNullException("message");

            List<SubscriptionItem> currentlySubscribed;
            lock (_SubscriptionsPadlock)
            {
                List<SubscriptionItem> currentSubscriptions;
                if (!_Subscriptions.TryGetValue(typeof(TMessage), out currentSubscriptions))
                    return;

                currentlySubscribed = (from sub in currentSubscriptions
                                       where sub.Subscription.ShouldAttemptDelivery(message)
                                       select sub).ToList();
            }

            currentlySubscribed.Each(sub =>
            {
                try
                {
                    sub.Proxy.Deliver(message, sub.Subscription);
                }
                catch (Exception)
                {
                    // Ignore any errors and carry on
                    // TODO - add to a list of erroring subs and remove them?
                }
            });
        }

        private void PublishAsyncInternal<TMessage>(TMessage message, AsyncCallback callback) where TMessage : class, IMessage
        {
            Action publishAction = () => PublishInternal<TMessage>(message);

            publishAction.BeginInvoke(callback, null);
        }
        #endregion
    }
    #endregion
}
