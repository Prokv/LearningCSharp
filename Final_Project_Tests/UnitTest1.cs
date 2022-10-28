using Final_Project;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Aspose.Pdf;


namespace Final_Project_Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SetAuthorReturned_True()
        {
            //Arrange
            int id = 1;
            string author = "test_Author";
            string pathFile = @"C:\Users\Admin\Documents\Учеба C#\LearningCSharp\Final_Project_Tests\bin\Debug\net6.0\Temp_books\Test.pdf";
            string fileName = "Test.pdf";
            string expected_input_author = "";

            BookInfo bookInfo = new BookInfo(id, "0", "0", "0", "0", fileName, pathFile);
            BookList.bookList.Add(bookInfo);
            List<BookInfo> output = new List<BookInfo>();

            //Act
            output =BookList.SetAuthor(id, author);
            expected_input_author = output[0].Author;

            //Assert
            Assert.That(author, Is.EqualTo(expected_input_author));
        }

        [Test]
        public void SetTitleReturned_True()
        {
            //Arrange
            int id = 1;
            string title = "test_Title";
            string pathFile = @"C:\Users\Admin\Documents\Учеба C#\LearningCSharp\Final_Project_Tests\bin\Debug\net6.0\Temp_books\Test.pdf";
            string fileName = "Test.pdf";
            string expected_input_title = "";

            BookInfo bookInfo = new BookInfo(id, "0", "0", "0", "0", fileName, pathFile);
            BookList.bookList.Add(bookInfo);
            List<BookInfo> output = new List<BookInfo>();

            //Act
            output = BookList.SetTitle(id, title);
            expected_input_title = output[0].Title;

            //Assert
            Assert.That(title, Is.EqualTo(expected_input_title));
        }

        [Test]
        public void SetKeywordsReturned_True()
        {
            //Arrange
            int id = 1;
            string keywords = "test_Title";
            string pathFile = @"C:\Users\Admin\Documents\Учеба C#\LearningCSharp\Final_Project_Tests\bin\Debug\net6.0\Temp_books\Test.pdf";
            string fileName = "Test.pdf";
            string expected_input_keywords = "";

            BookInfo bookInfo = new BookInfo(id, "0", "0", "0", "0", fileName, pathFile);
            BookList.bookList.Add(bookInfo);
            List<BookInfo> output = new List<BookInfo>();

            //Act
            output = BookList.SetKeywords(id, keywords);
            expected_input_keywords = output[0].Keywords;

            //Assert
            Assert.That(keywords, Is.EqualTo(expected_input_keywords));
        }

        [Test]
        public void SetGenreReturned_True()
        {
            //Arrange
            int id = 1;
            string genre = "test_Genre";
            string pathFile = @"C:\Users\Admin\Documents\Учеба C#\LearningCSharp\Final_Project_Tests\bin\Debug\net6.0\Temp_books\Test.pdf";
            string fileName = "Test.pdf";
            string expected_input_genre = "";

            BookInfo bookInfo = new BookInfo(id, "0", "0", "0", "0", fileName, pathFile);
            BookList.bookList.Add(bookInfo);
            List<BookInfo> output = new List<BookInfo>();

            //Act
            output = BookList.SetGenre(id, genre);
            expected_input_genre = output[0].Genre;

            //Assert
            Assert.That(genre, Is.EqualTo(expected_input_genre));
        }
    }
}