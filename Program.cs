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
            var command = new Removing<T> { Index = actionHistory.Count };
            actionHistory.Push(command);
            command.Execute(ListItem);
        }

        public void Forget() => actionHistory.Clear();

        /*public void Rollback(int statusIndex)
        {
            var rollbackHistory = new ActionHistory<IAction<T>>();
            for (int i = actionHistory.Count - 1; i > statusIndex; i--)
            {
                var command = actionHistory.Pop();
                command.Undo(ListItem);
                rollbackHistory.Push(command);
            }

        }*/
    }

    interface IAction<T>
    {
        void Execute(List<T> items);
        void Undo(List<T> items);
    }

    /*public class Rollbacking<T> : IAction<T>
    {
        public readonly int StatusIndex;
        public readonly LinkedList<T> ActionHistory;
        private readonly ActionHistory<IAction<T>> rollbackHistory;

        public Rollbacking(int statusIndex, LinkedList<T> actionHistory, List<T> listItem)
        {
            this.StatusIndex = statusIndex;
            this.ActionHistory = actionHistory;
            rollbackHistory = new ActionHistory<IAction<T>>();
        }

        public void Execute(List<T> items)
        {

        }

        public void Undo(List<T> items)
        {
            throw new NotImplementedException();
        }
    }*/

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
