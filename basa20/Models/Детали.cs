using System;
using System.Collections.Generic;

namespace basa20.Models;

public partial class Детали
{
    public int КодДетали { get; set; }

    public string НаименованиеДетали { get; set; } = null!;

    public decimal Цена { get; set; }

    public virtual ICollection<СоставИзделия> СоставИзделияs { get; set; } = new List<СоставИзделия>();
}
