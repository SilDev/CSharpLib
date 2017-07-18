#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: FormEx.cs
// Version:  2017-07-18 04:21
// 
// Copyright (c) 2017, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Forms
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Windows.Forms;

    /// <summary>
    ///     Expands the functionality for the <see cref="Form"/> class.
    /// </summary>
    public static class FormEx
    {
        /// <summary>
        ///     Provides special <see cref="Form"/> settings.
        /// </summary>
        [Flags]
        public enum PlusSettings
        {
            /// <summary>
            ///     Logs the loading time of the specified <see cref="Form"/>.
            /// </summary>
            LogLoadingTime = 0x10
        }

        /// <summary>
        ///     Allows to dock the specifed <see cref="Form"/> to the virtual screen edges.
        /// </summary>
        /// <param name="form">
        ///     The form window to be dockable.
        /// </param>
        public static void Dockable(Form form)
        {
            form.ResizeEnd += (sender, args) =>
            {
                var f = sender as Form;
                if (f == null)
                    return;
                WinApi.NativeHelper.MoveWindowToVisibleScreenArea(f.Handle);
                f.Update();
            };
        }

        /// <summary>
        ///     <para>
        ///         Determines special settings for the specified <see cref="Form"/>.
        ///     </para>
        ///     <para>
        ///         Hint: This function should be called before the <see cref="Form"/> is created.
        ///     </para>
        /// </summary>
        /// <param name="form">
        ///     The form window to determine the settings.
        /// </param>
        /// <param name="settings">
        ///     The settings to be applied.
        /// </param>
        public static Form Plus(this Form form, PlusSettings settings = PlusSettings.LogLoadingTime)
        {
            if (Log.DebugMode <= 0 || !settings.HasFlag(PlusSettings.LogLoadingTime))
                return form;
            try
            {
                if (Application.OpenForms.Cast<Form>().Any(x => x == form))
                    return form;
            }
            catch
            {
                return form;
            }
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            form.Shown += (s, e) =>
            {
                stopwatch.Stop();
                Log.Write("Stopwatch: " + form.Name + " loaded in " + stopwatch.ElapsedMilliseconds + "ms.");
            };
            return form;
        }
    }
}
