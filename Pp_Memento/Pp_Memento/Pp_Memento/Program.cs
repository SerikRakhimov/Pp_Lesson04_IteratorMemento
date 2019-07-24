using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// Memento (Снимок):
// Игровой персонаж совершает выстрелы,
// необходимо сохранять состояния игрока после серии выстрелов.

namespace Pp_Memento
{
    // Создатель содержит некоторое важное состояние, которое может со
    // временем меняться. Он также объявляет метод сохранения состояния внутри
    // снимка и метод восстановления состояния из него.
    class Originator
    {
        // Для удобства состояние создателя хранится внутри одной
        // переменной.
        private string _state;

        public Originator(string state)
        {
            this._state = state;
            Console.WriteLine("Первоначальное состояние: " + state);
        }

        // Бизнес-логика Создателя может повлиять на его внутреннее
        // состояние. Поэтому клиент должен выполнить резервное копирование
        // состояния с помощью метода save перед запуском методов бизнес-логики.
        public void DoSomething()
        {
            this._state = this.GenerateRandomString();
            Console.WriteLine($"Персонаж: {_state}");
        }

        private string GenerateRandomString()
        {
            string result = "Выстрел: " + (new Random().Next(0, 30) + 10).ToString();

            return result;
        }

        // Сохраняет текущее состояние внутри снимка.
        public IMemento Save()
        {
            return new ConcreteMemento(this._state);
        }

        // Восстанавливает состояние Создателя из объекта снимка.
        public void Restore(IMemento memento)
        {
            if (!(memento is ConcreteMemento))
            {
                throw new Exception("Неизвестный снимок " + memento.ToString());
            }

            this._state = memento.GetState();
            Console.WriteLine($"Текущее состояние: {_state}");
        }
    }

    // Интерфейс Снимка предоставляет способ извлечения метаданных снимка,
    // таких как дата создания или название. Однако он не раскрывает состояние
    // Создателя.
    public interface IMemento
    {
        string GetName();

        string GetState();

        DateTime GetDate();
    }

    // Конкретный снимок содержит инфраструктуру для хранения состояния
    // Создателя.
    class ConcreteMemento : IMemento
    {
        private string _state;

        private DateTime _date;

        public ConcreteMemento(string state)
        {
            this._state = state;
            this._date = DateTime.Now;
        }

        // Создатель использует этот метод, когда восстанавливает своё
        // состояние.
        public string GetState()
        {
            return this._state;
        }

        // Остальные методы используются Опекуном для отображения
        // метаданных.
        public string GetName()
        {
            return $"{this._date} / ({this._state.Substring(0, 11)})...";
        }

        public DateTime GetDate()
        {
            return this._date;
        }
    }

    // Опекун не зависит от класса Конкретного Снимка. Таким образом, он не
    // имеет доступа к состоянию создателя, хранящемуся внутри снимка. Он
    // работает со всеми снимками через базовый интерфейс Снимка.
    class Caretaker
    {
        private List<IMemento> _mementos = new List<IMemento>();

        private Originator _originator = null;

        public Caretaker(Originator originator)
        {
            this._originator = originator;
        }

        public void Backup()
        {
            Console.WriteLine("Сохранение снимка...\n");
            Thread.Sleep(1000);
            this._mementos.Add(this._originator.Save());
        }

        public void Undo()
        {
            if (this._mementos.Count == 0)
            {
                return;
            }

            var memento = this._mementos.Last();
            this._mementos.Remove(memento);

            Console.WriteLine("\nВосстановлено состояние: " + memento.GetName());

            try
            {
                this._originator.Restore(memento);
            }
            catch (Exception)
            {
                this.Undo();
            }
        }

        public void ShowHistory()
        {
            Console.WriteLine("Список сохраненных снимков:");

            foreach (var memento in this._mementos)
            {
                Console.WriteLine(memento.GetName());
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Клиентский код.
            Originator originator = new Originator("Подготовка ");
            Caretaker caretaker = new Caretaker(originator);
            caretaker.Backup();

            originator.DoSomething();
            caretaker.Backup();

            originator.DoSomething();
            caretaker.Backup();

            originator.DoSomething();
            caretaker.Backup();

            Console.WriteLine();
            caretaker.ShowHistory();

            caretaker.Undo();

            caretaker.Undo();

            Console.ReadLine();
        }
    }

}
