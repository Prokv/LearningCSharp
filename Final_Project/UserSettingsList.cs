using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final_Project
{
    public class UserSettingsList
    {
        /// <summary>
        /// Список пользовательских настроек
        /// </summary>
        public static List<UserInfo> UserInfoList=new List<UserInfo>();

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public UserSettingsList ()
        {
            UserInfoList = new List<UserInfo>();
        }

        /// <summary>
        /// Добавление новой строки в список
        /// </summary>
        /// <param name="Id">ИД пользователя</param>
        public static void Add (long Id)
        {
            if (!UserInfoList.Exists(x => x.Id == Id))
            {
                long id = Id;
                string userResponse = "";
                int bookId = 0;
                int startLine = 0;
                int step = 3;
                List<BookInfo> userBookList = new List<BookInfo>();
                UserInfo userInfo = new UserInfo(id, userResponse, bookId, startLine, step, userBookList);
                UserInfoList.Add(userInfo);
            }
        }

        /// <summary>
        /// Записать пользовательский список книг
        /// </summary>
        /// <param name="Id">ИД пользователя</param>
        /// <param name="UserBookList">Пользователский список книг</param>
        public static void SetUserBookList(long Id, List<BookInfo> UserBookList)
        {
            for (int i = 0; i < UserInfoList.Count; i++)
            {
                if (UserInfoList[i].Id == Id)
                {
                    UserInfoList[i].UserBookList=UserBookList;
                }
            }
        }

        /// <summary>
        /// Записать информацию по пользователю
        /// </summary>
        /// <param name="Id">ИД пользователя</param>
        /// <param name="UserInfo"></param>
        public static void SetUserInfo(UserInfo userInfo)
        {
            for (int i = 0; i < UserSettingsList.UserInfoList.Count; i++)
            {
                if (UserSettingsList.UserInfoList[i].Id == userInfo.Id)
                {
                    UserInfoList.RemoveAt(i);
                    UserInfoList.Add(userInfo);
                }
            }
        }
    }
}
