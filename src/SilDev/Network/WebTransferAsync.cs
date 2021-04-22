#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: WebTransferAsync.cs
// Version:  2021-04-22 19:46
// 
// Copyright (c) 2021, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Network
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Net;
    using Properties;

    /// <summary>
    ///     Provides asynchronous downloading of internet resources.
    /// </summary>
    public sealed class WebTransferAsync : IDisposable
    {
        private readonly Stopwatch _stopwatch = new();
        private WebClient _webClient;

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
        ///     Gets the total number of <see cref="BytesReceived"/> and
        ///     <see cref="BytesReceived"/> received in megabyte.
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
        ///     Initializes a new instance of the <see cref="WebTransferAsync"/> class.
        /// </summary>
        [SuppressMessage("ReSharper", "EmptyConstructor")]
        public WebTransferAsync() { }

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
                    FileName = path.Split('\\').Last();
                    FilePath = path;
                    if (checkExists && !Address.FileIsAvailable(userName, password, allowAutoRedirect, cookieContainer, timeout, userAgent))
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
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
        ///     <see langword="true"/> to check the file availability before downloading;
        ///     otherwise, <see langword="false"/>.
        /// </param>
        public void DownloadFile(string srcUri, string destPath, int timeout, string userAgent = null, bool checkExists = true) =>
            DownloadFile(srcUri.ToHttpUri(), destPath, null, null, true, null, timeout, userAgent, checkExists);

        /// <summary>
        ///     Cancels a pending asynchronous transfer.
        /// </summary>
        public void CancelAsync()
        {
            if (IsBusy)
                _webClient?.CancelAsync();
        }

        /// <summary>
        ///     Releases all resources used by this <see cref="WebTransferAsync"/>.
        /// </summary>
        public void Dispose()
        {
            CancelAsync();
            _webClient?.Dispose();
        }

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
    }
}
