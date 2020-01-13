#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: FormEx.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
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
    ///     Provides special <see cref="Form"/> settings.
    /// </summary>
    [Flags]
    public enum FormExPlusSettings
    {
        /// <summary>
        ///     Logs the loading time of the specified <see cref="Form"/>.
        /// </summary>
        LogLoadingTime = 0x10
    }

    /// <summary>
    ///     Expands the functionality for the <see cref="Form"/> class.
    /// </summary>
    public static class FormEx
    {
        /// <summary>
        ///     Allows to dock the specified <see cref="Form"/> to the virtual screen
        ///     edges.
        /// </summary>
        /// <param name="form">
        ///     The form window to be dock-able.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     form is null.
        /// </exception>
        public static void Dockable(Form form)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            form.ResizeEnd += (sender, e) =>
            {
                if (!(sender is Form f))
                    return;
                WinApi.NativeHelper.MoveWindowToVisibleScreenArea(f.Handle);
                f.Update();
            };
        }

        /// <summary>
        ///     Determines special settings for the specified <see cref="Form"/>.
        ///     <para>
        ///         Hint: This function should be called before the <see cref="Form"/> is
        ///         created.
        ///     </para>
        /// </summary>
        /// <param name="form">
        ///     The form window to determine the settings.
        /// </param>
        /// <param name="settings">
        ///     The settings to be applied.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     form is null.
        /// </exception>
        public static Form Plus(this Form form, FormExPlusSettings settings = FormExPlusSettings.LogLoadingTime)
        {
            if (form == null)
                throw new ArgumentNullException(nameof(form));
            if (Log.DebugMode <= 0 || !settings.HasFlag(FormExPlusSettings.LogLoadingTime))
                return form;
            try
            {
                if (Application.OpenForms.OfType<Form>().Any(x => x == form))
                    return form;
            }
            catch (Exception ex) when (ex.IsCaught())
            {
                return form;
            }
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            form.Shown += delegate
            {
                stopwatch.Stop();
                Log.Write($"Stopwatch: {form.Name} loaded in {stopwatch.ElapsedMilliseconds}ms.");
            };
            return form;
        }
    }
}
