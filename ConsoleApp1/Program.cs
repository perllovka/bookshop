using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class BooksSystem
{
    static void Main(string[] args)
    {
		MenuMain();
    }
    static void MenuMain()
    {
		Console.Write("\n1. Книги.\n2. Пользователи.\n0. Выход.\nВведите пункт меню: ");
		int code;
		string s = Console.ReadLine();
		if(!Int32.TryParse(s, out code)) { MenuMain();}
		switch (code) {
			case  0:
				Environment.Exit(0);
				break;
			case 1:
				MenuBooks();
				break;
			case 2:
				MenuUser();
				break;
			default:
				MenuMain();
				break;
		}
	}
	static void MenuUser()
	{
		Console.Write("\n1. Создать пользователя.\n2. Сменить пользователя.\n3. Вывести всех пользователей.\n0. Назад.\nВведите пункт меню: ");
		int code;
		string s = Console.ReadLine();
		if(!Int32.TryParse(s, out code)) { MenuUser();}
		switch (code) {
			case 0:
				MenuMain();
				break;
			case 1:
				NewUser();
				MenuUser();
				break;
			case 2:
				SwUser();
				break;
			case 3:
				AllUser();
				MenuUser();
				break;
			default:
				MenuUser();
				break;
		}
	}
	static void NewUser() {
		string writePath = @"/home/pashvell/Projects/users.txt";
		
		using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
		{	Console.Write("Никнейм: ");
			string nick = Console.ReadLine();
			Console.Write("Пароль: ");
			string passw = Console.ReadLine();
			sw.WriteLine(String.Join(" | ", new string[] {nick, passw}));
		}
	}
	static void SwUser() {}
	static void AllUser()
	{
		string path= @"/home/pashvell/Projects/users.txt";
		
		
		Console.WriteLine(new string('-', GlVars.W/2 - 7) + " Пользователи " +
						  new string('-', GlVars.W/2 - 7));
		
		using (StreamReader us = new StreamReader(path, System.Text.Encoding.Default))
		{
			string user;
			while ((user = us.ReadLine()) != null)
			{
				user = user.Split(" | ")[0];
				Console.WriteLine("| " + 
								  user + new string(' ', GlVars.W - user.Length - 3) + '|');
				Console.WriteLine(new string('-', GlVars.W));
			}
		}
		
	}
	static void MenuBooks()  // Меню выбора взаимодействия с книгами
	{
		Console.Write("\n1. Информация о книге.\n2. Найти книгу.\n3. Вывести все книги.\n4. Изменить кнгиу.\n0. Назад.\nВведите пункт меню: ");
		int code;
		string s = Console.ReadLine();
		if(!Int32.TryParse(s, out code)) { MenuBooks();}
		switch (code) {
			case 0:
				MenuMain();
				break;
			case 1:
				infBook();
				break;
			case 2:
				SearchBook();
				MenuBooks();
				break;
			case 3:
				AllBooks();
				MenuBooks();
				break;
			case 4:
				MenuEdit();
				break;
			default:
				MenuBooks();
				break;
		}
	}
	static void MenuEdit()   // Меню редактирования книг
	{
		Console.Write("\n1. Добавить книгу\n2. Изменить книгу\n3. Удалить книгу.\n0. Назад.\nВведите пункт меню: ");
		int code;
		string s = Console.ReadLine();
		if(!Int32.TryParse(s, out code)) { MenuEdit();}
		switch (code) {
			case 0:
				MenuBooks();
				break;
			case 1:
				AddBook();
				MenuEdit();
				break;
			case 2:
				editBook();
				break;
			case 3:
				RemoveBook();
				MenuEdit();
				break;
			default:
				MenuEdit();
				break;
		}
	}
	static void AllBooks()   // Список книг
	{
		
		string path= @"/home/pashvell/Projects/books.txt";
		
		
		
		Console.WriteLine(new string('-', GlVars.W/2 - 3) + " Книги " +
						  new string('-', GlVars.W/2 - 4));
		string title = "Название";
		string author = "Автор";
		string isbn = "ISBN";
		string year = "Год выпуска";
		Console.WriteLine(MakeBookStr(new string[] {title, author, isbn, year}));
		Console.WriteLine(new string('-', GlVars.W)); 
		using (StreamReader ree = new StreamReader(path, System.Text.Encoding.Default))
		{
			string book;
			while ((book = ree.ReadLine()) != null)
			{
				string[] bookinf = book.Split(" | ");
				Console.WriteLine(MakeBookStr(bookinf));
				Console.WriteLine(new string('-', GlVars.W));	
			}
		}
		
	}
	static void RemoveBook() // Удаление книги
	{
		Console.Write("Введите название книги или: ");
		string data = Console.ReadLine();
		string[] remBook = SearchingBook(data);
		if (!(remBook.Length == 1))
		{
			if(remBook.Length == 0)
			{
				Console.WriteLine("Такой книги нет, попробуйте еще раз");
				RemoveBook();
			} else {
			Console.Write("Книг несколько, уточните\n");
			Console.Write("l. Посмотреть список\nb. Вернуться");
			string code = Console.ReadLine();
			switch (code)
			{
				case "l":
					foreach(string book in remBook)
					{
						Console.WriteLine(book);
					}
					break;
				case "b":
					MenuEdit();
					break;
				default:
					RemoveBook();
					break;
			}
		}}
		string path = @"/home/pashvell/Projects/books.txt";
		
		var tempFile = Path.GetTempFileName();
		var linesToKeep = File.ReadLines(path).Where(l => l != remBook[0]);

		File.WriteAllLines(tempFile, linesToKeep);

		File.Delete(path);
		File.Move(tempFile, path);
		
		Console.WriteLine("Книга успешно удалена");
	
	}
	static void AddBook()    // Добавление книги
	{
		string writePath = @"/home/pashvell/Projects/books.txt";
		
		using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
		{	Console.Write("Название: ");
			string title = Console.ReadLine();
			Console.Write("Автор: ");
			string author = Console.ReadLine();
			Console.Write("ISBN: ");
			string isbn = Console.ReadLine();
			Console.Write("Год выпуска: ");
			string year = Console.ReadLine();
			sw.WriteLine(String.Join(" | ", new string[] {title, author, isbn, year}));
		}
	}
		
	static string MakeBookStr(string[] s)
		{
			return("| " +
				   s[0] + new string(' ', GlVars.W/4 - s[0].Length -2) + "| " +
				   s[1] + new string(' ', GlVars.W/4 - s[1].Length -2) + "| " +
				   s[2] + new string(' ', GlVars.W/4 - s[2].Length -2) + "| " +
				   s[3] + new string(' ', GlVars.W/4 - s[3].Length -1) + '|');
		}
	static void SearchBook()
	{
		Console.Write("Введите название книги или: ");
		string data = Console.ReadLine();
		string[] inBooks= SearchingBook(data);
		
		if (inBooks.Length == 0)
		{
			Console.WriteLine("Не найдено.");
			return;
		}
		Console.WriteLine(new string('-', GlVars.W));	
		foreach(string book in inBooks)
		{
			Console.WriteLine("| " + book);
			Console.WriteLine(new string('-', GlVars.W));	
		}
	}
	static string[] SearchingBook(string data)
	{
		string path= @"/home/pashvell/Projects/books.txt";
		

		
		string[] books = File.ReadAllLines(path);
		List<string> intinBooks = new List<string>();
		for (int i = 0; i < books.Length; i++)
		{
			if(books[i].Contains(data))
			{
				intinBooks.Add(books[i]);
			}
		}
		return intinBooks.ToArray();
	}
	static void infBook() {}
	static void editBook() {}
}


public static class GlVars
{
	public static int W = Console.WindowWidth; // Ширина терминала
}



