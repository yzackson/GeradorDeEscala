using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeradorDeEscala;

internal class Integrante
{
    public string Nome { get; set; }
    public MesDiasDeCulto[] DatasRestritas { get; set; }
}

internal class EscalaPorData
{
    public string Mes { get; set; }
    public string Dia { get; set; }
    public List<Integrante> Integrantes { get; set; }
}