using System;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Shots.Core.Extensions;

namespace Shots.Web.Helpers
{
    /// <summary>
    /// Helps with generating the auth signature.
    /// </summary>
    public static class ShotsAuthHelper
    {
        /// <summary>
        /// Gets the signature for the request path and time code.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="timeCode">The time code.</param>
        /// <returns></returns>
        public static string GetSignature(string path, string timeCode)
        {
            var builder = new StringBuilder();
            builder.Append(ShotsConstants.ApiBase);
            builder.Append(path);
            builder.Append(ShotsConstants.ClientHeader);
            builder.Append(ShotsConstants.ApiKey);

            // The time code must be hash with MD5
            builder.Append(GetHash(HashAlgorithmNames.Md5, timeCode));

            // While the end result must be hash with SHA1
            return GetHash(HashAlgorithmNames.Sha1, builder.ToString());
        }

        /// <summary>
        /// Gets the current time code.
        /// </summary>
        /// <returns></returns>
        public static string GetTimeCode()
        {
            return DateTime.Now.ToUnixTimestamp().ToString();
        }

        /// <summary>
        /// Gets the hashed value.
        /// </summary>
        /// <param name="algoritm">The algoritm.</param>
        /// <param name="s">The string to hash.</param>
        /// <returns></returns>
        public static string GetHash(string algoritm, string s)
        {
            var alg = HashAlgorithmProvider.OpenAlgorithm(algoritm);
            var buff = CryptographicBuffer.ConvertStringToBinary(s, BinaryStringEncoding.Utf8);
            var hashed = alg.HashData(buff);
            var res = CryptographicBuffer.EncodeToHexString(hashed);
            return res;
        }
    }
}