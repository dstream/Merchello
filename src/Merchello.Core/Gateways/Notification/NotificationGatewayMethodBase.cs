﻿using System;
using System.Collections.Generic;
using System.Linq;
using Merchello.Core.Gateways.Notification.Formatters;
using Merchello.Core.Models;
using Merchello.Core.Services;
using Umbraco.Core.Logging;

namespace Merchello.Core.Gateways.Notification
{
    /// <summary>
    /// Represents a NotificationGatewayMethodBase object
    /// </summary>
    public abstract class NotificationGatewayMethodBase : INotificationGatewayMethod
    {
        private readonly IGatewayProviderService _gatewayProviderService;
        private readonly INotificationMethod _notificationMethod;
        
        protected NotificationGatewayMethodBase(IGatewayProviderService gatewayProviderService, INotificationMethod notificationMethod)
        {
            Mandate.ParameterNotNull(gatewayProviderService, "gatewayProviderService");
            Mandate.ParameterNotNull(notificationMethod, "notificationMethod");

            _notificationMethod = notificationMethod;
            _gatewayProviderService = gatewayProviderService;
        }

        /// <summary>
        /// Creates a <see cref="INotificationMessage"/>
        /// </summary>
        /// <param name="name">A name for the message (used in Back Office UI)</param>
        /// <param name="description">A description for the message (used in Back Office UI)</param>
        /// <param name="fromAddress">The senders or "From Address"</param>
        /// <param name="recipients">A collection of recipients</param>
        /// <param name="bodyText">The body text for the message</param>
        /// <returns>A <see cref="INotificationMessage"/></returns>
        public INotificationMessage CreateNotificationMessage(string name, string description, string fromAddress, IEnumerable<string> recipients, string bodyText)
        {
            var attempt = GatewayProviderService.CreateNotificationMessageWithKey(_notificationMethod.Key, name,description, fromAddress, recipients, bodyText);

            if (attempt.Success)
            {
                _notificationMessages = null;

                return attempt.Result;
            }
            
            LogHelper.Error<NotificationGatewayMethodBase>("Failed to create and save a notification message", attempt.Exception);

            throw attempt.Exception;
        }

        /// <summary>
        /// Deletes a <see cref="INotificationMessage"/>
        /// </summary>
        /// <param name="message">The <see cref="INotificationMessage"/> to be deleted</param>
        public void DeleteNotificationMessage(INotificationMessage message)
        {
            GatewayProviderService.Delete(message);

            _notificationMessages = null;
        }

        /// <summary>
        /// Sends a <see cref="IFormattedNotificationMessage"/> given it's unique Key (Guid)
        /// </summary>
        /// <param name="messageKey">The unique key (Guid) of the <see cref="IFormattedNotificationMessage"/></param>
        public virtual bool Send(Guid messageKey)
        {
            return Send(messageKey, new DefaultNotificationFormatter());
        }

        /// <summary>
        /// Sends a <see cref="IFormattedNotificationMessage"/> given it's unique Key (Guid)
        /// </summary>
        /// <param name="messageKey">The unique key (Guid) of the <see cref="IFormattedNotificationMessage"/></param>
        /// <param name="formatter">The <see cref="INotificationFormatter"/> to use to format the message</param>
        public virtual bool Send(Guid messageKey, INotificationFormatter formatter)
        {
            var message = _gatewayProviderService.GetNotificationMessageByKey(messageKey);

            return Send(message, formatter);
        }

        /// <summary>
        /// Sends a <see cref="IFormattedNotificationMessage"/>
        /// </summary>
        /// <param name="notificationMessage">The <see cref="IFormattedNotificationMessage"/> to be sent</param>
        public virtual bool Send(INotificationMessage notificationMessage)
        {
            return Send(notificationMessage, new DefaultNotificationFormatter());
        }

        /// <summary>
        /// Sends a <see cref="IFormattedNotificationMessage"/>
        /// </summary>
        /// <param name="notificationMessage">The <see cref="IFormattedNotificationMessage"/> to be sent</param>
        /// <param name="formatter">The <see cref="INotificationFormatter"/> to use to format the message</param>
        public virtual bool Send(INotificationMessage notificationMessage, INotificationFormatter formatter)
        {
            return PerformSend(new FormattedNotificationMessage(notificationMessage, formatter)); 
        }

        /// <summary>
        /// Does the actual work of sending the <see cref="IFormattedNotificationMessage"/>
        /// </summary>
        /// <param name="message">The <see cref="IFormattedNotificationMessage"/> to be sent</param>
        public abstract bool PerformSend(IFormattedNotificationMessage message);
        

        /// <summary>
        /// Gets the <see cref="INotificationMethod"/>
        /// </summary>
        public INotificationMethod NotificationMethod 
        {
            get { return _notificationMethod; }
        }



        private IEnumerable<INotificationMessage> _notificationMessages;

        /// <summary>
        /// Gets a collection of <see cref="INotificationMessage"/>s associated with this NotificationMethod
        /// </summary>
        public IEnumerable<INotificationMessage> NotificationMessages
        {
            get {
                return _notificationMessages ??
                       (_notificationMessages =
                           GatewayProviderService.GetNotificationMessagesByMethodKey(_notificationMethod.Key));
            }
        }

        /// <summary>
        /// Gets the <see cref="IGatewayProviderService"/>
        /// </summary>
        protected IGatewayProviderService GatewayProviderService
        {
            get { return _gatewayProviderService; }
        }
    }
}