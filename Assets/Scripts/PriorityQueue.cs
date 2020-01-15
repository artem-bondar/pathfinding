using System;
using System.Collections.Generic;

public class PriorityQueue<T> where T: IComparable<T>
{
    List<T> data;

    public PriorityQueue() => this.data = new List<T>();

    public void Enqueue(T item)
    {
        data.Add(item);

        int childIndex = data.Count - 1;

        while (childIndex > 0)
        {
            int parentIndex = (childIndex - 1) / 2;

            if (data[childIndex].CompareTo(data[parentIndex]) >= 0)
            {
                break;
            }

            T temp = data[childIndex];
            data[childIndex] = data[parentIndex];
            data[parentIndex] = temp;

            childIndex = parentIndex;
        }
    }
}
