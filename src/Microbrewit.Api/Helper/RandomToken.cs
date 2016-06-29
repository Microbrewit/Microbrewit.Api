using System;

namespace Microbrewit.Api.Helper
{
    public static class RandomToken
    {
        private static int _defaultSize = 10;
        public static string Create()
        {
            return Create(_defaultSize);
        }
        public static string Create(int size)
        {
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                byte[] tokenData = new byte[size];
                rng.GetBytes(tokenData);

                string token = Convert.ToBase64String(tokenData);
                return token;
            }
        }
    }
}
