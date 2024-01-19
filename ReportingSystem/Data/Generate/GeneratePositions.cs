using ReportingSystem.Models.Company;
using ReportingSystem.Models.User;
using System.Diagnostics;

namespace ReportingSystem.Data.Generate
{
    public class GeneratePositions
    {
        public List<EmployeePositionModel> Positions()
        {
            List<EmployeePositionModel> UserPositions = [];

            Random random = new();

            List<string> positions =
            [
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
            ];

            int countPositions = 0;
            //директор
            EmployeePositionModel userPosition = new()
            {
                NamePosition = positions[0]
            };
            UserPositions.Add(userPosition);
            countPositions++;

            //адміністратор
            userPosition = new EmployeePositionModel
            {
                NamePosition = positions[1]
            };
            UserPositions.Add(userPosition);
            countPositions++;


            // заст директора
            int rnd = random.Next(2, 3);

            for (int i = 0; i < rnd; i++)
            {
                userPosition = new EmployeePositionModel
                {
                    NamePosition = positions[2]
                };
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel
                {
                    NamePosition = positions[3]
                };
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel
                {
                    NamePosition = positions[4]
                };
                UserPositions.Add(userPosition);
                userPosition = new EmployeePositionModel
                {
                    NamePosition = positions[5]
                };
                UserPositions.Add(userPosition);
            }
            countPositions += rnd;

            int countManager = rnd * 2;
            int countPos = countManager;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel
                {
                    NamePosition = positions[6]
                };
                UserPositions.Add(userPosition);
            }
            countPositions += countPos;


            int rnd1 = random.Next(1, 4);
            countPos = countManager * rnd1;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel
                {
                    NamePosition = positions[7]
                };
                UserPositions.Add(userPosition);
            }

            countPositions += countPos;

            rnd1 = random.Next(1, 4);
            countPos = countManager * rnd1;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel
                {
                    NamePosition = positions[8]
                };
                UserPositions.Add(userPosition);
            }
            countPositions += countPos;


            rnd1 = random.Next(1, 4);
            countPos = countManager * rnd1;
            for (int i = 0; i < countPos; i++)
            {
                userPosition = new EmployeePositionModel
                {
                    NamePosition = positions[9]
                };
                UserPositions.Add(userPosition);
            }
            countPositions += countPos;

            Debug.WriteLine($"All is position {countPositions}");
            return UserPositions;
        }
        
        
    }
}
