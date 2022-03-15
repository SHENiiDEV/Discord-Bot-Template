using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MINECRAFT_RU_v2.Handlers
{
   public class ErrorHandler
    {
        public static string CheckError(string task)
        {
            var owibka = "";
            switch (task)
            {
                case "BadArgCount":
                    owibka = "Вы не указали нужные параметры";
                    return owibka;
                case "ObjectNotFound":
                    owibka = "Указанный пользователь не найден.";
                    break;
            }
            return owibka;
        }

    }
}
