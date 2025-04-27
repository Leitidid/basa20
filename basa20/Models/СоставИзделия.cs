using System;
using System.Collections.Generic;

namespace basa20.Models;

public partial class СоставИзделия
{
    public int КодСостава { get; set; }

    public int КодИзделия { get; set; }

    public int КодДетали { get; set; }

    public int КоличествоДеталей { get; set; }

    public virtual Детали КодДеталиNavigation { get; set; } = null!;

    public virtual Изделия КодИзделияNavigation { get; set; } = null!;
}
