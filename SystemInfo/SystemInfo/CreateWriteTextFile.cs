using SystemInfo.Properties;

namespace SystemInfo
{
    public static class CreateWriteTextFile
    {
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
