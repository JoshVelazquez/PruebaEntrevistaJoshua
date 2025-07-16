using System;
using System.Collections.Generic;

namespace PruebaEntrevistaJoshua.Models;

public partial class Producto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int Precio { get; set; }
}
