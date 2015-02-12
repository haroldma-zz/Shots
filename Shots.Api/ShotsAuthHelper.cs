using System;
using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace Shots.Api
{
    public static class ShotsAuthHelper
    {
        private static readonly DateTime EpochTime = new DateTime(1970, 1, 1);

        public static string GetSignature(string path, string timeCode)
        {
            var builder = new StringBuilder();
            builder.Append(ShotsConstants.ApiBase);
            builder.Append(path);
            builder.Append(ShotsConstants.ClientHeader);
            builder.Append(ShotsConstants.ApiKey);
            builder.Append(GetHash(HashAlgorithmNames.Md5, timeCode));

            return GetHash(HashAlgorithmNames.Sha1, builder.ToString());
        }

        public static string GetTimeCode()
        {
            return (DateTime.UtcNow - EpochTime).TotalSeconds.ToString();
        }

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