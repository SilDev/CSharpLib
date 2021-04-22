#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: WebClientEx.cs
// Version:  2021-04-22 19:46
// 
// Copyright (c) 2021, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Network
{
    using System;
    using System.Net;

    /// <summary>
    ///     Provides common methods for sending data to and receiving data from a
    ///     resource identified by a URI.
    /// </summary>
    public class WebClientEx : WebClient
    {
        /// <summary>
        ///     Gets or sets a value that indicates whether the request should follow
        ///     redirection responses.
        /// </summary>
        public bool AllowAutoRedirect { get; set; }

        /// <summary>
        ///     Gets or sets the cookies associated with the request.
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        ///     Gets or sets the time-out value in milliseconds for the
        ///     <see cref="HttpWebRequest.GetResponse()"/> and
        ///     <see cref="HttpWebRequest.GetRequestStream()"/> methods.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebClientEx"/> class.
        /// </summary>
        /// <param name="allowAutoRedirect">
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds for the
        ///     <see cref="HttpWebRequest.GetResponse()"/> and
        ///     <see cref="HttpWebRequest.GetRequestStream()"/> methods.
        /// </param>
        public WebClientEx(bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 60000)
        {
            NetEx.EnsureDefaultSecurityProtocol();
            AllowAutoRedirect = allowAutoRedirect;
            CookieContainer = cookieContainer;
            Timeout = timeout;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebClientEx"/> class.
        /// </summary>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds for the
        ///     <see cref="HttpWebRequest.GetResponse()"/> and
        ///     <see cref="HttpWebRequest.GetRequestStream()"/> methods.
        /// </param>
        public WebClientEx(CookieContainer cookieContainer, int timeout = 60000) : this(true, cookieContainer, timeout) { }

        /// <summary>
        ///     Initializes a new instance of the <see cref="WebClientEx"/> class.
        /// </summary>
        /// <param name="timeout">
        ///     The time-out value in milliseconds for the
        ///     <see cref="HttpWebRequest.GetResponse()"/> and
        ///     <see cref="HttpWebRequest.GetRequestStream()"/> methods.
        /// </param>
        public WebClientEx(int timeout) : this(true, null, timeout) { }

        /// <summary>
        ///     Returns a <see cref="WebRequest"/> object for the specified resource.
        /// </summary>
        /// <param name="address">
        ///     A <see cref="Uri"/> that identifies the resource to request.
        /// </param>
        protected override WebRequest GetWebRequest(Uri address)
        {
            if (base.GetWebRequest(address) is not HttpWebRequest request)
                return null;
            request.AllowAutoRedirect = AllowAutoRedirect;
            if (CookieContainer != null)
                request.CookieContainer = CookieContainer;
            if (Timeout >= 0)
                request.Timeout = Timeout;
            return request;
        }
    }
}
