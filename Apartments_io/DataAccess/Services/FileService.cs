using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using System.IO;
using System.Threading.Tasks;

namespace DataAccess.Services
{
    /// <summary>
    /// Represents service to work with file
    /// </summary>
    public class FileService : Interfaces.IFileService
    {
        // FIELDS
        IHostingEnvironment hostingEnvironment;

        // CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of <see cref="FileService"/>
        /// </summary>
        /// <param name="environment">
        /// Service that provides information about the web hosting environment an application is running in
        /// </param>
        public FileService(IHostingEnvironment environment)
        {
            hostingEnvironment = environment;
        }

        // METHODS

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
        public async Task<string> UploadFileAsync(IFormFile uploadFile, params string[] serverDirectories)
        {
            if (uploadFile != null && uploadFile.Length > 0)
            {
                // uploads
                string filePath = GetRandomFreeName(uploadFile.FileName, serverDirectories);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadFile.CopyToAsync(stream);
                }

                // return relative path
                return GetRelativeServerPath(Path.GetFileName(filePath), serverDirectories);
            }

            return string.Empty;
        }
        /// <summary>
        /// Deletes file from server or do nothing if file does not exit
        /// </summary>
        /// <param name="fileName">
        /// File's name that should be deleted
        /// </param>
        /// <param name="serverDirectories">
        /// An array of foldeer to form server path
        /// </param>
        public void DeleteFile(string fileName, params string[] serverDirectories)
        {
            if (serverDirectories != null)
            {
                File.Delete(GetAbsoluteServerPath(serverDirectories) + fileName);
            }
        }

        private string GetRandomFreeName(string currentFileName, params string[] serverDirectories)
        {
            System.Random random = new System.Random();

            string fullPath;
            string serverPath = GetAbsoluteServerPath(serverDirectories);
            string fileExtension = Path.GetExtension(currentFileName);

            // gets random free name
            do
            {
                int fileHashName = random.Next().GetHashCode();

                fullPath = Path.Combine(serverPath, $"{fileHashName}{fileExtension}");
            } while (File.Exists(fullPath));

            return fullPath;
        }
        private string GetAbsoluteServerPath(params string[] serverDirectories)
        {
            System.Text.StringBuilder pathBuilder = new System.Text.StringBuilder();

            // build path
            pathBuilder.Append(hostingEnvironment.WebRootPath);
            pathBuilder.Append(Path.DirectorySeparatorChar);
            
            for (int i = 0; i < serverDirectories.Length; ++i)
            {
                pathBuilder.Append(serverDirectories[i]);
                pathBuilder.Append(Path.DirectorySeparatorChar);
            }

            return pathBuilder.ToString();
        }
        private string GetRelativeServerPath(string fileName, params string[] serverDirectories)
        {
            System.Text.StringBuilder pathBuilder = new System.Text.StringBuilder();
            
            // build path
            pathBuilder.Append(Path.DirectorySeparatorChar);            
            for (int i = 0; i < serverDirectories.Length; ++i)
            {
                pathBuilder.Append(serverDirectories[i]);
                pathBuilder.Append(Path.DirectorySeparatorChar);
            }
            pathBuilder.Append(fileName);

            return pathBuilder.ToString();
        }
    }
}
