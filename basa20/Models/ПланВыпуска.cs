using System;
using System.Collections.Generic;

namespace basa20.Models;

public partial class ПланВыпуска
{
    public int КодПлана { get; set; }

    public int КодИзделия { get; set; }

    public int КоличествоИзделий { get; set; }

    public int КодЦеха { get; set; }

    public virtual Изделия КодИзделияNavigation { get; set; } = null!;

    public virtual Цеха КодЦехаNavigation { get; set; } = null!;
}
