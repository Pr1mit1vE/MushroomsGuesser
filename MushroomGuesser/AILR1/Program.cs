using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AILR1
{
    public class Mushroom
    {
        public readonly int id;
        public string Name { get; set; }
        public bool IsQuestion { get; set; }
        public int Yes { get; set; }
        public int No { get; set; }


        public Mushroom(int id, string name, bool isQuestion, int yes, int no)
        {
            this.id = id;
            this.Name = name;
            this.IsQuestion = isQuestion;
            this.Yes = yes;
            this.No = no;
        }
    }

    public class Mushrooms
    {
        public List<Mushroom> mushrooms = new List<Mushroom>();
        public readonly List<Mushroom> history = new List<Mushroom>();

        public Mushrooms()
        {
            var json = File.ReadAllText("mushrooms.json");
            var stringItems = json.Split('\n');
            foreach (var item in stringItems)
            {
                if (item.Length > 0)
                {
                    var deserializedItem = JsonConvert.DeserializeObject<Mushroom>(item);
                    mushrooms.Add(deserializedItem);
                }
            }
        }

        public void SaveConfig()
        {
            File.Delete("mushrooms.json");
            mushrooms = mushrooms.OrderBy(x => x.id).ToList();
            foreach (var mushroom in mushrooms)
            {
                using (StreamWriter sw = File.AppendText("mushrooms.json"))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(mushroom));
                }
            }
        }

        public void GuessItem()
        {
            history.Clear();
            var mushroom = mushrooms[0];
            Console.WriteLine(mushroom.Name);
            history.Add(mushroom);
            var answer = Console.ReadLine();
            while (true)
            {
                if (answer == "+")
                {
                    if (mushroom.Yes != -1)
                    {
                        mushroom = mushrooms.Where(p => p.id == mushroom.Yes).First();
                        if (mushroom.IsQuestion)
                        {
                            Console.WriteLine(mushroom.Name);
                            history.Add(mushroom);
                            answer = Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine($"Это {mushroom.Name}?");
                            history.Add(mushroom);
                            answer = Console.ReadLine();
                            if (answer == "+")
                            {
                                Console.WriteLine("Я угадал!))))");
                                break;
                            }
                        }
                    }
                }
                else
                {
                    if (mushroom.No != -1)
                    {
                        mushroom = mushrooms.Where(p => p.id == mushroom.No).First();
                        if (mushroom.IsQuestion)
                        {
                            Console.WriteLine(mushroom.Name);
                            history.Add(mushroom);
                            answer = Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine($"Это {mushroom.Name}?");
                            answer = Console.ReadLine();
                            if (answer == "+")
                            {
                                Console.WriteLine("Я угадал!))))");
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Сдаюсь! Напишите какой гриб вы загадали");
                        var name = Console.ReadLine();
                        mushrooms.Add(new Mushroom(mushrooms.Count + 2, name, false, -1, -1));
                        if (!mushroom.IsQuestion)
                        {
                            Console.WriteLine($"Сформулируйте вопрос, ответ на который поможет отличить \r\n«{name}» от «{mushroom.Name}»");
                        }
                        else
                        {
                            Console.WriteLine($"Сформулируйте вопрос, ответ на который поможет отличить «{name}»");
                        }
                        mushroom.No = mushrooms.Count;
                        var question = Console.ReadLine();
                        mushrooms.Add(new Mushroom(mushrooms.Count, question, true, mushrooms.Count + 1, -1));
                        break;
                    }
                }
            }
        }
        public void GetInfoAboutAnswer()
        {
            if (history.Count == 0)
            {
                Console.WriteLine("Вы ещё не угадывали гриб");
                return;
            }
            for (var i = 0; i < history.Count() - 1; i++)
            {
                if (history[i].IsQuestion)
                {
                    if (history[i].No == history[i + 1].id)
                        Console.WriteLine($"На мой вопрос:\"{history[i].Name}\" вы ответили \"-\",");
                    else
                        Console.WriteLine($"На мой вопрос:\"{history[i].Name}\" вы ответили \"+\",");
                    if (history[i + 1].IsQuestion)
                        Console.WriteLine($"поэтому я решил задать вам следующий вопрос");
                    else
                        Console.WriteLine($"поэтому я предположил, что это \"{history[i + 1].Name}\"");
                }
                else
                {
                    if (history[i].No == history[i + 1].id)
                        Console.Write($"На моё предположение:\"Это {history[i].Name}?\" вы ответили \"-\", поэтому ");
                    else
                        Console.Write($"На моё предположение:\"Это {history[i].Name}?\" вы ответили \"+\", поэтому ");
                    if (history[i + 1].IsQuestion)
                        Console.WriteLine($"я решил задать вам следующий вопрос");
                    else
                        Console.WriteLine($"я предположил, что это \"{history[i + 1].Name}\"");
                }
            }

        }
        public void DrawTree(Mushroom mushroom, string indent)
        {
            Console.WriteLine(mushroom.Name);
            if (mushroom.Yes != -1)
            {
                var mushroomYes = mushrooms.Where(p => p.id == mushroom.Yes).First();
                Console.Write(indent + "├── Да: ");
                DrawTree(mushroomYes, indent + "    ");
            }
            else if (!mushroom.IsQuestion)
                Console.WriteLine(indent + "├── Да: Угадал)) ");
            if (mushroom.No != -1)
            {
                var mushroomNo = mushrooms.Where(p => p.id == mushroom.No).First();
                Console.Write(indent + "└── Нет: ");
                DrawTree(mushroomNo, indent + "    ");
            }
            else
                Console.WriteLine(indent + "└── Нет: ");
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var notExit = true;
            var mushrooms = new Mushrooms();
            while (notExit)
            {
                Console.WriteLine("Меню:");
                Console.WriteLine("1-Начать игру");
                Console.WriteLine("2-Найти конкретный гриб в базе данных");
                Console.WriteLine("3-Получить информацию об ответе");
                Console.WriteLine("4-Посмотреть базу данных");
                Console.WriteLine("5-Выйти");
                string number = Console.ReadLine();
                switch (number)
                {
                    case "1":
                        Console.WriteLine("Для ответа используйти + или -");
                        mushrooms.GuessItem();
                        mushrooms.SaveConfig();
                        break;
                    case "2":
                        Console.WriteLine("Для того чтобы найти конкретный гриб в базе, напишите его название с большой буквы." +
                            "Например: Мухомор");
                        var mushroom = Console.ReadLine();
                        Console.WriteLine(JsonConvert.SerializeObject(mushrooms.mushrooms.Where(p => p.Name == mushroom)));
                        break;
                    case "3":
                        mushrooms.GetInfoAboutAnswer();
                        break;
                    case "4":
                        mushrooms.DrawTree(mushrooms.mushrooms[0], "");
                        break;
                    case "5":
                        notExit = false;
                        break;

                }
            }
        }
    }
}
