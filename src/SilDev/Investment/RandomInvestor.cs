#region auto-generated FILE INFORMATION

// ==============================================
// This file is distributed under the MIT License
// ==============================================
// 
// Filename: RandomInvestor.cs
// Version:  2018-06-14 22:14
// 
// Copyright (c) 2018, Si13n7 Developments (r)
// All rights reserved.
// ______________________________________________

#endregion

namespace SilDev.Investment
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     A base class that provides multiple <see cref="Random"/> instances.
    /// </summary>
    public class RandomInvestor
    {
        private readonly Dictionary<int, Random> _random;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RandomInvestor"/> class.
        /// </summary>
        public RandomInvestor() =>
            _random = new Dictionary<int, Random>();

        /// <summary>
        ///     Gets the <see cref="Random"/> instance associated with the specified seed.
        /// </summary>
        /// <param name="seed">
        ///     A number used to calculate a starting value for the pseudo-random number sequence.
        /// </param>
        public Random GetGenerator(int seed = -1)
        {
            if (!_random.ContainsKey(seed))
                _random.Add(seed, seed != -1 ? new Random(seed) : new Random());
            return _random[seed];
        }
    }
}
