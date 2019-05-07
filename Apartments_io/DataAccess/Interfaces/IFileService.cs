namespace DataAccess.Interfaces
{
    /// <summary>
    /// Represents interface for service to work with file
    /// </summary>
    public interface IFileService
    {
        /// <summary>
        /// Upload file
        /// </summary>
        /// <param name="uploadFile">
        /// File sent with HTTP request
        /// </param>
        /// <param name="serverDirectories">
        /// An array of foldeer to form server path
        /// </param>
        /// <returns>
        /// Relative file path
        /// </returns>
        System.Threading.Tasks.Task<string> UploadFileAsync(Microsoft.AspNetCore.Http.IFormFile uploadFile, params string[] serverDirectories);
        /// <summary>
        /// Deletes file from server or do nothing if file does not exit
        /// </summary>
        /// <param name="fileName">
        /// File's name that should be deleted
        /// </param>
        /// <param name="serverDirectories">
        /// An array of foldeer to form server path
        /// </param>
        void DeleteFile(string fileName, params string[] serverDirectories);
    }
}
