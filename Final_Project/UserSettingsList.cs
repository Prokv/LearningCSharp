using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    internal class UserSettingsList
    {
        /// <summary>
        /// Список пользовательских настроек
        /// </summary>
        public static List<UserInfo> UserInfo=new List<UserInfo>();

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public UserSettingsList ()
        {
            UserInfo = new List<UserInfo>();
        }

        /// <summary>
        /// Добавление новой строки в список
        /// </summary>
        /// <param name="Id">ИД пользователя</param>
        public static void Add (long Id)
        {
            for (int i = 0; i < UserInfo.Count; i++)
            {
                if (UserInfo[i].Id == Id)
                {
                    return;
                }
            }
            long id = Id;
            string userReturn = "";
            int bookId=0;
            int lineNumber=0;
            int stepNumber= 3;
            List<BookInfo> userBookList = new List<BookInfo>();
            UserInfo userInfo = new UserInfo(id, userReturn, bookId, lineNumber,stepNumber, userBookList);
            UserInfo.Add(userInfo);
        }

        /// <summary>
        /// Получить пользовательский список книг
        /// </summary>
        /// <param name="Id">ИД пользователя</param>
        /// <returns>Ответ в виде списка книг</returns>
        public static List<BookInfo> GetUserBookList (long Id)
        {
            List<BookInfo> outputInfo=new List<BookInfo> ();
            for (int i = 0; i < UserInfo.Count; i++)
            {
                if (UserInfo[i].Id == Id)
                {
                    outputInfo = UserInfo[i].UserBookList;
                }
            }
            return outputInfo;
        }

        /// <summary>
        /// Записать пользовательский список книг
        /// </summary>
        /// <param name="Id">ИД пользователя</param>
        /// <param name="UserBookList">Пользователский список книг</param>
        public static void SetUserBookList(long Id, List<BookInfo> UserBookList)
        {
            for (int i = 0; i < UserInfo.Count; i++)
            {
                if (UserInfo[i].Id == Id)
                {
                    UserInfo[i].UserBookList=UserBookList;
                }
            }
        }

        /// <summary>
        /// Получить информацию по пользователю
        /// </summary>
        /// <param name="Id">ИД пользователя</param>
        /// <returns></returns>
        public static object[] GetUserInfo(long Id)
        {
            object[] outputInfo=new object[4];
            for (int i = 0; i < UserInfo.Count; i++)
            {
                if (UserInfo[i].Id == Id)
                {
                    outputInfo[0]=UserInfo[i].UserResponse;
                    outputInfo[1]=UserInfo[i].BookId;
                    outputInfo[2]=UserInfo[i].StartLine;
                    outputInfo[3]=UserInfo[i].Step;
                }
            }
            return outputInfo;
        }

        /// <summary>
        /// Записать информацию по пользователю
        /// </summary>
        /// <param name="Id">ИД пользователя</param>
        /// <param name="UserInfo"></param>
        public static void SetUserInfo(long Id, object[] UserInfo)
        {
            object[] inputInfo = UserInfo;
            for (int i = 0; i < UserSettingsList.UserInfo.Count; i++)
            {
                if (UserSettingsList.UserInfo[i].Id == Id)
                {
                    UserSettingsList.UserInfo[i].UserResponse = inputInfo[0].ToString();
                    UserSettingsList.UserInfo[i].BookId = Convert.ToInt32(inputInfo[1]);
                    UserSettingsList.UserInfo[i].StartLine = Convert.ToInt32(inputInfo[2]);
                    UserSettingsList.UserInfo[i].Step = Convert.ToInt32(inputInfo[3]);
                }
            }
        }
    }
}
