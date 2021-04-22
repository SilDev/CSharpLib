#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: FormEx.cs
// Version:  2021-04-22 19:45
// 
// Copyright (c) 2021, Si13n7 Developments(tm)
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
        ///     Activates fade-in effect.
        /// </summary>
        FadeIn = 4,

        /// <summary>
        ///     Logs the loading time.
        /// </summary>
        LogLoadingTime = 16
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
            form.ResizeEnd += (sender, _) =>
            {
                if (sender is not Form f)
                    return;
                WinApi.NativeHelper.MoveWindowToVisibleScreenArea(f.Handle);
                f.Update();
            };
        }

        /// <summary>
        ///     Applies a fade-in effect to the specified form.
        /// </summary>
        /// <param name="form">
        ///     The form to fade-in.
        /// </param>
        /// <param name="effectDuration">
        ///     The effect duration. Must be in range of 25 to 750.
        /// </param>
        /// <param name="maxOpacity">
        ///     The maximal opacity of the form.
        /// </param>
        /// <param name="setForeground">
        ///     <see langword="true"/> to bring the form into the foreground; otherwise,
        ///     <see langword="false"/>.
        /// </param>
        public static void FadeIn(Form form, int effectDuration = 25, double maxOpacity = 1d, bool setForeground = true)
        {
            if (form == null || form.Opacity > 0d)
                return;

            maxOpacity = Math.Min(Math.Max(effectDuration, .25d), 1d);
            if (form.Opacity >= maxOpacity)
                return;
            effectDuration = Math.Min(Math.Max(effectDuration, 25), 750);

            var timer = new Timer
            {
                Interval = 1,
                Enabled = true
            };
            timer.Tick += OnTick;

            void OnTick(object sender, EventArgs e)
            {
                if (sender is not Timer owner)
                    return;
                if (form.Opacity < maxOpacity)
                {
                    var opacity = maxOpacity / (effectDuration / 10d) + form.Opacity;
                    if (opacity < maxOpacity)
                    {
                        form.Opacity = opacity;
                        return;
                    }
                }
                owner.Enabled = false;
                form.Opacity = maxOpacity;
                if (setForeground)
                    WinApi.NativeHelper.SetForegroundWindow(form.Handle);
                owner.Dispose();
            }
        }

        /// <summary>
        ///     Determines special settings for this <see cref="Form"/>.
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
            if (Application.OpenForms.OfType<Form>().Any(f => f == form))
                return form;
            if (Log.DebugMode > 0 && settings.HasFlag(FormExPlusSettings.LogLoadingTime))
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                form.Shown += OnShown;

                void OnShown(object sender, EventArgs e)
                {
                    stopwatch.Stop();
                    Log.Write($"Stopwatch: {form.Name} loaded in {stopwatch.ElapsedMilliseconds}ms.");
                }
            }
            if (!settings.HasFlag(FormExPlusSettings.FadeIn))
                return form;
            form.Shown += (_, _) => FadeIn(form);
            return form;
        }
    }
}
