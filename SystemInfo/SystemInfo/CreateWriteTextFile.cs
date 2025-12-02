using SystemInfo.Properties;

namespace SystemInfo
{
    public static class CreateWriteTextFile
    {
        /// <summary>
        /// Creates a new text file with a name based on the current machine and date, and writes the specified
        /// information to it.
        /// </summary>
        /// <remarks>The file is named using the format '<MachineName>-<dd-MM-yyyy>.txt' and is created in
        /// the application's current working directory. If a file with the same name already exists, its contents will
        /// be overwritten.</remarks>
        /// <param name="infoToWrite">The content to write to the newly created file. If null or empty, an empty file will be created.</param>
        public static void CreateAndWriteFile(string infoToWrite)
        {
            // 1.Формуємо динамічну назву файлу
            string pcName = Environment.MachineName; // Ім'я ПК
            string currentDate = DateTime.Now.ToString("dd-MM-yyyy"); // Дата у форматі 25-11-2025

            string fileName = $"{pcName}-{currentDate}.txt";

            // 2.Записуємо інформацію у файл
            File.WriteAllText(fileName, infoToWrite);

            Console.WriteLine(Resources.FileCreatedSuccessfully + " " + fileName);
        }
    }
}
