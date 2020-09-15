using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HuanweiDuizhangConsole.Models
{
    public class Ledger : IList<LedgerItem>
    {
        private LedgerItem[] _contents;
        private int _count;
        public Ledger()
        {
            _count = 0;
        }
        public LedgerItem this[int index]
        {
            get
            {
                return _contents[index];
            }

            set
            {
                _contents[index] = value;
            }
        }

        public int Count => _count;

        public bool IsReadOnly => false;

        public void Add(LedgerItem item)
        {
            _contents[_count] = item;
            _count++;
        }

        public void Clear()
        {
            _count = 0;
        }

        public bool Contains(LedgerItem item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_contents[i] == item)
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(LedgerItem[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<LedgerItem> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(LedgerItem item)
        {
            for (int i = 0; i < Count; i++)
            {
                if (_contents[i] == item)
                {
                    return i;
                }
            }
            return -1;
        }

        public void Insert(int index, LedgerItem item)
        {
            if ((_count + 1 <= _contents.Length) && (index < Count) && (index >=0 ))
            {
                _count++;
                for (int i = Count - 1; i > index; i--)
                {
                    _contents[i] = _contents[i - 1];
                }
                _contents[index] = item;
            }
        }

        public bool Remove(LedgerItem item)
        {
            if (IndexOf(item) == -1) return false;
            RemoveAt(IndexOf(item));
            return true;
        }

        public void RemoveAt(int index)
        {
            if ((index >= 0) && (index < Count))
            {
                for (int i = index; i < Count - 1; i++)
                {
                    _contents[i] = _contents[i + 1];
                }
            }
            _count--;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void PrintContents()
        {
            foreach (LedgerItem item in _contents)
            {
                Console.WriteLine(item);
            }
        }
    }
}
