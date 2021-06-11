using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;

namespace dotGuava.Essential
{
    /// <summary>
    /// Class for directory compression and decompression.
    /// </summary>
    public static class Compresser
    {
        /// <summary>
        /// Compress a directory
        /// </summary>
        /// <param name="source">Directory to compress path.</param>
        /// <param name="destination">Destination compressed file path.</param>
        /// <param name="onlyFiles">Indicates if should compress only the files or the directory itself.</param>
        /// <param name="result">Message that helps in case of error, if everything is good it will be equal to a TrueString.</param>
        /// <returns>Resturns a boolean indicating compression result.</returns>
        public static bool Compress(string source, string destination, bool onlyFiles, out string result)
        {
            result = bool.TrueString;

            try
            {
                ZipFile.CreateFromDirectory(source, destination, CompressionLevel.Optimal, !onlyFiles);

                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// Decompress compressed file contens to a specified location.
        /// </summary>
        /// <param name="source">Compressed file path.</param>
        /// <param name="destination">Path of the destination directory.</param>
        /// <param name="overwrite">Indicates if should overwrite a file if one similar is found in destination directory.</param>
        /// <param name="result">Message that helps in case of error, if everything is good it will be equal to a TrueString.</param>
        /// <returns>Resturns a boolean indicating compression result.</returns>
        public static bool Decompress(string source, string destination, bool overwrite, out string result)
        {
            result = bool.TrueString;

            try
            {
                ZipFile.ExtractToDirectory(source, destination);

                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }
    }
}
