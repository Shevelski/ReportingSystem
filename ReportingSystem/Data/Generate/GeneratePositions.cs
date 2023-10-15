using ReportingSystem.Models.Company;
using ReportingSystem.Models.User;
using System.Diagnostics;

namespace ReportingSystem.Data.Generate
{
    public class GeneratePositions
    {
        public List<EmployeePositionModel> Positions()
        {
            List<EmployeePositionModel> UserPositions = new List<EmployeePositionModel>();

            Random random = new Random();

            List<string> positions = new List<string>
            {
                "Директор",
                "Адміністратор",
                "Заступник директора",
                "Економіст",
                "Юрист",
                "HR",
                "Проект-менеджер",
                "Розробник",
                "Тестувальник",
                "Графічний дизайнер"
            };

            int countPositions = 0;
            //директор
            EmployeePositionModel userPosition = new EmployeePositionModel();
            userPosition.namePosition = positions[0];
            UserPositions.Add(userPosition);
            countPositions++;

            //адміністратор
            userPosition = new EmployeePositionModel();
            userPosition.namePosition = positions[1];
            UserPositions.Add(userPosition);
            countPositions++;


            // заст директора
            int rnd = random.Next(2, 3);

            for (int i = 0; i < rnd; i++)
            {
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[2];
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[3];
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[4];
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[5];
                UserPositions.Add(userPosition);
            }
            countPositions += rnd;

            int countManager = rnd * 2;
            int countPos = countManager;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[6];
                UserPositions.Add(userPosition);
            }
            countPositions += countPos;


            int rnd1 = random.Next(1, 4);
            countPos = countManager * rnd1;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[7];
                UserPositions.Add(userPosition);
            }

            countPositions += countPos;

            rnd1 = random.Next(1, 4);
            countPos = countManager * rnd1;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[8];
                UserPositions.Add(userPosition);
            }
            countPositions += countPos;


            rnd1 = random.Next(1, 4);
            countPos = countManager * rnd1;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel();
                userPosition.namePosition = positions[9];
                UserPositions.Add(userPosition);
            }
            countPositions += countPos;

            Debug.WriteLine($"All is position {countPositions}");
            return UserPositions;
        }
        
        
    }
}
