using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace TeaFramework.Common.Utilities.Wrappers
{
    /// <summary>
    ///     Wraps around a List of TooltipLines for easy management.
    /// </summary>
    public readonly struct TooltipWrapper
    {
        public readonly List<TooltipLine> Lines;

        public TooltipWrapper(List<TooltipLine> lines)
        {
            Lines = lines;
        }

        public void HandleTooltips(Action<TooltipLine> action)
        {
            foreach (TooltipLine line in Lines)
                action(line);
        }

        public void Add(int index, TooltipLine line) => Lines.Insert(index, line);

        public void Add(TooltipLine line, bool front = false)
        {
            if (front)
                Lines.Insert(0, line);
            else
                Lines.Add(line);
        }

        public void Remove(int index) => Lines.RemoveAt(index);

        public void Remove(TooltipLine line) => Lines.Remove(line);

        public void Remove(Predicate<TooltipLine> predicate, bool all = false)
        {
            if (all)
                Lines.RemoveAll(predicate);
            else
            {
                TooltipLine? line = Lines.FirstOrDefault(predicate.Invoke);
                
                if (line is not null)
                    Lines.Remove(line);
            }
        }

        public void InsertBefore(string mod, string name, TooltipLine line)
        {
            TooltipLine? found = Lines.FirstOrDefault(x => x.mod == mod && x.Name == name);

            if (found is null)
                return;

            int index = Lines.IndexOf(found);

            if (index == -1)
                return;

            Lines.Insert(index, line);
        }

        public void InsertAfter(string mod, string name, TooltipLine line)
        {
            TooltipLine? found = Lines.FirstOrDefault(x => x.mod == mod && x.Name == name);

            if (found is null)
                return;

            int index = Lines.IndexOf(found);

            if (index == -1)
                return;

            Lines.Insert(index + 1, line);
        }
    }
}