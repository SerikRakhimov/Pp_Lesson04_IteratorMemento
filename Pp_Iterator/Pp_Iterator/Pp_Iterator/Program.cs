using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

// Iterator(Итератор)
// Дан список слов.
// Необходимо создать итератор,
// печатающий сперва все четные элементы, затем все нечетные.

namespace Pp_Iterator
{
    abstract class Iterator : IEnumerator
    {
        object IEnumerator.Current => Current();

        // Возвращает ключ текущего элемента
        public abstract int Key();

        // Возвращает текущий элемент.
        public abstract object Current();

        // Переходит к следующему элементу.
        public abstract bool MoveNext();

        // Перематывает Итератор к первому элементу.
        public abstract void Reset();
    }

    abstract class IteratorAggregate : IEnumerable
    {
        // Возвращает Iterator или другой IteratorAggregate для реализующего
        // объекта.
        public abstract IEnumerator GetEnumerator();
    }

    // Конкретные Итераторы реализуют различные алгоритмы обхода. Эти классы
    // постоянно хранят текущее положение обхода.
    class AlphabeticalOrderIterator : Iterator
    {
        private WordsCollection _collection;

        // Хранит текущее положение обхода. У итератора может быть множество
        // других полей для хранения состояния итерации, особенно когда он
        // должен работать с определённым типом коллекции.
        private int _position = -1;

        private bool _chetnye = true;

        public AlphabeticalOrderIterator(WordsCollection collection)
        {
            this._collection = collection;
            this._position = -2;
        }

        public override object Current()
        {
            return this._collection.getItems()[_position];
        }

        public override int Key()
        {
            return this._position;
        }

        public override bool MoveNext()
        {
            int updatedPosition = this._position + 2;

            if (updatedPosition >= 0 && updatedPosition < this._collection.getItems().Count)
            {
                this._position = updatedPosition;
                return true;
            }
            else
            {
                if (this._chetnye == true)
                {
                    this._chetnye = false;

                    updatedPosition = 1;

                    if (updatedPosition >= 0 && updatedPosition < this._collection.getItems().Count)
                    {
                        this._position = updatedPosition;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public override void Reset()
        {
            this._position = 0;
        }
    }

    // Конкретные Коллекции предоставляют один или несколько методов для
    // получения новых экземпляров итератора, совместимых с классом коллекции.
    class WordsCollection : IteratorAggregate
    {
        List<string> _collection = new List<string>();

        public List<string> getItems()
        {
            return _collection;
        }

        public void AddItem(string item)
        {
            this._collection.Add(item);
        }

        public override IEnumerator GetEnumerator()
        {
            return new AlphabeticalOrderIterator(this);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Клиентский код может знать или не знать о Конкретном
            // Итераторе или классах Коллекций, в зависимости от уровня
            // косвенности, который вы хотите сохранить в своей программе.
            var collection = new WordsCollection();
            collection.AddItem("Слива0");
            collection.AddItem("Персик1");
            collection.AddItem("Вишня2");
            collection.AddItem("Арбуз3");
            collection.AddItem("Дыня4");
            collection.AddItem("Виноград5");
            collection.AddItem("Яблоко6");
            collection.AddItem("Груша7");
            collection.AddItem("Апельсин8");

            Console.WriteLine("Сначала четные элементы, затем нечетные:");

            foreach (var element in collection)
            {
                Console.WriteLine(element);
            }

            Console.ReadLine();
        }
    }
}
