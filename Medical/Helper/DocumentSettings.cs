namespace Medical.Helper
{
    public class DocumentSettings
    {
        public static string SaveFile(string fileName, byte[] fileContent, string folderPath)
        {
            // Ensure folderPath exists
            if (!Directory.Exists(folderPath))
            { Directory.CreateDirectory(folderPath); }

            // Generate a unique file name to prevent overwriting existing files
            string uniqueFileName = $"{fileName.Replace(" ", "_")}-{Guid.NewGuid()}.png";
            Directory.CreateDirectory(folderPath);

            // Combine folderPath with unique file name and file extension to get the complete file path
            string filePath = Path.Combine(folderPath, uniqueFileName);

            // Write the file content to the file system
            File.WriteAllBytes(filePath, fileContent);

            // Return the unique file name
            return uniqueFileName;
        }

        public static string SaveFile(string fileName, IFormFile file, string folderPath)
        {

            // Ensure folderPath exists
            Directory.CreateDirectory(folderPath);

            // Generate a unique file name to prevent overwriting existing files
            string uniqueFileName = $"{fileName.Replace(" ", "_")}-{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            // Combine folderPath with unique file name to get the complete file path
            string filePath = Path.Combine(folderPath, uniqueFileName);

            // Create a FileStream to write the uploaded file to the file system
            using (FileStream stream = new(filePath, FileMode.Create))
            {
                // Copy the contents of the uploaded file to the FileStream
                file.CopyTo(stream);
            }

            // Return the unique file name
            return uniqueFileName;
        }
        public static bool RemoveFile(string filePath)
        {
            filePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);

                return true;
            }

            return false;
        }
    }
}
