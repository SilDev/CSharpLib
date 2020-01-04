#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: NetEx.cs
// Version:  2020-01-04 14:09
// 
// Copyright (c) 2020, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.NetworkInformation;
    using Properties;

    /// <summary>
    ///     Provides functionality for the access of internet resources.
    /// </summary>
    public static class NetEx
    {
        /// <summary>
        ///     Provides options for specifying a Domain Name System provider.
        /// </summary>
        [SuppressMessage("ReSharper", "CommentTypo")]
        public enum DnsOption
        {
            /// <summary>
            ///     Partnership between Cloudflare and APNIC. Cloudflare runs one of the world’s largest,
            ///     fastest networks. APNIC is a non-profit organization managing IP address allocation
            ///     for the Asia Pacific and Oceania regions. Cloudflare had the network. APNIC had the
            ///     IP address (1.1.1.1). Both were motivated by a mission to help build a better Internet.
            /// </summary>
            Cloudflare,

            /// <summary>
            ///     A free, global DNS resolution service that you can use as an alternative to your current
            ///     DNS provider. In addition to traditional DNS over UDP or TCP, Google also provide
            ///     DNS-over-HTTPS API.
            /// </summary>
            Google
        }

        /// <summary>
        ///     Gets internal download mirrors.
        /// </summary>
        public static string[] InternalDownloadMirrors =
        {
            "https://dl.si13n7.com",
            "https://dl.si13n7.de",
            "http://dl-0.de",
            "http://dl-1.de",
            "http://dl-2.de",
            "http://dl-3.de",
            "http://dl-4.de",
            "http://dl-5.de"
        };

        private static bool _defSecurityProtocolIsEnabled;
        private static bool? _ipv4IsAvalaible, _ipv6IsAvalaible;

        /// <summary>
        ///     Determines whether the current IPv4 connection is available.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static bool IPv4IsAvalaible
        {
            get
            {
                if (!_ipv4IsAvalaible.HasValue)
                    _ipv4IsAvalaible = InternetIsAvailable();
                return (bool)_ipv4IsAvalaible;
            }
        }

        /// <summary>
        ///     Determines whether the current IPv6 connection is available.
        /// </summary>
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static bool IPv6IsAvalaible
        {
            get
            {
                if (!_ipv6IsAvalaible.HasValue)
                    _ipv6IsAvalaible = InternetIsAvailable(true);
                return (bool)_ipv6IsAvalaible;
            }
        }

        /// <summary>
        ///     Gets the last result defined in the previous call to the <see cref="Ping(Uri, int)"/> function.
        /// </summary>
        public static PingReply LastPingReply { get; private set; }

        internal static void EnsureDefaultSecurityProtocol()
        {
            if (_defSecurityProtocolIsEnabled)
                return;
            _defSecurityProtocolIsEnabled = true;
            foreach (var type in Enum.GetValues(typeof(SecurityProtocolType)).Cast<SecurityProtocolType>().Where(type => type != SecurityProtocolType.SystemDefault))
                ServicePointManager.SecurityProtocol |= type;
        }

        /// <summary>
        ///     Returns the specified Domain Name System server addresses.
        /// </summary>
        /// <param name="dnsOptions">
        ///     The Domain Name System provider to get the addresses.
        /// </param>
        public static string[][] GetDnsAddresses(DnsOption dnsOptions)
        {
            switch (dnsOptions)
            {
                case DnsOption.Google:
                    return new[]
                    {
                        new[]
                        {
                            "8.8.8.8",
                            "8.8.4.4"
                        },
                        new[]
                        {
                            "[2001:4860:4860::8888]",
                            "[2001:4860:4860::8844]"
                        }
                    };
                default:
                    return new[]
                    {
                        new[]
                        {
                            "1.1.1.1",
                            "1.0.0.1"
                        },
                        new[]
                        {
                            "[2606:4700:4700::1111]",
                            "[2606:4700:4700::1001]"
                        }
                    };
            }
        }

        /// <summary>
        ///     Gets the full host component of this <see cref="Uri"/> instance.
        /// </summary>
        /// <param name="uri">
        ///     The <see cref="Uri"/> instance.
        /// </param>
        public static string GetFullHost(this Uri uri)
        {
            try
            {
                return uri.Host.ToLowerInvariant();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return null;
        }

        /// <summary>
        ///     Gets the full host component of this URL string.
        /// </summary>
        /// <param name="url">
        ///     The URL string.
        /// </param>
        public static string GetFullHost(string url)
        {
            try
            {
                var uri = new Uri(url);
                return uri.Host.ToLowerInvariant();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return null;
        }

        /// <summary>
        ///     Gets the short host component of this <see cref="Uri"/> instance.
        /// </summary>
        /// <param name="uri">
        ///     The <see cref="Uri"/> instance.
        /// </param>
        public static string GetShortHost(this Uri uri)
        {
            var host = uri?.GetFullHost();
            if (string.IsNullOrEmpty(host))
                return null;
            if (host.Count(x => x == '.') > 1)
                host = host.Split('.').TakeLast(2).Join('.');
            return host;
        }

        /// <summary>
        ///     Gets the short host component of this URL string.
        /// </summary>
        /// <param name="url">
        ///     The URL string.
        /// </param>
        public static string GetShortHost(string url)
        {
            var host = GetFullHost(url);
            if (string.IsNullOrEmpty(host))
                return null;
            if (host.Count(x => x == '.') > 1)
                host = host.Split('.').TakeLast(2).Join('.');
            return host;
        }

        /// <summary>
        ///     Checks the current network connection.
        /// </summary>
        /// <param name="iPv6">
        ///     true to check only the IPv6 protocol; otherwise, false to check only the IPv4 protocol.
        /// </param>
        /// <param name="dnsOptions">
        ///     The DNS servers to be used for the checks.
        /// </param>
        /// <param name="maxRoundtripTime">
        ///     The maximal number of milliseconds taken to send an Internet Control Message Protocol
        ///     (ICMP) echo request and receive the corresponding ICMP echo reply message.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     maxRoundtripTime is zero -or- negative.
        /// </exception>
        public static bool InternetIsAvailable(bool iPv6 = false, DnsOption dnsOptions = DnsOption.Cloudflare, int maxRoundtripTime = 3000)
        {
            if (maxRoundtripTime <= 0)
                throw new ArgumentOutOfRangeException(nameof(maxRoundtripTime));
            EnsureDefaultSecurityProtocol();
            try
            {
                var interfaces = NetworkInterface.GetAllNetworkInterfaces();
                if (!interfaces.Any())
                    throw new ArgumentNullException(nameof(interfaces));
                if (interfaces.All(x => x.OperationalStatus != OperationalStatus.Up))
                    throw new ArgumentException(ExceptionMessages.NetworkInterfacesNotFound);
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
                return false;
            }
            var addresses = GetDnsAddresses(dnsOptions);
            var protocol = Convert.ToInt32(iPv6);
            return addresses[protocol].Select(address => Ping(address, maxRoundtripTime) < maxRoundtripTime).Any(isAvailable => !iPv6 && (_ipv4IsAvalaible = isAvailable).ToBoolean() || iPv6 && (_ipv6IsAvalaible = isAvailable).ToBoolean());
        }

        /// <summary>
        ///     Attempts to send an Internet Control Message Protocol (ICMP) echo message to the specified
        ///     computer, and receive a corresponding ICMP echo replay message from that computer and
        ///     returns the number of milliseconds taken for this task.
        /// </summary>
        /// <param name="uri">
        ///     The address of the server to call.
        /// </param>
        /// <param name="timeout">
        ///     The maximum number of milliseconds (after sending the echo message) to wait for the ICMP
        ///     echo reply message.
        /// </param>
        public static long Ping(Uri uri, int timeout = 3000)
        {
            EnsureDefaultSecurityProtocol();
            long roundtripTime = timeout;
            try
            {
                if (uri == null)
                    throw new ArgumentNullException(nameof(uri));
                using (var ping = new Ping())
                {
                    LastPingReply = ping.Send(uri.Host, timeout);
                    if (LastPingReply?.Status == IPStatus.Success)
                        roundtripTime = LastPingReply.RoundtripTime;
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return roundtripTime;
        }

        /// <summary>
        ///     Attempts to send an Internet Control Message Protocol (ICMP) echo message to the specified
        ///     computer, and receive a corresponding ICMP echo replay message from that computer and
        ///     returns the number of milliseconds taken for this task.
        /// </summary>
        /// <param name="host">
        ///     The address of the server to call.
        /// </param>
        /// <param name="timeout">
        ///     The maximum number of milliseconds (after sending the echo message) to wait for the ICMP
        ///     echo reply message.
        /// </param>
        public static long Ping(string host, int timeout = 3000) =>
            Ping(host.ToHttpUri(), timeout);

        /// <summary>
        ///     Converts this <see cref="string"/> to a <see cref="Uri"/>.
        /// </summary>
        /// <param name="str">
        ///     The <see cref="string"/> to convert.
        /// </param>
        public static Uri ToUri(this string str)
        {
            try
            {
                var u = new Uri(str);
                return u;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                return null;
            }
        }

        /// <summary>
        ///     Converts this <see cref="string"/> to a <see cref="Uri"/> with HTTP scheme.
        /// </summary>
        /// <param name="str">
        ///     The <see cref="string"/> to convert.
        /// </param>
        public static Uri ToHttpUri(this string str)
        {
            var s = PathEx.AltCombine(default(char[]), str);
            try
            {
                var u = s.ToUri();
                if (u.Scheme.EqualsEx("https", "http"))
                    return u;
                s = "http://" + u.Host + u.PathAndQuery;
                u = s.ToUri();
                return u;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                try
                {
                    if (s.Contains("://"))
                        s = s.Split("://").Last();
                    s = "http://" + s;
                    var u = s.ToUri();
                    return u;
                }
                catch (Exception exc) when (exc.IsCaught())
                {
                    Log.Write(ex);
                    return null;
                }
            }
        }

        /// <summary>
        ///     Determines the availability of the specified internet address.
        /// </summary>
        /// <param name="uri">
        ///     The address to check.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool IsValid(this Uri uri, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null)
        {
            EnsureDefaultSecurityProtocol();
            var statusCode = 500;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(uri);
                request.Method = "HEAD";
                if (!string.IsNullOrEmpty(userAgent))
                    request.UserAgent = userAgent;
                request.AllowAutoRedirect = allowAutoRedirect;
                if (cookieContainer != null)
                    request.CookieContainer = cookieContainer;
                if (timeout >= 0)
                    request.Timeout = timeout;
                using (var response = (HttpWebResponse)request.GetResponse())
                    statusCode = (int)response.StatusCode;
                if (statusCode >= 500 && statusCode <= 510)
                    throw new HttpListenerException();
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return statusCode >= 100 && statusCode < 400;
        }

        /// <summary>
        ///     Determines the availability of the specified internet address.
        /// </summary>
        /// <param name="uri">
        ///     The address to check.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool IsValid(this Uri uri, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            uri.IsValid(allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet address.
        /// </summary>
        /// <param name="uri">
        ///     The address to check.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool IsValid(this Uri uri, CookieContainer cookieContainer, int timeout = 3000, string userAgent = null) =>
            uri.IsValid(true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet address.
        /// </summary>
        /// <param name="uri">
        ///     The address to check.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool IsValid(this Uri uri, int timeout, string userAgent = null) =>
            uri.IsValid(true, null, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(this Uri srcUri, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null)
        {
            EnsureDefaultSecurityProtocol();
            long contentLength = 0;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(srcUri);
                if (!string.IsNullOrEmpty(userAgent))
                    request.UserAgent = userAgent;
                request.AllowAutoRedirect = allowAutoRedirect;
                if (cookieContainer != null)
                    request.CookieContainer = cookieContainer;
                if (timeout >= 0)
                    request.Timeout = timeout;
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                    request.Credentials = new NetworkCredential(userName, password);
                using (var response = (HttpWebResponse)request.GetResponse())
                    contentLength = response.ContentLength;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return contentLength > 0;
        }

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(this Uri srcUri, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.FileIsAvailable(userName, password, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(this Uri srcUri, string userName, string password, CookieContainer cookieContainer, int timeout = 3000, string userAgent = null) =>
            srcUri.FileIsAvailable(userName, password, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(this Uri srcUri, string userName, string password, int timeout, string userAgent = null) =>
            srcUri.FileIsAvailable(userName, password, true, null, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(this Uri srcUri, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null) =>
            srcUri.FileIsAvailable(null, null, allowAutoRedirect, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(this Uri srcUri, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.FileIsAvailable(null, null, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(this Uri srcUri, CookieContainer cookieContainer, int timeout, string userAgent = null) =>
            srcUri.FileIsAvailable(null, null, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(this Uri srcUri, int timeout, string userAgent = null) =>
            srcUri.FileIsAvailable(null, null, true, null, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(string srcUri, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().FileIsAvailable(userName, password, allowAutoRedirect, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(string srcUri, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().FileIsAvailable(userName, password, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(string srcUri, string userName, string password, CookieContainer cookieContainer, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().FileIsAvailable(userName, password, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(string srcUri, string userName, string password, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().FileIsAvailable(userName, password, true, null, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(string srcUri, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().FileIsAvailable(null, null, allowAutoRedirect, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(string srcUri, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().FileIsAvailable(null, null, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(string srcUri, CookieContainer cookieContainer, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().FileIsAvailable(null, null, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Determines the availability of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static bool FileIsAvailable(string srcUri, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().FileIsAvailable(null, null, true, null, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(this Uri srcUri, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null)
        {
            EnsureDefaultSecurityProtocol();
            var lastModified = DateTime.Now;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(srcUri);
                if (!string.IsNullOrEmpty(userAgent))
                    request.UserAgent = userAgent;
                request.AllowAutoRedirect = allowAutoRedirect;
                if (cookieContainer != null)
                    request.CookieContainer = cookieContainer;
                if (timeout >= 0)
                    request.Timeout = timeout;
                if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                    request.Credentials = new NetworkCredential(userName, password);
                using (var response = (HttpWebResponse)request.GetResponse())
                    lastModified = response.LastModified;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return lastModified;
        }

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(this Uri srcUri, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.GetFileDate(userName, password, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(this Uri srcUri, string userName, string password, CookieContainer cookieContainer, int timeout = 3000, string userAgent = null) =>
            srcUri.GetFileDate(userName, password, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(this Uri srcUri, string userName, string password, int timeout, string userAgent = null) =>
            srcUri.GetFileDate(userName, password, true, null, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(this Uri srcUri, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null) =>
            srcUri.GetFileDate(null, null, allowAutoRedirect, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(this Uri srcUri, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.GetFileDate(null, null, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(this Uri srcUri, CookieContainer cookieContainer, int timeout, string userAgent = null) =>
            srcUri.GetFileDate(null, null, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(this Uri srcUri, int timeout, string userAgent = null) =>
            srcUri.GetFileDate(null, null, true, null, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(string srcUri, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileDate(userName, password, allowAutoRedirect, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(string srcUri, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileDate(userName, password, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(string srcUri, string userName, string password, CookieContainer cookieContainer, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileDate(userName, password, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(string srcUri, string userName, string password, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileDate(userName, password, true, null, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(string srcUri, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileDate(null, null, allowAutoRedirect, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(string srcUri, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileDate(null, null, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(string srcUri, CookieContainer cookieContainer, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileDate(null, null, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the last date and time of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static DateTime GetFileDate(string srcUri, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileDate(null, null, true, null, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(this Uri srcUri, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null)
        {
            var name = string.Empty;
            try
            {
                using (var wc = new WebClientEx(allowAutoRedirect, cookieContainer, timeout))
                {
                    if (!string.IsNullOrEmpty(userAgent))
                        wc.Headers.Add("User-Agent", userAgent);
                    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                        wc.Credentials = new NetworkCredential(userName, password);
                    using (wc.OpenRead(srcUri))
                    {
                        var cd = wc.ResponseHeaders["content-disposition"];
                        if (!string.IsNullOrWhiteSpace(cd))
                        {
                            var i = cd.IndexOf("filename=", StringComparison.OrdinalIgnoreCase);
                            if (i >= 0)
                                name = cd.Substring(i + 0xa);
                        }
                    }
                }
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                Log.Write(ex);
            }
            return name;
        }

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(this Uri srcUri, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.GetFileName(userName, password, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(this Uri srcUri, string userName, string password, CookieContainer cookieContainer, int timeout = 3000, string userAgent = null) =>
            srcUri.GetFileName(userName, password, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(this Uri srcUri, string userName, string password, int timeout, string userAgent = null) =>
            srcUri.GetFileName(userName, password, true, null, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(this Uri srcUri, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null) =>
            srcUri.GetFileName(null, null, allowAutoRedirect, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(this Uri srcUri, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.GetFileName(null, null, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(this Uri srcUri, CookieContainer cookieContainer, int timeout, string userAgent = null) =>
            srcUri.GetFileName(null, null, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(this Uri srcUri, int timeout, string userAgent = null) =>
            srcUri.GetFileName(null, null, true, null, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(string srcUri, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileName(userName, password, allowAutoRedirect, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(string srcUri, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileName(userName, password, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(string srcUri, string userName, string password, CookieContainer cookieContainer, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileName(userName, password, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="userName">
        ///     The username associated with the credential.
        /// </param>
        /// <param name="password">
        ///     The password associated with the credential.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(string srcUri, string userName, string password, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileName(userName, password, true, null, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(string srcUri, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileName(null, null, allowAutoRedirect, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="allowAutoRedirect">
        ///     true to indicate that the request should follow redirection responses;
        ///     otherwise, false.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(string srcUri, bool allowAutoRedirect, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileName(null, null, allowAutoRedirect, null, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="cookieContainer">
        ///     The cookies associated with the request.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(string srcUri, CookieContainer cookieContainer, int timeout = 3000, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileName(null, null, true, cookieContainer, timeout, userAgent);

        /// <summary>
        ///     Gets the filename of the specified internet resource.
        /// </summary>
        /// <param name="srcUri">
        ///     The full path of the resource to access.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        public static string GetFileName(string srcUri, int timeout, string userAgent = null) =>
            srcUri.ToHttpUri().GetFileName(null, null, true, null, timeout, userAgent);

        /// <summary>
        ///     Provides static methods for downloading internet resources.
        /// </summary>
        public static class Transfer
        {
            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(Uri srcUri, string destPath, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null, bool checkExists = true)
            {
                try
                {
                    if (srcUri == null)
                        throw new ArgumentNullException(nameof(srcUri));
                    var path = PathEx.Combine(destPath);
                    if (File.Exists(path))
                        File.Delete(path);
                    if (checkExists && !FileIsAvailable(srcUri, userName, password, allowAutoRedirect, cookieContainer, timeout, userAgent))
                        throw new PathNotFoundException(srcUri.ToString());
                    using (var wc = new WebClientEx(allowAutoRedirect, cookieContainer, timeout))
                    {
                        if (!string.IsNullOrEmpty(userAgent))
                            wc.Headers.Add("User-Agent", userAgent);
                        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                            wc.Credentials = new NetworkCredential(userName, password);
                        wc.DownloadFile(srcUri, path);
                    }
                    return File.Exists(path);
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                    return false;
                }
            }

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(Uri srcUri, string destPath, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, userName, password, allowAutoRedirect, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(Uri srcUri, string destPath, string userName, string password, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, userName, password, true, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(Uri srcUri, string destPath, string userName, string password, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, userName, password, true, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(Uri srcUri, string destPath, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, null, null, allowAutoRedirect, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(Uri srcUri, string destPath, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, null, null, true, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(Uri srcUri, string destPath, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, null, null, true, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(string srcUri, string destPath, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, userName, password, allowAutoRedirect, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(string srcUri, string destPath, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, userName, password, allowAutoRedirect, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(string srcUri, string destPath, string userName, string password, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, userName, password, true, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(string srcUri, string destPath, string userName, string password, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, userName, password, true, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(string srcUri, string destPath, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, null, null, allowAutoRedirect, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(string srcUri, string destPath, bool allowAutoRedirect, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, null, null, allowAutoRedirect, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(string srcUri, string destPath, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, null, null, true, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public static bool DownloadFile(string srcUri, string destPath, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, null, null, true, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(Uri srcUri, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null)
            {
                try
                {
                    byte[] ba;
                    using (var wc = new WebClientEx(allowAutoRedirect, cookieContainer, timeout))
                    {
                        if (!string.IsNullOrEmpty(userAgent))
                            wc.Headers.Add("User-Agent", userAgent);
                        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                            wc.Credentials = new NetworkCredential(userName, password);
                        ba = wc.DownloadData(srcUri);
                    }
                    if (ba == null)
                        throw new ArgumentNullException(nameof(ba));
                    return ba;
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                    return null;
                }
            }

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(Uri srcUri, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null) =>
                DownloadData(srcUri, userName, password, allowAutoRedirect, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(Uri srcUri, string userName, string password, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null) =>
                DownloadData(srcUri, userName, password, true, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(Uri srcUri, string userName, string password, int timeout, string userAgent = null) =>
                DownloadData(srcUri, userName, password, true, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(Uri srcUri, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null) =>
                DownloadData(srcUri, null, null, allowAutoRedirect, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(Uri srcUri, bool allowAutoRedirect, int timeout, string userAgent = null) =>
                DownloadData(srcUri, null, null, allowAutoRedirect, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(Uri srcUri, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null) =>
                DownloadData(srcUri, null, null, true, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(Uri srcUri, int timeout, string userAgent = null) =>
                DownloadData(srcUri, null, null, true, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(string srcUri, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null) =>
                DownloadData(srcUri.ToHttpUri(), userName, password, allowAutoRedirect, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(string srcUri, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null) =>
                DownloadData(srcUri.ToHttpUri(), userName, password, allowAutoRedirect, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(string srcUri, string userName, string password, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null) =>
                DownloadData(srcUri.ToHttpUri(), userName, password, true, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(string srcUri, string userName, string password, int timeout, string userAgent = null) =>
                DownloadData(srcUri.ToHttpUri(), userName, password, true, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(string srcUri, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null) =>
                DownloadData(srcUri.ToHttpUri(), null, null, allowAutoRedirect, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(string srcUri, bool allowAutoRedirect, int timeout, string userAgent = null) =>
                DownloadData(srcUri.ToHttpUri(), null, null, allowAutoRedirect, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(string srcUri, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null) =>
                DownloadData(srcUri.ToHttpUri(), null, null, true, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="byte"/> array.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static byte[] DownloadData(string srcUri, int timeout, string userAgent = null) =>
                DownloadData(srcUri.ToHttpUri(), null, null, true, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(Uri srcUri, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null)
            {
                try
                {
                    string s;
                    using (var wc = new WebClientEx(allowAutoRedirect, cookieContainer, timeout))
                    {
                        if (!string.IsNullOrEmpty(userAgent))
                            wc.Headers.Add("User-Agent", userAgent);
                        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                            wc.Credentials = new NetworkCredential(userName, password);
                        s = wc.DownloadString(srcUri);
                    }
                    if (string.IsNullOrEmpty(s))
                        throw new ArgumentNullException(nameof(s));
                    return s;
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                    return string.Empty;
                }
            }

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(Uri srcUri, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null) =>
                DownloadString(srcUri, userName, password, allowAutoRedirect, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(Uri srcUri, string userName, string password, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null) =>
                DownloadString(srcUri, userName, password, true, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(Uri srcUri, string userName, string password, int timeout, string userAgent = null) =>
                DownloadString(srcUri, userName, password, true, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(Uri srcUri, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null) =>
                DownloadString(srcUri, null, null, allowAutoRedirect, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(Uri srcUri, bool allowAutoRedirect, int timeout, string userAgent = null) =>
                DownloadString(srcUri, null, null, allowAutoRedirect, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(Uri srcUri, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null) =>
                DownloadString(srcUri, null, null, true, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(Uri srcUri, int timeout, string userAgent = null) =>
                DownloadString(srcUri, null, null, true, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(string srcUri, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null) =>
                DownloadString(srcUri.ToHttpUri(), userName, password, allowAutoRedirect, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(string srcUri, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null) =>
                DownloadString(srcUri.ToHttpUri(), userName, password, allowAutoRedirect, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(string srcUri, string userName, string password, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null) =>
                DownloadString(srcUri.ToHttpUri(), userName, password, true, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(string srcUri, string userName, string password, int timeout, string userAgent = null) =>
                DownloadString(srcUri.ToHttpUri(), userName, password, true, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(string srcUri, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null) =>
                DownloadString(srcUri.ToHttpUri(), null, null, allowAutoRedirect, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(string srcUri, bool allowAutoRedirect, int timeout, string userAgent = null) =>
                DownloadString(srcUri.ToHttpUri(), null, null, allowAutoRedirect, null, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(string srcUri, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null) =>
                DownloadString(srcUri.ToHttpUri(), null, null, true, cookieContainer, timeout, userAgent);

            /// <summary>
            ///     Downloads the specified internet resource as a <see cref="string"/>.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            public static string DownloadString(string srcUri, int timeout, string userAgent = null) =>
                DownloadString(srcUri.ToHttpUri(), null, null, true, null, timeout, userAgent);
        }

        /// <summary>
        ///     Provides asynchronous downloading of internet resources.
        /// </summary>
        public sealed class AsyncTransfer : IDisposable
        {
            private readonly Stopwatch _stopwatch = new Stopwatch();
            private WebClient _webClient;

            /// <summary>
            ///     Initializes a new instance of the <see cref="AsyncTransfer"/> class.
            /// </summary>
            [SuppressMessage("ReSharper", "EmptyConstructor")]
            public AsyncTransfer() { }

            /// <summary>
            ///     Gets the address to the resource.
            /// </summary>
            public Uri Address { get; private set; }

            /// <summary>
            ///     Gets the local file name of the resource.
            /// </summary>
            public string FileName { get; private set; }

            /// <summary>
            ///     Gets the local path of the resource.
            /// </summary>
            public string FilePath { get; private set; }

            /// <summary>
            ///     Gets the number of bytes received.
            /// </summary>
            public long BytesReceived { get; private set; }

            /// <summary>
            ///     Gets the total number of bytes received.
            /// </summary>
            public long TotalBytesToReceive { get; private set; }

            /// <summary>
            ///     Gets the total number of <see cref="BytesReceived"/> and <see cref="BytesReceived"/>
            ///     received in megabyte.
            /// </summary>
            public string DataReceived
            {
                get
                {
                    var current = BytesReceived.FormatSize(SizeUnit.MB);
                    var total = TotalBytesToReceive.FormatSize(SizeUnit.MB);
                    return $"{current} / {total}";
                }
            }

            /// <summary>
            ///     Gets the asynchronous progress percentage.
            /// </summary>
            public int ProgressPercentage { get; private set; }

            /// <summary>
            ///     Gets the megabyte per second of the asynchronous transfer.
            /// </summary>
            public double TransferSpeed { get; private set; }

            /// <summary>
            ///     Gets the string representation of the speed of the asynchronous transfer.
            /// </summary>
            public string TransferSpeedAd { get; private set; } = "0.00 bit/s";

            /// <summary>
            ///     Gets the total elapsed time.
            /// </summary>
            public TimeSpan TimeElapsed { get; private set; } = TimeSpan.MinValue;

            /// <summary>
            ///     Determines whether the transfer has been canceled.
            /// </summary>
            public bool HasCanceled { get; set; } = true;

            /// <summary>
            ///     Gets whether a transfer is in progress.
            /// </summary>
            public bool IsBusy
            {
                get
                {
                    try
                    {
                        return _webClient.IsBusy;
                    }
                    catch (Exception ex) when (ex.IsCaught())
                    {
                        return false;
                    }
                }
            }

            /// <summary>
            ///     Releases all resources used by this <see cref="AsyncTransfer"/>.
            /// </summary>
            public void Dispose()
            {
                CancelAsync();
                _webClient?.Dispose();
            }

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(Uri srcUri, string destPath, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null, bool checkExists = true)
            {
                try
                {
                    if (srcUri == null)
                        throw new ArgumentNullException(nameof(srcUri));
                    if (IsBusy)
                        throw new NotSupportedException(ExceptionMessages.AsyncDownloadIsBusy);
                    var path = PathEx.Combine(destPath);
                    if (File.Exists(path))
                        File.Delete(path);
                    try
                    {
                        _webClient = new WebClientEx(allowAutoRedirect, cookieContainer, timeout);
                        if (!string.IsNullOrEmpty(userAgent))
                            _webClient.Headers.Add("User-Agent", userAgent);
                        _webClient.DownloadFileCompleted += DownloadFile_Completed;
                        _webClient.DownloadProgressChanged += DownloadFile_ProgressChanged;
                        Address = srcUri;
                        FileName = path.Split('\\').Last() ?? string.Empty;
                        FilePath = path;
                        if (checkExists && !FileIsAvailable(Address, userName, password, allowAutoRedirect, cookieContainer, timeout, userAgent))
                            throw new PathNotFoundException(srcUri.ToString());
                        if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
                            _webClient.Credentials = new NetworkCredential(userName, password);
                        _webClient.DownloadFileAsync(Address, FilePath);
                        _stopwatch.Start();
                    }
                    finally
                    {
                        _webClient?.Dispose();
                    }
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                    HasCanceled = true;
                    _stopwatch.Reset();
                }
            }

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(Uri srcUri, string destPath, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, userName, password, allowAutoRedirect, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(Uri srcUri, string destPath, string userName, string password, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, userName, password, true, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(Uri srcUri, string destPath, string userName, string password, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, userName, password, true, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(Uri srcUri, string destPath, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, null, null, allowAutoRedirect, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(Uri srcUri, string destPath, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, null, null, true, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(Uri srcUri, string destPath, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri, destPath, null, null, true, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(string srcUri, string destPath, string userName = null, string password = null, bool allowAutoRedirect = true, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, userName, password, allowAutoRedirect, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(string srcUri, string destPath, string userName, string password, bool allowAutoRedirect, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, userName, password, allowAutoRedirect, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(string srcUri, string destPath, string userName, string password, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, userName, password, true, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="userName">
            ///     The username associated with the credential.
            /// </param>
            /// <param name="password">
            ///     The password associated with the credential.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(string srcUri, string destPath, string userName, string password, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, userName, password, true, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(string srcUri, string destPath, bool allowAutoRedirect, CookieContainer cookieContainer = null, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, null, null, allowAutoRedirect, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="allowAutoRedirect">
            ///     true to indicate that the request should follow redirection responses;
            ///     otherwise, false.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(string srcUri, string destPath, bool allowAutoRedirect, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, null, null, allowAutoRedirect, null, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="cookieContainer">
            ///     The cookies associated with the request.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(string srcUri, string destPath, CookieContainer cookieContainer, int timeout = 60000, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, null, null, true, cookieContainer, timeout, userAgent, checkExists);

            /// <summary>
            ///     Downloads the specified internet resource to a local file.
            /// </summary>
            /// <param name="srcUri">
            ///     The full path of the resource to download.
            /// </param>
            /// <param name="destPath">
            ///     The local destination path of the file.
            /// </param>
            /// <param name="timeout">
            ///     The time-out value in milliseconds.
            /// </param>
            /// <param name="userAgent">
            ///     The value of the User-agent HTTP header.
            /// </param>
            /// <param name="checkExists">
            ///     true to check the file availability before downloading; otherwise, false.
            /// </param>
            public void DownloadFile(string srcUri, string destPath, int timeout, string userAgent = null, bool checkExists = true) =>
                DownloadFile(srcUri.ToHttpUri(), destPath, null, null, true, null, timeout, userAgent, checkExists);

            private void DownloadFile_ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
            {
                try
                {
                    BytesReceived = e.BytesReceived;
                    TotalBytesToReceive = e.TotalBytesToReceive;
                    ProgressPercentage = e.ProgressPercentage;
                    TimeElapsed = _stopwatch.Elapsed;
                    TransferSpeed = e.BytesReceived;
                    if (e.BytesReceived <= 0)
                        return;
                    TransferSpeed = TransferSpeed / 1000 / TimeElapsed.TotalSeconds;
                    var speed = (long)(e.BytesReceived / TimeElapsed.TotalSeconds);
                    var speedAd = speed.FormatSize(false).ToLowerInvariant();
                    if (speedAd.Contains("byte"))
                        speedAd = speedAd.TrimEnd('s').Replace("byte", "b");
                    TransferSpeedAd = $"{speedAd}it/s";
                }
                catch (Exception ex) when (ex.IsCaught())
                {
                    Log.Write(ex);
                    TimeElapsed = _stopwatch.Elapsed;
                    HasCanceled = true;
                    _stopwatch.Reset();
                }
            }

            private void DownloadFile_Completed(object sender, AsyncCompletedEventArgs e)
            {
                _stopwatch.Reset();
                if (e.Cancelled)
                {
                    HasCanceled = true;
                    _webClient?.Dispose();
                    try
                    {
                        if (File.Exists(FilePath))
                            File.Delete(FilePath);
                    }
                    catch (Exception ex) when (ex.IsCaught())
                    {
                        Log.Write(ex);
                    }
                }
                else
                {
                    if (File.Exists(FilePath))
                        BytesReceived = new FileInfo(FilePath).Length;
                    HasCanceled = !(File.Exists(FilePath) && BytesReceived == TotalBytesToReceive);
                }
            }

            /// <summary>
            ///     Cancels a pending asynchronous transfer.
            /// </summary>
            public void CancelAsync()
            {
                if (IsBusy)
                    _webClient?.CancelAsync();
            }
        }
    }
}
