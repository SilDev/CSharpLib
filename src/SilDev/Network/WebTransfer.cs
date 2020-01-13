#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: WebTransfer.cs
// Version:  2020-01-13 13:04
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Network
{
    using System;
    using System.IO;
    using System.Net;

    /// <summary>
    ///     Provides static methods for downloading internet resources.
    /// </summary>
    public static class WebTransfer
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
                if (checkExists && !srcUri.FileIsAvailable(userName, password, allowAutoRedirect, cookieContainer, timeout, userAgent))
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        /// <param name="checkExists">
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        /// <param name="checkExists">
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
        /// </param>
        /// <param name="timeout">
        ///     The time-out value in milliseconds.
        /// </param>
        /// <param name="userAgent">
        ///     The value of the User-agent HTTP header.
        /// </param>
        /// <param name="checkExists">
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
                    throw new InvalidOperationException();
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
                    throw new InvalidOperationException();
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to indicate that the request should follow
        ///     redirection responses; otherwise, <see langword="false"/>.
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
}
