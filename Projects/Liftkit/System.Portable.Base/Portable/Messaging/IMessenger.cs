#region ApacheLicense

// System.Portable.Base
// Copyright © 2013 Nick Daniels et all, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing 	terms.
// 
// You may not use this file except in compliance with its License(s).
// 
// You may obtain a copy of the Apache License, Version 2.0 at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 

#endregion

#region

using System.Contracts;

#endregion

namespace System.Messaging {

    #region Message Types / Interfaces

    /// <summary>
    ///     A Message to be published/delivered by Messenger
    /// </summary>
    public interface IMessage {
        /// <summary>
        ///     The sender of the message, or null if not supported by the message implementation.
        /// </summary>
        object Sender { get; }
    }

    public interface IMessageSubscriptionToken : IToken {}

    /// <summary>
    ///     Represents an active subscription to a message
    /// </summary>
    /// <summary>
    ///     Represents a message subscription
    /// </summary>
    public interface IMessageSubscription {
        /// <summary>
        ///     Token returned to the subscribed to reference this subscription
        /// </summary>
        IMessageSubscriptionToken SubscriptionToken { get; }

        /// <summary>
        ///     Whether delivery should be attempted.
        /// </summary>
        /// <param name="message">Message that may potentially be delivered.</param>
        /// <returns>True - ok to send, False - should not attempt to send</returns>
        bool ShouldAttemptDelivery(IMessage message);

        /// <summary>
        ///     Deliver the message
        /// </summary>
        /// <param name="message">Message to deliver</param>
        void Deliver(IMessage message);
    }

    /// <summary>
    ///     Message proxy definition.
    ///     A message proxy can be used to intercept/alter messages and/or
    ///     marshall delivery actions onto a particular thread.
    /// </summary>
    public interface IMessageProxy {
        void Deliver(IMessage message, IMessageSubscription subscription);
    }

    #endregion

    #region Hub Interface

    /// <summary>
    ///     Messenger hub responsible for taking subscriptions/publications and delivering of messages.
    /// </summary>
    public interface IMessengerHub {
        /// <summary>
        ///     Subscribe to a message type with the given destination and delivery action.
        ///     All references are held with WeakReferences
        ///     All messages of this type will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction) where TMessage : class, IMessage;

        /// <summary>
        ///     Subscribe to a message type with the given destination and delivery action.
        ///     Messages will be delivered via the specified proxy.
        ///     All references (apart from the proxy) are held with WeakReferences
        ///     All messages of this type will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="proxy">Proxy to use when delivering the messages</param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, IMessageProxy proxy) where TMessage : class, IMessage;

        /// <summary>
        ///     Subscribe to a message type with the given destination and delivery action.
        ///     All messages of this type will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, bool useStrongReferences) where TMessage : class, IMessage;

        /// <summary>
        ///     Subscribe to a message type with the given destination and delivery action.
        ///     Messages will be delivered via the specified proxy.
        ///     All messages of this type will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
        /// <param name="proxy">Proxy to use when delivering the messages</param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, bool useStrongReferences, IMessageProxy proxy) where TMessage : class, IMessage;

        /// <summary>
        ///     Subscribe to a message type with the given destination and delivery action with the given filter.
        ///     All references are held with WeakReferences
        ///     Only messages that "pass" the filter will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter) where TMessage : class, IMessage;

        /// <summary>
        ///     Subscribe to a message type with the given destination and delivery action with the given filter.
        ///     Messages will be delivered via the specified proxy.
        ///     All references (apart from the proxy) are held with WeakReferences
        ///     Only messages that "pass" the filter will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="proxy">Proxy to use when delivering the messages</param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter, IMessageProxy proxy) where TMessage : class, IMessage;

        /// <summary>
        ///     Subscribe to a message type with the given destination and delivery action with the given filter.
        ///     All references are held with WeakReferences
        ///     Only messages that "pass" the filter will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter, bool useStrongReferences) where TMessage : class, IMessage;

        /// <summary>
        ///     Subscribe to a message type with the given destination and delivery action with the given filter.
        ///     Messages will be delivered via the specified proxy.
        ///     All references are held with WeakReferences
        ///     Only messages that "pass" the filter will be delivered.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="deliveryAction">Action to invoke when message is delivered</param>
        /// <param name="useStrongReferences">Use strong references to destination and deliveryAction </param>
        /// <param name="proxy">Proxy to use when delivering the messages</param>
        /// <returns>MessageSubscription used to unsubscribing</returns>
        IMessageSubscriptionToken Subscribe<TMessage>(Action<TMessage> deliveryAction, Func<TMessage, bool> messageFilter, bool useStrongReferences, IMessageProxy proxy) where TMessage : class, IMessage;

        /// <summary>
        ///     Unsubscribe from a particular message type.
        ///     Does not throw an exception if the subscription is not found.
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="subscriptionToken">Subscription token received from Subscribe</param>
        void Unsubscribe<TMessage>(IMessageSubscriptionToken subscriptionToken) where TMessage : class, IMessage;

        /// <summary>
        ///     Publish a message to any subscribers
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="message">Message to deliver</param>
        void Publish<TMessage>(TMessage message) where TMessage : class, IMessage;

        /// <summary>
        ///     Publish a message to any subscribers asynchronously
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="message">Message to deliver</param>
        void PublishAsync<TMessage>(TMessage message) where TMessage : class, IMessage;

        /// <summary>
        ///     Publish a message to any subscribers asynchronously
        /// </summary>
        /// <typeparam name="TMessage">Type of message</typeparam>
        /// <param name="message">Message to deliver</param>
        /// <param name="callback">AsyncCallback called on completion</param>
        void PublishAsync<TMessage>(TMessage message, AsyncCallback callback) where TMessage : class, IMessage;
    }

    #endregion
}