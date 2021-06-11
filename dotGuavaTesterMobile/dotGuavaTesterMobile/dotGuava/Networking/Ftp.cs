using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace dotGuava.Networking
{
    /// <summary>
    /// Class that provides a simple way to download or upload files and directories through a FTP connection.
    /// </summary>
    public class FTP
    {
        #region Properties

        /// <summary>
        /// Server to access.
        /// </summary>
        string Servidor { get; set; }

        /// <summary>
        /// File path.
        /// </summary>
        string URL { get; set; }

        /// <summary>
        /// Username to access ftp server.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// Password to access ftp server with indicated user.
        /// </summary>
        string Password { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Generates a new intance of ftp manager.
        /// </summary>
        /// <param name="server">Server to connect.</param>
        /// <param name="username">Username to access ftp server.</param>
        /// <param name="password">Password to access ftp server with indicated user.</param>
        public FTP(string server, string username, string password)
        {
            Servidor = server;
            Username = username;
            Password = password;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Uploads a file to ftp server
        /// </summary>
        /// <param name="sourcePath">Full file to be upload path.</param>
        /// <param name="destinationPath">Full file destination path on the server.</param>
        /// <param name="result">A string indicating additional information if operation cannot be completed.</param>
        /// <returns>A boolean indicating operation result.</returns>
        public bool UploadFile(string sourcePath, string destinationPath, out string result)
        {
            result = bool.TrueString;

            try
            {
                new WebClient { Credentials = new NetworkCredential(Username, Password) }.UploadFile(destinationPath, sourcePath);

                return true;
            }
            catch (WebException ex)
            {
                result = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Reads a file on the ftp server.
        /// </summary>
        /// <param name="filePath">Full path on the server of the file to be read.</param>
        /// <param name="result">A string indicating additional information if operation cannot be completed.</param>
        /// <returns>A string with the file content.</returns>
        public string DownloadFileContent(string filePath, out string result)
        {
            result = bool.TrueString;

            try
            {
                byte[] downloadedData = new WebClient { Credentials = new NetworkCredential(Username, Password) }.DownloadData(filePath);
                string processedData = Encoding.UTF8.GetString(downloadedData);

                if (String.IsNullOrEmpty(processedData))
                {
                    return string.Empty;
                }

                return processedData;
            }
            catch (WebException ex)
            {
                result = ex.Message;
                return string.Empty;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return string.Empty;
            }
        }

        // <summary>
        // Descarga un archivo del servidor
        // </summary>
        // <param name="archivoDescargar">Archivo a descargar</param>
        // <param name="archivoSalida">Donde se guardará este archivo</param>
        // <param name="requerirConfirmacionArchivoExiste">Si el archivo ya existe pide confirmación para eliminarlo y guardar el descargado</param>
        // <param name="requisicionInterna">Confirma internamente eliminar el archivo existente</param>
        // <returns>Un booleano indicando el resultado de la operacion</returns>

        /// <summary>
        /// Downloads a file from the ftp server.
        /// </summary>
        /// <param name="sourcePath">Full file path on the ftp server.</param>
        /// <param name="destinationPath">Full file destination path on local machine</param>
        /// <param name="overwrite">If file exists on local machine should this be overwritten?</param>
        /// <param name="result">A string indicating additional information if operation cannot be completed.</param>
        /// <returns>A boolean indicating operation result.</returns>        
        public bool DownloadFile(string sourcePath, string destinationPath, bool overwrite, out string result)
        {
            result = bool.TrueString;
            var fileToBeCreatedStream = File.Create(destinationPath);

            try
            {
                byte[] downloadedData = new WebClient { Credentials = new NetworkCredential(Username, Password) }.DownloadData(sourcePath);

                Stream dataStream = null;
                int readBytes = dataStream.Read(downloadedData, 0, downloadedData.Length);

                fileToBeCreatedStream.Write(downloadedData, 0, readBytes);
                fileToBeCreatedStream.Close();

                return true;
            }
            catch (IOException ex)
            {
                result = ex.Message;

                if (overwrite)
                {
                    File.Delete(destinationPath);
                    if (DownloadFile(sourcePath, destinationPath, false, out result))
                    {
                        return true;
                    }
                }

                return false;
            }
            catch (WebException ex)
            {
                result = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                result = ex.Message;
                return false;
            }
            finally
            {
                fileToBeCreatedStream.Close();
            }
        }

        #endregion             
    }
}
