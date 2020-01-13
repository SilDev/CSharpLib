#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: ImageFrame.cs
// Version:  2020-01-13 13:03
// 
// Copyright (c) 2020, Si13n7 Developments(tm)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Drawing
{
    using System;
    using System.Drawing;
    using System.Runtime.Serialization;
    using System.Security;

    /// <summary>
    ///     An base class that provides the <see cref="System.Drawing.Image"/> and
    ///     duration of a single frame.
    /// </summary>
    [Serializable]
    public class ImageFrame : IDisposable, ISerializable, IEquatable<ImageFrame>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageFrame"/> class from the
        ///     specified existing image and duration time of a single frame.
        /// </summary>
        /// <param name="image">
        ///     The frame image from which to create the new Frame.
        /// </param>
        /// <param name="duration">
        ///     The duration time, in milliseconds, of the new frame.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     image is null.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     duration is negative or zero.
        /// </exception>
        public ImageFrame(Image image, int duration)
        {
            Image = image ?? throw new ArgumentNullException(nameof(image));
            if (duration < 1)
                throw new ArgumentOutOfRangeException(nameof(duration));
            Duration = duration;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ImageFrame"/> class.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     info is null.
        /// </exception>
        protected ImageFrame(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (Log.DebugMode > 1)
                Log.Write($"{nameof(ImageFrame)}.ctor({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");
            Image = (Image)info.GetValue(nameof(Image), typeof(Image));
            Duration = info.GetInt32(nameof(Duration));
        }

        /// <summary>
        ///     Gets the image of this <see cref="ImageFrame"/>.
        /// </summary>
        public Image Image { get; }

        /// <summary>
        ///     Gets the duration time, in milliseconds, of this <see cref="ImageFrame"/>.
        /// </summary>
        public int Duration { get; }

        /// <summary>
        ///     Releases all resources used by this <see cref="ImageFrame"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Determines whether the specified frame is equal to the current frame.
        /// </summary>
        /// <param name="other">
        ///     The frame to compare with the current frame.
        /// </param>
        public virtual bool Equals(ImageFrame other) =>
            Duration == other?.Duration && Image.GetHashCodeEx() == other.Image.GetHashCodeEx();

        /// <summary>
        ///     Populates a <see cref="SerializationInfo"/> with the data needed to
        ///     serialize the target object.
        /// </summary>
        /// <param name="info">
        ///     The object that holds the serialized object data.
        /// </param>
        /// <param name="context">
        ///     The contextual information about the source or destination.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     info is null.
        /// </exception>
        [SecurityCritical]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException(nameof(info));
            if (Log.DebugMode > 1)
                Log.Write($"{nameof(ImageFrame)}.get({nameof(SerializationInfo)}, {nameof(StreamingContext)}) => info: {Json.Serialize(info)}, context: {Json.Serialize(context)}");
            info.AddValue(nameof(Image), Image);
            info.AddValue(nameof(Duration), Duration);
        }

        /// <summary>
        ///     Releases all resources used by this <see cref="ImageFrame"/>.
        /// </summary>
        /// <param name="disposing">
        ///     <see langword="true"/> to release both managed and unmanaged resources;
        ///     otherwise, <see langword="false"/> to release only unmanaged resources.
        ///     <para>
        ///         Please note that this parameter is ignored for the
        ///         <see cref="ImageFrame"/> class.
        ///     </para>
        /// </param>
        protected virtual void Dispose(bool disposing) =>
            Image?.Dispose();

        /// <summary>
        ///     Allows an object to try to free resources and perform other cleanup
        ///     operations before it is reclaimed by garbage collection.
        /// </summary>
        ~ImageFrame() =>
            Dispose(false);

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">
        ///     The object to compare with the current object.
        /// </param>
        public override bool Equals(object other) =>
            Equals(other as ImageFrame);

        /// <summary>
        ///     Returns the hash code for the current image pair.
        /// </summary>
        public override int GetHashCode() =>
            Crypto.GetClassHashCode(this);

        /// <summary>
        ///     Determines whether two specified frames have the same value.
        /// </summary>
        /// <param name="left">
        ///     The first frame to compare, or null.
        /// </param>
        /// <param name="right">
        ///     The second frame to compare, or null.
        /// </param>
        public static bool operator ==(ImageFrame left, ImageFrame right)
        {
            var obj = (object)left;
            if (obj != null)
                return left.Equals(right);
            obj = right;
            return obj == null;
        }

        /// <summary>
        ///     Determines whether two specified frames have different values.
        /// </summary>
        /// <param name="left">
        ///     The first frame to compare, or null.
        /// </param>
        /// <param name="right">
        ///     The second frame to compare, or null.
        /// </param>
        public static bool operator !=(ImageFrame left, ImageFrame right) =>
            !(left == right);
    }
}
