using System;
using System.Collections.Generic;

namespace basa20.Models;

public partial class Изделия
{
    public int КодИзделия { get; set; }

    public string НаименованиеИзделия { get; set; } = null!;

    public decimal СтоимостьСборки { get; set; }

    public virtual ICollection<ПланВыпуска> ПланВыпускаs { get; set; }

    public virtual ICollection<СоставИзделия> СоставИзделияs { get; set; } = new List<СоставИзделия>();
}
