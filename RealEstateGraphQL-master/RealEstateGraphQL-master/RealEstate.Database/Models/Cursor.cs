﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RealEstate.Database.Models
{
    public static class Cursor
    {
        private const string Prefix = "arrayconnection";

        public static T FromCursor<T>(string cursor)
        {
            if (string.IsNullOrEmpty(cursor))
                return default;

            string decodedValue;
            try
            {
                decodedValue = Base64Decode(cursor);
            }
            catch (FormatException)
            {
                return default;
            }

            var prefixIndex = Prefix.Length + 1;
            if (decodedValue.Length <= prefixIndex)
                return default;

            var value = decodedValue.Substring(prefixIndex);
            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        public static (string firstCursor, string lastCursor) GetFirstAndLastCursor<TItem, TCursor>(
            IEnumerable<TItem> enumerable,
            Func<TItem, TCursor> getCursorProperty)
        {
            if (getCursorProperty == null)
                throw new ArgumentNullException(nameof(getCursorProperty));
            if (enumerable == null || enumerable.Count() == 0)
                return (null, null);

            var firstCursor = ToCursor(getCursorProperty(enumerable.First()));
            var lastCursor = ToCursor(getCursorProperty(enumerable.Last()));
            return (firstCursor, lastCursor);
        }

        public static string ToCursor<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return Base64Encode(string.Format(CultureInfo.InvariantCulture, "{0}:{1}", Prefix, value));
        }

        private static string Base64Decode(string value) => Encoding.UTF8.GetString(Convert.FromBase64String(value));

        private static string Base64Encode(string value) => Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
    }
}
