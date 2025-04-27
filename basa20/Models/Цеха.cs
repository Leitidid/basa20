using System;
using System.Collections.Generic;

namespace basa20.Models;

public partial class Цеха
{
    public int КодЦеха { get; set; }

    public string НаименованиеЦеха { get; set; } = null!;

    public string Начальник { get; set; } = null!;

    public virtual ICollection<ПланВыпуска> ПланВыпускаs { get; set; } = new List<ПланВыпуска>();
}
