/*Версионный стек. Поддерживаются операции Push, Pop, Rollback. Состояния стека после выполнения этих операций нумеруются. 
    С помощью Rollback можно откатиться на любое состояние, указав его номер. Rollback тоже можно откатить. 
    Помимо этого, существует операция Forget, позволяющая забыть всю историю изменений. 
    После Forget нумерация операций начинается с начала, Forget нельзя откатить. 
    Все 4 операции должны работать за O(1).*/

using System;
using System.Collections.Generic;

namespace SQ7
{
    public class ActionHistory<T>
    {
        private Example<T> last;
        private readonly List<Example<T>> actionHistory = new List<Example<T>>();

        public void Push(T value)
        {
            last = new Example<T>(value, last);
            actionHistory.Add(last);
        }

        public T Pop()
        {
            var value = last.Value;
            last = last.Previous;
            actionHistory.RemoveAt(Count);
            return value;
        }

        /*public void Rollback(int statusIndex)
        {
            last = actionHistory[statusIndex];
            actionHistory.Clear();
            actionHistory.Add(last);
        }*/

        public void Clear() => last = null;

        public int Count => actionHistory.Count;
    }

    public class Example<T>
    {
        public T Value;
        public Example<T> Previous;

        public Example(T value, Example<T> previous)
        {
            Value = value;
            Previous = previous;
        }
    }

    public class Stack<T>
    {
        private readonly ActionHistory<IAction<T>> actionHistory =
            new ActionHistory<IAction<T>>();

        public List<T> ListItem = new List<T>();

        public void Push(T item)
        {
            var command = new Adding<T>(item);
            actionHistory.Push(command);
            command.Execute(ListItem);
        }

        public void Pop()
        {
            var command = new Removing<T> { Index = actionHistory.Count };
            actionHistory.Push(command);
            command.Execute(ListItem);
        }

        public void Forget() => actionHistory.Clear();

        public void Rollback(int statusIndex)
        {
      

        }
    }

    interface IAction<T>
    {
        void Execute(List<T> items);
        void Undo(List<T> items);
    }

    public class Adding<T> : IAction<T>
    {
        public readonly T Item;

        public Adding(T item)
        {
            this.Item = item;
        }

        public void Undo(List<T> items)
        {
            items.Remove(Item);
        }

        public void Execute(List<T> items)
        {
            items.Add(Item);
        }
    }

    public class Removing<T> : IAction<T>
    {
        public T Item;
        public int Index;

        public void Undo(List<T> items)
        {
            items.Add(Item);
        }

        public void Execute(List<T> items)
        {
            Item = items[Index];
            items.RemoveAt(Index);
        }
    }
}
