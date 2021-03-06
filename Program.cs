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
        private readonly LinkedList<T> actionHistory = new LinkedList<T>();

        public void Push(T item) => actionHistory.AddLast(item);

        public T Pop()
        {
            var currentItem = actionHistory.Last.Value;
            actionHistory.RemoveLast();
            return currentItem;
        }

        public int Count => actionHistory.Count;

        public void Clear() => actionHistory.Clear();
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
            if(ListItem.Count == 0)
                throw new ArgumentOutOfRangeException();

            var command = new Removing<T>();
            actionHistory.Push(command);
            command.Execute(ListItem);
        }

        public void Forget() => actionHistory.Clear();

        public void Rollback(int statusIndex)
        {
            for (int i = actionHistory.Count; i > statusIndex; i--)
            {
                var command = actionHistory.Pop();
                command.Undo(ListItem);
            }
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

        public void Undo(List<T> items)
        {
            items.Add(Item);
        }

        public void Execute(List<T> items)
        {
            Item = items[items.Count - 1];
            items.RemoveAt(items.Count - 1);
        }
    }

    class Program
    {
        public static void Main()
        {

        }
    }
}
