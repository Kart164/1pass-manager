using DbUp;
using System;
using System.Reflection;

namespace _1Pass.DBEngine
{
    public static class UpgradeEngine
    {
        static int UpdgradeToActualVersion(string connection)
        {
            var dbUpgradeEngine = DeployChanges.To
                .SQLiteDatabase(connection)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .WithVariablesDisabled()
                .LogToConsole()
                .Build();

            if (dbUpgradeEngine.IsUpgradeRequired())
            {
                Console.WriteLine("Upgrades have been detected. Upgrading database now...");
                var result = dbUpgradeEngine.PerformUpgrade();
                if (!result.Successful)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(result.Error);
                    Console.ResetColor();
                    return -1;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(value: "Upgrade completed successfully.");
                Console.ResetColor();
            }

            Console.WriteLine("Database is up to date now.");
            return 0;
        }
    }
}
