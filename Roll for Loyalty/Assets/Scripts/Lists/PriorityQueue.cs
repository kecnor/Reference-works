using System;
using System.Collections.Generic;

public class PriorityQueue<T>
{
    #region Variable
    private List<Tuple<T, int>> elements = new List<Tuple<T, int>>();

    //Getter
    public int Count
    {
        get { return elements.Count; }
    }
    #endregion
    #region Functions
    public void Enqueue(T item, int priority)
    {
        elements.Add(Tuple.Create(item, priority));
    }

    public T Dequeue()
    {
        int bestIndex = 0;

        for (int i = 0; i < elements.Count; i++)
        {
            if (elements[i].Item2 < elements[bestIndex].Item2)
            {
                bestIndex = i;
            }
        }

        T bestItem = elements[bestIndex].Item1;
        elements.RemoveAt(bestIndex);
        return bestItem;
    }
    #endregion
}