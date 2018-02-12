using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace UPF
{
    public static class RNGHelper
    {
        #region Member Fields

        private static byte[] _RandomBytes = new byte[4];
        private static RNGCryptoServiceProvider _Rand = new RNGCryptoServiceProvider();

        #endregion Member Fields

        #region Methods

        /// <summary>
        /// Method used to generate a random int value.
        /// </summary>
        /// <returns>An integer</returns>
        public static int GenerateRandomInt()
        {
            _Rand.GetBytes(_RandomBytes);
            int value = BitConverter.ToInt32(_RandomBytes, 0);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary>
        /// Method used to generate a random int value.
        /// </summary>
        /// <param name="max">The highest int value allowed.</param>
        /// <returns>An integer</returns>
        public static int GenerateRandomInt(int max)
        {
            _Rand.GetBytes(_RandomBytes);
            int value = BitConverter.ToInt32(_RandomBytes, 0);
            value = value % (max + 1); // % calculates remainder
            if (value < 0) value = -value;
            return value;
        }

        /// <summary>
        /// Method used to generate a random int value.
        /// </summary>
        /// <param name="min">The lowest int value allowed.</param>
        /// <param name="max">The highest int value allowed.</param>
        /// <returns>An integer</returns>
        public static int GenerateRandomInt(int min, int max)
        {
            int value = GenerateRandomInt(max - min) + min;
            return value;
        }

        #endregion Methods
    }
}
